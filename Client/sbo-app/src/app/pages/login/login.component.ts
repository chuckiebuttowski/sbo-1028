import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

declare var $: any;

//services
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  Username: string = '';
  Password: string = '';
  isLogginIn:boolean = false;
  
  constructor(private authService: AuthService, private router: Router) {
  
  }
  
  ngOnInit() {
    
  }
  
  goLogin(event){
    if(event.keyCode == 13) {
      this.onLogin();    
    }
  }

  async onLogin() {
    this.isLogginIn = true;
    let msg = await this.authService.login(this.Username, this.Password);
    this.isLogginIn = false;
    if(msg == 'success'){
      this.router.navigate(['/dashboard/transaction-logs']);
    }
    else{
      $.notify({
        icon: "notifications",
        message: "<b>Login Failed</b> - " + msg

      }, {
          type: 'danger',
          timer: 500,
          placement: {
            from: 'top',
            align: 'right'
          }
        });
    }
  }

}
