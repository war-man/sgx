﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 2 type: [bao co thue] vs [Thuc Te]
    /// 1. BAO CAO THUE: Law:true. Base MaSo (Name)
    /// 2. THUC TE: Law:false. Base ViTri. (Future update VITRI thuộc MaSo)
    /// </summary>

    // Moi chuc vu cong viec, 1 bac luong 1 record.
    // Tạo mới bac luong: foreach chuc vu, tao theo 1 record cho bac luong do.
    // Truy suat simple hơn de chung.
    // List: group by query theo chuc vu, bac luong, set value.... 
    public class SalaryThangBangLuong
    {
        [BsonId]
        // Mvc don't know how to create ObjectId from string
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        // Theo qui mô công ty.
        // Thang bang luong cong ty.
        // Theo từng vị trí trong công ty.
        // Vị trí sẽ được xếp vô [Name - Chức danh công việc]
        // Vị trí null => summarry. (báo cáo thuế,...)
        [Display(Name = "Vị trí")]
        public string ViTri { get; set; }

        [Display(Name = "Chức danh công việc")]
        public string Name { get; set; }

        // Bao cao thue, ap dung vo tung nhan vien sau.
        [Display(Name = "Mã số")]
        public string MaSo { get; set; }

        [Display(Name = "Bảng lương theo chức danh")]
        public string TypeRole { get; set; }

        // bac 0: default user set, if no use min value
        // 1->10 (current). upto .... Do later
        public int Bac { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal HeSo { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal MucLuong { get; set; }

        // My auto generate
        public string ViTriCode { get; set; }

        public string ViTriAlias { get; set; }

        public string NameAlias { get; set; }

        public string TypeRoleAlias { get; set; }

        // Auto generate
        public string TypeRoleCode { get; set; }

        // muc luong theo luat VN, false theo cong ty
        public bool Law { get; set; } = true;

        // Thuc te - true;
        public bool FlagReal { get; set; } = false;

        public bool Enable { get; set; } = true;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Start { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;

    }
}
