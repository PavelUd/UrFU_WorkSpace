import {postSender, decodeJwtToken, parseDate, getShortTime} from "./utils.js";

const sucFunc = (jsonReservation) =>{
    let reservation = JSON.parse(jsonReservation);
    document.getElementById("bookBody").innerHTML = renderSuccessBookBody();
    document.getElementById("bookTitle").textContent ="Бронирование успешно завершено";
    document.getElementById("booked-time__value").textContent =`${getShortTime(reservation.timeStart)}-${getShortTime(reservation.timeEnd)}`;
    document.getElementById("booked-date__value").textContent =  parseDate(reservation.date);
    getBookDiagram(reservation.idObject)
};

function renderSuccessBookBody(){
    return `<div class="selected-time" style="color: black;">
              Вы выбрали:
              <span style="color: #1861ac; font-size: 16px; font-weight: 600" id="booked-date__value"></span>
              <span style="color: #1861ac; font-size: 16px; font-weight: 600" id="booked-time__value"></span> 
               <span style="color: #1861ac; font-size: 16px; font-weight: 600" id="booked-type__value"></span>
            </span>
            <div id="bookDiagram" style="width:100%; height:350px; border-radius: 10px; background: lightgray; border:2px solid black; margin-top: 1rem"></div>`
} 

const errorFunc = (error) => {
    document.querySelector("#bookTitle").textContent = "Что-то пошло не так";
    document.getElementById("bookBody").innerHTML = `<div class="container" style="color: black; font-size: 20px; text-align: center; padding:0 30px 50px 30px">Ваше место не забранировано. Обратитесь в техническую поддержку</div>`;
    console.log(error);
}
const sendReservationRequest = (evt) => {
    evt.preventDefault();
    const idWorkspace = document.getElementById('idWorkspace').value;
    const time= document.getElementById('selected-time__value').value.split('-');
    const selectedObject = document.getElementById("selected-object").value
    const date = document.getElementById("date").value
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

function getBookDiagram(idObj) {
    const diagram = go.Diagram.fromDiv(document.getElementById("showDiagram"));
    init("bookDiagram");
    const objects = JSON.parse(diagram.model.toJSON()).nodeDataArray;
    document.getElementById("booked-type__value").textContent = objects.find(obj => obj.key === idObj).category
    const result = objects.map(obj => ({
        ...obj,
        x: obj.pos.split(" ")[0],
        y: obj.pos.split(" ")[1],
        height: obj.size.split(" ")[0],
        width: obj.size.split(" ")[0],
        template: {
            picture: obj.image,
            category: obj.category
        }
    }));
    loadObjects("bookDiagram", result,(obj) => obj.key == idObj ? '' : 'invert(35%) opacity(50%)')
}

document.querySelector('#bookResultModal').addEventListener('hide.bs.modal', onHideBookModal);
document.querySelector('#book-btn').addEventListener('click', checkAuthentication);
document.querySelector('#reservation-button').addEventListener('click', sendReservationRequest);