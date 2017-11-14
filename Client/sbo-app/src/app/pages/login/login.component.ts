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
  
  constructor(private authService: AuthService, private router: Router) {
  
  }
  
  ngOnInit() {
    
  }
  
  async onLogin() {
    let msg = await this.authService.login(this.Username, this.Password);
    if(msg == 'success'){
      this.router.navigate(['/dashboard/home']);
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
