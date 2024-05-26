document.querySelector('#loginModal form').addEventListener('submit', function(event) {
    event.preventDefault();

    const formData = new FormData(event.target);
    $.post('/Authentication/Login/', {
        login : formData.get('login'),
        password : formData.get('password')
    }).then(token => {
        if (token) {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            const payloadinit = atob(base64);
            const payload = JSON.parse(payloadinit);
            sessionStorage.setItem("token", token);
            $('#lk').replaceWith('<div style="margin-right: 5rem" class="btn-reset btn nav__btn"><img src="/img/account.svg" alt="Лк">' + payload.Login + '</div>');
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
        .then(Name => {
            $('#lk').replaceWith('<div style="margin-right: 5rem" class="btn-reset btn nav__btn"><img src="img/account.svg" alt="Лк">' + Name + '</div>');
        }).catch(error => {
        console.error(error);
    })
        .finally($('#verifyCodeModal').modal('hide'));
}
document.querySelector('#registerModal form').addEventListener('submit', sendCheckUserRequest);
document.querySelector('#verifyCodeModal form').addEventListener('submit', sendreg);
