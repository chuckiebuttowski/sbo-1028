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

  Model: TransactionLog;
  Type: typeof SBOType = SBOType;
  
  constructor(private router: Router, private route: ActivatedRoute, private service: TransactionLogService) { }

  ngOnInit() {
    $.material.options.autofill = true;
    this.loadData();
  }

  async loadData(){
    let id: number = this.route.snapshot.params['id'];
    this.Model = await this.service.getLogDetail(id);
  }
}
