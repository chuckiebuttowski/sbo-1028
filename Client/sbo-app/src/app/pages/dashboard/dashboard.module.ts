import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard.component';
import { HomeComponent } from './home/home.component';

import { AppRoutingModule } from '../../app.routing';

//pipes
import { TransactionLogFilterPipe } from '../../pipes/transaction-log.pipe';

//shared
import { SharedModule } from '../shared/shared.module';
import { ServerConfigComponent } from './server-config/server-config.component';
import { TransactionLogsComponent } from './transaction-logs/transaction-logs.component';
import { ClientApiComponent } from './client-api/client-api.component';
import { SboApiComponent } from './sbo-api/sbo-api.component';
import { AccountsComponent } from './accounts/accounts.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    RouterModule,
    SharedModule,
    AppRoutingModule,
  ],
  declarations: [DashboardComponent, HomeComponent, ServerConfigComponent, TransactionLogsComponent, ClientApiComponent, SboApiComponent, AccountsComponent, TransactionLogFilterPipe]
})
export class DashboardModule { }
