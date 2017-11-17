import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

//model
import { Account } from '../models/account.model';

import { ServiceSetting } from './service.setting';

@Injectable()
export class AccountService {
    
    constructor(private http: HttpClient) { }

    async getAccounts() : Promise<Account[]> {
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/app-user/get-users").toPromise();

        return res as Account[];
    }

    ///use in creating and updating account
    async saveAccount(acct: Account) : Promise<string> {

        if(acct.DisplayName == ""){
            return "Display name is required.";
        }

        if(acct.Username == ""){
            return "Username is required.";
        }

        if(acct.Password == ""){
            return "Password is required.";
        }

        const res = await this.http.post(ServiceSetting.BaseAPIUrl + "/app-user/add-or-update", acct).toPromise();

        return res as string;
    }

    async deleteAccount(id: number) : Promise<string>{
        const res = await this.http.get(ServiceSetting.BaseAPIUrl + "/app-user/delete?id=" + id).toPromise();
        
        return res as string;
    }
}