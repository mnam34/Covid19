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
    $(document).on('change', '#ProvinceId', function (e) {
        e.preventDefault();
        GetDistrictByProvince($(this).val(), 'DistrictId', 'CommuneId', '');
    });
    $(document).on('change', '#DistrictId', function (e) {
        e.preventDefault();
        GetCommuneByDistrict($(this).val(), 'CommuneId', '');
    });
});