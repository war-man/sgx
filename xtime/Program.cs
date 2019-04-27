﻿using Common.Enums;
using Common.Utilities;
using Data;
using MimeKit;
using Models;
using MongoDB.Driver;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Configuration;

namespace xtime
{
    class Program
    {
        static void Main(string[] args)
        {
            #region setting
            var location = ConfigurationSettings.AppSettings.Get("location").ToString();
            var modeData = ConfigurationSettings.AppSettings.Get("modeData").ToString() == "1" ? true : false; // true: Get all data | false get by date
            var day = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("day").ToString());
            var isMail = ConfigurationSettings.AppSettings.Get("isMail").ToString() == "1" ? true : false;
            var debug = ConfigurationSettings.AppSettings.Get("debug").ToString() == "1" ? true : false;
            var connection = "mongodb://localhost:27017";
            var database = "tribat";
            #endregion

            UpdateTimeKeeper(location, modeData, day, isMail, connection, database, debug);
        }

        static void UpdateTimeKeeper(string location, bool modeData, int day, bool isMail, string connection, string database, bool debug)
        {
            #region Connection, Setting & Filter
            MongoDBContext.ConnectionString = connection;
            MongoDBContext.DatabaseName = database;
            MongoDBContext.IsSSL = true;
            MongoDBContext dbContext = new MongoDBContext();

            var dateCrawled = DateTime.Now.AddDays(day);
            var builder = Builders<AttLog>.Filter;
            var filter = builder.Gt(m => m.Date, dateCrawled.AddDays(-1));

            if (debug)
            {
                filter = filter & builder.Eq(m => m.EnrollNumber,Convert.ToInt32(ConfigurationSettings.AppSettings.Get("debugString")).ToString());
            }
            #endregion

            if (modeData)
            {
                // remove CN, Leave, Holiday
                dbContext.EmployeeWorkTimeLogs.DeleteMany(m => string.IsNullOrEmpty(m.SecureCode) && m.WorkplaceCode.Equals(location));
                var statusRemove = new List<int>()
                {
                    (int)EStatusWork.XacNhanCong,
                    (int)EStatusWork.DuCong,
                    (int)EStatusWork.Wait
                };
                dbContext.EmployeeWorkTimeLogs.DeleteMany(m => true && statusRemove.Contains(m.Status) && m.WorkplaceCode.Equals(location));

                dbContext.EmployeeWorkTimeMonthLogs.DeleteMany(m => m.WorkplaceCode.Equals(location));
            }

            var attlogs = new List<AttLog>();
            if (location == "NM")
            {
                attlogs = modeData ? dbContext.X928CNMAttLogs.Find(m => true).ToList() : dbContext.X928CNMAttLogs.Find(filter).ToList();
            }
            else
            {
                attlogs = modeData ? dbContext.X628CVPAttLogs.Find(m => true).ToList() : dbContext.X628CVPAttLogs.Find(filter).ToList();
            }

            Proccess(dbContext, location, modeData, day, isMail, attlogs, debug);
        }

