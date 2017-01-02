(function ($) {
	'use strict';

	var getBudgetUrl = '/Wallet/GetBudget';
	var currencyCode = 'zł';
    var $box = $('.budget-infobox');

    $box.on('box.refresh', refreshBudget);

	$('#modal-container').on('loaded.bs.modal', function () {
		$('#edit-budget-form', $(this)).on('submit', function (event) {
			event.preventDefault();

            if (!$(this).valid()) {
                return;
            }

			var $this = $(this);
			$.ajax({
				url: $this.attr('action'),
				type: $this.attr('method'),
				data: $this.serialize()
			}).done(function () {
			    $box.trigger('box.refresh');
				toastr.success('Budget has been edited');
				$('#modal-container').modal('hide');
			});
		});
	});

	function refreshBudget() {
		$('.info-box-inner-content', $box).addClass('hidden');
		$('.spinner', $box).removeClass('hidden');

		$.getJSON(getBudgetUrl).done(function (result) {
			$('#total-budget', $box).text(formatCurrency(result.totalBudget));
			$('#free-budget', $box).text(formatCurrency(result.freeBudget));
			$('#all-stocks', $box).text(formatCurrency(result.allStocksValue));

			$('.info-box-inner-content', $box).removeClass('hidden');
			$('.spinner', $box).addClass('hidden');

			$box.trigger('box.loaded');
		});
	}

    function formatCurrency(value) {
        return value.toFixed(2) + ' ' + currencyCode;
    }

})(jQuery);