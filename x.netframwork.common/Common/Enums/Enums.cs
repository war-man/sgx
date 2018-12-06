﻿namespace Common.Enums
{
    public enum Phase { Years, Months, Days, Done }

    public enum EEmailStatus
    {
        Send = 0,
        Ok = 1,
        Fail = 2,
        Resend = 3,
        Schedule = 4,
        ScheduleASAP = 5
    }

    public enum EPCPL
    {
        PC = 1,
        PL = 2
    }

    public enum ESalaryType
    {
        VP = 1,
        NM = 2,
        SX = 3
    }

    public enum StatusLeave
    {
        New = 0,
        Accept = 1,
        Cancel = 2,
        Pending = 3
    }

    public enum StatusWork
    {
        XacNhanCong = 0,
        DuCong = 1,
        DaGuiXacNhan = 2,
        DongY = 3,
        TuChoi = 4,
        Wait = 5
    }

    public enum TimeWork
    {
        None = 0,
        Normal = 1,
        Sunday = 2,
        LeavePhep = 3,
        LeaveHuongLuong = 4,
        LeaveKhongHuongLuong = 5,
        Holiday = 6,
        Other = 7,
        Wait = 8
    }

    public enum ETexts
    {
        None = 1,
        ErrorSystem = 2
        // up to 10
    }

    public enum ERights
    {
        // Owner
        None = 0,
        // List, Detail
        View = 1,
        Add = 2,
        Edit = 3,
        Disable = 4,
        Delete = 5,
        History = 6
    }

    public enum EStatus
    {
        // implement later
    }

    public enum ELogType
    {
        Other = 0,
        Performance = 1
    }

    public enum ENotification
    {
        Other = 0,
        HR = 1
    }

    public enum EMonths
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public enum EDownTimeMessageType
    {
        DownTimeWarningMessage = 1,
        DownTimeErrorMessage = 2
    }

    public enum EAlertType
    {
        /// <summary>
        /// remove all flag states from database completly
        /// </summary>
        Remove = -1,

        /// <summary>
        /// set flag to none
        /// </summary>
        None = 0,

        /// <summary>
        /// new flag
        /// </summary>
        New = 1,

        /// <summary>
        /// updated flag
        /// </summary>
        Update = 2
    }
}