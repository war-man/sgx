﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    // Mục đích lấy dữ liệu nhanh, phục vụ tính lương, danh sách, report,...
    // Lay du lieu cua 1 nhan vien base EmployeeId.
    // Lay du lieu va kiem tra tung location theo enrollNumber.
    // Vd: nhan vien A làm việc 2 nơi: VP, NM thì mỗi tháng sẽ có 2 records. 1 nm, 1 vp. base enrollNumber.
    public class EmployeeWorkTimeMonthLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        #region Employee
        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string Department { get; set; } // PhongBan

        public string DepartmentId { get; set; } // PhongBanId

        public string DepartmentAlias { get; set; } // PhongBanId

        public string Part { get; set; } // BoPhan

        public string PartId { get; set; } // BoPhanId

        public string PartAlias { get; set; } // BoPhanId

        public string Title { get; set; } // ChucVu

        public string TitleId { get; set; } // ChucVuId

        public string TitleAlias { get; set; } // ChucVuId

        public string EnrollNumber { get; set; }

        public string WorkplaceCode { get; set; }
        #endregion

        public int Year { get; set; } = DateTime.Now.Year;

        public int Month { get; set; } = DateTime.Now.Month;

        public double Workday { get; set; }

        // store miliseconds
        public double WorkTime { get; set; } = 0;

        public double CongCNGio { get; set; } = 0;

        public double CongTangCaNgayThuongGio { get; set; } = 0;

        public double CongLeTet { get; set; } = 0;

        public double Late { get; set; }

        public double Early { get; set; }

        public double NghiPhepNam { get; set; } = 0;

        public double NghiViecRieng { get; set; } = 0;

        public double NghiBenh { get; set; } = 0;

        public double NghiKhongPhep { get; set; } = 0;

        // thai san, dam cuoi
        public double NghiHuongLuong { get; set; } = 0;

        public double NghiLe { get; set; } = 0;

        public double KhongChamCong { get; set; } = 0;

        public double ChuNhat { get; set; } = 0;

        public string Rule { get; set; } = "26-25";

        #region Dieu Chinh Tay (Lấy dữ liệu ở đây)
        public double NgayLamViecChinhTay { get; set; }
        public double PhepNamChinhTay { get; set; }
        public double LeTetChinhTay { get; set; }
        #endregion

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
