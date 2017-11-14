import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { SAPProfile, ServerConfig } from '../models/config.model';

@Injectable()
export class ConfigService {

    constructor(private http: HttpClient) { }

    async testAccessAuthorizeAPI() {
        var res = await this.http.get("http://localhost:51788/api/configuration/test").toPromise();
        console.log("response from authorize api: " + res);
    }

    getSAPProfile() : Observable<SAPProfile> {
        return Observable.of(new SAPProfile());
    }

    saveSAPProfile(profile: SAPProfile) : Observable<string> {
        profile = profile || new SAPProfile();

        if(profile.UserId == ''){
            return Observable.of("User id is required.");
        }

        if(profile.Password == ''){
            return Observable.of("Password is required.");
        }

        return Observable.of("Success");
    }

    async getRecentServers(): Promise<ServerConfig[]> { 
        let servers : ServerConfig[] = new Array<ServerConfig>();

        const res = await this.http.get("http://localhost:51788/api/configuration/get-recent-servers").toPromise();

        servers = res as ServerConfig[];

        return servers;
    }

    setActiveServer(server: ServerConfig) : Observable<boolean> {
        return Observable.of(true);
    }

    async saveServer(server : ServerConfig) : Promise<string> {
        server = server || new ServerConfig();

        if(server.ServerName == "") {
            return "Server name is required.";
        }

        if(server.DatabaseName == "") {
            return "Database name is required.";
        }

        if(server.Username == "") {
            return "Username is required.";
        }

        if(server.Password == "") {
            return "Password is required.";
        }

        let body = { ServerName: server.ServerName, DatabaseName: server.DatabaseName, Username: server.Username, Password: server.Password };
        const res = await this.http.post("http://localhost:51788/api/configuration/add", body).toPromise();

        return res as string;
    }

    deleteServer(server: ServerConfig) : Observable<boolean> {
        return Observable.of(true);
    }
}