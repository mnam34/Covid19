$(document).ready(function () {
   var table= $('#datatable_none_ajax_source').DataTable(
        {
            //dom: 'Bfrtip',
           dom: '',
            buttons: [
                {
                    extend: 'excel', title: 'Danh sách F0', exportOptions: {
                        columns: ':visible'
                    }
                },
           ],
           columnDefs: [
               {
                   targets: 0,
                   orderable: false,
               }
           ],
        }
    );
    $(document).on('click', '.tool-action',function () {
        var action = $(this).attr('data-action');
        table.button(action).trigger();
    });
});