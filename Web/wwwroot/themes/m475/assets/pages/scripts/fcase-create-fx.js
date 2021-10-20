jQuery(document).ready(function () {   
    $('#AddressProvinceId').select2({
        placeholder: "---Vui lòng chọn tỉnh/thành phố---",
        allowClear: true,
        width: 150
    });
    $('#AddressDistrictId').select2({
        placeholder: "---Vui lòng chọn quận/huyện---",
        allowClear: true,
        width: 180
    });
    $('#AddressCommuneId').select2({
        placeholder: "---Vui lòng chọn phường/xã---",
        allowClear: true,
        width: 180
    });

    $(document).on('change', '#AddressProvinceId', function (e) {
        e.preventDefault();
        GetDistrictByProvince($(this).val(), 'AddressDistrictId', 'AddressCommuneId', '');
    });
    $(document).on('change', '#AddressDistrictId', function (e) {
        e.preventDefault();
        GetCommuneByDistrict($(this).val(), 'AddressCommuneId', '');
    });

});