import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { ServiceSetting } from './service.setting';

import { TransactionLog, TransactionData, ErrorLog, SBOType } from '../models/log.model';

@Injectable()
export class TransactionLogService {

    constructor(private http: HttpClient) { }

    async getCounts() : Promise<any[]>{
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/transaction-log/get-counts").toPromise();

        return res as any[];
    }

    async getAllLogs() : Promise<TransactionLog[]>{
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/transaction-log/get-all").toPromise();

        return res as TransactionLog[];
    }

    async getLogDetail(id: number) : Promise<TransactionLog>{
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/transaction-log/get-log-detail?id=" + id).toPromise();
        
        return res as TransactionLog;
    }

    async retryPosting(id: number) : Promise<string> {
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/transaction-log/retry-posting?id=" + id).toPromise();
        
        return res as string;
    }
}