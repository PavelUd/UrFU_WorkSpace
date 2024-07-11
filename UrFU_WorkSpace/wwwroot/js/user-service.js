import {logOut} from "./authentication-service.js"

const logOutBtn = document.querySelector('.log-out');
logOutBtn.addEventListener('click', logOut)