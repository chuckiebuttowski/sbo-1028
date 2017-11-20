import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { APIControllerDocumentation } from '../models/sbo-api.model';

import { ServiceSetting } from './service.setting';
import { promise } from 'selenium-webdriver';

@Injectable()
export class SBOApiService {

    constructor(private http: HttpClient) {

    }

    async exploreAPI() : Promise<APIControllerDocumentation[]> {
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + '/sbo-api-docs/explore-sbo-apis').toPromise();

        return res as APIControllerDocumentation[];
    }

    async getAPIDetail(name: string) : Promise<APIControllerDocumentation> {
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + '/sbo-api-docs/get-api-detail?name=' + name).toPromise();
        
        return res as APIControllerDocumentation;
    }
}