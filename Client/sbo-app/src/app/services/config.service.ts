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

    async getSAPProfile() : Promise<SAPProfile> {
        const res = await this.http.get("http://localhost:51788/api/sap-profile/get-profile").toPromise();
        return res as SAPProfile;
    }

    async saveSAPProfile(profile: SAPProfile) : Promise<string> {
        profile = profile || new SAPProfile();

        if(profile.UserId == ''){
            return "User id is required.";
        }

        if(profile.Password == ''){
            return "Password is required.";
        }

        let body = {UserId: profile.UserId, Password: profile.Password };
        const res = await this.http.post("http://localhost:51788/api/sap-profile/update-profile", body).toPromise();
        
        return res as string;
    }

    async getRecentServers(): Promise<ServerConfig[]> { 
        let servers : ServerConfig[] = new Array<ServerConfig>();

        const res = await this.http.get("http://localhost:51788/api/configuration/get-recent-servers").toPromise();

        servers = res as ServerConfig[];

        return servers;
    }

    async saveServer(server : ServerConfig, isActive: boolean) : Promise<string> {
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
        const res = await this.http.post("http://localhost:51788/api/configuration/add?isActive=" + isActive, body).toPromise();

        return res as string;
    }

    async activateServer(id: number) : Promise<string> {
        const res = await this.http.get("http://localhost:51788/api/configuration/activate-server?id=" + id).toPromise();
        
        return res as string;
    }

    async deleteServer(id: number) : Promise<string> {
        const res = await this.http.get("http://localhost:51788/api/configuration/delete?id=" + id).toPromise();

        return res as string;
    }
}