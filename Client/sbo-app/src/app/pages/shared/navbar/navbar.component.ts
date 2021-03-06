import { Component, OnInit, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { ROUTES } from '../sidebar/sidebar.component';
import {Location, LocationStrategy, PathLocationStrategy} from '@angular/common';

//services
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
    private listTitles: any[];
    location: Location;
    private toggleButton: any;
    private sidebarVisible: boolean;

    UserDisplayName: string;

    constructor(location: Location,  private element: ElementRef, private authService: AuthService, private router: Router) {
      this.location = location;
          this.sidebarVisible = false;
    }

    ngOnInit(){
      this.listTitles = ROUTES.filter(listTitle => listTitle);
      const navbar: HTMLElement = this.element.nativeElement;
      this.toggleButton = navbar.getElementsByClassName('navbar-toggle')[0];
      this.UserDisplayName = this.authService.getUserDisplayName();
    }

    sidebarOpen() {
        const toggleButton = this.toggleButton;
        const body = document.getElementsByTagName('body')[0];
        setTimeout(function(){
            toggleButton.classList.add('toggled');
        }, 500);
        body.classList.add('nav-open');

        this.sidebarVisible = true;
    };
    sidebarClose() {
        const body = document.getElementsByTagName('body')[0];
        this.toggleButton.classList.remove('toggled');
        this.sidebarVisible = false;
        body.classList.remove('nav-open');
    };
    sidebarToggle() {
        // const toggleButton = this.toggleButton;
        // const body = document.getElementsByTagName('body')[0];
        if (this.sidebarVisible === false) {
            this.sidebarOpen();
        } else {
            this.sidebarClose();
        }
    };

    getTitle(){
      var titlee = this.location.prepareExternalUrl(this.location.path());
      if(titlee.charAt(0) === '#'){
          titlee = titlee.slice( 2 );
      }

    //   titlee = titlee.split('/').pop();

      let paths: string[] =  titlee.split('/');
      let first = paths.pop();
      let second = paths[paths.length - 1];
      let third = paths[paths.length - 2];

      for(var item = 0; item < this.listTitles.length; item++){
          if(this.listTitles[item].path === first){
              return this.listTitles[item].title;
          }
      }
    
      for(var item = 0; item < this.listTitles.length; item++){
        if(this.listTitles[item].path === paths[paths.length - 2] + '/' + second){
            return this.listTitles[item].title;
        }
    }
    for(var item = 0; item < this.listTitles.length; item++){
        if(this.listTitles[item].path === paths[paths.length - 3] + '/' + third + '/' + second){
            return this.listTitles[item].title;
        }
    }

    // for(var item = 0; item < this.listTitles.length; item++){
    //     if(this.listTitles[item].path === titlee){
    //         return this.listTitles[item].title;
    //     }
    // }

      return 'Dashboard';
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }
}
