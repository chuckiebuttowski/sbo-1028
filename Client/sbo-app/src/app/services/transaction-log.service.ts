import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { TransactionLog, TransactionData, ErrorLog, SBOType } from '../models/log.model';

@Injectable()
export class TransactionLogService {

    constructor(private http: HttpClient) { }

    async getAllLogs() : Promise<TransactionLog[]>{
        const res = await this.http.get("http://localhost:51788/api/transaction-log/get-all").toPromise();

        return res as TransactionLog[];
    }

    async getLogDetail(id: number) : Promise<TransactionLog>{
        const res = await this.http.get("http://localhost:51788/api/transaction-log/get-log-detail?id=" + id).toPromise();
        
        return res as TransactionLog;
    }

    async retryPosting(id: number) : Promise<string> {
        const res = await this.http.get("http://localhost:51788/api/transaction-log/retry-posting?id=" + id).toPromise();
        
        return res as string;
    }
}