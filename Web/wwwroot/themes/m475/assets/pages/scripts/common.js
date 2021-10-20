function GetDistrictByProvince(id, _container, _container2, _container3) {
    var URL = '/shared/get-district-by-province/' + id;
    var items = "<option value=''></option>";
    $('#' + _container).html(items);
    if (_container2 != '')
        $('#' + _container2).html(items);
    if (_container3 != '')
        $('#' + _container3).html(items);
    if (id != '') {
        $.getJSON(URL, function (data) {
            $.each(data, function (i, item) {
                items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#' + _container).html(items);
            $('#' + _container).select2({
                placeholder: "--Vui lòng chọn quận/huyện--",
                allowClear: true,
            });
        });
    }
};
function GetCommuneByDistrict(id, _container, _container2) {
    var URL = '/shared/get-commune-by-district/' + id;
    var items = "<option value=''></option>";
    $('#' + _container).html(items);
    if (_container2 != '')
        $('#' + _container2).html(items);
    if (id != '') {
        $.getJSON(URL, function (data) {
            $.each(data, function (i, item) {
                items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#' + _container).html(items);
            $('#' + _container).select2({
                placeholder: "--Vui lòng chọn phường/xã--",
                allowClear: true,
            });
        });
    }
};
function GetEpidemicAreaByCommune(id, _container) {
    var URL = '/shared/get-epidemic-area-by-commune/' + id;
    var items = "<option value=''></option>";
    $('#' + _container).html(items);
    if (id != '') {
        $.getJSON(URL, function (data) {
            $.each(data, function (i, item) {
                items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#' + _container).html(items);
            $('#' + _container).select2({
                placeholder: "--Vui lòng chọn Khu vực/vùng/điểm dịch--",
                allowClear: true,
            });
        });
    }
};
jQuery(document).ready(function () {
    //$(document).on('click', '.tab_5_1', function () {
    //    location.hash = 'tab_5_1';
    //});
    //$(document).on('click', '.tab_5_2', function () {
    //    location.hash = 'tab_5_2';
    //});
    //$(document).on('click', '.tab_5_3', function () {
    //    location.hash = 'tab_5_3';
    //});
    //$(document).on('click', '.tab_5_4', function () {
    //    location.hash = 'tab_5_4';
    //});
    //$(document).on('click', '.tab_5_5', function () {
    //    location.hash = 'tab_5_5';
    //});
    //$(document).on('click', '.tab_5_6', function () {
    //    location.hash = 'tab_5_6';
    //});
    if (jQuery().datepicker) {
        $('.date-picker').datepicker({
            rtl: App.isRTL(),
            orientation: "left",
            autoclose: true,
            format: 'dd/mm/yyyy',
            language: 'vi'
        });
    };

});