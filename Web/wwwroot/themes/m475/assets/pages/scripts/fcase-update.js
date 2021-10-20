jQuery(document).ready(function () {
    $('#ProvinceId').select2({
        placeholder: "---Vui lòng chọn tỉnh/thành phố---",
        allowClear: true
    });
    $('#DistrictId').select2({
        placeholder: "---Vui lòng chọn quận/huyện---",
        allowClear: true
    });
    $('#CommuneId').select2({
        placeholder: "---Vui lòng chọn phường/xã---",
        allowClear: true
    });

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

    $(document).on('change', '#ProvinceId', function (e) {
        e.preventDefault();
        GetDistrictByProvince($(this).val(), 'DistrictId', 'CommuneId', 'EpidemicAreaId');
    });
    $(document).on('change', '#DistrictId', function (e) {
        e.preventDefault();
        GetCommuneByDistrict($(this).val(), 'CommuneId', 'EpidemicAreaId');
    });
    $(document).on('change', '#CommuneId', function (e) {
        e.preventDefault();
        GetEpidemicAreaByCommune($(this).val(), 'EpidemicAreaId');
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