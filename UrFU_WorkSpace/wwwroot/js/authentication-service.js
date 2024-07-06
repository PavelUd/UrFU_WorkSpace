import {decodeJwtToken} from "./utils.js";
import AuthenticationClient from "./Clients/authentication-client.js";

const errorLoginContainer = document.getElementById("login-error-message");
const registerBtn = document.querySelector('.register-btn');
const authenticationClient = new AuthenticationClient("Authentication");
const loginBtn = document.getElementById('lk');
const loginModal = document.querySelector('#loginModal');
const emailContainer = document.getElementById("verifyEmail");
const registerModal = document.getElementById("registerModal");
const registerForm = registerModal.querySelector('form');
const registerErrorContainer = document.getElementById("error-message");
const verifyModal = document.getElementById('verifyCodeModal');

let code;

const getActiveLoginBtn = (login) => {
    return `<div style=" margin-right: 5rem" class="btn-reset btn nav__btn select-btn">
               <img src="/img/account.svg" alt="Лк"> ${login}
            </div>`
}

const login = async (evt) => {
    evt.preventDefault();
    let data = Object.fromEntries(new FormData(evt.target))
        try {
            let token = await authenticationClient.login(data);
            sessionStorage.setItem("token", token);
            const user = decodeJwtToken(token);
            loginBtn.innerHTML = getActiveLoginBtn(user.Login);
            $(loginModal).modal('hide');
        }
        catch (e){
            errorLoginContainer.textContent = e;
        }
}

const verifyUser = async (evt) => {

    evt.preventDefault();
    const user = decodeJwtToken(sessionStorage.getItem("token"));
    const codeForm = new FormData(evt.target);
    try {
        await authenticationClient.verifyUser({
            idUser: user.Id,
            code: codeForm.get('code')
        });
        loginBtn.innerHTML = getActiveLoginBtn(user.Login);
        
    }
    catch (e){
        console.log(e)
    }
    finally {
        $(verifyModal).modal('hide');
    }
}

const register = async  (evt) => {
    evt.preventDefault();
    const formData = new FormData(registerForm);

    let data = Object.fromEntries(formData);
    data.correctCode = code;
    
    try {
            let token = await authenticationClient.register(data);
            sessionStorage.setItem("token", token);
            const user = decodeJwtToken(token);
            emailContainer.textContent = user.Email;
            $(registerModal).modal('hide');
            $(verifyModal).modal('show');
        }
    
    catch(e){
        registerErrorContainer.textContent = e;
    }
}

registerForm.addEventListener('submit', register);
verifyModal.querySelector('form').addEventListener('submit', verifyUser);
loginModal.querySelector('form').addEventListener('submit', login);
registerBtn.addEventListener('click', () => $(loginModal).modal('hide'));