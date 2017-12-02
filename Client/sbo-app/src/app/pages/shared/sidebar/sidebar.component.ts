import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

//services
import { AuthService } from '../../../services/auth.service';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    isChildPage: boolean;
}
export const ROUTES: RouteInfo[] = [
    // { path: 'home', title: 'Dashboard',  icon: 'dashboard', class: '', isChildPage: false},
    { path: 'transaction-logs', title: 'Transaction Logs',  icon: 'svg/transaction.svg', class: '', isChildPage: false },
    { path: 'config', title: 'Server Configuration',  icon: 'svg/config.svg', class: '', isChildPage: false },
    { path: 'client-api', title: 'APIs',  icon: 'svg/apis.svg', class: '', isChildPage: false },
    { path: 'sbo-api', title: 'SBO API Docs',  icon: 'svg/sapdiapi.svg', class: '', isChildPage: false },
    { path: 'sbo-api/detail', title: 'API Documentation',  icon: 'dashboard', class: '', isChildPage: true },
    { path: 'sbo-api/model/detail', title: 'SBO Model Documentation',  icon: 'dashboard', class: '', isChildPage: true },
    { path: 'accounts', title: 'Accounts',  icon:'svg/accounts.svg', class: '', isChildPage: false },
    { path: 'user-profile', title: 'User Profile',  icon:'person', class: '', isChildPage: true },
    { path: 'transaction-logs/detail', title: 'Log Detail',  icon:'person', class: '', isChildPage: true },
    // { path: 'table-list', title: 'Table List',  icon:'content_paste', class: '' },
    // { path: 'typography', title: 'Typography',  icon:'library_books', class: '' },
    // { path: 'icons', title: 'Icons',  icon:'bubble_chart', class: '' },
    // { path: 'maps', title: 'Maps',  icon:'location_on', class: '' },
    // { path: 'notifications', title: 'Notifications',  icon:'notifications', class: '' },
    // { path: 'upgrade', title: 'Upgrade to PRO',  icon:'unarchive', class: 'active-pro' },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  UserDisplayName: string;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => !menuItem.isChildPage);
    this.UserDisplayName = this.authService.getUserDisplayName();
  }

  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
}
}
