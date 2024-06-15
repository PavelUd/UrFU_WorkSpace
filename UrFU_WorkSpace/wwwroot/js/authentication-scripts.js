import {decodeJwtToken} from "./utils.js";

document.querySelector('#loginModal form').addEventListener('submit', function(event) {
    event.preventDefault();

    const formData = new FormData(event.target);
    $.post('/Authentication/Login/', {
        login : formData.get('login'),
        password : formData.get('password')
    }).then(token => {
        if (token) {
            const user = decodeJwtToken(token)
            sessionStorage.setItem("token", token);
            createLK();
            
            $('#loginModal').modal('hide');
        }
    })
        .catch(error => {
            document.getElementById("login-error-message").textContent = error.responseText;
            console.error(error);
        });
});

$('a[data-bs-target="#registerModal"]').click(function() {
    $('#loginModal').modal('hide');
    $('#registerModal').modal('show');
});

const sendCheckUserRequest = (evt) => {

    evt.preventDefault();
    const emailElement = document.getElementById("email")
    const loginElement = document.getElementById("login")
    document.getElementById("verifyEmail").textContent = emailElement.value;

    $.post('/Authentication/CheckUserExistence/', {
        login: loginElement.value,
        email : emailElement.value
    })
        .then(message => {
            if (message != ""){
                document.getElementById("error-message").textContent = message;
            }
            else{
                document.getElementById("error-message").textContent = "";
                $('#registerModal').modal('hide');
                $('#verifyCodeModal').modal('show');
                sendRegistrationRequest(evt);
            }
        }).catch(error => {
        console.error(error);
        $('#registerModal').modal('hide');
    });
}

const sendRegistrationRequest = (evt) => {

    evt.preventDefault();
    const emailElement = document.getElementById("email")
    document.getElementById("verifyEmail").textContent = emailElement.value;

    $.post('/Authentication/SendCode/', {
        email : emailElement.value
    })
        .then(code => {
                document.getElementById("code").value = code;
            }
        ).catch(error => {
        console.error(error);
        $('#registerModal').modal('hide');
    });
}

const sendreg = (evt) => {
    evt.preventDefault();
    const registerElement = document.getElementById("registerForm")
    const codeForm = new FormData(evt.target);
    const formData = new FormData(registerElement);
    $.post('/Authentication/Register/', {
        firstName:formData.get('first-name'),
        secondName:formData.get('second-name'),
        email:formData.get('email'),
        login : formData.get('login'),
        password : formData.get('password'),
        correctCode : formData.get('code'),
        code : codeForm.get('code')
    })
        .then(token => {
            const user = decodeJwtToken(token)
            sessionStorage.setItem("token", token);
            createLK();
        }).catch(error => {
        console.error(error);
    })
        .finally($('#verifyCodeModal').modal('hide'));
}

function createLK() {
    const token = sessionStorage.getItem("token");
    if(token){
        const user = decodeJwtToken(token);
        $('#lk').replaceWith(getLKDropdown(user));
        if(user.AccessLevel === "2"){
            document.querySelector('.options').insertAdjacentHTML('afterbegin', `
              <li class="option">
                <i class="bx bxl-linkedin-square" style="color: #0E76A8;"></i>
                <span class="option-text"><a data-bs-toggle="modal" href="#" data-bs-target="#constructorModal">Конструктор</a></span>
              </li>
              <li class="option">
                <i class="bx bxl-twitter" style="color: #1DA1F2;"></i>
                <span class="option-text" data-bs-target="#generateReservationGenerateCodeModal" data-bs-toggle="modal">Сгенерировать код подтверждения</span>
              </li>`);
        }
        toggleDropdown();
        getWorkspaceCodes();
        getUserReservations()
        document.querySelector('.log-out').addEventListener('click', logOut);
        document.querySelector('#generateCodeBtn').addEventListener('click', generateNewCode);
        document.querySelector('#confirmReservationBtn').addEventListener('click', confirmReservation);
    }
}

function logOut() {
    const lkBtn = `<div id="lk">
                            <button class="btn-reset btn nav__btn" style="margin-right: 5rem" data-bs-toggle="modal" data-bs-target="#loginModal">
                                Войти
                            </button>
                           </div>`
    
    sessionStorage.clear();
    $.post('/log-out', {});
    $('#lk').replaceWith(lkBtn);
}

