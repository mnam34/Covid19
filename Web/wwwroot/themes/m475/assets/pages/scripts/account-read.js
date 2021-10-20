var currentAccountId = $('#CurrentAccountId').val();

jQuery(document).ready(function () {
    
    $(document).on('click', '.select-image', function () {
        $('#file').click();
    });
    $('input:file').change(function (e) {
        $('#CheckValidFileType').val('');
        var files = e.originalEvent.target.files;
        for (var i = 0, len = files.length; i < len; i++) {
            var n = files[i].name,
                s = files[i].size,
                t = files[i].type;
            var ext = n.split('.').pop().toLowerCase();
            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'bmp']) == -1) {
                var msg1 = 'Ảnh không hợp lệ! \nChỉ chấp nhận các loại tệp tin ảnh: gif, png, jpg, jpeg, bmp\nVui lòng chọn ảnh khác!';
                $('#CheckValidFileType').val(msg1);
                //alert(msg1);
                swal(msg1);
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#Image_Preview').attr('src', e.target.result);
            }
            reader.readAsDataURL(files[i]);
        }
    });
    $('#main_form').on('submit', function (e) {
        if ($('#CheckValidFileType').val() != '') {
            //alert($('#CheckValidFileType').val());
            swal($('#CheckValidFileType').val());
            e.preventDefault();
        }
    });
    //Cập nhật thông tin tài khoản
    $(document).on('click', '.btn-update', function () {
        App.blockUI({
            message: 'Vui lòng đợi!',
            overlayColor: 'none',
            cenrerY: true,
            boxed: true,
            zIndex: 999999999
        });
        var $url = '/sm/account/update/' + currentAccountId;
        //Dùng FormData để upload file
        var formData = new FormData($('.form-update-account')[0]);
        $.ajax({
            type: 'POST',
            cache: false,
            async: true,
            url: $url,
            data: formData,
            dataType: 'json',
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    swal({
                        title: result.message,
                        timer: 3000,
                        icon: "success",
                    });
                    setTimeout("location.reload(true);", 1000);
                }
                else
                    swal({
                        title: result.message,
                        icon: "error",
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({
                    title: errorThrown,
                    icon: "error",
                });
            },
            complete: function (result) {
                App.unblockUI();
            }
        });
        return;
    });
    //Xóa tài khoản
    $(document).on('click', '.delete-account', function (e) {
        e.preventDefault();
        swal({
            title: "Cảnh báo!",
            text: "Quí vị có chắc chắn muốn xóa tài khoản này không?",
            icon: "error",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    App.blockUI({
                        message: 'Vui lòng đợi!',
                        overlayColor: 'none',
                        cenrerY: true,
                        boxed: true,
                        zIndex: 999999999
                    });
                    
                    $.ajax({
                        type: 'POST',
                        url: '/sm/account/delete/' + currentAccountId,
                        dataType: 'json',
                        data: $('.form-account-delete').serialize(),
                        success: function (result) {
                            if (result.success == true) {
                                swal({
                                    title: result.message,
                                    timer: 3000,
                                    icon: "success",
                                });
                            }
                            else {
                                swal({
                                    title: result.message,
                                    icon: "error",
                                });
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            swal({
                                title: 'Đã xảy ra lỗi khi thực hiện yêu cầu của quí vị. Vui lòng kiểm tra và thử lại!\nLỗi: ' + errorThrown,
                                icon: "error",
                            });
                        },
                        complete: function (result) {
                            App.unblockUI();
                            setTimeout(function () {
                                window.location = '/sm/account';
                            }, 1000);
                           
                        }
                    });
                } else {

                }
            });
    });
    //Đổi mật khẩu cho thành viên
    $(document).on('click', '.btn-change-pw', function () {
        App.blockUI({
            message: 'Vui lòng đợi!',
            overlayColor: 'none',
            cenrerY: true,
            boxed: true,
            zIndex: 999999999
        });
        var $url = '/sm/account/change-password/' + currentAccountId;
        $.ajax({
            type: 'POST',
            cache: false,
            async: true,
            url: $url,
            data: $('.form-change-member-password').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    swal({
                        title: result.message,
                        timer: 3000,
                        icon: "success",
                    });
                }
                else
                    swal({
                        title: result.message,
                        icon: "error",
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({
                    title: errorThrown,
                    icon: "error",
                });
            },
            complete: function (result) {
                App.unblockUI();
                setTimeout("location.reload(true);", 1000);
            }
        });
        return;
    });
    //Phân quyền
    $(document).on('click', '.btn-account-mapping-role', function () {
        App.blockUI({
            message: 'Vui lòng đợi!',
            overlayColor: 'none',
            cenrerY: true,
            boxed: true,
            zIndex: 999999999
        });
        var roles = $('#tab_1_2 input:checkbox:checked').map(function () {
            return $(this).attr('data-roleid');
        }).get();

        var $url = '/sm/account/mapping-role?accountId=' + currentAccountId + '&roles=' + roles;

        $.ajax({
            type: 'POST',
            cache: false,
            async: true,
            url: $url,
            data: $('.form-account-mapping-role').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    swal({
                        title: result.message,
                        timer: 3000,
                        icon: "success",
                    });
                }
                else
                    swal({
                        title: result.message,
                        icon: "error",
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({
                    title: errorThrown,
                    icon: "error",
                });
            },
            complete: function (result) {
                App.unblockUI();
            }
        });
        return;
    });
    //Phân quyền người dùng đơn vị: TDCV
    $(document).on('click', '.btn-account-mapping-division-wt', function () {
        App.blockUI({
            message: 'Vui lòng đợi!',
            overlayColor: 'none',
            cenrerY: true,
            boxed: true,
            zIndex: 999999999
        });
        var donVis = $('#tab_1_4 input:checkbox:checked').map(function () {
            //return $(this).attr('data-madonvi');
            return $(this).attr('data-orgid');
        }).get();
        //alert(donVis)
        var $url = '/sm/account/mapping-wt?accountId=' + currentAccountId + '&DivisionId=' + donVis;
        $.ajax({
            type: 'POST',
            cache: false,
            async: true,
            url: $url,
            data: $('.form-account-mapping-division-wt').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    swal({
                        title: result.message,
                        timer: 3000,
                        icon: "success",
                    });
                }
                else
                    swal({
                        title: result.message,
                        icon: "error",
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                swal({
                    title: errorThrown,
                    icon: "error",
                });
            },
            complete: function (result) {
                App.unblockUI();
                setTimeout("location.reload(true);", 1000);
            }
        });
        return;
    });
});