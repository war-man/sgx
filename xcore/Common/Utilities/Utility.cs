﻿using System;
using System.Linq;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Common.Enums;
using System.Text;
using System.Net;
using System.Globalization;
using System.Collections.Generic;
using Data;
using MongoDB.Driver;
using Models;
using System.Reflection;
using NPOI.SS.UserModel;
//using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Common.Utilities
{
    public static class Utility
    {
        private static MongoDBContext dbContext = new MongoDBContext();

        static Utility()
        {
        }

        public static void AutoInitSalary(int salaryType, int month, int year)
        {
            var endDateMonth = new DateTime(year, month, 25);
            // Check data in month. If no, create list.
            // Exist: don't do anything.
            var congtychinhanhs = dbContext.CongTyChiNhanhs.Find(m => m.Enable.Equals(true)).ToList();
            var ctcnvp = congtychinhanhs.First(x => x.Code.Equals("CT1"));
            var ctcnnm = congtychinhanhs.First(x => x.Code.Equals("CT2"));
            var settings = dbContext.Settings.Find(m => m.Type.Equals((int)ESetting.Salary) && m.Enable.Equals(true)).ToList();

            #region Filter Salary Data
            var builder = Builders<SalaryEmployeeMonth>.Filter;
            var filter = builder.Eq(m => m.Year, year)
                        & builder.Eq(m => m.Month, month);
            switch (salaryType)
            {
                case (int)ESalaryType.VP:
                    filter = filter & builder.Eq(x => x.CongTyChiNhanhId, ctcnvp.Id);
                    break;
                case (int)ESalaryType.NM:
                    filter = filter & builder.Eq(x => x.CongTyChiNhanhId, ctcnnm.Id) & !builder.Eq(x => x.ChucVuId, "5c88d09bd59d56225c4324de"); // cong-nhan-dong-goi
                    break;
                default:
                    filter = filter & builder.Eq(x => x.CongTyChiNhanhId, ctcnnm.Id) & builder.Eq(x => x.ChucVuId, "5c88d09bd59d56225c4324de"); // cong-nhan-dong-goi
                    break;
            }
            #endregion

            var exist = dbContext.SalaryEmployeeMonths.CountDocuments(filter);
            if (exist == 0)
            {
                #region Common
                var thamsotinhluong = BusinessDaysUntil(endDateMonth.AddMonths(-1).AddDays(1), endDateMonth);

                //var settingTSTL = dbContext.Settings.Find(m => m.Key.Equals("mau-so-lam-viec")).FirstOrDefault();

                var luongtoithieuvungE = dbContext.SalaryMucLuongVungs.Find(m => m.Enable.Equals(true)).SortByDescending(m => m.Year).SortByDescending(m => m.Month).FirstOrDefault();
                var luongtoithieuvung = luongtoithieuvungE != null ? luongtoithieuvungE.ToiThieuVungDoanhNghiepApDung : 0;

                #endregion

                #region Filter Employee Data
                var builderE = Builders<Employee>.Filter;
                var filterE = builderE.Eq(m => m.Enable, true)
                            & builderE.Eq(m => m.Leave, false)
                            & builderE.Eq(m => m.Official, true)
                            & !builderE.Eq(m => m.UserName, Constants.System.account);
                switch (salaryType)
                {
                    case (int)ESalaryType.VP:
                        filterE = filterE & builderE.Eq(x => x.CongTyChiNhanh, ctcnvp.Id);
                        break;
                    case (int)ESalaryType.NM:
                        filterE = filterE & builderE.Eq(x => x.CongTyChiNhanh, ctcnnm.Id) & !builderE.Eq(x => x.ChucVu, "5c88d09bd59d56225c4324de"); // cong-nhan-dong-goi
                        break;
                    default:
                        filterE = filterE & builderE.Eq(x => x.CongTyChiNhanh, ctcnnm.Id) & builderE.Eq(x => x.ChucVu, "5c88d09bd59d56225c4324de"); // cong-nhan-dong-goi
                        break;
                }
                #endregion
                var employees = dbContext.Employees.Find(filterE).ToList();
                foreach (var employee in employees)
                {
                    var luongcanban = GetLuongCanBan(employee.NgachLuongCode, employee.NgachLuongLevel);
                    // Lương căn bản của công nhân sản xuất là:  4,012,500 
                    if (salaryType == (int)ESalaryType.SX)
                    {
                        luongcanban = 4012500;
                    }
                    else if (salaryType == (int)ESalaryType.VP)
                    {
                        luongcanban = GetLuongCanBanVP(employee.ChucVu, employee.NgachLuongLevel);
                    }
                    // Get direct to employees
                    decimal luongThamGiaBHXH = employee.LuongBHXH;

                    #region ThamNien
                    var ngaythamnien = (endDateMonth - employee.Joinday).TotalDays;
                    double thangthamnien = Math.Round(ngaythamnien / 30, 0);
                    double namthamnien = Math.Round(thangthamnien / 12, 0);
                    var hesothamnien = 0;
                    // 3 năm đầu ko tăng, bắt đầu năm thứ 4 sẽ có thâm niên 3%, thêm 1 năm tăng 1%
                    if (namthamnien >= 4)
                    {
                        hesothamnien = 3;
                        for (int i = 5; i <= namthamnien; i++)
                        {
                            hesothamnien++;
                        }
                    }
                    #endregion

                    #region PCPL
                    decimal nangNhocDocHai = 0;
                    decimal trachNhiem = 0;
                    decimal thuHut = 0;
                    decimal xang = 0;
                    decimal dienThoai = 0;
                    decimal com = 0;
                    decimal nhaO = 0;
                    decimal kiemNhiem = 0;
                    decimal bhytDacBiet = 0;
                    decimal viTriCanKnNhieuNam = 0;
                    decimal viTriDacThu = 0;
                    if (employee.PhuCapPhucLoi != null)
                    {
                        var phucapphucloi = employee.PhuCapPhucLoi;
                        nangNhocDocHai = phucapphucloi.NangNhocDocHai;
                        trachNhiem = phucapphucloi.TrachNhiem;
                        thuHut = phucapphucloi.ThuHut;
                        xang = phucapphucloi.Xang;
                        dienThoai = phucapphucloi.DienThoai;
                        com = phucapphucloi.Com;
                        nhaO = phucapphucloi.NhaO;
                        kiemNhiem = phucapphucloi.KiemNhiem;
                        bhytDacBiet = phucapphucloi.BhytDacBiet;
                        viTriCanKnNhieuNam = phucapphucloi.ViTriCanKnNhieuNam;
                        viTriDacThu = phucapphucloi.ViTriDacThu;
                    }
                    #endregion

                    var salary = new SalaryEmployeeMonth()
                    {
                        Year = year,
                        Month = month,
                        EmployeeId = employee.Id,
                        EmployeeCode = employee.CodeOld,
                        EmployeeFullName = employee.FullName,
                        CongTyChiNhanhId = employee.CongTyChiNhanh,
                        CongTyChiNhanhName = employee.CongTyChiNhanhName,
                        KhoiChucNangId = employee.KhoiChucNang,
                        KhoiChucNangName = employee.KhoiChucNangName,
                        PhongBanId = employee.PhongBan,
                        PhongBanName = employee.PhongBanName,
                        BoPhanId = employee.BoPhan,
                        BoPhanName = employee.BoPhanName,
                        BoPhanConId = employee.BoPhanCon,
                        BoPhanConName = employee.BoPhanConName,
                        ChucVuId = employee.ChucVu,
                        ChucVuName = employee.ChucVuName,
                        NgachLuongCode = employee.NgachLuongCode,
                        NgachLuongLevel = employee.NgachLuongLevel,
                        JoinDate = employee.Joinday,
                        MauSo = thamsotinhluong,
                        Type = salaryType,
                        LuongToiThieuVung = luongtoithieuvung,
                        LuongCanBan = luongcanban,
                        ThamNienMonth = (int)thangthamnien,
                        ThamNienYear = (int)namthamnien,
                        HeSoThamNien = hesothamnien,
                        ThamNien = luongcanban * hesothamnien / 100,
                        NangNhocDocHai = nangNhocDocHai,
                        TrachNhiem = trachNhiem,
                        ThuHut = thuHut,
                        DienThoai = dienThoai,
                        Xang = xang,
                        Com = com,
                        NhaO = nhaO,
                        KiemNhiem = kiemNhiem,
                        BhytDacBiet = bhytDacBiet,
                        ViTriCanKnNhieuNam = viTriCanKnNhieuNam,
                        ViTriDacThu = viTriDacThu,
                        LuongThamGiaBHXH = luongThamGiaBHXH
                    };
                    dbContext.SalaryEmployeeMonths.InsertOne(salary);
                }
            }
        }

        public static SalaryEmployeeMonth SalaryEmployeeMonthFillData(SalaryEmployeeMonth salary)
        {
            var employeeId = salary.EmployeeId;
            var year = salary.Year;
            var month = salary.Month;

            double ngayLamViec = 0;
            double phepNam = 0;
            double leTet = 0;
            var chamCongs = dbContext.EmployeeWorkTimeMonthLogs.Find(m => m.EmployeeId.Equals(employeeId) & m.Year.Equals(year) & m.Month.Equals(month)).ToList();
            if (chamCongs != null & chamCongs.Count > 0)
            {
                foreach (var chamCong in chamCongs)
                {
                    ngayLamViec = chamCong.NgayLamViecChinhTay > 0 ? chamCong.NgayLamViecChinhTay : chamCong.Workday;
                    phepNam = chamCong.PhepNamChinhTay > 0 ? chamCong.PhepNamChinhTay : chamCong.NghiPhepNam;
                    leTet = chamCong.LeTetChinhTay > 0 ? chamCong.LeTetChinhTay : chamCong.NghiLe;
                }
            }
            salary.NgayCongLamViec = ngayLamViec;
            salary.NgayNghiPhepNam = phepNam;
            salary.NgayNghiLeTetHuongLuong = leTet;
            decimal luongDinhMuc = Convert.ToDecimal((double)salary.LuongCanBan / salary.MauSo * ngayLamViec);
            salary.LuongDinhMuc = luongDinhMuc;
            decimal tienPhepNamLeTet = Convert.ToDecimal((phepNam * (double)salary.LuongCanBan / salary.MauSo) + (leTet * (double)salary.LuongCanBan / salary.MauSo));
            salary.TienPhepNamLeTet = tienPhepNamLeTet;
            decimal thanhTienLuongCanBan = luongDinhMuc + tienPhepNamLeTet;
            salary.ThanhTienLuongCanBan = thanhTienLuongCanBan;

            decimal congTong = 0;
            decimal tongPhuCap = 0;
            decimal comSX = 0;
            decimal comKD = 0;
            decimal comNM = 0;
            decimal comVP = 0;
            var dataPr = dbContext.EmployeeCongs.Find(m => m.Enable.Equals(true) && m.EmployeeId.Equals(employeeId) && m.Month.Equals(month) && m.Year.Equals(year)).FirstOrDefault();
            if (dataPr != null)
            {
                congTong = dataPr.ThanhTienTrongGio + dataPr.ThanhTienNgoaiGio;
                comSX = Convert.ToDecimal(dataPr.Com * (double)1);
                comNM = Convert.ToDecimal(dataPr.Com * (double)1);
                comKD = Convert.ToDecimal(dataPr.Com * (double)1);
                comVP = Convert.ToDecimal(dataPr.Com * (double)1);
                salary.ComSX = comSX;
                tongPhuCap += comSX;
                salary.ComNM = comNM;
                tongPhuCap += comNM;
                salary.ComKD = comKD;
                tongPhuCap += comKD;
                salary.ComVP = comVP;
                tongPhuCap += comVP;
            }

            //tongPhuCap += salary.Com;
            //salary.NhaO = 0;
            //tongPhuCap += salary.NhaO;
            //salary.Xang = 0;
            //tongPhuCap += salary.Xang;
            //salary.NangNhocDocHai = 0;
            //tongPhuCap += salary.NangNhocDocHai;
            //salary.ThamNien = 0;
            //tongPhuCap += salary.ThamNien;
            //salary.TrachNhiem = 0;
            //tongPhuCap += salary.TrachNhiem;
            //salary.PhuCapChuyenCan = 0;
            //tongPhuCap += salary.PhuCapChuyenCan;
            //salary.PhuCapKhac = 0;
            //tongPhuCap += salary.PhuCapKhac;
            //salary.TongPhuCap = tongPhuCap;

            #region LUONG SX: KO CO PHU CAP (THAM NIEN,CHUYEN CAN...)
            if (salary.ChucVuId == "5c88d09bd59d56225c4324de")
            {
                salary.Com = 0;
                salary.NhaO = 0;
                salary.Xang = 0;
                salary.NangNhocDocHai = 0;
                salary.ThamNien = 0;
                salary.TrachNhiem = 0;

                salary.TongPhuCap = 0;
            }
            #endregion

            #region VP
            decimal tongthunhapVP = 0;
            if (salary.Type == (int)ESalaryType.VP)
            {
                decimal luongcobanbaogomphucap = salary.LuongCanBan 
                                            + salary.NangNhocDocHai + salary.TrachNhiem + salary.ThamNien + salary.ThuHut 
                                            + salary.Xang + salary.Com + salary.KiemNhiem + salary.BhytDacBiet
                                            + salary.ViTriCanKnNhieuNam + salary.ViTriDacThu;
                salary.LuongCoBanBaoGomPhuCap = luongcobanbaogomphucap;
                decimal congtacxa = 0;
                decimal mucdattrongthang = 0;
                decimal luongtheodoanhthudoanhso = 0;
                double tongbunboc = 0;
                decimal thanhtienbunboc = 0;
                
                var logisticData = dbContext.LogisticEmployeeCongs.Find(m => m.EmployeeId.Equals(employeeId) & m.Year.Equals(salary.YearLogistic) & m.Month.Equals(salary.MonthLogistic)).FirstOrDefault();
                var saleData = dbContext.SaleKPIEmployees.Find(m => m.EmployeeId.Equals(employeeId) & m.Year.Equals(salary.YearSale) & m.Month.Equals(salary.MonthSale)).FirstOrDefault();
                if (logisticData != null)
                {
                    congtacxa = logisticData.CongTacXa;
                    tongbunboc = logisticData.KhoiLuongBun;
                    thanhtienbunboc = logisticData.ThanhTienBun;
                    if (logisticData.ChucVu == "Tài xế")
                    {
                        mucdattrongthang = logisticData.TongSoChuyen;
                        luongtheodoanhthudoanhso = logisticData.TienChuyen;
                    }
                    else
                    {
                        mucdattrongthang = logisticData.DoanhThu / 1000;
                        luongtheodoanhthudoanhso = logisticData.LuongTheoDoanhThuDoanhSo;
                    }
                }
                if (saleData != null)
                {
                    luongtheodoanhthudoanhso += saleData.TongThuong;
                }
                
                salary.CongTacXa = congtacxa;
                salary.MucDatTrongThang = mucdattrongthang;
                salary.LuongTheoDoanhThuDoanhSo = luongtheodoanhthudoanhso;
                salary.TongBunBoc = tongbunboc;
                salary.ThanhTienBunBoc = thanhtienbunboc;

                // Luong Khac
                decimal luongkhac = 0;
                salary.LuongKhac = luongkhac;
                // Thi dua
                decimal thidua = 0;
                salary.ThiDua = thidua;
                // Ho Tro Ngoai Luong
                decimal hotrongoailuong = 0;
                salary.HoTroNgoaiLuong = hotrongoailuong;
                //UAT
                salary.MauSo = 26;
                //END
                tongthunhapVP = Convert.ToDecimal((double)luongcobanbaogomphucap / salary.MauSo * (salary.NgayCongLamViec 
                                    + salary.CongCNGio / 8 * 2 
                                    + salary.CongTangCaNgayThuongGio / 8 * 1.5 
                                    + salary.CongLeTet * 3)
                                    + (double)salary.LuongCanBan / salary.MauSo * (salary.NgayNghiPhepNam + salary.NgayNghiLeTetHuongLuong)
                                    + (double)congtacxa 
                                    + (double)salary.DienThoai
                                    + (double)luongtheodoanhthudoanhso 
                                    + (double)thanhtienbunboc 
                                    + (double)luongkhac 
                                    + (double)thidua 
                                    + (double)hotrongoailuong);
                if (logisticData != null && logisticData.ChucVu != "Tài xế")
                {
                    tongthunhapVP = tongthunhapVP + mucdattrongthang;
                }
            }
            #endregion

            decimal luongVuotDinhMuc = congTong - luongDinhMuc;
            if (luongVuotDinhMuc < 0)
            {
                luongVuotDinhMuc = 0;
            }
            salary.LuongVuotDinhMuc = luongVuotDinhMuc;
            decimal tongthunhap = thanhTienLuongCanBan + tongPhuCap + luongVuotDinhMuc;
            if (salary.Type == (int)ESalaryType.VP)
            {
                tongthunhap = tongthunhapVP;
            }
            salary.TongThuNhap = tongthunhap;

            decimal tamung = 0;
            var credits = dbContext.CreditEmployees.Find(m => m.EmployeeId.Equals(employeeId) && m.Month.Equals(month) && m.Year.Equals(year)).ToList();
            if (credits != null && credits.Count > 0)
            {
                foreach (var credit in credits)
                {
                    tamung += credit.Money;
                }
            }
            salary.TamUng = tamung;
            decimal thuongletet = salary.ThuongLeTet;
            salary.ThuongLeTet = thuongletet;
            decimal luongthamgiabhxh = salary.LuongThamGiaBHXH;
            salary.LuongThamGiaBHXH = luongthamgiabhxh;
            salary.BHXH = Convert.ToDecimal((double)luongthamgiabhxh * 0.08);
            salary.BHYT = Convert.ToDecimal((double)luongthamgiabhxh * 0.015);
            salary.BHTN = Convert.ToDecimal((double)luongthamgiabhxh * 0.01);
            decimal bhxhbhyt = Convert.ToDecimal((double)luongthamgiabhxh * 0.105);
            salary.BHXHBHYT = bhxhbhyt;

            decimal thuclanh = tongthunhap - tamung + thuongletet - bhxhbhyt;
            salary.ThucLanh = thuclanh;
            decimal thucLanhTronSo = (Math.Round(thuclanh / 10000) * 10000);
            if (salary.Type == (int)ESalaryType.VP)
            {
                thucLanhTronSo = Constants.RoundOff(thuclanh);
            }
            salary.ThucLanhTronSo = thucLanhTronSo;

            // Update current. use other
            var builderSalary = Builders<SalaryEmployeeMonth>.Filter;
            var filterSalary = builderSalary.Eq(m => m.Id, salary.Id);
            var updateSalary = Builders<SalaryEmployeeMonth>.Update
                        .Set(m => m.LuongDinhMuc, salary.LuongDinhMuc)
                        .Set(m => m.ThanhTienLuongCanBan, salary.ThanhTienLuongCanBan)
                        .Set(m => m.LuongVuotDinhMuc, salary.LuongVuotDinhMuc)
                        .Set(m => m.PhuCapChuyenCan, salary.PhuCapChuyenCan)
                        .Set(m => m.PhuCapKhac, salary.PhuCapKhac)
                        .Set(m => m.TongPhuCap, salary.TongPhuCap)
                        .Set(m => m.ThucLanhTronSo, salary.ThucLanhTronSo)
                        .Set(m => m.ComSX, salary.ComSX)
                        .Set(m => m.ComNM, salary.ComNM)
                        .Set(m => m.ComKD, salary.ComKD)
                        .Set(m => m.ComVP, salary.ComVP)
                        .Set(m => m.LuongCoBanBaoGomPhuCap, salary.LuongCoBanBaoGomPhuCap)
                        .Set(m => m.NgayCongLamViec, salary.NgayCongLamViec)
                        .Set(m => m.NgayNghiPhepNam, salary.NgayNghiPhepNam)
                        .Set(m => m.NgayNghiPhepHuongLuong, salary.NgayNghiPhepHuongLuong)
                        .Set(m => m.NgayNghiLeTetHuongLuong, salary.NgayNghiLeTetHuongLuong)
                        .Set(m => m.CongCNGio, salary.CongCNGio)
                        .Set(m => m.CongTangCaNgayThuongGio, salary.CongTangCaNgayThuongGio)
                        .Set(m => m.CongLeTet, salary.CongLeTet)
                        .Set(m => m.TienPhepNamLeTet, salary.TienPhepNamLeTet)
                        .Set(m => m.YearLogistic, salary.YearLogistic)
                        .Set(m => m.MonthLogistic, salary.MonthLogistic)
                        .Set(m => m.YearSale, salary.YearSale)
                        .Set(m => m.MonthSale, salary.MonthSale)
                        .Set(m => m.CongTacXa, salary.CongTacXa)
                        .Set(m => m.MucDatTrongThang, salary.MucDatTrongThang)
                        .Set(m => m.LuongTheoDoanhThuDoanhSo, salary.LuongTheoDoanhThuDoanhSo)
                        .Set(m => m.TongBunBoc, salary.TongBunBoc)
                        .Set(m => m.ThanhTienBunBoc, salary.ThanhTienBunBoc)
                        .Set(m => m.LuongKhac, salary.LuongKhac)
                        .Set(m => m.ThiDua, salary.ThiDua)
                        .Set(m => m.HoTroNgoaiLuong, salary.HoTroNgoaiLuong)
                        .Set(m => m.ThuNhap, salary.ThuNhap)
                        .Set(m => m.TongThuNhap, salary.TongThuNhap)
                        .Set(m => m.BHXH, salary.BHXH)
                        .Set(m => m.BHYT, salary.BHYT)
                        .Set(m => m.BHTN, salary.BHTN)
                        .Set(m => m.BHXHBHYT, salary.BHXHBHYT)
                        .Set(m => m.TamUng, salary.TamUng)
                        .Set(m => m.ThuongLeTet, salary.ThuongLeTet)
                        .Set(m => m.ThucLanh, salary.ThucLanh)
                        .Set(m => m.UpdatedOn, DateTime.Now);
            dbContext.SalaryEmployeeMonths.UpdateOne(filterSalary, updateSalary);
            return salary;
        }

        public static SalaryMucLuongVung SalaryMucLuongVung(int month, int year)
        {
            var result = new SalaryMucLuongVung();
            result = dbContext.SalaryMucLuongVungs.Find(m => m.Enable.Equals(true) && m.Month.Equals(month) && m.Year.Equals(year)).FirstOrDefault();
            if (result == null)
            {
                var lastItemVung = dbContext.SalaryMucLuongVungs.Find(m => m.Enable.Equals(true)).SortByDescending(m => m.Year).SortByDescending(m => m.Month).FirstOrDefault();
                var lastMonthVung = lastItemVung.Month;
                var lastYearVung = lastItemVung.Year;
                lastItemVung.Id = null;
                lastItemVung.Month = month;
                lastItemVung.Year = year;
                dbContext.SalaryMucLuongVungs.InsertOne(lastItemVung);
                result = dbContext.SalaryMucLuongVungs.Find(m => m.Enable.Equals(true) && m.Month.Equals(month) && m.Year.Equals(year)).FirstOrDefault();
            }
            return result;
        }

        public static decimal GetLuongCanBan(string code, int level)
        {
            decimal result = 0;
            var lastest = dbContext.NgachLuongs.Find(m => m.Enable.Equals(true) && m.Law.Equals(false)
                                && m.MaSo.Equals(code) && m.Bac.Equals(level))
                                .SortByDescending(m => m.Year).SortByDescending(m => m.Month).FirstOrDefault();
            if (lastest != null)
            {
                result = lastest.MucLuongThang;
            }
            return result;
        }

        public static decimal GetLuongCanBanVP(string chucvuId, int level)
        {
            decimal mucluong = 4013;
            double tile = 1;
            var lastest = dbContext.SalaryThangBangLuongs.Find(m => m.Enable.Equals(true) && m.Law.Equals(false)
                                && m.ViTriId.Equals(chucvuId))
                                .SortByDescending(m => m.Year).SortByDescending(m => m.Month).FirstOrDefault();
            if (lastest != null)
            {
                mucluong = lastest.MucLuong;
                tile = lastest.TiLe;
            }

            decimal result = Constants.RoundOff(mucluong);
            for (var iNo = 1; iNo <= level; iNo++)
            {
                if (iNo != 1)
                {
                    result = Convert.ToDecimal((double)result * tile);
                }
            }
            return Constants.RoundOff(result);
        }

        public static List<MonthYear> DllMonths()
        {
            var monthYears = new List<MonthYear>();
            var date = new DateTime(2018, 02, 01);
            var endDate = DateTime.Now;
            while (date.Year < endDate.Year || (date.Year == endDate.Year && date.Month <= endDate.Month))
            {
                monthYears.Add(new MonthYear
                {
                    Month = date.Month,
                    Year = date.Year
                });
                date = date.AddMonths(1);
            }
            if (endDate.Day > 25)
            {
                monthYears.Add(new MonthYear
                {
                    Month = endDate.AddMonths(1).Month,
                    Year = endDate.AddMonths(1).Year
                });
            }
            var sortTimes = monthYears.OrderByDescending(x => x.Month).OrderByDescending(x => x.Year).ToList();
            return sortTimes;
        }

        #region Rights
        public static bool IsRight(string userId, string role, int action)
        {
            // check system
            if (userId == Constants.System.accountId)
            {
                return true;
            }
            #region Filter
            var builder = Builders<RoleUser>.Filter;
            var filter = builder.Eq(m => m.User, userId);
            filter = filter & builder.Eq(m => m.Role, role);
            filter = filter & builder.Gte(m => m.Action, action);
            #endregion

            var item = dbContext.RoleUsers.Find(filter).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        private static readonly Random random = new Random((int)DateTime.Now.Ticks);
        private const string Chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        private const string CharsNoO0 = "abcdefghijklmnpqrstuvwxyz123456789";

        /// <summary>
        /// Generates a random string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Random(int length)
        {
            var stringChars = new char[length];

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = Chars[random.Next(Chars.Length)];
            }

            return new String(stringChars);
        }

        //Generate RandomNo
        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        /// <summary>
        /// Generates a random string without 'o' and '0'
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNoO0(int length)
        {
            var stringChars = new char[length];

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = CharsNoO0[random.Next(CharsNoO0.Length)];
            }

            return new String(stringChars);
        }

        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                            "đ",
                                            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                            "í","ì","ỉ","ĩ","ị",
                                            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                            "d",
                                            "e","e","e","e","e","e","e","e","e","e","e",
                                            "i","i","i","i","i",
                                            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                            "u","u","u","u","u","u","u","u","u","u","u",
                                            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                //text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i], arr2[i]);
            }

            #region Resolve error
            // Copy for text error above. No write. (because special character)
            string[] earr1 = new string[] { "á" };
            string[] earr2 = new string[] { "a" };
            for (int i = 0; i < earr1.Length; i++)
            {
                text = text.Replace(earr1[i], earr2[i]);
            }
            #endregion
            return text;
        }

        public static string EmailConvert(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.ToLower();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            text = regex.Replace(text, " ");
            text = NonUnicode(text);
            var inputs = text.Split(new string[] { " ", "-" },
                            StringSplitOptions.RemoveEmptyEntries).ToList();
            var last = inputs.Last();
            inputs.RemoveAt(inputs.Count - 1);
            var output = ".";
            foreach (var item in inputs)
            {
                output += item[0];
            }
            return last + output + Constants.MailExtension;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string ReadTextFile(string filePath)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets plain text from html text
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public static string HtmlToPlainText(string htmlText)
        {
            return string.IsNullOrWhiteSpace(htmlText) ? string.Empty : Regex.Replace(htmlText, "<[^>]*>", string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type">1)field 2)condition 3)prompt  </param>
        /// <returns>
        /// if no regexToMatch return NULL
        /// </returns>
        public static Hashtable FillAllReplacableFields(string content, string type)
        {
            Hashtable hstReplacableFields = null;
            hstReplacableFields = new Hashtable();
            //m_ListSeqOrder = new List<string>();
            //regexToMatch = "<" + type + ">[a-zA-Z0-9\\s<>/',:=-]+?" + "</" + type + ">"; //[field]Applicant Name[/field]
            var regexToMatch = @"\[" + type + @"\][^#]+?" + @"\[/" + type + @"\]";

            var startIndex = type.Length + 2;
            if (regexToMatch == string.Empty) return hstReplacableFields;
            foreach (Match match in Regex.Matches(content, regexToMatch))
            {
                if (!hstReplacableFields.Contains(match.ToString()))
                {
                    int endIndex = match.ToString().Length - (type.Length + 3);
                    string result = match.ToString().Substring(startIndex, endIndex - startIndex);
                    hstReplacableFields.Add(match.ToString(), result); //key :[field]Applicant Name[/field]  value: Applicant name
                    //m_ListSeqOrder.Add(match.ToString());
                }
            }
            return hstReplacableFields;
        }

        /// <summary>
        /// This function returns the Calendar year on basis of month and year.
        /// </summary>
        public static int GetCalendarYearFromAcademicCycle(int year, int month)
        {
            int resultYear = 0;
            switch ((EMonths)month)
            {
                case EMonths.January:
                case EMonths.February:
                case EMonths.March:
                case EMonths.April:
                case EMonths.May:
                case EMonths.June:
                case EMonths.July:
                    resultYear = year + 1;
                    break;
                case EMonths.August:
                case EMonths.September:
                case EMonths.October:
                case EMonths.November:
                case EMonths.December:
                    resultYear = year;
                    break;
            }
            return resultYear;
        }

        public static string GetMonthStringByMonthNumber(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return string.Empty;
            }
        }

        public static bool IsDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date) || date == "null" || date.Length < 8) return false;

            try
            {
                Convert.ToDateTime(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetQueryInGivenCase(string caseString, string alias)
        {
            string value;
            caseString = caseString.Remove(0, alias.Length);
            if (caseString.ToLower() == caseString)
            {
                value = "LOWER(" + alias + caseString + ")";
            }
            else if (caseString.ToUpper() == caseString)
            {
                value = "UPPER(" + alias + caseString + ")";
            }
            else if (System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(caseString) == caseString)
            {
                value = "dbo.TitleCase(" + alias + caseString + ")";
            }
            else
            {
                //value = "dbo.TitleCase("  + alias + caseString + ")";
                value = alias + caseString;
            }

            return value;
        }

        /// <summary>
        /// This function returns the type of document like doc,pdf,jpg
        /// </summary>
        /// <param name="fileName">GetDocumentType</param>
        /// <returns>string</returns>
        public static string GetDocumentType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.Contains(".")) return string.Empty;
            return Path.GetExtension(fileName).ToUpper();
        }

        public static string TwoToFourLanguage(string text)
        {
            var cultureInfo = new CultureInfo(text);
            return cultureInfo.Name;
        }

        public static string NoUnicodeBlankConvert(string text)
        {
            text = NonUnicode(text).ToLower();

            return text.Replace(" ", "");
        }

        public static string AliasConvert(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.Trim().ToLower();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            text = regex.Replace(text, " ");
            text = NonUnicode(text);
            text = RemoveSpecialCharacters(text);
            return text;
        }

        public static string UpperCodeConvert(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.Trim().ToLower();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            text = regex.Replace(text, " ");
            text = NonUnicode(text);
            text = RemoveSpecialCharactersNear(text);
            return text.ToUpper();
        }
        //public static string RemoveSpecialCharacters(string str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (char c in str)
        //    {
        //        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
        //        {
        //            sb.Append(c);
        //        }
        //    }
        //    return sb.ToString();
        //}

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^0-9a-zA-Z]+", "-");
            //return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public static string RemoveSpecialCharactersNear(string str)
        {
            return Regex.Replace(str, "[^0-9a-zA-Z]+", "");
        }

        public static string LinkConvert(string text)
        {
            text = "/" + NonUnicode(text).ToLower() + "/";

            return text.Replace(" ", "-");
        }

        // CURRENT NOT WORKING
        public static string TranslateText(string input, string languagePair)
        {
            try
            {
                if (string.IsNullOrEmpty(languagePair))
                {
                    languagePair = "en";
                }
                string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);

                WebClient webClient = new WebClient
                {
                    Encoding = Encoding.UTF8
                };

                string result = webClient.DownloadString(url);

                result = result.Substring(result.IndexOf("<span title=\"") + "<span title=\"".Length);
                result = result.Substring(result.IndexOf(">") + 1);
                result = result.Substring(0, result.IndexOf("</span>"));

                return string.Empty;
                //return result.Trim();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static int BusinessDaysUntil(this DateTime fromDate,
                                    DateTime toDate,
                                    IEnumerable<DateTime> holidays = null)
        {
            int result = 0;

            for (DateTime date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
                if (!IsHoliday(date, holidays) && !IsSunday(date))
                    result += 1;

            return result;
        }

        public static double GetBussinessDaysBetweenTwoDates(DateTime start, DateTime end, TimeSpan workdayStartTime, TimeSpan workdayEndTime, IEnumerable<DateTime> holidays = null)
        {
            if (start > end)
            {
                return -1;
            }

            var startTime = start.TimeOfDay;
            var endTime = end.TimeOfDay;
            // If the start time is before the starting hours, set it to the starting hour.
            if (startTime < workdayStartTime) startTime = workdayStartTime;
            if (endTime > workdayEndTime) endTime = workdayEndTime;

            double bd = 0;
            double hour = 0;
            // Tính ngày theo giờ. 0.5 day < 4h ; 1 day > 4h
            if (start.Date.CompareTo(end.Date) == 0)
            {
                if (!IsHoliday(start, holidays) && !IsSunday(start))
                {
                    hour = (endTime - startTime).Hours - 1; // 1 h nghi trua
                    bd = hour <= 4 ? 0.5 : 1;
                }
            }
            else
            {
                for (DateTime d = start; d <= end; d = d.AddDays(1))
                {
                    if (d.Date.CompareTo(start.Date) == 0)
                    {
                        if (!IsHoliday(d, holidays) && !IsSunday(d))
                        {
                            hour = (workdayEndTime - d.TimeOfDay).Hours;
                            if (hour < 1)
                            {
                                bd += 0;
                            }
                            else
                            {
                                bd += hour <= 4 ? 0.5 : 1;
                            }
                        }
                    }
                    else if (d.Date.CompareTo(end.Date) == 0)
                    {
                        if (!IsHoliday(d, holidays) && !IsSunday(d))
                        {
                            hour = (endTime - workdayStartTime).Hours;
                            if (hour < 1)
                            {
                                bd += 0;
                            }
                            else
                            {
                                bd += hour <= 4 ? 0.5 : 1;
                            }
                        }
                    }
                    else
                    {
                        if (!IsHoliday(d, holidays) && !IsSunday(d))
                        {
                            ++bd;
                        }
                    }

                    // update start to start Working hour
                    // TimeSpan ts = new TimeSpan(10, 30, 0);
                    d = d.Date + workdayStartTime;
                }
            }

            return bd;
        }

        public static double GetHolidaysBetweenTwoDates(DateTime start, DateTime end, TimeSpan workdayStartTime, TimeSpan workdayEndTime, IEnumerable<DateTime> holidays = null)
        {
            if (start > end)
                return -1;

            var startTime = start.TimeOfDay;
            var endTime = end.TimeOfDay;
            // If the start time is before the starting hours, set it to the starting hour.
            if (startTime < workdayStartTime) startTime = workdayStartTime;
            if (endTime > workdayEndTime) endTime = workdayEndTime;

            double bd = 0;
            double hour = 0;
            // Tính ngày theo giờ. 0.5 day < 4h ; 1 day > 4h
            if (start.Date.CompareTo(end.Date) == 0)
            {
                if (IsHoliday(start, holidays))
                {
                    hour = (endTime - startTime).Hours;
                    bd = hour < 4 ? 0.5 : 1;
                }
            }
            else
            {
                for (DateTime d = start; d < end; d = d.AddDays(1))
                {
                    if (d.Date.CompareTo(start.Date) == 0)
                    {
                        if (IsHoliday(d, holidays))
                        {
                            hour = (workdayEndTime - d.TimeOfDay).Hours;
                            if (hour < 1)
                            {
                                bd += 0;
                            }
                            else
                            {
                                bd += hour <= 4 ? 0.5 : 1;
                            }
                        }
                    }
                    else if (d.Date.CompareTo(end.Date) == 0)
                    {
                        if (IsHoliday(start, holidays))
                        {
                            hour = (endTime - workdayStartTime).Hours;
                            if (hour < 1)
                            {
                                bd += 0;
                            }
                            else
                            {
                                bd += hour <= 4 ? 0.5 : 1;
                            }
                        }
                    }
                    else
                    {
                        if (IsHoliday(d, holidays))
                        {
                            ++bd;
                        }
                    }

                    // update start to start Working hour
                    // TimeSpan ts = new TimeSpan(10, 30, 0);
                    d = d.Date + workdayStartTime;
                }
            }

            return bd;
        }

        private static Boolean IsSunday(DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Sunday;
        }

        private static Boolean IsHoliday(DateTime value, IEnumerable<DateTime> holidays = null)
        {
            if (null == holidays)
                holidays = VietNamHolidays;

            return holidays.Any(holiday => holiday.Day == value.Day &&
                                            holiday.Month == value.Month);
        }

        private static Boolean IsHolidaySunday(DateTime value, IEnumerable<DateTime> holidays = null)
        {
            if (null == holidays)
                holidays = VietNamHolidays;

            return (value.DayOfWeek == DayOfWeek.Sunday) ||
                    //(value.DayOfWeek == DayOfWeek.Saturday) ||
                    holidays.Any(holiday => holiday.Day == value.Day &&
                                            holiday.Month == value.Month);
        }

        private static readonly List<DateTime> VietNamHolidays = new List<DateTime>() {
          new DateTime(1, 1, 1) //New Year Day
        };

        public static int GetDaysUntilBirthday(DateTime birthday)
        {
            var nextBirthday = birthday.AddYears(DateTime.Today.Year - birthday.Year);
            if (nextBirthday < DateTime.Today)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }
            return (nextBirthday - DateTime.Today).Days;
        }

        public static string GetMonthsDaysUntilBirthday(DateTime birthday)
        {
            DateTime today = DateTime.Today;
            int months = 0;
            int days = 0;

            DateTime nextBirthday = birthday.AddYears(today.Year - birthday.Year);
            if (nextBirthday < today)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }

            while (today.AddMonths(months + 1) <= nextBirthday)
            {
                months++;
            }
            days = nextBirthday.Subtract(today.AddMonths(months)).Days;

            return string.Format("Next birthday is in {0} month(s) and {1} day(s).", months, days);
        }

        public static DateTime WorkingMonthToDate(string times)
        {
            if (string.IsNullOrEmpty(times))
            {
                var now = DateTime.Now;
                times = now.Month + "-" + now.Year;
                if (now.Day < 26)
                {
                    var lastMonth = now.AddMonths(-1);
                    times = lastMonth.Month + "-" + lastMonth.Year;
                }
            }
            int month = Convert.ToInt32(times.Split("-")[0]);
            int year = Convert.ToInt32(times.Split("-")[1]);
            return new DateTime(year, month, 25);
        }

        public static DateTime GetToDate(string thang)
        {
            if (string.IsNullOrEmpty(thang))
            {
                var today = DateTime.Now;
                return today.Day > 25 ? new DateTime(today.AddMonths(1).Year, today.AddMonths(1).Month, 25) : new DateTime(today.Year, today.Month, 25);
            }

            int month = Convert.ToInt32(thang.Split("-")[0]);
            int year = Convert.ToInt32(thang.Split("-")[1]);
            return new DateTime(year, month, 25);
        }

        public static DateTime GetSalaryToDate(string thang)
        {
            if (string.IsNullOrEmpty(thang))
            {
                var today = DateTime.Now;
                return today.Day > 25 ? new DateTime(today.Year, today.Month, 25) : new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 25);
            }

            int month = Convert.ToInt32(thang.Split("-")[0]);
            int year = Convert.ToInt32(thang.Split("-")[1]);
            return new DateTime(year, month, 25);
        }

        public static DateTime EndWorkingMonthByDate(DateTime? date)
        {
            var dateHere = DateTime.Now;
            if (date.HasValue)
            {
                dateHere = date.Value;
            }
            // calculator date: 26 - > 25
            // now: 25/08 => [from] times: -> 25/08
            // now: 26/08 => [from] times: -> 25/09
            // now: 01/09 => [from] times: -> 25/09
            // now: 24/09 => [from] times: -> 25/09
            var times = dateHere.Month + "-" + dateHere.Year;
            if (dateHere.Day > 25)
            {
                var lastMonth = dateHere.AddMonths(-1);
                times = lastMonth.Month + "-" + lastMonth.Year;
            }
            int month = Convert.ToInt32(times.Split("-")[0]);
            int year = Convert.ToInt32(times.Split("-")[1]);
            return new DateTime(year, month, 25);
        }

        public static string TruncateLongString(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.Substring(0, Math.Min(str.Length, maxLength)) + "...";
        }

        public static int ClosestTo(this IEnumerable<int> collection, int target)
        {
            // NB Method will return int.MaxValue for a sequence containing no elements.
            // Apply any defensive coding here as necessary.
            var closest = int.MaxValue;
            var minDifference = int.MaxValue;
            foreach (var element in collection)
            {
                var difference = Math.Abs((long)element - target);
                if (minDifference > difference)
                {
                    minDifference = (int)difference;
                    closest = element;
                }
            }

            return closest;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        #region FACTORY
        public static string NoPhieuInCa(DateTime date, string xe)
        {
            if (string.IsNullOrEmpty(xe))
            {
                return "";
            }

            int month = date.Month;
            int year = date.Year;
            // check db if exist date vs xe
            var phieu = dbContext.FactoryVanHanhs.Find(m => m.XeCoGioiMayAlias.Equals(xe) && m.Date.Equals(date.Date)).FirstOrDefault();
            if (phieu != null)
            {
                return phieu.PhieuInCa;
            }
            // [no] increase by month.
            if (dbContext.FactoryVanHanhs.CountDocuments(m => m.Month.Equals(month)) > 0)
            {
                var max = dbContext.FactoryVanHanhs.Find(m => m.Year.Equals(year) && m.Month.Equals(month) && !m.PhieuInCa.Equals("")).SortByDescending(m => m.CreatedOn).First();
                return year + month.ToString("D2") + "-" + (Convert.ToInt32(max.PhieuInCa.Split("-")[1]) + 1).ToString("D4");
            }
            return year + month.ToString("D2") + "-" + 1.ToString("D4");
        }
        #endregion

        public static IEnumerable<string> EnumeratePropertyDifferences<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> changes = new List<string>();


            return changes;
        }

        public static List<Variance> DetailedCompare<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<Variance> variances = new List<Variance>();

            var outs = new List<string>{
                    "Id",
                    "EmployeeId",
                    "Timestamp",
                    "CreatedOn",
                    "UpdatedOn",
                    "CheckedOn",
                    "ApprovedOn",
                    "CreatedBy",
                    "UpdatedBy",
                    "CheckedBy",
                    "ApprovedBy"};

            foreach (PropertyInfo pi in properties)
            {
                // not compare field
                if (!outs.Any(s => pi.Name.Contains(s)))
                {
                    object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                    object value2 = typeof(T).GetProperty(pi.Name).GetValue(obj2, null);
                    if (!string.IsNullOrEmpty(value2.ToString()))
                    {
                        Type type = pi.PropertyType;
                        if (type.Namespace == "System.Collections.Generic")
                        {
                            var a = (IList)value1;
                            var b = (IList)value2;
                            var i = 0;
                            foreach (var item in a)
                            {
                                var otherItem = b[i];
                                var newDiffe = item.ChildCompare(otherItem);
                                i++;
                            }
                        }
                        else
                        {
                            if (value1 != value2 && (value1 == null || !value1.Equals(value2)))
                            {
                                variances.Add(new Variance
                                {
                                    Prop = pi.Name,
                                    ValA = value1,
                                    ValB = value2
                                });
                            }
                        }
                    }
                }
            }
            return variances;
        }

        public static List<Variance> ChildCompare<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<Variance> variances = new List<Variance>();

            var outs = new List<string>{
                    "Id",
                    "EmployeeId",
                    "Timestamp",
                    "CreatedOn",
                    "UpdatedOn",
                    "CheckedOn",
                    "ApprovedOn",
                    "CreatedBy",
                    "UpdatedBy",
                    "CheckedBy",
                    "ApprovedBy"};

            foreach (PropertyInfo pi in properties)
            {
                // not compare field
                if (!outs.Any(s => pi.Name.Contains(s)))
                {
                    object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                    object value2 = typeof(T).GetProperty(pi.Name).GetValue(obj2, null);
                    if (value2 != null)
                    {
                        if (value1 != value2 && (value1 == null || !value1.Equals(value2)))
                        {
                            variances.Add(new Variance
                            {
                                Prop = pi.Name,
                                ValA = value1,
                                ValB = value2
                            });
                        }
                    }
                }
            }
            return variances;
        }

        public static int GetYearAge(DateTime fromdate)
        {
            var today = DateTime.Now;
            var age = today.Year - fromdate.Year;
            // Go back to the year the person was born in case of a leap year
            if (fromdate > today.AddYears(-age)) age--;

            return age;
        }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            DateTimeSpan span = new DateTimeSpan();
            int officialDay = current.Day;

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                current = current.AddDays(officialDay - current.Day);
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }


        #region EXCEL
        public static string GetFormattedCellValue(ICell cell)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue.Trim();

                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            //DateTime date = cell.DateCellValue;
                            //ICellStyle style = cell.CellStyle;
                            //// Excel uses lowercase m for month whereas .Net uses uppercase
                            //string format = style.GetDataFormatString().Replace('m', 'M');
                            //string format = "dd/MM/yyyy hh:mm:ss";
                            //return date.ToString(format);
                            return cell.DateCellValue.ToString().Trim();
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString().Trim();
                        }

                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "TRUE" : "FALSE";

                    case CellType.Formula:
                        switch (cell.CachedFormulaResultType)
                        {
                            case CellType.String:
                                return cell.StringCellValue.Trim();
                            case CellType.Boolean:
                                return cell.BooleanCellValue ? "TRUE" : "FALSE";
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    DateTime date = cell.DateCellValue;
                                    ICellStyle style = cell.CellStyle;
                                    // Excel uses lowercase m for month whereas .Net uses uppercase
                                    string format = style.GetDataFormatString().Replace('m', 'M');
                                    return date.ToString(format);
                                }
                                else
                                {
                                    return cell.NumericCellValue.ToString().Trim();
                                }
                        }
                        return cell.CellFormula.Trim();

                        //case CellType.Error:
                        //    return FormulaError.ForInt(cell.ErrorCellValue).String;
                }
            }
            // null or blank cell, or unknown cell type
            return string.Empty;
        }

        public static string GetFormattedCellValue2(ICell cell, string format)
        {
            if (cell != null)
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue;

                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            //DateTime date = cell.DateCellValue;
                            //ICellStyle style = cell.CellStyle;
                            //// Excel uses lowercase m for month whereas .Net uses uppercase
                            //string format = style.GetDataFormatString().Replace('m', 'M');
                            //string format = "dd/MM/yyyy hh:mm:ss";
                            //return date.ToString(format);
                            return cell.DateCellValue.ToString(format);
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString();
                        }

                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "TRUE" : "FALSE";

                    case CellType.Formula:
                        switch (cell.CachedFormulaResultType)
                        {
                            case CellType.String:
                                return cell.StringCellValue;
                            case CellType.Boolean:
                                return cell.BooleanCellValue ? "TRUE" : "FALSE";
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    DateTime date = cell.DateCellValue;
                                    ICellStyle style = cell.CellStyle;
                                    // Excel uses lowercase m for month whereas .Net uses uppercase
                                    format = style.GetDataFormatString().Replace('m', 'M');
                                    return date.ToString(format);
                                }
                                else
                                {
                                    return cell.NumericCellValue.ToString();
                                }
                        }
                        return cell.CellFormula;

                        //case CellType.Error:
                        //    return FormulaError.ForInt(cell.ErrorCellValue).String;
                }
            }
            // null or blank cell, or unknown cell type
            return string.Empty;
        }

        public static DateTime GetDateCellValue(ICell cell)
        {
            if (cell != null)
            {
                if (cell.CellType == CellType.Numeric)
                {
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    else
                    {
                        return DateTime.FromOADate(cell.NumericCellValue);
                    }
                }
                else if (cell.CellType == CellType.Formula)
                {
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                }
                else if (cell.CellType == CellType.String)
                {
                    try
                    {
                        return DateTime.ParseExact(cell.StringCellValue, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            return DateTime.ParseExact(cell.StringCellValue, "M/d/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex2)
                        {
                            try
                            {
                                return DateTime.ParseExact(cell.StringCellValue, "M/dd/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex3)
                            {
                                return DateTime.Now;
                            }
                        }
                    }

                }
            }
            // null or blank cell, or unknown cell type
            return DateTime.Now;
        }

        public static DateTime? GetDateCellValue2(ICell cell)
        {
            if (cell != null)
            {
                if (cell.CellType == CellType.Numeric)
                {
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    else
                    {
                        return DateTime.FromOADate(cell.NumericCellValue);
                    }
                }
            }
            return null;
        }

        public static double GetNumbericCellValue(ICell cell)
        {
            if (cell != null)
            {
                if (cell.CellType == CellType.Numeric)
                {
                    return cell.NumericCellValue;
                }
                if (cell.CellType == CellType.Formula)
                {
                    return cell.NumericCellValue;
                }
            }
            return 0;
        }

        public static DateTime ParseExcelDate(string date)
        {
            if (DateTime.TryParse(date, out DateTime dt))
            {
                return dt;
            }

            return double.TryParse(date, out double oaDate) ? DateTime.FromOADate(oaDate) : DateTime.MinValue;
        }
        #endregion
    }
}