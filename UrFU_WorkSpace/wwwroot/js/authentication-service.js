import {decodeJwtToken} from "./utils.js";
import AuthenticationClient from "./Clients/authentication-client.js";

const errorLoginContainer = document.getElementById("login-error-message");
const registerBtn = document.querySelector('.register-btn');
const authenticationClient = new AuthenticationClient("/Authentication");
const loginBtn = document.getElementById('lk');
const loginModal = document.querySelector('#loginModal');
const emailContainer = document.getElementById("verifyEmail");
const registerModal = document.getElementById("registerModal");
const registerForm = registerModal.querySelector('form');
const registerErrorContainer = document.getElementById("error-message");
const verifyModal = document.getElementById('verifyCodeModal');
const logOutBtn = document.querySelector('.log-out');


let code;

const getActiveLoginBtn = (login, id) => {
    return `<a style=" margin-right: 5rem" class="btn-reset btn nav__btn select-btn" href="/users/${id}">
               <img src="/img/account.svg" alt="Лк"> ${login}
            </a>`
}

const login = async (evt) => {
    evt.preventDefault();
    let data = Object.fromEntries(new FormData(evt.target))
        try {
            let tokenData = await authenticationClient.getToken(data);
            let token = tokenData.accessToken;
            sessionStorage.setItem("token", token);
            const user = decodeJwtToken(token);
            loginBtn.innerHTML = getActiveLoginBtn(user.Login, user.Id);
            $(loginModal).modal('hide');
        }
        catch (e){
            errorLoginContainer.textContent = e.responseJSON.message;
        }
}

const verifyUser = async (evt) => {

    evt.preventDefault();
    const codeForm = Object.fromEntries(new FormData(evt.target));
    try {
        let tokenData = await authenticationClient.getToken(codeForm);
        let token = tokenData.accessToken;
        sessionStorage.setItem("token", token);
        const user = decodeJwtToken(token);
        loginBtn.innerHTML = getActiveLoginBtn(user.Login, user.Id);
        
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
    
    try {
            await authenticationClient.register(data); 
            emailContainer.textContent = data.email;
            $(registerModal).modal('hide');
            $(verifyModal).modal('show');
        }
    
    catch(e){
        registerErrorContainer.textContent = e.responseJSON.message;
    }
}

export const logOut = async () => {
    await authenticationClient.logOut();
    sessionStorage.removeItem("token");
    window.location.href = '/';
}

registerForm.addEventListener('submit', register);
verifyModal.querySelector('form').addEventListener('submit', verifyUser);
loginModal.querySelector('form').addEventListener('submit', login);
registerBtn.addEventListener('click', () => $(loginModal).modal('hide'));