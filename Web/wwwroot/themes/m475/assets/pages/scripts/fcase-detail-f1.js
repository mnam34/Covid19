var FxTable = function () {
    var initF2Table = function () {
        var table = $('#f2_table');
        var oTable = table.dataTable({
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "Không có dữ liệu",
                "info": "Hiển thị từ _START_ đến _END_ trong tổng số _TOTAL_ F2",
                "infoEmpty": "Không có dữ liệu",
                "infoFiltered": "(lọc trong tổng số _MAX_ F2)",
                "lengthMenu": "Hiển thị _MENU_ F2 mỗi trang",
                "search": "Tìm kiếm:",
                "zeroRecords": "Không có dữ liệu",
                buttons: {
                    copyTitle: 'Copy danh sách F2',
                    copySuccess: {
                        1: "Đã copy 1 F2",
                        _: "Đã copy %d F2"
                    },
                }
            },
            "bSort": false,
            "bStateSave": false,
            responsive: false,
            "ordering": false, //disable column ordering 
            "autoWidth": false,
            buttons: [
                {
                    extend: 'excel', title: 'Danh sách F2',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                    }
                },
                {
                    extend: 'csv', title: 'Danh sách F2',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                    }
                },
                {
                    extend: 'pdf', title: 'Danh sách F2',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                    }
                },
                {
                    extend: 'print', title: 'Danh sách F2',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                    }
                },
                {
                    extend: 'copy', title: 'Danh sách F2',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                    }
                },
            ],
            //"paging": false, disable pagination
            "order": [
                [0, 'asc']
            ],
            "lengthMenu": [
                [20, 50, 100, 200, 1000, -1],
                [20, 50, 100, 200, 1000, "All"] // change per page values here
            ],
            "pageLength": 50,
            //"dom": "<'row' <'col-md-12'>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
        // handle datatable custom tools
        $('#f2_table_tools > li > a.tool-action').on('click', function () {
            var action = $(this).attr('data-action');
            oTable.DataTable().button(action).trigger();
        });
    }
    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }
            initF2Table();
        }
    };
}();
jQuery(document).ready(function () {
    var fcaseId = $('#CurrentFCaseId').val();
    FxTable.init();
    //#region fileupload
    $('#fileupload').fileupload({
        url: '/shared/fcase/upload-files/' + fcaseId,
        autoUpload: true,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|bmp|tif|gif|xls|xlsx|doc|docx|pdf|txt|zip|rar|7z|ppt|pptx)$/i,

    }).bind('fileuploadchange', function (e, data) {
        //alert('d');
    }).bind('fileuploadadd', function (e, data) {
        //alert('d2');
    }).bind('fileuploaddestroy', function (e, data) {
        //alert(data.deleteUrl)
    }).bind('fileuploaddone', function (e, data) {
        //alert(data.url)
    });

    $(document).on('click', '.delete-file', function (e) {
        var nRow = $(this).parents('tr')[0];
        var id = $(nRow).attr('data-objectid');
        $.ajax({
            type: 'POST',
            url: '/shared/fcase/delete-file-2/' + id,
            dataType: 'json',
            success: function (result) {
                if (result.success == true) {
                    nRow.remove();
                }
                else {
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function (result) {
            }
        });
    });
    //#endregion
    //#region Xác nhận đã trở thành F0
    $(document).on('click', '.btn-modal-submit-fcase-confirm-f0', function (e) {
        swal({
            title: "Cảnh báo!",
            text: "Bạn có chắc chắn xác nhận không?",
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
                    var $url = '/sm/fcase/confirm-f0-post';
                    var formData = new FormData($('.frm-modal-form-fcase-confirm-f0')[0]);
                    $.ajax({
                        type: 'POST',
                        cache: false,
                        async: true,
                        url: $url,
                        data: formData,
                        dataType: 'json',
                        contentType: false,
                        processData: false,
                        success: function (result) {
                            if (result.success) {
                                swal({
                                    title: result.message,
                                    timer: 3000,
                                    icon: "success",
                                });

                                $('.modal-header-fcase-confirm-f0 .close').click();
                                setTimeout("location.reload(true);", 1000);
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
                                title: 'Đã xảy ra lỗi khi thực hiện yêu cầu của quí vị. Vui lòng kiểm tra và thử lại! Lỗi: ' + errorThrown,
                                icon: "error",
                            });
                        },
                        complete: function (result) {
                            App.unblockUI();

                        }
                    });
                } else {

                }
            });
    });
    //#endregion
    //#region Nhập kết quả xét nghiệm
    $(document).on('click', '.btn-modal-submit-test-result', function (e) {
        App.blockUI({
            message: 'Vui lòng đợi!',
            overlayColor: 'none',
            cenrerY: true,
            boxed: true,
            zIndex: 999999999
        });
        var $url = '/sm/fcase/test-result-post';
        var formData = new FormData($('.frm-modal-form-test-result')[0]);
        $.ajax({
            type: 'POST',
            cache: false,
            async: true,
            url: $url,
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    swal({
                        title: result.message,
                        timer: 3000,
                        icon: "success",
                    });

                    $('.modal-header-test-result .close').click();
                    window.location.hash = '#tab_5_2';
                    setTimeout("location.reload(true);", 1000);
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
                    title: 'Đã xảy ra lỗi khi thực hiện yêu cầu của quí vị. Vui lòng kiểm tra và thử lại! Lỗi: ' + errorThrown,
                    icon: "error",
                });
            },
            complete: function (result) {
                App.unblockUI();

            }
        });
    });
    //#endregion
    //#region Xóa kết quả xét nghiệm
    $(document).on('click', '.delete-test-result', function (e) {
        e.preventDefault(); swal({
            title: "Cảnh báo!",
            text: "Quí vị có chắc chắn muốn xóa kết quả không?",
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
                    var nRow = $(this).parents('tr')[0];
                    var id = $(nRow).attr('data-objectid');
                    $.ajax({
                        type: 'POST',
                        url: '/sm/fcase/test-result-delete/' + id,
                        dataType: 'json',
                        data: $('#frm_token').serialize(),
                        success: function (result) {
                            if (result.success == true) {
                                swal({
                                    title: 'Đã xóa Kết quả thành công!',
                                    timer: 3000,
                                    icon: 'success'
                                });
                                nRow.remove();
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
                                title: 'Đã xảy ra lỗi khi thực hiện yêu cầu của quí vị. Vui lòng kiểm tra và thử lại!<br />Lỗi: ' + errorThrown,
                                icon: "error",
                            });
                        },
                        complete: function (result) {
                            App.unblockUI();
                        }
                    });
                } else {

                }
            });
    });
    //#endregion
    //#region Cập nhật kết quả xét nghiệm
    $(document).on('click', '.btn-modal-submit-test-result-update', function (e) {
        App.blockUI({
            message: 'Vui lòng đợi!',
            overlayColor: 'none',
            cenrerY: true,
            boxed: true,
            zIndex: 999999999
        });
        var $url = '/sm/fcase/test-result-update-post';
        var formData = new FormData($('.frm-modal-form-test-result-update')[0]);
        $.ajax({
            type: 'POST',
            cache: false,
            async: true,
            url: $url,
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    swal({
                        title: result.message,
                        timer: 3000,
                        icon: "success",
                    });

                    $('.modal-header-test-result-update .close').click();
                    window.location.hash = '#tab_5_2';
                    setTimeout("location.reload(true);", 1000);
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
                    title: 'Đã xảy ra lỗi khi thực hiện yêu cầu của quí vị. Vui lòng kiểm tra và thử lại! Lỗi: ' + errorThrown,
                    icon: "error",
                });
            },
            complete: function (result) {
                App.unblockUI();

            }
        });
    });
    //#endregion
    //#region Xóa trường hợp Fx
    $(document).on('click', '.delete-fx', function (e) {
        e.preventDefault(); swal({
            title: "Cảnh báo!",
            text: "Quí vị có chắc chắn muốn xóa không?",
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
                    var nRow = $(this).parents('tr')[0];
                    var id = $(nRow).attr('data-objectid');
                    $.ajax({
                        type: 'POST',
                        url: '/sm/fcase/delete-fx/' + id,
                        dataType: 'json',
                        data: $('#frm_token').serialize(),
                        success: function (result) {
                            if (result.success == true) {
                                swal({
                                    title: 'Đã xóa thành công!',
                                    timer: 3000,
                                    icon: 'success'
                                });
                                nRow.remove();
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
                                title: 'Đã xảy ra lỗi khi thực hiện yêu cầu của quí vị. Vui lòng kiểm tra và thử lại!<br />Lỗi: ' + errorThrown,
                                icon: "error",
                            });
                        },
                        complete: function (result) {
                            App.unblockUI();
                        }
                    });
                } else {

                }
            });
    });
    //#endregion

});