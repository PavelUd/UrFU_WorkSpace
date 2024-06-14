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
        toggleDropdown();
    }
}

function getLKDropdown(user){
    return `<div class="select-menu">
            <div style=" margin-right: 5rem" class="btn-reset btn nav__btn select-btn">
               <img src="/img/account.svg" alt="Лк"> ${user.Login}
                  </div>

            <ul class="options">
              <li class="option">
                <i class="bx bxl-linkedin-square" style="color: #0E76A8;"></i>
                <span class="option-text"><a data-bs-toggle="modal" href="#" data-bs-target="#constructorModal">Конструктор</a></span>
              </li>
              <li class="option">
                <i class="bx bxl-facebook-circle" style="color: #4267B2;"></i>
                <span class="option-text">Мои Коворкинги</span>
              </li>
              <li class="option">
                <i class="bx bxl-twitter" style="color: #1DA1F2;"></i>
                <span class="option-text">Мои Брони</span>
              </li>
            </ul>
          </div>`
}

function toggleDropdown(){
        const optionMenu = document.querySelector(".select-menu"),
        selectBtn = optionMenu.querySelector(".select-btn"),
        options = optionMenu.querySelectorAll(".option"),
        sBtn_text = optionMenu.querySelector(".sBtn-text");

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
document.querySelector('#registerModal form').addEventListener('submit', sendCheckUserRequest);
document.querySelector('#verifyCodeModal form').addEventListener('submit', sendreg);
