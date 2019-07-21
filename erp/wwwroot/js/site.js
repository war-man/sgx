﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    document.onkeypress = keyPress;

    $(".toggle-password").on('click', function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $($(this).attr("toggle"));
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    });

    $('.btn-language').on('click', function () {
        var language = $(this).attr("data-value");
        var url = $('.' + language, $('.link-languages')).data('value');
        console.log(url);
        $.ajax({
            url: '/language/change/',
            data: { language: language },
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data === true) {
                    //window.location.reload();
                    console.log(url);
                    if (typeof url !== "undefined") {
                        window.location.replace(url);
                    }
                    else {
                        window.location.reload();
                    }
                }
            }
        });
    });

    loadCommonData();

    $('[data-toggle="tooltip"]').tooltip();

    $(".datepicker").datepicker({
        language: "vi",
        format: 'dd/mm/yyyy',
        todayHighlight: true,
        autoclose: true
    });

    $('.multi-select').multiselect({
        //enableFiltering: true,
        nonSelectedText: '',
        nSelectedText: '',
        allSelectedText: ''
    });

    $('.btn-save-pwd').on('click', function () {
        var check = true;
        if ($('#newpassword').val().trim().length < 0) {
            toastr.error("Mật khẩu không để trống, khoảng cách.");
            check = false;
        }
        if ($('#newpassword').val().trim() !== $('#renewpassword').val().trim()) {
            toastr.error("[Nhập lại Mật khẩu mới] không khớp. Vui lòng kiểm tra.");
            check = false;
        }

        if (check) {
            $.ajax({
                type: 'post',
                url: '/api/updatepassword?newpassword=' + $('#newpassword', $('.data-form-change-pwd')).val(),
                processData: false,
                contentType: false,
            })
                .done(function (data) {
                    if (data.result === true) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                    $('#pwdModal').modal('hide');
                })
                .fail(function () {
                    toastr.error(data.message)
                });
        }
    });

    $('textarea.js-auto-size').textareaAutoSize();

    $("#more").on("hide.bs.collapse", function () {
        $(".custom-more").html('<i class="icon icon-add-to-list"></i> Mở rộng');
    });

    $("#more").on("show.bs.collapse", function () {
        $(".custom-more").html('<i class="icon icon-list"></i> Thu gọn');
    });

    fixImg();

    $(window).on('resize', function () {
        fixImg();
    });

    function fixImg() {
        if ($(window).width() < 575.98) {
            $('.content-body img').css({ 'width': '100%' });
        }
    }    

    toastr.options = {
        preventDuplicates : true
    };
});

function keyPress(e) {
    var x = e || window.event;
    var key = x.keyCode || x.which;
    if (key === 13 || key === 3) {
        formSubmit();
    }
}

function formSubmit() {
    document.getElementById("form-main").submit();
} 

function loadCommonData() {
    $.ajax({
        type: "GET",
        url: "/helper/ErpCommon",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            //console.log(data);
            if (data.result === false) {
                //console.log("Error.");
                //console.log(data);
            }
            else {
                if (data.length !== 0) {
                    $('.card-info').html($.templates("#tmplCardInfo").render(data.userInformation));
                    $('#ownerInfo li').html($.templates("#tmplOwnerInfo").render(data));
                }
            }
        }
    });
}

function enableRemove() {
    $('.remove-item').on('click', function (e) {
        $(this).closest('.node').remove();
    });
}

function registerDatePicker() {
    $(".datepicker").datepicker({
        language: "vi",
        format: 'dd/mm/yyyy',
    });
    $('.datepicker').on('changeDate', function () {
        var date = moment($(this).datepicker('getFormattedDate'), 'DD-MM-YYYY')
        $('.hidedatepicker', $(this).closest('.form-group')).val(
            date.format('MM-DD-YYYY')
        );
    });
}

//const num2Word2 = function () {
//    var t = ["không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"],
//        r = function (r, n) {
//            var o = "",
//                a = Math.floor(r / 10),
//                e = r % 10;
//            return a > 1 ? (o = " " + t[a] + " mươi", 1 == e && (o += " mốt")) : 1 == a ? (o = " mười", 1 == e && (o += " một")) : n && e > 0 && (o = " lẻ"), 5 == e && a >= 1 ? o += " lăm" : 4 == e && a >= 1 ? o += " tư" : (e > 1 || 1 == e && 0 == a) && (o += " " + t[e]), o
//        },
//        n = function (n, o) {
//            var a = "",
//                e = Math.floor(n / 100),
//                n = n % 100;
//            return o || e > 0 ? (a = " " + t[e] + " trăm", a += r(n, !0)) : a = r(n, !1), a;
//        },
//        o = function (t, r) {
//            var o = "",
//                a = Math.floor(t / 1e6),
//                t = t % 1e6;
//            a > 0 && (o = n(a, r) + " triệu", r = !0);
//            var e = Math.floor(t / 1e3),
//                t = t % 1e3;
//            return e > 0 && (o += n(e, r) + " ngàn", r = !0), t > 0 && (o += n(t, r)), o
//        };
//    return {
//        convert: function (r) {
//            if (0 == r) return t[0];
//            var n = "",
//                a = "";
//            do ty = r % 1e9, r = Math.floor(r / 1e9), n = r > 0 ? o(ty, !0) + a + n : o(ty, !1) + a + n, a = " tỷ"; while (r > 0);
//            return n.trim()
//        }
//    };
//}();

