declare const $: any;

import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { TransactionLog, TransactionData, ErrorLog, SBOType } from '../../../models/log.model';

import { TransactionLogService } from '../../../services/transaction-log.service';

@Component({
  selector: 'app-log-detail',
  templateUrl: './log-detail.component.html',
  styleUrls: ['./log-detail.component.css']
})
export class LogDetailComponent implements OnInit {

  Model: TransactionLog = new TransactionLog();
  Type: typeof SBOType = SBOType;
  CardBg: string = "green";
  constructor(private router: Router, private route: ActivatedRoute, private service: TransactionLogService) { }

  ngOnInit() {
    $.material.options.autofill = true;
    this.loadData();
  }

  async loadData(){
    let id: number = this.route.snapshot.params['id'];
    this.Model = await this.service.getLogDetail(id);
    this.CardBg = this.Model.IsPosted ? "green" : "red";
  }

  async onRetryPosting(){
    let result = await this.service.retryPosting(this.Model.ID);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>Error</b> - " + result

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
      this.loadData();
      $.notify({
        icon: "notifications",
        message: "<b>Success</b> - Payload is posted."

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
}
