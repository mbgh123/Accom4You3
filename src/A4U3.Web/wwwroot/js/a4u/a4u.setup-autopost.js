
// automatically post back on filter changes

$(function () {
	$('#Bedrooms').on("change", function () {
		$('form').submit();
	});
});

$(function () {
	$('#RentMax').on("change", function () {
		$('form').submit();
	});
});

$(function () {
	$('#SortOrder').on("change", function () {
		$('form').submit();
	});
});