function capitalizeFirstLetter(string) {
    return string[0].toUpperCase() + string.slice(1);
}

var NanValue = function (entry) {
    if (entry === "NaN") {
        return 0.00;
    } else {
        return entry;
    }
};

function secondToHHMM(inSecs) {
    var sec_num = parseInt(inSecs, 10); // don't forget the second param
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    return hours + ':' + minutes;
}

function secondToHHMMSS(inSecs) {
    var sec_num = parseInt(inSecs, 10); // don't forget the second param
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);
    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    return hours + ':' + minutes + ':' + seconds;
}

function secondToDDHHMMSS(s) {
    var fm = [
        Math.floor(s / 60 / 60 / 24), // DAYS
        Math.floor(s / 60 / 60) % 24, // HOURS
        Math.floor(s / 60) % 60, // MINUTES
        s % 60 // SECONDS
    ];
    return $.map(fm, function (v, i) { return ((v < 10) ? '0' : '') + v; }).join(':');
}

function ConvertToDateFromhhmm(time) {
    var h = time.split(':')[0];
    var m = time.split(':')[1];
    var d = new Date();
    d.setHours(h);
    d.setMinutes(m);
    return d;
}

function calculatorHmsToSecond(hms) {
    var a = hms.split(':'); // split it at the colons
    // minutes are worth 60 seconds. Hours are worth 60 minutes.
    var seconds = (+a[0]) * 60 * 60 + (+a[1]) * 60 + (+a[2]);
    return seconds;
}

Date.daysBetween = function (date1, date2) {
    //Get 1 day in milliseconds
    //var one_day = 1000 * 60 * 60 * 24;
    //Get 1 day in milliseconds
    var one_day = 1000 * 60 * 60 * 24;

    // Convert both dates to milliseconds
    var date1_ms = date1.getTime();
    var date2_ms = date2.getTime();

    // Calculate the difference in milliseconds
    var difference_ms = date2_ms - date1_ms;
    //take out milliseconds
    difference_ms = difference_ms / 1000;
    var seconds = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    var minutes = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    var hours = Math.floor(difference_ms % 24);
    var days = Math.floor(difference_ms / 24);

    return days + ' days, ' + hours + ' hours, ' + minutes + ' minutes, and ' + seconds + ' seconds';
}

function setValueDateFormat() {
    $('.datepicker').each(function (i, obj) {
        var date = moment($(obj).datepicker('getFormattedDate'), 'DD-MM-YYYY');
        $('.hidedatepicker', $(obj).closest('.date-area')).val(
            date.format('MM-DD-YYYY')
        );
    });
}

function DateFullToString(date) {
    var ouputs = date.substring(0, 10).split("-");
    return ouputs[2] + "/" + ouputs[1] + "/" + ouputs[0];
}

function NullToText(text) {
    if (text === null) {
        text = "--";
    }
    return text;
}

// UPLOAD FILE
var $progress = $('.progress', parent)[0];

function uploadProgress(e) {
    if (e.lengthComputable) {
        var percentComplete = (e.loaded * 100) / e.total;
        $('.progress-bar', $progress).css('width', percentComplete + "%");
        if (percentComplete >= 100) {
            // process completed
        }
    }
}

function downloadProgress(e) {
    if (e.lengthComputable) {
        var percentage = (e.loaded * 100) / e.total;
        $('.progress-bar', $progress).css('width', percentage + "%");
        if (percentage >= 100) {
            // process completed
        }
    }
}

function showProgress(parent) {
    $('.btnUpload', parent).prop('disabled', true);
    $('.btnUpload', parent).addClass('d-none');
    $('input', parent).prop('disabled', true);
    $('.btn-upload-process', parent).removeClass('d-none');
}

function resetFormUpload(parent) {
    $('.btnUpload', parent).removeClass('d-none');
    $('.btnUpload', parent).prop('disabled', false);
    $('input', parent).prop('disabled', false);
    $('.btn-upload-process', parent).addClass('d-none');
}
// END UPLOAD FILE

function eventAutocomplete() {
    // https://www.devbridge.com/sourcery/components/jquery-autocomplete/
    //$('#autocomplete').autocomplete({
    //    serviceUrl: '/autocomplete/countries',
    //    onSelect: function (suggestion) {
    //        alert('You selected: ' + suggestion.value + ', ' + suggestion.data);
    //    }
    //});

    $('.autocomplete').on("focus", function () {
        $(this).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/api/employees/",
                    data: {
                        type: $(this).data('type'),
                        term: request.term
                    },
                    success: function (data) {
                        response(data.outputs);
                    }
                });
            },
            minLength: 2,
            focus: function (event, ui) {
                $(this).val(ui.item.fullName);
                return false;
            },
            select: function (event, ui) {
                //$('#name').val(ui.item.name);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div>" + item.fullName + "<br>" + item.email + " - " + item.title + "</div>")
                .appendTo(ul);
        };
    });
}