import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

//services
import { TransactionLogService } from '../../../services/transaction-log.service';
import { DataTable } from '../../../extensions/array.toDataTable';

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
  HasLogs: boolean;
  DataTable: DataTable;

  constructor(private service: TransactionLogService, private router: Router) { }

  ngOnInit() {
    this.Logs = new Array<TransactionLog>();
    this.loadData();
  }

  async loadData() {
    let logs = await this.service.getAllLogs();
    this.DataTable = logs.toDataTable(5);
    this.HasLogs = logs.length > 0;
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

  viewDetail(log: TransactionLog){
    this.router.navigate(['/dashboard/transaction-logs/detail/' + log.ID]);
  }

}
