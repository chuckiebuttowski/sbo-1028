import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
//import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app.routing';
//import { ComponentsModule } from './components/components.module';

import { AppComponent } from './app.component';

//import { DashboardComponent } from './dashboard/dashboard.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { TableListComponent } from './table-list/table-list.component';
import { TypographyComponent } from './typography/typography.component';
import { IconsComponent } from './icons/icons.component';
import { MapsComponent } from './maps/maps.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { UpgradeComponent } from './upgrade/upgrade.component';

//modules
import { DashboardModule } from './pages/dashboard/dashboard.module';
//components
import { LoginComponent } from './pages/login/login.component';

//extension
import { DataTable } from './extensions/array.toDataTable';

//services
import { AuthService } from './services/auth.service';
import { ConfigService } from './services/config.service';
import { AccountService } from './services/account.service';
import { TransactionLogService } from './services/transaction-log.service';
import { ClientAPIService } from './services/client-api.service';
import { SBOApiService } from './services/sbo-api.service';

//guards
import { AuthGuard } from './guards/auth.guard';

//interceptors
import { TokenInterceptor } from './interceptors/token.interceptor';

//pipe
//import { TransactionLogFilterPipe } from './pipes/transaction-log.pipe';

@NgModule({
  declarations: [
    AppComponent,
    // DashboardComponent,
    UserProfileComponent,
    TableListComponent,
    TypographyComponent,
    IconsComponent,
    MapsComponent,
    NotificationsComponent,
    UpgradeComponent,
    LoginComponent,

  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule,
    AppRoutingModule,
    DashboardModule
  ],
  providers: [DataTable, AuthService, ConfigService, AccountService, TransactionLogService, ClientAPIService, SBOApiService, AuthGuard,{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true,
  }],
  bootstrap: [AppComponent],
})
export class AppModule { }
