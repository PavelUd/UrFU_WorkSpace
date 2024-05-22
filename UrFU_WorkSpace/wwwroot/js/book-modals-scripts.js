import {postSender, decodeJwtToken} from "./utils.js";

const sucFunc = () => document.querySelector("#book-result").textContent = "Бронирование успешно завершено";

const errorFunc = (error) => {
    document.querySelector("#book-result").textContent = "Что-то пошло не так";
    console.log(error);
}
const sendReservationRequest = (evt) => {
    evt.preventDefault();
    const idWorkspace = document.getElementById('idWorkspace').value;
    const time= document.getElementById('selected-time__value').value.split('-');
    const selectedObject = document.getElementById("selected-object").value
    const date = document.getElementById("date").value
    console.log(selectedObject)
    if(selectedObject === "" || date == null){
        return
    }

    $('#selectWorkplaceModal').modal('hide');
    $('#bookResultModal').modal('show');
    const obj = {
        idObject: selectedObject,
        idUSer: decodeJwtToken(sessionStorage.token).Id,
        timeStart: time[0],
        timeEnd: time[1],
        date : date
    };
    
    postSender(`/workspaces/${idWorkspace}/reserve`, obj, sucFunc, errorFunc);
};
const checkAuthentication = () => {
    if (sessionStorage.token === undefined){
        $('#loginModal').modal('show');
    }
    else{
        $('#bookModal').modal('show');
    }
}

const onHideBookModal = () => {
    const time =document.getElementById('selected-time__value');
    const date = document.getElementById('selected-date__value');
    const selectedObject = document.getElementById("selected-object");
    const result = document.querySelector("#book-result");
    time.textContent = "";
    time.value = null;
    date.value = null;
    selectedObject.value = null;
    result.textContent = "";
    date.textContent = "";
}

document.querySelector('#bookResultModal').addEventListener('hide.bs.modal', onHideBookModal);
document.querySelector('#book-btn').addEventListener('click', checkAuthentication);
document.querySelector('#reservation-button').addEventListener('click', sendReservationRequest);