﻿$(function () {
    var $table = $('table.floating-header');
    $table.floatThead();

    //$('.left-menu').addClass('d-none');
    $('.js-select2-basic-single').select2(
        {
            theme: "bootstrap"
        });

    $('#thang').on('change', function (e) {
        formSubmit();
    });

    $('#Id').on('change', function (e) {
        formSubmit();
    });

    $('#phongban').on('change', function (e) {
        formSubmit();
    });
});