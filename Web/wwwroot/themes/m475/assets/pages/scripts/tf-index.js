var TableDatatablesNoneAjax = function () {
    var handleRecords = function () {
        var grid = new DatatableNoneAjaxSource();
        grid.init({
            src: $("#datatable_none_ajax_source"),
            onSuccess: function (grid) {
            },
            onError: function (grid) {
            },
            loadingMessage: 'Đang tải dữ liệu...',
            dataTable: {
                //"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
                //"dom": "<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                //"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable
                "dom": "<'row'<'col-md-12 col-sm-12'<'display-inline pull-right'f><'table-group-actions-2 pull-right'>>r><'row'<'col-md-12 col-sm-12'<'table-group-actions-1 pull-right'>>r><t><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
                "lengthMenu": [
                    [15, 25, 50, 100, 150, -1],
                    [15, 25, 50, 100, 150, "Tất cả"]//Ô chọn hiển thị số bản ghi mỗi trang
                ],
                "pageLength": 15, // mặc định số bản ghi mỗi trang
                "bSort": false,
                "order": [
                    [4, "desc"]
                ],
                "columnDefs": [{
                    'orderable': false,
                    "searchable": false,
                    'targets': [0, -1]
                }],
                //"columnDefs": [{
                //    "targets": -1,
                //    "searchable": false,
                //    'orderable': false,
                //}],
                buttons: [
                    {
                        extend: 'print', className: 'btn dark btn-outline', exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    {
                        extend: 'copy', className: 'btn red btn-outline', exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    {
                        extend: 'pdf', className: 'btn green btn-outline', exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    {
                        extend: 'excel', className: 'btn yellow btn-outline ', exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    {
                        extend: 'csv', className: 'btn purple btn-outline ', exportOptions: {
                            columns: [0, 1, 2, 3, 4]
                        }
                    },
                    //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
                ],
            }
        });
        $('#datatable_none_ajax_source_tools > li > a.tool-action').on('click', function () {
            var action = $(this).attr('data-action');
            grid.getDataTable().button(action).trigger();
        });
        grid.getTableWrapper().on('click', '.delete', function (e) {
            e.preventDefault(); swal({
                title: "Cảnh báo!",
                text: "Quí vị có chắc chắn muốn xóa Cơ sở điều trị bệnh này không?",
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
                            url: '/sm/tf/delete/' + id,
                            dataType: 'json',
                            data: $('#frm_token').serialize(),
                            success: function (result) {
                                if (result.success == true) {
                                    swal({
                                        title: 'Đã xóa Cơ sở điều trị bệnh thành công!',
                                        timer: 3000,
                                        icon: 'success'
                                    });
                                    grid.getDataTable().row(nRow).remove().draw();
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
    }
    return {
        init: function () {
            handleRecords();
        }
    };
}();
jQuery(document).ready(function () {
    TableDatatablesNoneAjax.init();
});