import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';

//guards
import { AuthGuard } from './guards/auth.guard';

import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { HomeComponent } from './pages/dashboard/home/home.component';
import { ServerConfigComponent } from './pages/dashboard/server-config/server-config.component';
import { TransactionLogsComponent } from './pages/dashboard/transaction-logs/transaction-logs.component';
import { LogDetailComponent } from './pages/dashboard/log-detail/log-detail.component';
import { ClientApiComponent } from './pages/dashboard/client-api/client-api.component';
import { SboApiComponent } from './pages/dashboard/sbo-api/sbo-api.component';
import { SboApiDetailComponent } from './pages/dashboard/sbo-api-detail/sbo-api-detail.component';
import { AccountsComponent } from './pages/dashboard/accounts/accounts.component';

// import { DashboardComponent } from './dashboard/dashboard.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { TableListComponent } from './table-list/table-list.component';
import { TypographyComponent } from './typography/typography.component';
import { IconsComponent } from './icons/icons.component';
import { MapsComponent } from './maps/maps.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { UpgradeComponent } from './upgrade/upgrade.component';

const routes: Routes =[
    { path: 'login',          component: LoginComponent },
    { path: 'dashboard',      component: DashboardComponent, canActivate: [AuthGuard], children:[
        {path: 'home',              component: HomeComponent },
        {path: 'config',            component: ServerConfigComponent },
        {path: 'transaction-logs',  component: TransactionLogsComponent },
        {path: 'transaction-logs/detail/:id',  component: LogDetailComponent },
        {path: 'client-api',               component: ClientApiComponent },
        {path: 'sbo-api',           component: SboApiComponent },
        {path: 'sbo-api/detail/:d',           component: SboApiDetailComponent },
        {path: 'accounts',          component: AccountsComponent },
        { path: 'user-profile',   component: UserProfileComponent },
        { path: 'table-list',     component: TableListComponent },
        { path: 'typography',     component: TypographyComponent },
        { path: 'icons',          component: IconsComponent },
        { path: 'maps',           component: MapsComponent },
        { path: 'notifications',  component: NotificationsComponent },
        { path: 'upgrade',        component: UpgradeComponent },
    ]},
    { path: '',               redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
  ],
})
export class AppRoutingModule { }
