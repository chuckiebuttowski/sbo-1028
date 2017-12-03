import { Component, OnInit } from '@angular/core';

declare var $: any;

//extensions
import { DataTable } from '../../../extensions/array.toDataTable';

//service
import { AccountService } from '../../../services/account.service';

//model
import { Account } from '../../../models/account.model';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit {
  UserAccount: Account = new Account();
  Accounts: DataTable;

  constructor(private acctService: AccountService) { }

  ngOnInit() {
    $.material.options.autofill = true;
    $.material.init();
    this.loadData();
  }

  async loadData(){
    this.Accounts = (await this.acctService.getAccounts()).toDataTable(10);
  }

  async onSaveAccount(){
    let result = await this.acctService.saveAccount(this.UserAccount);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>Create User</b> - " + result

      }, {
          type: 'danger',
          timer: 500,
          placement: {
            from: 'top',
            align: 'right'
          }
        });
    }
    else {
      this.UserAccount = new Account();
      this.loadData();
      $.notify({
        icon: "notifications",
        message: "<b>Success</b> - User is saved."

      }, {
          type: 'success',
          timer: 500,
          placement: {
            from: 'top',
            align: 'right'
          }
        });
    }
  }

  onEdit(acct: Account){
    this.UserAccount = acct;
  }

  async onDelete(acct: Account){
    let result = await this.acctService.deleteAccount(acct.ID);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>Create User</b> - " + result

      }, {
          type: 'danger',
          timer: 500,
          placement: {
            from: 'top',
            align: 'right'
          }
        });
    }
    else {
      this.UserAccount = new Account();
      this.loadData();
    }
  }

  reset(){
    this.UserAccount = new Account();
  }

}
