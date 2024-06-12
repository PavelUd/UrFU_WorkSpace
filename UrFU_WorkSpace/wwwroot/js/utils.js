export const decodeJwtToken = (token) =>{
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const payloadinit = atob(base64);
    return JSON.parse(payloadinit);
}

export const parseDate = (date) => {
    let dateValue = date.split('-');
    return `${dateValue[2]}.${dateValue[1]}.${dateValue[0]}`
}

export const getShortTime = (time) =>{
    return time.slice(0, 5)
}

export const postSender = (route, obj, sucFunc, errorFunc) => {
    $.post(route, obj)
        .then(code => sucFunc(code))
        .catch(error => errorFunc(error));
}