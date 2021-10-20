jQuery(document).ready(function () {
	jQuery(document).on('change', '.check-all', function () {
		if ($(this).is(':checked')) $('.checkpermission').prop('checked', true);
		else $('.checkpermission').prop('checked', false);
	});
});