var DatatableNoneAjaxSource = function () {
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
        var text = tableOptions.dataTable.language.metronicGroupActions;
        if (selected > 0) {
            $('.table-group-actions > span', tableWrapper).text(text.replace("_TOTAL_", selected));
        } else {
            $('.table-group-actions > span', tableWrapper).text("");
        }
    };

    return {

        //main function to initiate the module
        init: function (options) {

            if (!$().dataTable) {
                return;
            }

            the = this;

            // default settings
            options = $.extend(true, {
                src: "", // actual table  
                filterApplyAction: "filter",
                filterCancelAction: "filter_cancel",
                resetGroupActionInputOnSuccess: true,
                loadingMessage: 'Đang tải dữ liệu...',
                dataTable: {
                    //"dom": "<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                    "dom": "<'row'<'col-md-12'<'row table-group-actions-3'>>><'row'<'col-md-6 col-sm-12'<'table-group-actions'>><'col-md-6 col-sm-12 display-inline'<'display-inline pull-right'f><'table-group-actions-2 display-inline pull-right'>>r><'table-responsive't><'row'<'col-md-12 col-sm-12'pli>>",
                    "lengthMenu": [
                        [10, 20, 50, 100, 150, -1],
                        [10, 20, 50, 100, 150, "Tất cả"]//Ô chọn hiển thị số bản ghi mỗi trang
                    ],
                    "pageLength": 10, // default records per page
                    "language": { // language settings
                        // metronic spesific
                        "metronicGroupActions": "_TOTAL_ bản ghi đã chọn",
                        "metronicAjaxRequestGeneralError": "Không lấy được dữ liệu, vui lòng tải lại trang!",
                        "search": "Tìm kiếm: ",
                        "lengthMenu": "<span class='seperator'>|</span>Hiển thị _MENU_ bản ghi mỗi trang",
                        "info": "<span class='seperator'>|</span>Tổng số _TOTAL_ bản ghi",
                        "infoFiltered": "(tìm trong tổng số _MAX_ bản ghi)",
                        "infoEmpty": " Không tìm thấy bản ghi nào",
                        "emptyTable": " Không có dữ liệu",
                        "zeroRecords": " Không tìm thấy dữ liệu",
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
                    "serverSide": false, // enable/disable server side ajax loading     

                    "drawCallback": function (oSettings) { // run some code on table redraw
                        if (tableInitialized === false) { // check if table has been initialized
                            tableInitialized = true; // set table initialized
                            table.show(); // display table
                        }
                        //App.initUniform($('input[type="checkbox"]', table)); // reinitialize uniform checkboxes on each table reload
                        //App.initUniform($('input[type="checkbox"]:not(.toggle, .md-check, .md-radiobtn, .make-switch, .icheck), input[type=radio]:not(.toggle, .md-check, .md-radiobtn, .star, .make-switch, .icheck, .not-radio)', table)); // reinitialize uniform checkboxes on each table reload
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
            if ($('.table-actions-wrapper', tableContainer).size() === 1) {
                $('.table-group-actions', tableWrapper).html($('.table-actions-wrapper', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper', tableContainer).remove(); // remove the template container
            }
            if ($('.table-actions-2-wrapper', tableContainer).size() === 1) {
                $('.table-group-actions-2', tableWrapper).html($('.table-actions-2-wrapper', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-2-wrapper', tableContainer).remove(); // remove the template container
            }
            if ($('.table-actions-3-wrapper', tableContainer).size() === 1) {
                $('.table-group-actions-3', tableWrapper).html($('.table-actions-3-wrapper', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-3-wrapper', tableContainer).remove(); // remove the template container
            }
            // handle group checkboxes check/uncheck
            $('.group-checkable', table).change(function () {
                var set = table.find('tbody > tr > td:nth-child(1) input[type="checkbox"]');
                var checked = $(this).prop("checked");
                $(set).each(function () {
                    $(this).prop("checked", checked);
                });
                //$.uniform.update(set);
                countSelectedRecords();
            });

            // handle row's checkbox click
            table.on('change', 'tbody > tr > td:nth-child(1) input[type="checkbox"]', function () {
                countSelectedRecords();
            });

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