        private static void Proccess(MongoDBContext dbContext, string location, bool modeData, int day, bool isMail, List<AttLog> attlogs, bool debug)
        {
            #region Config
            var linkChamCong = Constants.System.domain + "/" + Constants.LinkTimeKeeper.Main + "/" + Constants.LinkTimeKeeper.Index;
            var dateNewPolicy = new DateTime(2018, 10, 01);
            var lunch = TimeSpan.FromHours(1);
            var now = DateTime.Now.Date;
            var startWorkingScheduleTime = new TimeSpan(7, 30, 0);
            var endWorkingScheduleTime = new TimeSpan(16, 30, 0);
            #endregion

            var holidays = dbContext.Holidays.Find(m => m.Enable.Equals(true)).ToEnumerable().Where(m => m.Year.Equals(DateTime.Now.Year)).ToList();

            var leaveTypes = dbContext.LeaveTypes.Find(m => m.Enable.Equals(true) && m.Display.Equals(true)).ToList();

            var times = (from p in attlogs
                         group p by new
                         {
                             p.EnrollNumber,
                             p.Date.Date
                         }
                              into d
                         select new
                         {
                             d.Key.Date,
                             groupDate = d.Key.Date.ToString("yyyy-MM-dd"),
                             groupCode = d.Key.EnrollNumber,
                             count = d.Count(),
                             times = d.ToList()
                         }).OrderBy(m => m.Date).ToList();

            var today = DateTime.Now.Date;
            var endDay = today.Day > 25 ? new DateTime(today.AddMonths(1).Year, today.AddMonths(1).Month, 25) : new DateTime(today.Year, today.Month, 25);

            var filterEmp = Builders<Employee>.Filter.Eq(m => m.Enable, true) & Builders<Employee>.Filter.Eq(m => m.Leave, false)
                    & Builders<Employee>.Filter.ElemMatch(z => z.Workplaces, a => a.Code == location);
            var employees = dbContext.Employees.Find(filterEmp).ToList();
            if (debug)
            {
                var debugString = ConfigurationSettings.AppSettings.Get("debugString").ToString();
                employees = dbContext.Employees.Find(m => m.Workplaces.Any(w => w.Code.Equals(location) && w.Fingerprint.Equals(debugString))).ToList();
            }

            // FUTURE SET CAP BAC. QUÉT HẾT CHỪA BGĐ....

            foreach (var employee in employees)
            {
                Console.WriteLine("Employee Name: " + employee.FullName);
                var employeeLocation = employee.Workplaces.FirstOrDefault(a => a.Code == location);
                if (string.IsNullOrEmpty(employeeLocation.Fingerprint))
                {
                    continue;
                }
                var employeeId = employee.Id;
                var employeeFinger = employeeLocation.Fingerprint;
                var fingerInt = employeeFinger != null ? Convert.ToInt32(employeeFinger) : 0;
                if (!string.IsNullOrEmpty(employeeLocation.WorkingScheduleTime))
                {
                    startWorkingScheduleTime = TimeSpan.Parse(employeeLocation.WorkingScheduleTime.Split('-')[0].Trim());
                    endWorkingScheduleTime = TimeSpan.Parse(employeeLocation.WorkingScheduleTime.Split('-')[1].Trim());
                }
                var email = employee.Email;
                var fullName = employee.FullName;
                var linkFinger = linkChamCong + employee.Id;

                var leaves = dbContext.Leaves.Find(m => m.EmployeeId.Equals(employeeId)).ToList();

                var startDate = today.AddDays(day);
                if (modeData)
                {
                    startDate = today.AddMonths(-4);
                }
                var conditionDate = startDate;

                Console.WriteLine("End date +: " + endDay);
                #region PROCESS
                while (startDate <= endDay)
                {
                    Console.WriteLine("Start date: " + startDate);
                    int year = startDate.Day > 25 ? startDate.AddMonths(1).Year : startDate.Year;
                    int month = startDate.Day > 25 ? startDate.AddMonths(1).Month : startDate.Month;
                    var endDateMonth = new DateTime(year, month, 25);
                    var startDateMonth = endDateMonth.AddMonths(-1).AddDays(1);

                    // Phan tich truong hop ca dem. Vd: ca 1: 6h-14h ; 14h-22h ; 22h-6h
                    // Later
                    var timekeepings = times.Where(m => m.groupCode.Equals(fingerInt.ToString()) && m.Date >= startDateMonth && m.Date <= endDateMonth).OrderBy(m => m.Date).ToList();
                    for (DateTime date = startDateMonth; date <= endDateMonth; date = date.AddDays(1))
                    {
                        Console.WriteLine("Date: " + date);
                        if (!modeData)
                        {
                            if (date < conditionDate)
                            {
                                continue;
                            }
                        }
                        if (date > today)
                        {
                            continue;
                        }

                        var employeeWorkTimeLog = new EmployeeWorkTimeLog
                        {
                            EmployeeId = employee.Id,
                            EmployeeName = employee.FullName,
                            Department = employee.PhongBanName,
                            DepartmentId = employee.PhongBan,
                            DepartmentAlias = Utility.AliasConvert(employee.PhongBanName),
                            Part = employee.BoPhanName,
                            PartId = employee.BoPhan,
                            PartAlias = Utility.AliasConvert(employee.BoPhanName),
                            EmployeeTitle = employee.ChucVuName,
                            EmployeeTitleId = employee.ChucVu,
                            EmployeeTitleAlias = Utility.AliasConvert(employee.ChucVuName),
                            EnrollNumber = employeeFinger,
                            WorkplaceCode = location,
                            Year = year,
                            Month = month,
                            Date = date,
                            Workcode = employee.SalaryType,
                            Start = startWorkingScheduleTime,
                            End = endWorkingScheduleTime,
                            Mode = (int)ETimeWork.Normal
                        };
                        var holiday = holidays.Where(m => m.Date.Equals(date)).FirstOrDefault();
                        var existLeave = leaves.FirstOrDefault(item => item.Enable.Equals(true) 
                                                && date >= item.From.Date 
                                                && date <= item.To.Date);

                        if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            employeeWorkTimeLog.Mode = (int)ETimeWork.Sunday;
                            employeeWorkTimeLog.Reason = "Chủ nhật";
                            // ChuNhat++;
                        }

                        if (holiday != null)
                        {
                            employeeWorkTimeLog.Mode = (int)ETimeWork.Holiday;
                            employeeWorkTimeLog.Reason = holiday.Name;
                            employeeWorkTimeLog.ReasonDetail = holiday.Detail;
                            //NghiLe++;
                        }

                        if (existLeave != null)
                        {
                            double numberLeave = (double)existLeave.Number;

                            var leaveType = leaveTypes.Where(m => m.Id.Equals(existLeave.TypeId)).FirstOrDefault();
                            if (leaveType != null)
                            {
                                if (leaveType.Alias == "phep-nam")
                                {
                                    //NghiPhepNam += (double)numberLeave;
                                    employeeWorkTimeLog.Mode = (int)ETimeWork.LeavePhep;
                                }
                                else if (leaveType.Alias == "phep-khong-huong-luong")
                                {
                                    //NghiViecRieng += (double)numberLeave;
                                    employeeWorkTimeLog.Mode = (int)ETimeWork.LeaveKhongHuongLuong;
                                }
                                else if (leaveType.Alias == "nghi-huong-luong")
                                {
                                    //NghiHuongLuong += (double)numberLeave;
                                    employeeWorkTimeLog.Mode = (int)ETimeWork.LeaveHuongLuong;
                                }
                                else if (leaveType.Alias == "nghi-bu")
                                {
                                    // do later
                                }
                            }

                            employeeWorkTimeLog.SoNgayNghi = 1;
                            // Check off 0.5
                            var leaveFrom = existLeave.From.Date.Add(existLeave.Start);
                            var leaveTo = existLeave.To.Date.Add(existLeave.End);
                            for(DateTime dateL = leaveFrom; dateL <= leaveTo; dateL = dateL.Date.AddDays(1))
                            {
                                if (date == dateL.Date)
                                {
                                    var leaveTimeSpan = endWorkingScheduleTime - dateL.Date.Add(startWorkingScheduleTime).TimeOfDay;
                                    if (dateL == leaveFrom)
                                    {
                                        leaveTimeSpan = endWorkingScheduleTime - dateL.TimeOfDay;
                                    }
                                    if (dateL == leaveTo.Date)
                                    {
                                        leaveTimeSpan = leaveTo.TimeOfDay - startWorkingScheduleTime;
                                    }
                                    if (leaveTimeSpan.TotalHours <= 4)
                                    {
                                        employeeWorkTimeLog.SoNgayNghi = 0.5;
                                    }
                                }
                            }

                            employeeWorkTimeLog.Reason = existLeave.TypeName;
                            employeeWorkTimeLog.ReasonDetail = Constants.StatusLeave(existLeave.Status);
                        }

                        var workTime = new TimeSpan(0);
                        var tangcathucte = new TimeSpan(0);
                        double workDay = 1;
                        var late = new TimeSpan(0);
                        var early = new TimeSpan(0);
                        //var status = (int)EStatusWork.DuCong;
                        //var statusLate = (int)EStatusWork.DuCong;
                        //var statusEarly = (int)EStatusWork.DuCong;

                        var timekeeping = timekeepings.FirstOrDefault(item => item.Date == date);
                        if (timekeeping == null)
                        {
                            if (employeeWorkTimeLog.Mode == (int)ETimeWork.Normal)
                            {
                                employeeWorkTimeLog.Status = (int)EStatusWork.XacNhanCong;
                            }
                        }
                        else
                        {
                            var records = timekeeping.times.OrderBy(m => m.Date).ToList();
                            #region Procees Times
                            var inLogTime = records.First().TimeOnlyRecord;
                            var outLogTime = records.Last().TimeOnlyRecord;

                            // Phan tich thoi gian lam viec
                            var workingArr = new int[] { startWorkingScheduleTime.Hours, endWorkingScheduleTime.Hours };
                            // phan tich
                            // nhieu gio vo <12
                            // nhieu gio sau > 13
                            // 1 records...
                            var incheck = workingArr.ClosestTo(inLogTime.Hours);
                            var outcheck = workingArr.ClosestTo(outLogTime.Hours);

                            if (incheck == outcheck)
                            {
                                if (incheck > startWorkingScheduleTime.Add(new TimeSpan(4, 0, 0)).Hours)
                                {
                                    workTime = outLogTime - endWorkingScheduleTime.Add(new TimeSpan(4, 0, 0));
                                }
                                else
                                {
                                    workTime = startWorkingScheduleTime.Add(new TimeSpan(4,0,0)) - inLogTime;
                                }

                                if (employeeWorkTimeLog.Mode != (int)ETimeWork.Normal)
                                {
                                    tangcathucte = workTime;
                                }
                                employeeWorkTimeLog.Status = (int)EStatusWork.XacNhanCong;
                                workDay = 0.5;
                                if (incheck == startWorkingScheduleTime.Hours)
                                {
                                    employeeWorkTimeLog.StatusEarly = (int)EStatusWork.XacNhanCong;
                                    if (inLogTime > startWorkingScheduleTime)
                                    {
                                        late = inLogTime - startWorkingScheduleTime;
                                        if (late.TotalMinutes < 1)
                                        {
                                            late = new TimeSpan(0);
                                        }
                                        if (late.TotalMinutes > 0)
                                        {
                                            employeeWorkTimeLog.StatusLate = (int)EStatusWork.XacNhanCong;
                                        }
                                        if (late.TotalMinutes > 15)
                                        {
                                            workDay = 0;
                                        }
                                    }
                                }
                                if (incheck == endWorkingScheduleTime.Hours)
                                {
                                    employeeWorkTimeLog.StatusLate = (int)EStatusWork.XacNhanCong;
                                    if (outLogTime < endWorkingScheduleTime)
                                    {
                                        early = endWorkingScheduleTime - outLogTime;
                                        if (early.TotalMinutes < 1)
                                        {
                                            early = new TimeSpan(0);
                                        }
                                        if (early.TotalMinutes > 0)
                                        {
                                            employeeWorkTimeLog.StatusEarly = (int)EStatusWork.XacNhanCong;
                                        }
                                        if (early.TotalMinutes > 15)
                                        {
                                            workDay = 0;
                                        }
                                    }
                                }

                                if (employeeWorkTimeLog.SoNgayNghi > 0)
                                {
                                    employeeWorkTimeLog.Status = (int)EStatusWork.DuCong;
                                }
                            }
                            else
                            {
                                workTime = (outLogTime - inLogTime) - lunch;
                                if (workTime.TotalHours > 8)
                                {
                                    tangcathucte = workTime - new TimeSpan(8, 0, 0);
                                }
                                if (employeeWorkTimeLog.Mode != (int)ETimeWork.Normal)
                                {
                                    tangcathucte = workTime;
                                }

                                if (inLogTime > startWorkingScheduleTime)
                                {
                                    employeeWorkTimeLog.StatusLate = (int)EStatusWork.XacNhanCong;
                                    employeeWorkTimeLog.Status = (int)EStatusWork.XacNhanCong;
                                    late = inLogTime - startWorkingScheduleTime;
                                    if (late.TotalMinutes < 1)
                                    {
                                        late = new TimeSpan(0);
                                        employeeWorkTimeLog.StatusLate = (int)EStatusWork.DuCong;
                                        employeeWorkTimeLog.Status = (int)EStatusWork.DuCong;
                                    }
                                    if (late.TotalMinutes > 15)
                                    {
                                        late = new TimeSpan(0);
                                        workDay += -0.5;
                                    }
                                }
                                if (outLogTime < endWorkingScheduleTime)
                                {
                                    employeeWorkTimeLog.StatusEarly = (int)EStatusWork.XacNhanCong;
                                    employeeWorkTimeLog.Status = (int)EStatusWork.XacNhanCong;
                                    early = endWorkingScheduleTime - outLogTime;
                                    if (early.TotalMinutes < 1)
                                    {
                                        early = new TimeSpan(0);
                                        employeeWorkTimeLog.StatusEarly = (int)EStatusWork.DuCong;
                                    }
                                    if (early.TotalMinutes > 15)
                                    {
                                        early = new TimeSpan(0);
                                        workDay += -0.5;
                                    }
                                }

                                if (employeeWorkTimeLog.SoNgayNghi > 0)
                                {
                                    employeeWorkTimeLog.Status = (int)EStatusWork.DuCong;
                                }
                            }

                            if (tangcathucte.TotalHours >= 1)
                            {
                                employeeWorkTimeLog.StatusTangCa = (int)ETangCa.GuiXacNhan;
                            }
                            if (employeeWorkTimeLog.Mode != (int)ETimeWork.Normal)
                            {
                                employeeWorkTimeLog.StatusTangCa = (int)ETangCa.GuiXacNhan;
                            }

                            employeeWorkTimeLog.VerifyMode = records[0].VerifyMode;
                            employeeWorkTimeLog.InOutMode = records[0].InOutMode;
                            employeeWorkTimeLog.In = inLogTime;
                            employeeWorkTimeLog.Out = outLogTime;
                            employeeWorkTimeLog.WorkTime = workTime;
                            employeeWorkTimeLog.TangCaThucTe = tangcathucte;
                            employeeWorkTimeLog.WorkDay = workDay;
                            employeeWorkTimeLog.Late = late;
                            employeeWorkTimeLog.Early = early;
                            employeeWorkTimeLog.Logs = records;
                            #endregion
                        }

                        #region DB
                        var workTimeLogDb = dbContext.EmployeeWorkTimeLogs
                                                    .Find(m => m.EmployeeId.Equals(employeeWorkTimeLog.EmployeeId)
                                                        && m.Date.Equals(employeeWorkTimeLog.Date)).FirstOrDefault();

                        if (workTimeLogDb == null)
                        {
                            dbContext.EmployeeWorkTimeLogs.InsertOne(employeeWorkTimeLog);
                        }
                        else
                        {
                            employeeWorkTimeLog.Id = workTimeLogDb.Id;
                            bool isUpdate = true;
                            if (!string.IsNullOrEmpty(workTimeLogDb.SecureCode))
                            {
                                isUpdate = false;
                                // Fix data, remove after fix
                                if (workTimeLogDb.Logs != null && employeeWorkTimeLog.Logs != null)
                                {
                                    if (workTimeLogDb.Logs.Count < employeeWorkTimeLog.Logs.Count)
                                    {
                                        isUpdate = true;
                                        employeeWorkTimeLog.SecureCode = string.Empty;
                                    }
                                }
                                if (workTimeLogDb.Logs == null && employeeWorkTimeLog.Logs != null)
                                {
                                    isUpdate = true;
                                    employeeWorkTimeLog.SecureCode = string.Empty;
                                }
                                if (employeeWorkTimeLog.Mode > (int)ETimeWork.Normal)
                                {
                                    isUpdate = true;
                                    employeeWorkTimeLog.SecureCode = string.Empty;
                                }
                            }
                            if (isUpdate)
                            {
                                // Truong hop bam 2 noi: sang 1 noi, chieu 1 noi...
                                // Lưu mỗi nơi theo mã chấm công
                                // Tính công cộng 2 nơi lại. Check không vượt quá 1 ngày.
                                if (employeeWorkTimeLog.Logs != null && employeeWorkTimeLog.Logs.Count > 0)
                                {
                                    var filter = Builders<EmployeeWorkTimeLog>.Filter.Eq(m => m.Id, workTimeLogDb.Id);
                                    var update = Builders<EmployeeWorkTimeLog>.Update
                                        .Set(m => m.EnrollNumber, employeeWorkTimeLog.EnrollNumber)
                                        .Set(m => m.WorkplaceCode, location)
                                        .Set(m => m.In, employeeWorkTimeLog.In)
                                        .Set(m => m.Out, employeeWorkTimeLog.Out)
                                        .Set(m => m.WorkTime, employeeWorkTimeLog.WorkTime)
                                        .Set(m => m.Workcode, employeeWorkTimeLog.Workcode)
                                        .Set(m => m.WorkDay, employeeWorkTimeLog.WorkDay)
                                        .Set(m => m.Late, employeeWorkTimeLog.Late)
                                        .Set(m => m.Early, employeeWorkTimeLog.Early)
                                        .Set(m => m.Status, employeeWorkTimeLog.Status)
                                        .Set(m => m.StatusLate, employeeWorkTimeLog.StatusLate)
                                        .Set(m => m.StatusEarly, employeeWorkTimeLog.StatusEarly)
                                        .Set(m => m.Logs, employeeWorkTimeLog.Logs)
                                        .Set(m => m.Mode, employeeWorkTimeLog.Mode)
                                        .Set(m => m.SoNgayNghi, employeeWorkTimeLog.SoNgayNghi)
                                        .Set(m => m.StatusTangCa, employeeWorkTimeLog.StatusTangCa)
                                        .Set(m => m.TangCaThucTe, employeeWorkTimeLog.TangCaThucTe)
                                        .Set(m => m.Reason, employeeWorkTimeLog.Reason)
                                        .Set(m => m.UpdatedOn, DateTime.Now);
                                    dbContext.EmployeeWorkTimeLogs.UpdateOne(filter, update);
                                }
                            }
                        }
                        #endregion

                        #region Send Mail
                        if (isMail && !employeeWorkTimeLog.IsSendMail)
                        {
                            var iDateSent = -1;
                            if (DateTime.Now.Date.AddDays(iDateSent).DayOfWeek == DayOfWeek.Sunday)
                            {
                                iDateSent--;
                            }
                            if (employeeWorkTimeLog.Status == (int)EStatusWork.XacNhanCong && date == today.AddDays(iDateSent) && !string.IsNullOrEmpty(email))
                            {
                                Console.WriteLine("Sending mail...");
                                var tos = new List<EmailAddress>
                                        {
                                            new EmailAddress { Name = fullName, Address = email }
                                        };
                                var webRoot = Environment.CurrentDirectory;
                                var pathToFile = @"C:\Projects\App.Schedule\Templates\TimeKeeperNotice.html";
                                var subject = "Xác nhận thời gian làm việc.";
                                var bodyBuilder = new BodyBuilder();
                                using (StreamReader SourceReader = File.OpenText(pathToFile))
                                {
                                    bodyBuilder.HtmlBody = SourceReader.ReadToEnd();
                                }
                                var logsHtml = string.Empty;
                                if (employeeWorkTimeLog.Logs != null && employeeWorkTimeLog.Logs.Count > 0)
                                {
                                    logsHtml += "<br />";
                                    logsHtml += "<small style='font-size:10px;'>Máy chấm công ghi nhận:</small>";
                                    logsHtml += "<br />";
                                    logsHtml += "<table class='MsoNormalTable' border='0 cellspacing='0' cellpadding='0' width='738' style='width: 553.6pt; margin-left: -1.15pt;'>";
                                    foreach (var log in employeeWorkTimeLog.Logs)
                                    {
                                        logsHtml += "<tr style='height: 12.75pt'>";
                                        logsHtml += "<td nowrap='nowrap'><small style='font-size:10px;'>" + log.Date.ToString("dd/MM/yyyy HH:mm:ss") + "</small></td>";
                                        logsHtml += "</tr>";
                                    }
                                    logsHtml += "</table>";
                                }
                                var url = Constants.System.domain;
                                var forgot = url + Constants.System.login;
                                string messageBody = string.Format(bodyBuilder.HtmlBody,
                                    subject,
                                    fullName,
                                    employeeWorkTimeLog.EnrollNumber,
                                    employeeWorkTimeLog.WorkplaceCode,
                                    employeeWorkTimeLog.Start + "-" + employeeWorkTimeLog.End,
                                    employeeWorkTimeLog.Date.ToString("dd/MM/yyyy"),
                                    employeeWorkTimeLog.In,
                                    employeeWorkTimeLog.Out,
                                    employeeWorkTimeLog.Late == TimeSpan.FromHours(0) ? string.Empty : employeeWorkTimeLog.Late.ToString(),
                                    employeeWorkTimeLog.Early == TimeSpan.FromHours(0) ? string.Empty : employeeWorkTimeLog.Early.ToString(),
                                    employeeWorkTimeLog.WorkTime,
                                    Math.Round(employeeWorkTimeLog.WorkDay, 2),
                                    logsHtml,
                                    linkChamCong,
                                    url,
                                    forgot,
                                    DateTime.Now.AddDays(1).ToShortDateString()
                                    );
                                var emailMessage = new EmailMessage()
                                {
                                    ToAddresses = tos,
                                    Subject = subject,
                                    BodyContent = messageBody,
                                    EmployeeId = employeeWorkTimeLog.EmployeeId
                                };

                                var scheduleEmail = new ScheduleEmail
                                {
                                    Status = (int)EEmailStatus.Schedule,
                                    To = emailMessage.ToAddresses,
                                    CC = emailMessage.CCAddresses,
                                    BCC = emailMessage.BCCAddresses,
                                    Type = emailMessage.Type,
                                    Title = emailMessage.Subject,
                                    Content = emailMessage.BodyContent,
                                    EmployeeId = emailMessage.EmployeeId
                                };
                                dbContext.ScheduleEmails.InsertOne(scheduleEmail);

                                // update db
                                var filter = Builders<EmployeeWorkTimeLog>.Filter.Eq(m => m.Id, employeeWorkTimeLog.Id);
                                var update = Builders<EmployeeWorkTimeLog>.Update
                                    .Set(m => m.IsSendMail, true)
                                    .Set(m => m.UpdatedOn, DateTime.Now);
                                dbContext.EmployeeWorkTimeLogs.UpdateOne(filter, update);
                            }
                        }
                        #endregion
                    }

                    Summary(dbContext, employee, location, month, year, modeData, debug);

                    startDate = startDate.AddMonths(1);
                }
                #endregion
            }
        }

        private static void Summary(MongoDBContext dbContext, Employee employee, string location, int month, int year, bool modeData, bool debug)
        {
            var now = DateTime.Now;
            var workplace = employee.Workplaces.FirstOrDefault(a => a.Code == location);

            #region INIT
            var checkExist = dbContext.EmployeeWorkTimeMonthLogs
                        .Find(m => m.EmployeeId.Equals(employee.Id)
                        && m.EnrollNumber.Equals(workplace.Fingerprint)
                        && m.WorkplaceCode.Equals(workplace.Code)
                        && m.Year.Equals(year) && m.Month.Equals(month)).FirstOrDefault();
            if (checkExist == null)
            {
                dbContext.EmployeeWorkTimeMonthLogs.InsertOne(new EmployeeWorkTimeMonthLog
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    Department = employee.PhongBanName,
                    DepartmentId = employee.PhongBan,
                    DepartmentAlias = Utility.AliasConvert(employee.PhongBanName),
                    Part = employee.BoPhanName,
                    PartId = employee.BoPhan,
                    PartAlias = Utility.AliasConvert(employee.BoPhanName),
                    Title = employee.ChucVuName,
                    TitleId = employee.ChucVu,
                    TitleAlias = Utility.AliasConvert(employee.ChucVuName),
                    EnrollNumber = workplace.Fingerprint,
                    WorkplaceCode = workplace.Code,
                    Month = month,
                    Year = year
                });
            }
            #endregion

            var current = dbContext.EmployeeWorkTimeMonthLogs
                        .Find(m => m.EmployeeId.Equals(employee.Id)
                        && m.EnrollNumber.Equals(workplace.Fingerprint)
                        && m.WorkplaceCode.Equals(workplace.Code)
                        && m.Year.Equals(year) && m.Month.Equals(month)).FirstOrDefault();

            // foreach timework
            var times = dbContext.EmployeeWorkTimeLogs.Find(m => m.Enable.Equals(true)
            && m.EmployeeId.Equals(employee.Id) && m.WorkplaceCode.Equals(location)
            && m.Month.Equals(month) && m.Year.Equals(year)).SortBy(m => m.Date).ToList();
            #region Declare
            double Workday = 0;
            double WorkTime = 0; // store miliseconds
            double CongCNGio = 0;
            double CongTangCaNgayThuongGio = 0;
            double CongLeTet = 0;
            double Late = 0;
            double Early = 0;
            double NghiPhepNam = 0;
            double NghiViecRieng = 0;
            double NghiBenh = 0;
            double NghiKhongPhep = 0;
            double NghiHuongLuong = 0;
            double NghiLe = 0;
            double KhongChamCong = 0;
            double ChuNhat = 0;
            #endregion
            foreach (var time in times)
            {
                switch (time.Mode)
                {
                    case (int)ETimeWork.Normal:
                        {
                            Workday += time.WorkDay;
                            WorkTime += time.WorkTime.TotalMilliseconds;
                            CongTangCaNgayThuongGio += time.TangCaDaXacNhan.TotalMilliseconds;
                            Late += time.Late.TotalMilliseconds;
                            Early += time.Early.TotalMilliseconds;
                            break;
                        }
                    case (int)ETimeWork.Sunday:
                        {
                            ChuNhat++;
                            CongCNGio += time.TangCaDaXacNhan.TotalMilliseconds;
                            break;
                        }
                    case (int)ETimeWork.LeavePhep:
                        {
                            NghiPhepNam++;
                            break;
                        }
                    case (int)ETimeWork.LeaveHuongLuong:
                        {
                            NghiHuongLuong++;
                            break;
                        }
                    case (int)ETimeWork.LeaveKhongHuongLuong:
                        {
                            NghiViecRieng++;
                            break;
                        }
                    case (int)ETimeWork.Holiday:
                        {
                            NghiLe++;
                            CongLeTet += time.TangCaDaXacNhan.TotalMilliseconds;
                            break;
                        }
                    case (int)ETimeWork.Other:
                        Console.WriteLine((int)ETimeWork.Other);
                        break;
                    case (int)ETimeWork.Wait:
                        Console.WriteLine((int)ETimeWork.Wait);
                        break;
                    default:
                        {
                            KhongChamCong++;
                            break;
                        }
                }
            }

            var builder = Builders<EmployeeWorkTimeMonthLog>.Filter;
            var filter = builder.Eq(m => m.Id, current.Id);
            var update = Builders<EmployeeWorkTimeMonthLog>.Update
                .Set(m => m.Workday, Workday)
                .Set(m => m.WorkTime, WorkTime)
                .Set(m => m.CongCNGio, CongCNGio)
                .Set(m => m.CongTangCaNgayThuongGio, CongTangCaNgayThuongGio)
                .Set(m => m.CongLeTet, CongLeTet)
                .Set(m => m.Late, Late)
                .Set(m => m.Early, Early)
                .Set(m => m.NghiPhepNam, NghiPhepNam)
                .Set(m => m.NghiViecRieng, NghiViecRieng)
                .Set(m => m.NghiBenh, NghiBenh)
                .Set(m => m.NghiKhongPhep, NghiKhongPhep)
                .Set(m => m.NghiHuongLuong, NghiHuongLuong)
                .Set(m => m.NghiLe, NghiLe)
                .Set(m => m.KhongChamCong, KhongChamCong)
                .Set(m => m.ChuNhat, ChuNhat)
                .Set(m => m.LastUpdated, now);
            dbContext.EmployeeWorkTimeMonthLogs.UpdateOne(filter, update);
        }
    }
}
