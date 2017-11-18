import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { ServiceSetting } from './service.setting';

import { ClientAPI, APIParameter, ParameterType } from '../models/client-api.model';

@Injectable()
export class ClientAPIService {
    
    constructor(private http: HttpClient){

    }

    async getAll() : Promise<ClientAPI[]> {
        console.log('asking server')
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/client-api/get-all").toPromise();

        return res as ClientAPI[];
    }

    async saveAPI(api: ClientAPI) : Promise<string> {
        const res = await this.http.post(ServiceSetting.BaseAPIUrl + "/client-api/save-api", api).toPromise();

        return res as string;
    }

    async deleteAPI(id: number) : Promise<string> {
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/client-api/delete-api?id=" + id).toPromise();
        
        return res as string;
    }
}