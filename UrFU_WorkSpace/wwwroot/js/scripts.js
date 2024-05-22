$('body').on('click', '.password-control', function(){
    var targetId = $(this).data("target");
    var $targetInput = $('#' + targetId);
	if ($targetInput.prop('type') == 'password'){
		$targetInput.prop('type', 'text');
	} else {
		$targetInput.prop('type', 'password');
	}
	return false;
});


const checkboxes = document.querySelectorAll('.time__checkbox');
const selectedTimeSpan = document.querySelector('.selected-time__value');
checkboxes.forEach(checkbox => {
  checkbox.addEventListener('change', function() {
    let selectedDate = document.querySelector('.book__date').value;
    if (this.checked) {
      checkboxes.forEach(otherCheckbox => {
        if (otherCheckbox !== this) {
          otherCheckbox.checked = false;
        }
      });
      const label = this.nextElementSibling;
      selectedTimeSpan.textContent = selectedDate + ' ' + label.textContent;
    } else {
      selectedTimeSpan.textContent = '';
    }
  });
});

