var canDelete = $('#hid_canDelete').val();
var TableDatatablesAjax = function () {
    var handleRecords = function () {
        var grid = new Datatable();
        grid.init({
            src: $("#datatable_ajax"),
            onSuccess: function (grid, response) {
            },
            onError: function (grid) {
            },
            onDataLoad: function (grid) {
            },
            dataTable: {
                "bSort": false,
                //"dom": "<'row'<'col-md-12 col-sm-12'<'display-inline pull-right'f><'table-group-actions-2 pull-right'>>r><'row'<'col-md-12 col-sm-12'<'table-group-actions-1 pull-right'>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                "dom": "<'row'<'col-md-12 col-sm-12'<'display-inline pull-right'f><'table-group-actions-2 pull-right'>>r><'row'<'col-md-12 col-sm-12'<'table-group-actions-1 pull-right'>>r><t><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                "ajax": {
                    "url": "/sm/summary/assigned-role-json",
                },
                "columnDefs": [{
                    "targets": -1,
                    'orderable': false,
                    "data": null,
                    "render": function (data, type, full) {
                        var val = '';
                        if (canDelete == 'true') {
                            val += '<a class="delete btn default btn-xs red-flamingo" href="javascript:;" data-objectid="' + data.Id + '"><i class="fa fa-trash-o"></i> Xóa</a>';
                        }
                        return val;
                    },
                },
                {
                    "targets": 0,
                    'orderable': false,
                }
                ],
                "columns": [

                    { data: "STT" },
                    { data: "AccountName" },
                    { data: "LoginName" },
                    { data: "Email" },
                    
                    { data: "RoleCode" },
                    { data: "RoleName" },
                    { data: "CreateDate" },
                    { data: "CreateBy" },
                    { data: "AccessRight" },
                    { data: null }
                ],
                "order": [
                    [1, "asc"]
                ],
                buttons: [
                    {
                        extend: 'print', className: 'btn dark btn-outline', exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                    {
                        extend: 'copy', className: 'btn red btn-outline', exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                    {
                        extend: 'pdf', className: 'btn green btn-outline', exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                    {
                        extend: 'excel', className: 'btn yellow btn-outline ', exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                    {
                        extend: 'csv', className: 'btn purple btn-outline ', exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                        }
                    },
                ]
            }
        });
        $('#datatable_ajax_tools > li > a.tool-action').on('click', function () {
            var action = $(this).attr('data-action');
            grid.getDataTable().button(action).trigger();
        });
        grid.getTableWrapper().on('change', '.filter-1', function (e) {
            e.preventDefault();
            grid.setAjaxParam("filter-1", $(this).val());
            grid.getDataTable().ajax.reload();
            grid.clearAjaxParams();
        });
        //Xóa/hủy phân quyền
        grid.getTableWrapper().on('click', '.delete', function (e) {
            e.preventDefault();
            swal({
                title: "Cảnh báo!",
                text: "Quí vị có chắc chắn muốn xóa phân quyền không?",
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
                        var id = $(this).attr('data-objectid');
                        $.ajax({
                            type: 'POST',
                            url: '/sm/summary/assigned-role-delete-permanent/' + id,
                            dataType: 'json',
                            data: $('#frm_token').serialize(),
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
                                grid.getDataTable().ajax.reload();
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
    TableDatatablesAjax.init();
    $('#Roles').select2({
        placeholder: "---Tất cả nhóm quyền---",
        allowClear: true
    });
});