import {Method} from "../const.js";
import BaseClient from '../Clients/base-client.js';

export default class AuthenticationClient extends BaseClient{

    async getToken(authenticationInfo) {
        const response = await this._load({
            url: 'GetToken/',
            method: 'POST',
            data: authenticationInfo
        });
        return response;
    }

    async register(user){
        const response = await this._load({
            url: 'Register',
            method : 'POST',
            data: user,
        });
        return response
    }

    async logOut(){
        await this._load({
            url: 'LogOut',
            method : 'POST',
            data: {}
        });
    }
}