function getWorkspaceCodes(){
    let token = sessionStorage.getItem("token");
    const user = decodeJwtToken(token)
    const codeSelector = document.querySelector('#workspaceNames');
    $.post(`/${user.Id}/verification-codes`, {}).then(x => {
        x.forEach(code =>{ 
            codeSelector.innerHTML += `<option data-code="${code.code}" data-id-code = "${code.id}" value = ${code.idWorkspace}>${code.workspaceName}</option>`
        });
    });
    codeSelector.addEventListener('change', function(){
        const selectedOption = codeSelector.options[codeSelector.selectedIndex];
        changeCode(selectedOption.dataset.code, selectedOption.dataset.idCode);
    });
}


function getUserReservations(){
    const reservationSelector = document.querySelector('#reservations');
    let token = sessionStorage.getItem("token");
    const user = decodeJwtToken(token)
    let currentDate = new Date().setHours(0, 0, 0, 0);
    $.post(`/${user.Id}/reservations`, {}).then(x => {
        x.forEach(reservation =>{
            let reservationDate = new Date(reservation.date).setHours(0, 0, 0, 0);
            if (reservationDate < currentDate){
                return;
            }
            reservationSelector.innerHTML += `<option value = ${reservation.idReservation} data-id-workspace =${reservation.idWorkspace}>Бронь ${reservation.date} ${reservation.timeStart} ${reservation.timeEnd}</option>`
        });
        const bookBtn = document.querySelector('#book-btn');
        if(bookBtn){
            const url = window.location.href;
            const workspaceId = url.split('/').pop();
            let t = x.find(r => r.idWorkspace === Number(workspaceId) && currentDate <= new Date(r.date).setHours(0, 0, 0, 0));
            if(t){
                console.log(x);
                bookBtn.disabled = true;
            }
        }
    });
}

function changeCode(code, idCode){
    const codeElement = document.getElementById('reservationCode');
    codeElement.textContent = code;
    codeElement.dataset.id = idCode;
}

function confirmReservation(evt){
    const reservationSelector = document.querySelector('#reservations');
    const statusElement = document.querySelector('#statusConfirmCode');
    const code = document.querySelector('#userCode');
    const selectedOption = reservationSelector.options[reservationSelector.selectedIndex];
    if(selectedOption.value === 0 || !code.value){
        return;
    }
    let token = sessionStorage.getItem("token");
    const user = decodeJwtToken(token)
    $.post(`/${user.Id}/confirm-reservation`, {
        code: code.value,
        id: selectedOption.value,
        idWorkspace: selectedOption.dataset.idWorkspace
    }).then(x => {
        if(Boolean(x) === true) {
            statusElement.style.color = "green"
            selectedOption.remove();
            const bookBtn = document.querySelector('#book-btn');
            if(bookBtn){
                bookBtn.disabled = false;
            }
            statusElement.textContent = "Успешно подтверждено"
        }
        else{
            statusElement.style.color = "red"
            statusElement.textContent = "Неверный код подтверждения"
        }
    });
}

function generateNewCode(){
    const codeSelector = document.querySelector('#workspaceNames');
    const selectedOption = codeSelector.options[codeSelector.selectedIndex];
    $.post('/update-code', {
        idWorkspace: selectedOption.value,
        idCode: selectedOption.dataset.idCode
    }).then(x => {
        changeCode(x.code, x.id)
    })
}

function getLKDropdown(user){
    return `<div class="select-menu" id="lk">
            <div style=" margin-right: 5rem" class="btn-reset btn nav__btn select-btn">
               <img src="/img/account.svg" alt="Лк"> ${user.Login}
            </div>
            <ul class="options">
              <li class="option">
                <i class="bx bxl-facebook-circle" style="color: #4267B2;"></i>
                <span class="option-text" data-bs-toggle="modal" data-bs-target="#userReservationsModal">Подтвердить Бронь</span>
              </li>
              
              <li class="option">
                <i class="bx bxl-facebook-circle" style="color: #4267B2;"></i>
                <a class="option-text log-out">Выход</a>
              </li>
            </ul>
          </div>`
}

function toggleDropdown(){
        const optionMenu = document.querySelector(".select-menu"),
        selectBtn = optionMenu.querySelector(".select-btn");

        selectBtn.addEventListener("click", () =>
        optionMenu.classList.toggle("active")
        );

        document.addEventListener('click', (event) => {
        if (!optionMenu.contains(event.target) && optionMenu.classList.contains('active')) {
        optionMenu.classList.remove('active');
    }
    });


        
}


window.addEventListener('DOMContentLoaded', createLK);
document.getElementById('userReservationsModal').addEventListener('hidden.bs.modal', () => {
    const statusElement = document.querySelector('#statusConfirmCode');
    statusElement.textContent = "";
})
document.querySelector('#registerModal form').addEventListener('submit', sendCheckUserRequest);
document.querySelector('#verifyCodeModal form').addEventListener('submit', sendreg);
