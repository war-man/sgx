﻿$(function () {
    setValue();

    //googleInit();

    eventInit();

    $(".data-form").on("submit", function (event) {
        if (!confirm("Bạn chắc chắn thông tin đã được kiểm tra và muốn cập nhật!")) {
            return false;
        }

        if ($('#IsWelcomeEmail').val() === "true") {
            if (!confirm("Bạn đã chọn <Gửi email thông báo nhân sự mới> !! Hệ thống sẽ gửi email thông báo tới tất cả nhân viên công ty, cc cho các cấp lãnh đạo. Chọn hủy/cancel và bỏ dấu <Gửi email thông báo nhân sự mới> nếu không muốn gửi email.")) {
                return false;
            }
            else {
                // Load content email:...
                // Do later...
            }
        }

        if ($('#IsLeaveEmail').val() === "true") {
            if (!confirm("Bạn đã chọn <Gửi email thông báo nhân sự nghỉ việc> !! Hệ thống sẽ gửi email thông báo tới tất cả nhân viên công ty, cc cho các cấp lãnh đạo. Chọn hủy/cancel và bỏ dấu <Gửi email thông báo nhân sự nghỉ việc> nếu không muốn gửi email.")) {
                return false;
            }
            else {
                // Load content email:...
                // Do later...
            }
        }

        event.preventDefault();
        var formData = new FormData($(this)[0]);
        $('#btn-save-submit').prop('disabled', true);
        $('input', $('.data-form')).prop('disabled', true);
        $('select', $('.data-form')).prop('disabled', true);
        $('textarea', $('.data-form')).prop('disabled', true);
        var loadingText = '<i class="fas fa-spinner"></i> đang xử lý...';
        $('#btn-save-submit').html(loadingText);
        var $this = $(this);
        $.ajax({
            type: $this.attr('method'),
            url: $this.attr('action'),
            enctype: 'multipart/form-data',
            processData: false,  // Important!
            contentType: false,
            data: formData,
            success: function (data) {
                if (data.result === true) {
                    toastr.success(data.message);
                    setTimeout(function () {
                        window.location = "/nhan-su";
                    }, 1000);
                }
                else {
                    if (data.source === "user") {
                        resetForm();
                        $('input[name="Employee.UserName"]').focus();
                    }
                    if (data.source === "email") {
                        resetForm();
                        $('input[name="Employee.Email"]').focus();
                    }
                    toastr.error(data.message);
                }
            }
        });
    });

    $('.custom-file-input').on('change', function () {
        changeImgProfile(this);
    });

    $('input[name="Employee.FullName"]').on('change',function (e) {
        if ($(this).val().length > 0) {
            $.ajax({
                type: "GET",
                url: "/helper/fullnamegenerate",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: { name: $(this).val() },
                success: function (data) {
                    if (data.result === false) {
                        $('.user-name-error').html(data.userName + " đã được sử dụng. Đề xuất: " + data.suggest + ",...");
                    }
                    else {
                        if (data.length !== 0) {
                            $('input[name="Employee.UserName"]').val(data.userName);
                            $('input[name="Employee.Email"]').val(data.email);
                        }
                    }
                }
            });
        }
        $('input[name="Employee.EmployeeBank.BankHolder"]').val($(this).val());
    });

    $('#newCategoryModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var ecategory = parseInt(button.data('ecategory'));
        var categoryName = "";
        var parent = "";
        if (ecategory === parseInt($('.egender-val-hide').val())) {
            categoryName = "Giới tính";
        }
        else if (ecategory === parseInt($('.ekhoichucnang-val-hide').val())) {
            categoryName = "Khối chức năng";
        }
        else if (ecategory === parseInt($('.ephongban-val-hide').val())) {
            categoryName = "Phòng ban";
            parent = $('.ekhoichucnang-val-hide').val();
        }
        else if (ecategory === parseInt($('.ebophan-val-hide').val())) {
            categoryName = "Bộ phận";
            parent = $('.ephongban-val-hide').val();
        }
        else if (ecategory === parseInt($('.echucvu-val-hide').val())) {
            categoryName = "Chức vụ";
            if ($('select[name="Employee.BoPhan"]').val() !== "") {
                parent = $('.ebophan-val-hide').val();
            }
            else if ($('select[name="Employee.PhongBan"]').val() !== "") {
                parent = $('.ephongban-val-hide').val();
            }
            else{
                parent = $('.ekhoichucnang-val-hide').val();
            }
        }
        else if (ecategory === parseInt($('.ecompany-val-hide').val())) {
            categoryName = "Công ty/Chi nhánh";
        }
        else if (ecategory === parseInt($('.ehospital-val-hide').val())) {
            categoryName = "Nơi khám chữa bệnh ban đầu";
        }
        else if (ecategory === parseInt($('.ebank-val-hide').val())) {
            categoryName = "Ngân hàng";
        }
        else if (ecategory === parseInt($('.econtract-val-hide').val())) {
            categoryName = "Loại hợp đồng";
        }
        else if (ecategory === parseInt($('.etimework-val-hide').val())) {
            categoryName = "Thời gian làm việc";
        }
        else if (ecategory === parseInt($('.eprobation-val-hide').val())) {
            categoryName = "Thời gian thử việc";
        }
        var modal = $(this);
        $('input', modal).val('');
        $('textarea', modal).val('');
        $('.parent-area', modal).addClass('d-none');
        $('.error-message', modal).addClass('d-none');

        modal.find('.modal-title').text("Thêm mới " + categoryName);
        modal.find('.type-hide').val(ecategory);
        modal.find('.modal-body .name-property').html(categoryName);
        eventModal(modal, ecategory, parent);
    });

    eventContractType();

    enableRemove();

    function googleInit() {
        var bornplace = new google.maps.places.Autocomplete($("#Employee_Bornplace")[0], { componentRestrictions: { country: 'vn' } });
        google.maps.event.addListener(bornplace, 'place_changed', function () {
            var place = bornplace.getPlace();
        });

        var addressResident = new google.maps.places.Autocomplete($("#Employee_AddressResident")[0], { componentRestrictions: { country: 'vn' } });
        google.maps.event.addListener(addressResident, 'place_changed', function () {
            var place = addressResident.getPlace();
        });

        var addressTemporary = new google.maps.places.Autocomplete($("#Employee_AddressTemporary")[0], {});
        google.maps.event.addListener(addressTemporary, 'place_changed', function () {
            var place = addressTemporary.getPlace();
        });
    }

    function eventInit() {
        $('#Employee_KhoiChucNang').on('change', function () {
            loadCategory($(this).val(), $('.ephongban-val-hide').val(), $('#Employee_PhongBan')); // parent , data
        });

        $('#Employee_PhongBan').on('change', function () {
            loadCategory($(this).val(), $('.ebophan-val-hide').val(), $('#Employee_BoPhan'));
        });

        $('#check-timekeeper').on('change', function () {
            if ($(this).is(':checked')) {
                $('input[name="Employee.IsTimeKeeper"]').val(true);
                $('.time-working').removeClass('d-none');
            } else {
                $('input[name="Employee.IsTimeKeeper"]').val(false);
                $('.time-working').addClass('d-none');
            }
        });

        $('.chkOfficial').on('change', function () {
            if ($(this).is(':checked')) {
                $('#Employee_Official').val(true);
            } else {
                $('#Employee_Official').val(false);
            }
        });

        $('#check-enable').on('change', function () {
            if ($(this).is(':checked')) {
                $('#Employee_Leave').val(true);
            } else {
                $('#Employee_Leave').val(false);
            }
        });

        $('.chkWelcomeEmailSend').on('change', function () {
            if ($(this).is(':checked')) {
                $('#IsWelcomeEmail').val(true);
                $('.welcomeEmail').removeClass('d-none');
                var phongban = $('#Employee_PhongBan').val();
                if (phongban === "") {
                    $('.welcomeToEmailList').text("Chưa chọn phòng ban! Danh sách email này dựa vào phòng ban.");
                }
                else {
                    $.ajax({
                        type: "GET",
                        url: "/api/GetWelcomeToEmails",
                        contentType: "application/json; charset=utf-8",
                        data: { PhongBan: $('#Employee_PhongBan').val() },
                        dataType: "json",
                        success: function (data) {
                            if (data.result === true) {
                                $('.welcomeToEmailList').text("TO: " + data.tohtml);
                                $('.welcomeCCEmailList').text("CC: " + data.cchtml);
                            }
                        }
                    });
                }
            } else {
                $('#IsWelcomeEmail').val(false);
                $('.welcomeEmail').addClass('d-none');
            }
        });

        $('.chkLeaveEmailSend').on('change', function () {
            if ($(this).is(':checked')) {
                $('#IsLeaveEmail').val(true);
                $('.leaveEmail').removeClass('d-none');
                var phongban = $('#Employee_PhongBan').val();
                if (phongban === "") {
                    $('.welcomeToEmailList').text("Chưa chọn phòng ban! Danh sách email này dựa vào phòng ban.");
                }
                else {
                    $.ajax({
                        type: "GET",
                        url: "/api/GetWelcomeToEmails", // dung chung
                        contentType: "application/json; charset=utf-8",
                        data: { PhongBan: $('#Employee_PhongBan').val() },
                        dataType: "json",
                        success: function (data) {
                            if (data.result === true) {
                                $('.leaveToEmailList').text("TO: " + data.tohtml);
                                $('.leaveCCEmailList').text("CC: " + data.cchtml);
                            }
                        }
                    });
                }
            } else {
                $('#IsLeaveEmail').val(false);
                $('.leaveEmail').addClass('d-none');
            }
        });

        $('.chkWelcomeEmailAll').on('change', function () {
            if ($(this).is(':checked')) {
                $('#WelcomeEmailAll').val(true);
                $.ajax({
                    type: "GET",
                    url: "/api/GetWelcomeToEmails",
                    contentType: "application/json; charset=utf-8",
                    data: { PhongBan: '', All: true },
                    dataType: "json",
                    success: function (data) {
                        console.log(data.cchtml);
                        if (data.result === true) {
                            $('.welcomeToEmailList').text("TO: " + data.tohtml);
                            $('.welcomeCCEmailList').text("CC: " + data.cchtml);
                        }
                    }
                });
            } else {
                $('#WelcomeEmailAll').val(false);
            }
        });

        $('.chkLeaveEmailAll').on('change', function () {
            if ($(this).is(':checked')) {
                $('#LeaveEmailAll').val(true);
                $.ajax({
                    type: "GET",
                    url: "/api/GetWelcomeToEmails",
                    contentType: "application/json; charset=utf-8",
                    data: { PhongBan: '', All: true },
                    dataType: "json",
                    success: function (data) {
                        console.log(data.cchtml);
                        if (data.result === true) {
                            $('.leaveToEmailList').text("TO: " + data.tohtml);
                            $('.leaveCCEmailList').text("CC: " + data.cchtml);
                        }
                    }
                });
            } else {
                $('#LeaveEmailAll').val(false);
            }
        });

        $('#check-enable').on('change', function () {
            if ($(this).is(':checked')) {
                $('input[name="Employee.Enable"]').val(false);
                $('.leave-extend').removeClass('d-none');
            } else {
                $('input[name="Employee.Enable"]').val(true);
                $('.leave-extend').addClass('d-none');
            }
        });

        $('.js-select2-basic-single').select2(
            {
                theme: "bootstrap"
            });

        $('.datepicker').on('changeDate', function () {
            var date = moment($(this).datepicker('getFormattedDate'), 'DD-MM-YYYY');
            $('.hidedatepicker', $(this).closest('.form-group')).val(
                date.format('MM-DD-YYYY')
            );
        });

        $('.multi-select-workplace').multiselect({
            nonSelectedText: '',
            nSelectedText: '',
            allSelectedText: '',
            onDropdownHide: function (event) {
                var workplaces = $('.multi-select-workplace').val();
                $('.nodeWorkplace').remove();
                var i = 0;
                workplaces.forEach(function (item) {
                    var data = [
                        {
                            "code": i,
                            "nameWorkplace": $(".multi-select-workplace option[value='" + item + "']").text(),
                            "codeWorkplace": item
                        }
                    ];
                    $('.workplace').after($.templates("#tmplWorkplace").render(data));
                    i++;
                });
            }
        });

        $('.data-form input').each(function () {
            var elem = $(this);
            // Save current value of element
            elem.data('oldVal', elem.val());

            // Look for changes in the value
            elem.bind("propertychange change click keyup input paste", function (event) {
                // If value has changed...
                if (elem.data('oldVal') !== elem.val()) {
                    // Updated stored value
                    elem.data('oldVal', elem.val());
                    // Do action
                    $('.btn-submit').prop('disabled', false);
                }
            });
        });

        $('#public').on('change', function () {
            if ($(this).is(':checked')) {
                $('input[name*="BhxhEnable"]').val(true);
                $('.enableBhxh').removeClass('d-none');
            } else {
                $('input[name*="BhxhEnable"]').val(false);
                $('.enableBhxh').addClass('d-none');
            }
        });

        $('.addMobile').click(function (e) {
            // Can remove > 1 element
            e.preventDefault();
            var code = 0;
            if ($('.codeMobile')[0]) {
                code = parseInt($('.codeMobile:last').val()) + 1;
            }

            var data = [
                {
                    "code": code
                }
            ];
            $('.more-mobile').before($.templates("#tmplMobile").render(data));
            $('.remove-item', $('.nodeMobile')).removeClass('d-none');
            enableRemoveMobileExist();
        });

        $('.addBHXH').click(function (e) {
            e.preventDefault();
            var code = 0;
            if ($('.codeBHXH')[0]) {
                code = parseInt($('.codeBHXH:last').val()) + 1;
            }

            var data = [
                {
                    "code": code
                }
            ];

            $('.more-task-bhxh').before($.templates("#tmplBHXH").render(data));
            registerDatePicker();
            enableRemove();
        });

        $('.btn-save-hospital').on('click', function () {
            var form = $(this).closest('form');
            var frmValues = form.serialize();
            $.ajax({
                type: form.attr('method'),
                url: form.attr('action'),
                data: frmValues,
                success: function (data) {
                    if (data.result === true) {
                        toastr.info(data.message);
                        // Update ddl
                        $('select[name="Employee.BhxhHospital"] option:first').after('<option value="' + data.entity.name + '">' + data.entity.name + '</option>');
                        $('select[name="Employee.BhxhHospital"]').val(data.entity.name);
                        $('input', form).val('');
                        $('textarea', form).val('');
                        $('#newHospital').modal('hide');

                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        });

        $('.task-bhxh').on('change', function () {
            $('.task-bhxh-display', $(this).closest('.node')).val($(this).find("option:selected").text());
        });

        $('.addFamily').click(function (e) {
            e.preventDefault();
            var code = 0;
            if ($('.codeFamily')[0]) {
                code = parseInt($('.codeFamily:last').val()) + 1;
            }

            var data = [
                {
                    "code": code
                }
            ];

            $('.more-family').before($.templates("#tmplFamily").render(data));
            registerDatePicker();
            enableRemove();
        });

        $('.add-contract').click(function (e) {
            e.preventDefault();
            var code = 0;
            if ($('.codeContract')[0]) {
                code = parseInt($('.codeContract:last').val()) + 1;
            }

            var data = [
                {
                    "code": code
                }
            ];
            $('.more-contract').before($.templates("#tmplContract").render(data));
            //setValue();
            eventContractType();
            registerDatePicker();
            enableRemove();
        });

        $('.addCertificate').click(function (e) {
            e.preventDefault();
            var code = 0;
            if ($('.codeCertificate')[0]) {
                code = parseInt($('.codeCertificate:last').val()) + 1;
            }

            var data = [
                {
                    "code": code
                }
            ];

            $('.more-certificate').before($.templates("#tmplCertificate").render(data));
            enableAutoSize();
            enableRemove();
        });

        $('.add-store-paper').click(function (e) {
            e.preventDefault();
            var code = 0;
            if ($('.codeStorePaper')[0]) {
                code = parseInt($('.codeStorePaper:last').val()) + 1;
            }

            var data = [
                {
                    "code": code
                }
            ];

            $('.more-store-paper').before($.templates("#tmplStorePaper").render(data));
            enableRemove();
        });

        $('.addEducation').click(function (e) {
            e.preventDefault();
            var code = 0;
            if ($('.codeEducation')[0]) {
                code = parseInt($('.codeEducation:last').val()) + 1;
            }
            var data = [
                {
                    "code": code
                }
            ];

            $('.more-education').before($.templates("#tmplEducation").render(data));
            enableAutoSize();
            enableRemove();
        });
    }

    function eventModal(modal, ecategory, parent) {
        var $curr = $('.exist-data', modal);
        $curr.empty();
        if (parent !== "") {
            $('.parent-area', modal).removeClass('d-none');
        }
        // Load current && Load value to parent-id
        $.ajax({
            type: "GET",
            url: "/api/category",
            contentType: "application/json; charset=utf-8",
            data: { type: ecategory, parentType: parent },
            dataType: "json",
            success: function (data) {
                if (data.result === true) {
                    if (data.types.length > 1) {
                        $curr.addClass('scroll-y');
                        $curr.append($("<li class='list-group-item'></li>").text('Dữ liệu hiện có:'));
                        $.each(data.types, function (key, item) {
                            $curr.append($("<li class='list-group-item'></li>").text(item.name));
                        });
                    }
                    if (parent !== "") {
                        var $pr = $('.parent-id', modal);
                        $pr.empty();
                        if (data.categories.length > 1) {
                            $pr.append($("<option></option>").attr("value", "").text("Chọn"));
                        }
                        $.each(data.categories, function (key, item) {
                            $pr.append($("<option></option>").attr("value", item.id).text(item.name));
                        });
                    }
                }
            }
        });

        $('.btn-save-category').unbind().on('click', function () {
            var form = $(this).closest('form');
            if ($('.name-input-modal', form).val() === "") {
                $('.error-message', modal).removeClass('d-none');
                $('.error-message', modal).text($('.name-property', modal).text());
            }
            var frmValues = form.serialize();
            $.ajax({
                type: form.attr('method'),
                url: form.attr('action'),
                data: frmValues,
                success: function (data) {
                    if (data.result === true) {
                        ecategory = parseInt(data.entity.type);
                        console.log("after save: " + ecategory);

                        if (ecategory === parseInt($('.egender-val-hide').val())) {
                            $('select[name="Employee.Gender"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.Gender"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.ekhoichucnang-val-hide').val())) {
                            $('select[name="Employee.KhoiChucNang"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.KhoiChucNang"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.ephongban-val-hide').val())) {
                            $('select[name="Employee.PhongBan"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.PhongBan"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.ebophan-val-hide').val())) {
                            $('select[name="Employee.BoPhan"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.BoPhan"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.echucvu-val-hide').val())) {
                            $('select[name="Employee.ChucVu"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.ChucVu"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.ecompany-val-hide').val())) {
                            $('select[name="Employee.CongTyChiNhanh"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.CongTyChiNhanh"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.ehospital-val-hide').val())) {
                            $('select[name="Employee.BhxhHospital"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.BhxhHospital"]').val(data.entity.id);
                        }
                        else if (ecategory === parseInt($('.eprobation-val-hide').val())) {
                            $('select[name="Employee.Probation"] option:first').after('<option value="' + data.entity.id + '">' + data.entity.name + '</option>');
                            $('select[name="Employee.Probation"]').val(data.entity.id);
                        }
                        $('input', form).val('');
                        $('textarea', form).val('');
                        $('#newCategoryModal').modal('hide');
                        toastr.info(data.message);
                    }
                    else {
                        $('.error-message', modal).removeClass('d-none');
                        $('.error-message', modal).text(data.message);
                    }
                }
            });
        });
    }

    function loadCategory(parentId, type, objectLoad) {
        console.log("parent:" + parentId + "; type:" + type);
        $.ajax({
            type: "GET",
            url: "/api/loadcategory",
            contentType: "application/json; charset=utf-8",
            data: { parentid: parentId, type: type },
            dataType: "json",
            success: function (data) {
                console.log(data);
                if (data.result === true) {
                    objectLoad.empty();
                    if (data.categories.length > 1) {
                        objectLoad.append($("<option></option>").attr("value", "").text("Chọn"));
                    }
                    $.each(data.categories, function (key, item) {
                        objectLoad.append($("<option></option>").attr("value", item.id).text(item.name));
                    });
                }
            }
        });
    }

    function resetForm() {
        $('#btn-save-submit').prop('disabled', false);
        $('input', $('.data-form')).prop('disabled', false);
        $('select', $('.data-form')).prop('disabled', false);
        $('textarea', $('.data-form')).prop('disabled', false);
        $('#btn-save-submit').html('Lưu');
    }

    function setValue() {
        $('.datepicker').each(function (i, obj) {
            var date = moment($(obj).datepicker('getFormattedDate'), 'DD-MM-YYYY')
            $('.hidedatepicker', $(obj).closest('.form-group')).val(
                date.format('MM-DD-YYYY')
            );
        });
    }

    function eventContractType() {
        $('.contract-type').on('change', function () {
            var parent = $(this).closest('.nodeContract');
            $('.contract-type-name', parent).val($(".contract-type option:selected", parent).text());
        });
    }

    function enableRemoveMobileExist() {
        $('.remove-item').on('click', function (e) {
            $(this).closest('.node').remove();
            // Hide remove button if only 1 item
            if ($('.nodeMobile').length === 1) {
                $('.remove-item', $('.nodeMobile')).addClass('d-none');
            }
        });
    }

    function enableAutoSize() {
        $('textarea.js-auto-size').textareaAutoSize();
    }
});

