import { Component, OnInit } from '@angular/core';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    isChildPage: boolean;
}
export const ROUTES: RouteInfo[] = [
    { path: 'home', title: 'Dashboard',  icon: 'dashboard', class: '', isChildPage: false},
    { path: 'config', title: 'Server Configuration',  icon: 'dashboard', class: '', isChildPage: false },
    { path: 'transaction-logs', title: 'Transaction Logs',  icon: 'dashboard', class: '', isChildPage: false },
    { path: 'client-api', title: 'APIs',  icon: 'dashboard', class: '', isChildPage: false },
    { path: 'sbo-api', title: 'SBO API Docs',  icon: 'dashboard', class: '', isChildPage: false },
    { path: 'accounts', title: 'Accounts',  icon:'person', class: '', isChildPage: false },
    { path: 'user-profile', title: 'User Profile',  icon:'person', class: '', isChildPage: true },
    { path: 'detail', title: 'Log Detail',  icon:'person', class: '', isChildPage: true },
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

  constructor() { }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => !menuItem.isChildPage);
  }
  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };
}
