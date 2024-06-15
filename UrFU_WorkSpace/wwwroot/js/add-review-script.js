import {decodeJwtToken} from "./utils.js";

const form = document.getElementById("reviewForm")
let name = document.getElementById("formName")
let date = document.getElementById("formDate")
const formStars = document.getElementById("formStars")
const starsCount = document.getElementById("starsCount")
const dateInput = document.getElementById("dateInput")
const toggleForm =() =>{
    if(sessionStorage.token === undefined){
        return;
    }
    let token = decodeJwtToken(sessionStorage.token);
    name.textContent = token["Login"]
    parseCurrentDate();
    $('#addReviewModal').modal('show');
}

const parseCurrentDate = () => {
    const today = new Date();
    const day = String(today.getDate()).padStart(2, '0');
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const year = today.getFullYear();
    dateInput.value = `${year}-${month}-${day}`;

}

const starts = (evt) => {
    const count =Number(evt.target.id.split('-')[1]) + 1;
    const max = 5;
    for (let i = 0; i < max; i++) {
        if(count > i) {
            document.getElementById(`star-${i}`).src = "../img/big-blue-star.png"
        }
        else  {
            document.getElementById(`star-${i}`).src = "../img/grey-star.png"
        }
    }
    starsCount.value = count;
}

formStars.childNodes.forEach(x => {
    x.addEventListener('click', starts)
})
document.getElementById("reviewButton").addEventListener('click', toggleForm)