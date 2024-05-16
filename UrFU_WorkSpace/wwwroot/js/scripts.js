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

const modalWrappers = document.querySelectorAll('.modal__wrapper');
const modalBtns = document.querySelectorAll('.open-modal__btn');
const btnNexts = document.querySelectorAll('.btn__next');
const btnBacks = document.querySelectorAll('.btn__back');

modalWrappers.forEach(modalWrapper => {
  modalWrapper.addEventListener('click', function(event) {
    if (event.target === modalWrapper) {
      modalWrapper.classList.add('hidden');
    }
  });
})

// При нажатии на кнопку "Забронировать место" показываем первый шаг
modalBtns.forEach((btn, index) => {
  btn.addEventListener('click', function() {
    modalWrappers[index].classList.remove('hidden');
  });
});

// При нажатии на кнопку "Продолжить" переходим к следующему шагу
btnNexts.forEach((btn, index) => {
  btn.addEventListener('click', function() {
    if(index !== 2){
      modalWrappers[index].classList.add('hidden');
      modalWrappers[index + 1].classList.remove('hidden');
    }
  });
});

// При нажатии на кнопку "Вернуться" возвращаемся к предыдущему шагу
btnBacks.forEach((btn, index) => {
  btn.addEventListener('click', function() {
    if(index !== 0){
      modalWrappers[index].classList.add('hidden');
      modalWrappers[index - 1].classList.remove('hidden');
    }
  });
});

