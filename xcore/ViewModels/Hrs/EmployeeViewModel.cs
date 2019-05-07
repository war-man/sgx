﻿using Models;
using System;
using System.Collections.Generic;
using ViewModels;

namespace ViewModels
{
    public class EmployeeViewModel : ExtensionViewModel
    {
        public IList<Employee> Employees { get; set; }

        public Employee Employee { get; set; }

        public bool StatusChange { get; set; }

        public Employee EmployeeChance { get; set; }

        public IList<CongTyChiNhanh> CongTyChiNhanhs { get; set; }

        public IList<KhoiChucNang> KhoiChucNangs { get; set; }

        public IList<PhongBan> PhongBans { get; set; }

        public IList<BoPhan> BoPhans { get; set; }

        public IList<BoPhan> BoPhanCons { get; set; }

        public IList<ChucVu> ChucVus { get; set; }

        public IList<Employee> EmployeesDdl { get; set; }

        public IList<WorkTimeType> WorkTimeTypes { get; set; }

        public IList<BHYTHospital> Hospitals { get; set; }

        public IList<ContractType> Contracts { get; set; }

        //public IList<WorkTimeType> WorkTimeTypes { get; set; }

        public IList<NgachLuong> NgachLuongs { get; set; }

        #region Search
        public string Id { get; set; }

        public string Ten { get; set; }

        public string Code { get; set; }

        public string Fg { get; set; }

        public string Nl { get; set; }

        public string Kcn { get; set; }

        public string Pb { get; set; }

        public string Bp { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public string Sortby { get; set; }

        public string Sort { get; set; }

        public string LinkCurrent { get; set; }
        #endregion

        public Employee Manager { get; set; }

        public string EmailGroup { get; set; }

        public string EmailLeaveGroup { get; set; }

        // Welcome
        public bool EmailSend { get; set; } = false;

        public bool EmailLeave { get; set; } = false;

        public string OtherWelcomeEmail { get; set; } // Customer email send.

        public string OtherLeaveEmail { get; set; } // Customer email send.

        public int RecordCurrent { get; set; }

        public int RecordLeave { get; set; }

        public int Records { get; set; }

        public int Pages { get; set; }
    }
}
