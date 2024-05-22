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
const getDiagram = (objects) => {
    const myDiagram = go.Diagram.fromDiv(document.getElementById('myDiagramDiv'))
    let modelNodes = [];
    objects.forEach(obj => {
            let color = obj['IsReserve'] ? "red" : "lightblue";
            modelNodes.push({ "key": obj['IdObject'], "pos": `${obj['X']} ${obj['Y']}`,"isReserve" : obj['IsReserve'],  "size": `${obj['Height']} ${obj['Width']}`, "color": color })
        })
    myDiagram.model = new go.GraphLinksModel(modelNodes);
}

const onClickResBtnBack = () =>{
    $('#bookModal').modal('show');
    $('#selectWorkplaceModal').modal('hide');
}

document.querySelector('#btn-back').addEventListener('click', onClickResBtnBack);
document.querySelector('#btn-next').addEventListener('click', getWorkspaceObjects)