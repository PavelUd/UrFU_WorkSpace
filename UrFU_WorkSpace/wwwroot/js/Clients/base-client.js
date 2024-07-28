export default class  BaseClient {
    constructor(endPoint) {
        this._endPoint = endPoint;
    }

    async _load({
                    url,
                    method = 'GET',
                    data = null,
                }) {

        const response = await $.ajax({
            type: method,
            url: `${this._endPoint}/${url}`,
            data: data
        });

        try {
            BaseClient.checkStatus(response);
            return response;
        } catch (err) {
            BaseClient.catchError(err);
        }
    }
    
    static checkStatus(response) {
        if(response.statusCode >= 400) {
            throw response.message;
        }
    }
    
    static catchError(err) {
        throw err;
    }
    
}