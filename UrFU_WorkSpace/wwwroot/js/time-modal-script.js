const sendTimeSlotsRequest = (evt) => {

    evt.preventDefault();
    const idWorkspace = document.getElementById('idWorkspace').value;
    const date = document.getElementById("date").value;
    const timeType = document.getElementById("timeType").value;
    const objectType = document.getElementById("objectType").value;
    console.log(date,  timeType, objectType)

    $.post(`/workspaces/${idWorkspace}/get-time-slots`, {
        date : date,
        timeType: timeType,
        objectType: objectType
    })
        .then(code => createSlotTimeElements(code))
        .catch(error => console.error(error));
    };

const getCheckbox = (times, i) =>{
    const checkbox = document.createElement('input');
    checkbox.classList.add("time__checkbox");
    checkbox.value = `${times['TimeStart'].slice(0, 5)}-${times['TimeEnd'].slice(0, 5)}`;
    checkbox.id = `time__${i + 1}`;

    return checkbox;
}

const getLabel = (timeSlotInfo, num) =>{
    const label = document.createElement('label');
    let timeStart = timeSlotInfo['TimeStart']
    let timeEnd = timeSlotInfo['TimeEnd'];
    label.for = `time__${num + 1}`;
    label.classList.add('checkbox__text');
    label.value = `${timeStart}-${timeEnd}`
    label.textContent = `${timeStart.slice(0, 5)}-${timeEnd.slice(0, 5)}`
    label.style.color = timeSlotInfo['IsDisable'];
    label.style.fontSize = "13px";
    
    return label;
}
const createSlotTimeElement = (timeSlotInfo, num) =>{
    const timeSlot = document.createElement("li");
    timeSlot.classList.add('time__elem');
    let checkbox = getCheckbox(timeSlotInfo, num);
    let label= getLabel(timeSlotInfo, num);
    
    timeSlot.appendChild(checkbox);
    timeSlot.appendChild(label);

    if(!timeSlotInfo['IsDisable']){
        timeSlot.addEventListener('click', onClickTimeSlot)
    }
    
    return timeSlot
}

let previousSelectedElement = null;

const onClickTimeSlot = (evt) => {
    const timeInfo = evt.target.value.split('-')
    const time =document.getElementById('selected-time__value');
    const date = document.getElementById('selected-date__value');
    const dateValue = document.getElementById("date").value.split('-')
    
    date.textContent = `${dateValue[2]}.${dateValue[1]}.${dateValue[0]}`
    time.value = evt.target.value
    time.textContent = `${timeInfo[0].slice(0, 5)}-${timeInfo[1].slice(0, 5)}`

    if (previousSelectedElement) {
        previousSelectedElement.classList.remove('selected-style');
    }
    evt.target.classList.toggle('selected-style');
    previousSelectedElement = evt.target;
}

const createSlotTimeElements = (code) => {
    const slotList = document.querySelector(".time__list");
    slotList.innerHTML = ""
    let timeSlotsInfos = JSON.parse(code);
    for (let i = 0; i < timeSlotsInfos.length; i++) {
        const newParagraph = createSlotTimeElement(timeSlotsInfos[i], i);
        slotList.appendChild(newParagraph);
    }
}

document.querySelector('#bookModal').addEventListener('shown.bs.modal', sendTimeSlotsRequest);
document.querySelector('#date').addEventListener('change', sendTimeSlotsRequest);
document.querySelector('#timeType').addEventListener('change', sendTimeSlotsRequest);
document.querySelector('#objectType').addEventListener('change', sendTimeSlotsRequest);