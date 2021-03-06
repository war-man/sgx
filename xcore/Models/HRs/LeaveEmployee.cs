﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    // Quản lý số ngày nghỉ phép còn lại (nghỉ phép, các loại nghỉ bù,...)
    // Khong quan ly nghi phep huong luong (thai san, dam cuoi,..)
    public class LeaveEmployee : Common
    {
        [BsonId]
        // Mvc don't know how to create ObjectId from string
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Loại phép")]
        public string LeaveTypeId { get; set; }

        [Display(Name = "Mã nhân viên")]
        [Required]
        public string EmployeeId { get; set; }

        public string LeaveTypeName { get; set; }

        [Display(Name = "Tên nhân viên")]
        public string EmployeeName { get; set; }

        // So ngay phep hien tai
        [Display(Name = "Số ngày")]
        public double Number { get; set; } = 0;

        public string Department { get; set; }

        public string Part { get; set; }

        public string Title { get; set; }

        public double LeaveLevel { get; set; } = 12;

        public double NumberUsed { get; set; } = 0;

        // Define probation. Probation no use leave phep-nam.
        // UseFlag = false if probation
        public bool UseFlag { get; set; } = true;

        // Rule: phép của năm trước xài tới tháng 6 năm hiện tại. Sau ngày này reset về 0.
        public int Year { get; set; } = DateTime.Now.Year;
    }
}
