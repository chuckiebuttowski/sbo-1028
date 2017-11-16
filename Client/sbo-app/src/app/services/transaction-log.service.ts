import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { TransactionLog, TransactionData, ErrorLog, SBOType } from '../models/log.model';

@Injectable()
export class TransactionLogService {

    constructor(private http: HttpClient) { }

    async getAllLogs() : Promise<TransactionLog[]>{
        const res = await this.http.get("http://localhost:51788/api/transaction-log/get-all").toPromise();
        
        console.log(res as TransactionLog[]);

        return res as TransactionLog[];
    }
}