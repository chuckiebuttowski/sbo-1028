import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/observable/of';

import { ServiceSetting } from './service.setting';

@Injectable()
export class AuthService {
  private token: string;
  private User: any;

  constructor(private http: Http) { }

  async login (username: string, password: string) : Promise<string> {
    try {
      if (username === '') {
        return 'Username is required.';
      }
      
      if (password === '') {
        return 'Password is required.';
      }
      
      let body = { username: username, password: password};
      
      let res: Response = await this.http.post(ServiceSetting.BaseAPIUrl + '/account/login', body).toPromise();
      
      sessionStorage.setItem('User', JSON.stringify(res.json().User));

      if(res.json().Message.toLowerCase().indexOf('success') < 0) {
        return res.json().Message;
      }
      
      sessionStorage.setItem("token", res.json().Token);
      return "success"; 

    } catch (error) {
      return "An error occur while processing your request.";
    }
  }

  getUserDisplayName(): string{
    return JSON.parse(sessionStorage.getItem('User')).DisplayName;
  }

  getBuildNo():string{
    return JSON.parse(sessionStorage.getItem('BuildNo'));
  }

  logout () {
    sessionStorage.removeItem("token");
    sessionStorage.removeItem("User");
  }

  isUserLoggedIn(){
    if(sessionStorage.getItem("token") == undefined || sessionStorage.getItem("token") == null) {
      return false;
    }
    else{
      return true;
    }
  }

}
