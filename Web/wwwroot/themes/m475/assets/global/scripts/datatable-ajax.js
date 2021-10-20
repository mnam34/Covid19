var Datatable = function () {
    var tableOptions; // main options
    var dataTable; // datatable object
    var table; // actual table jquery object
    var tableContainer; // actual table container object
    var tableWrapper; // actual table wrapper jquery object
    var tableInitialized = false;
    var ajaxParams = {}; // set filter mode
    var the;

    var countSelectedRecords = function () {
        var selected = $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).size();
        var text = '_TOTAL_ bản ghi đã chọn';
        if (selected > 0) {
            $('.table-group-actions-0 > span', tableWrapper).text(text.replace("_TOTAL_", selected));
        } else {
            $('.table-group-actions-0 > span', tableWrapper).text("");
        }
    };

    return {
        init: function (options) {
            if (!$().dataTable) {
                return;
            }
            the = this;
            options = $.extend(true, {
                src: "", // actual table  
                filterApplyAction: "filter",
                filterCancelAction: "filter_cancel",
                resetGroupActionInputOnSuccess: true,
                loadingMessage: 'Đang tải dữ liệu...',
                dataTable: {
                    //"dom": "<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                    "dom": "<'row'<'col-md-12 col-sm-12'<'display-inline pull-right'f><'table-group-actions-1 pull-right'>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                    "bStateSave": true,
                    "fnStateSaveParams": function (oSettings, sValue) {
                        $("#datatable_ajax tr.filter .form-control").each(function () {
                            sValue[$(this).attr('name')] = $(this).val();
                        });
                        return sValue;
                    },

                    "fnStateLoadParams": function (oSettings, oData) {
                        $("#datatable_ajax tr.filter .form-control").each(function () {
                            var element = $(this);
                            if (oData[element.attr('name')]) {
                                element.val(oData[element.attr('name')]);
                            }
                        });
                        return true;
                    },
                    "lengthMenu": [
                        [10, 20, 50, 100, 150, -1],
                        [10, 20, 50, 100, 150, "Tất cả"]
                    ],
                    "pageLength": 10, // default records per page                    
                    "language": {
                        "search": "&nbsp;&nbsp;&nbsp;Tìm kiếm: ",
                        "lengthMenu": "<span class='seperator'>|</span>Hiển thị _MENU_ bản ghi mỗi trang",
                        "info": "<span class='seperator'>|</span>Tổng số _TOTAL_ bản ghi",
                        "infoFiltered": "(tìm trong tổng số _MAX_ bản ghi)",
                        "infoEmpty": " Không tìm thấy bản ghi nào",
                        "emptyTable": "Không có dữ liệu",
                        "zeroRecords": "Không tìm thấy dữ liệu",
                        "paginate": {
                            "previous": "Trang trước",
                            "next": "Trang sau",
                            "last": "Trang cuối",
                            "first": "Trang đầu",
                            "page": "Trang",
                            "pageOf": " trong tổng số"
                        }
                    },

                    "orderCellsTop": true,
                    "columnDefs": [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                        'orderable': false,
                        'targets': [0]
                    }],

                    "pagingType": "bootstrap_extended", // pagination type(bootstrap, bootstrap_full_number or bootstrap_extended)
                    "autoWidth": false, // disable fixed width and enable fluid table
                    "processing": false, // enable/disable display message box on record load
                    "serverSide": true, // enable/disable server side ajax loading

                    "ajax": { // define ajax settings
                        "url": "", // ajax URL
                        "type": "POST", // request type
                        "timeout": 2000000,
                        "data": function (data) { // add request parameters before submit
                            $.each(ajaxParams, function (key, value) {
                                data[key] = value;
                            });
                            App.blockUI({
                                message: tableOptions.loadingMessage,
                                target: tableContainer,
                                overlayColor: 'none',
                                cenrerY: true,
                                boxed: true
                            });
                        },
                        "dataSrc": function (res) { // Manipulate the data returned from the server
                            if (res.customActionMessage) {
                                App.alert({
                                    type: (res.customActionStatus == 'OK' ? 'success' : 'danger'),
                                    icon: (res.customActionStatus == 'OK' ? 'check' : 'warning'),
                                    message: res.customActionMessage,
                                    container: tableWrapper,
                                    place: 'prepend'
                                });
                            }

                            if (res.customActionStatus) {
                                if (tableOptions.resetGroupActionInputOnSuccess) {
                                    $('.table-group-action-input', tableWrapper).val("");
                                }
                            }

                            if ($('.group-checkable', table).size() === 1) {
                                $('.group-checkable', table).attr("checked", false);
                            }

                            if (tableOptions.onSuccess) {
                                tableOptions.onSuccess.call(undefined, the, res);
                            }

                            App.unblockUI(tableContainer);

                            return res.data;
                        },
                        "error": function (jqXHR, textStatus, errorThrown) { // handle general connection errors
                            if (tableOptions.onError) {
                                tableOptions.onError.call(undefined, the);
                            }

                            App.alert({
                                type: 'danger',
                                icon: 'warning',
                                message: errorThrown,
                                container: tableWrapper,
                                place: 'prepend'
                            });

                            App.unblockUI(tableContainer);
                        }
                    },

                    "drawCallback": function (oSettings) { // run some code on table redraw
                        if (tableInitialized === false) { // check if table has been initialized
                            tableInitialized = true; // set table initialized
                            table.show(); // display table
                        }
                        countSelectedRecords(); // reset selected records indicator

                        // callback for ajax data load
                        if (tableOptions.onDataLoad) {
                            tableOptions.onDataLoad.call(undefined, the);
                        }
                    }
                }
            }, options);

            tableOptions = options;

            // create table's jquery object
            table = $(options.src);
            tableContainer = table.parents(".table-container");

            // apply the special class that used to restyle the default datatable
            var tmp = $.fn.dataTableExt.oStdClasses;

            $.fn.dataTableExt.oStdClasses.sWrapper = $.fn.dataTableExt.oStdClasses.sWrapper + " dataTables_extended_wrapper";
            $.fn.dataTableExt.oStdClasses.sFilterInput = "form-control input-xs input-sm input-inline";
            $.fn.dataTableExt.oStdClasses.sLengthSelect = "form-control input-xs input-sm input-inline";

            // initialize a datatable
            dataTable = table.DataTable(options.dataTable);

            // revert back to default
            $.fn.dataTableExt.oStdClasses.sWrapper = tmp.sWrapper;
            $.fn.dataTableExt.oStdClasses.sFilterInput = tmp.sFilterInput;
            $.fn.dataTableExt.oStdClasses.sLengthSelect = tmp.sLengthSelect;

            // get table wrapper
            tableWrapper = table.parents('.dataTables_wrapper');
            //tableWrapper.find(".dataTables_length select").select2({
            //    showSearchInput: false
            //});
            // build table group actions panel
            if ($('.table-actions-wrapper-0', tableContainer).size() === 1) {
                $('.table-group-actions-0', tableWrapper).html($('.table-actions-wrapper-0', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper-0', tableContainer).remove(); // remove the template container
            }
            if ($('.table-actions-wrapper-1', tableContainer).size() === 1) {
                $('.table-group-actions-1', tableWrapper).html($('.table-actions-wrapper-1', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper-1', tableContainer).remove(); // remove the template container

            }
            if ($('.table-actions-wrapper-2', tableContainer).size() === 1) {
                $('.table-group-actions-2', tableWrapper).html($('.table-actions-wrapper-2', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper-2', tableContainer).remove(); // remove the template container
                //$('.table-group-actions-2', tableWrapper).find(".filter-1").select2({
                //    showSearchInput: false
                //});
            }
            if ($('.table-actions-wrapper-3', tableContainer).size() === 1) {
                $('.table-group-actions-3', tableWrapper).html($('.table-actions-wrapper-3', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper-3', tableContainer).remove(); // remove the template container
            }
            if ($('.table-actions-wrapper-4', tableContainer).size() === 1) {
                $('.table-group-actions-4', tableWrapper).html($('.table-actions-wrapper-4', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper-4', tableContainer).remove(); // remove the template container
            }
            // handle group checkboxes check/uncheck
            $('.group-checkable', table).change(function () {
                var set = table.find('tbody > tr > td:nth-child(1) input[type="checkbox"]');
                var checked = $(this).prop("checked");
                $(set).each(function () {
                    $(this).prop("checked", checked);
                });

                countSelectedRecords();
            });

            // handle row's checkbox click
            table.on('change', 'tbody > tr > td:nth-child(1) input[type="checkbox"]', function () {
                countSelectedRecords();
            });

            // handle filter submit button click
            table.on('click', '.filter-submit', function (e) {
                e.preventDefault();
                the.submitFilter();
            });

            // handle filter cancel button click
            table.on('click', '.filter-cancel', function (e) {
                e.preventDefault();
                the.resetFilter();
            });
            table.on('search.dt', function () {
                if ($('.filter-1'))
                    the.setAjaxParam('filter-1', $('.filter-1').val());
                if ($('.filter-2'))
                    the.setAjaxParam('filter-2', $('.filter-2').val());
                if ($('.filter-3'))
                    the.setAjaxParam('filter-3', $('.filter-3').val());
                if ($('.filter-4'))
                    the.setAjaxParam('filter-4', $('.filter-4').val());
                if ($('.filter-5'))
                    the.setAjaxParam('filter-5', $('.filter-5').val());
                if ($('.filter-6'))
                    the.setAjaxParam('filter-6', $('.filter-6').val());
            });
            table.on('order.dt', function () {
                if ($('.filter-1'))
                    the.setAjaxParam('filter-1', $('.filter-1').val());
                if ($('.filter-2'))
                    the.setAjaxParam('filter-2', $('.filter-2').val());
                if ($('.filter-3'))
                    the.setAjaxParam('filter-3', $('.filter-3').val());
                if ($('.filter-4'))
                    the.setAjaxParam('filter-4', $('.filter-4').val());
                if ($('.filter-5'))
                    the.setAjaxParam('filter-5', $('.filter-5').val());
                if ($('.filter-6'))
                    the.setAjaxParam('filter-6', $('.filter-6').val());
            });
            table.on('page.dt', function () {
                if ($('.filter-1'))
                    the.setAjaxParam('filter-1', $('.filter-1').val());
                if ($('.filter-2'))
                    the.setAjaxParam('filter-2', $('.filter-2').val());
                if ($('.filter-3'))
                    the.setAjaxParam('filter-3', $('.filter-3').val());
                if ($('.filter-4'))
                    the.setAjaxParam('filter-4', $('.filter-4').val());
                if ($('.filter-5'))
                    the.setAjaxParam('filter-5', $('.filter-5').val());
                if ($('.filter-6'))
                    the.setAjaxParam('filter-6', $('.filter-6').val());
            });
            table.on('length.dt', function () {
                if ($('.filter-1'))
                    the.setAjaxParam('filter-1', $('.filter-1').val());
                if ($('.filter-2'))
                    the.setAjaxParam('filter-2', $('.filter-2').val());
                if ($('.filter-3'))
                    the.setAjaxParam('filter-3', $('.filter-3').val());
                if ($('.filter-4'))
                    the.setAjaxParam('filter-4', $('.filter-4').val());
                if ($('.filter-5'))
                    the.setAjaxParam('filter-5', $('.filter-5').val());
                if ($('.filter-6'))
                    the.setAjaxParam('filter-6', $('.filter-6').val());
            });
        },

        submitFilter: function () {
            the.setAjaxParam("action", tableOptions.filterApplyAction);

            // get all typeable inputs
            $('textarea.form-filter, select.form-filter, input.form-filter:not([type="radio"],[type="checkbox"])', table).each(function () {
                the.setAjaxParam($(this).attr("name"), $(this).val());
            });

            // get all checkboxes
            $('input.form-filter[type="checkbox"]:checked', table).each(function () {
                the.addAjaxParam($(this).attr("name"), $(this).val());
            });

            // get all radio buttons
            $('input.form-filter[type="radio"]:checked', table).each(function () {
                the.setAjaxParam($(this).attr("name"), $(this).val());
            });

            dataTable.ajax.reload();
        },

        resetFilter: function () {
            $('textarea.form-filter, select.form-filter, input.form-filter', table).each(function () {
                $(this).val("");
            });
            $('input.form-filter[type="checkbox"]', table).each(function () {
                $(this).attr("checked", false);
            });
            the.clearAjaxParams();
            the.addAjaxParam("action", tableOptions.filterCancelAction);
            dataTable.ajax.reload();
        },

        getSelectedRowsCount: function () {
            return $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).size();
        },

        getSelectedRows: function () {
            var rows = [];
            $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).each(function () {
                rows.push($(this).val());
            });

            return rows;
        },

        setAjaxParam: function (name, value) {
            ajaxParams[name] = value;
        },

        addAjaxParam: function (name, value) {
            if (!ajaxParams[name]) {
                ajaxParams[name] = [];
            }

            skip = false;
            for (var i = 0; i < (ajaxParams[name]).length; i++) { // check for duplicates
                if (ajaxParams[name][i] === value) {
                    skip = true;
                }
            }

            if (skip === false) {
                ajaxParams[name].push(value);
            }
        },

        clearAjaxParams: function (name, value) {
            ajaxParams = {};
        },

        getDataTable: function () {
            return dataTable;
        },

        getTableWrapper: function () {
            return tableWrapper;
        },

        gettableContainer: function () {
            return tableContainer;
        },

        getTable: function () {
            return table;
        }

    };

};