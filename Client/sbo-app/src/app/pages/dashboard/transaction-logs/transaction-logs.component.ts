import { Component, OnInit } from '@angular/core';

//services
import { TransactionLogService } from '../../../services/transaction-log.service';

//models
import { TransactionLog, TransactionData, ErrorLog, SBOType } from '../../../models/log.model';

@Component({
  selector: 'app-transaction-logs',
  templateUrl: './transaction-logs.component.html',
  styleUrls: ['./transaction-logs.component.css'],
})
export class TransactionLogsComponent implements OnInit {

  Logs: TransactionLog[];
  Type: typeof SBOType = SBOType;

  constructor(private service: TransactionLogService) { }

  ngOnInit() {
    this.loadData();
  }

  async loadData() {
    this.Logs = await this.service.getAllLogs();
  }

  getTotalLogs() : number{
    return this.Logs.length;
  }

  getTotalPostedLogs() : number {
    return this.Logs.filter((item: TransactionLog) => item.IsPosted ).length;
  }

  getTotalUnPostedLogs() : number {
    return this.Logs.filter((item: TransactionLog) => !item.IsPosted).length;
  }

}
