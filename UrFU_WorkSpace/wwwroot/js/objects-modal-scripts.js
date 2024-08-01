const getWorkspaceObjects = (evt) => {

    evt.preventDefault();
    const date = document.getElementById("date").value;
    const objectType = document.getElementById("objectType").value;
    const idWorkspace = document.getElementById('idWorkspace').value;
    const time = document.getElementById('selected-time__value').value.split('-');
    if (time == null || date == null || objectType == null){
        return;
    }
    $('#bookModal').modal('hide');
    $('#selectWorkplaceModal').modal('show');
    
    $.post(`/workspaces/${idWorkspace}/workspace-objects`, {
        date : date,
        objectType: objectType,
        timeEnd: time[0],
        timeStart: time[1],
    })
        .then(code => getDiagram(JSON.parse(code)))
        .catch(error => console.log(error));
}
const getDiagram = (objects) =>{
    console.log(objects)
    loadObjects("myDiagramDiv", objects,(obj) => obj.isReserved ? "invert(35%) opacity(50%)" : "opacity(60%)")
}

const onClickResBtnBack = () =>{
    $('#bookModal').modal('show');
    $('#selectWorkplaceModal').modal('hide');
}

document.querySelector('#btn-back').addEventListener('click', onClickResBtnBack);
document.querySelector('#btn-next').addEventListener('click', getWorkspaceObjects)