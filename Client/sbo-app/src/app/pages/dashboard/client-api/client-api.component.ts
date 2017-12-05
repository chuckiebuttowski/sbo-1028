import { Component, OnInit } from '@angular/core';

declare var $: any;

//extension
import { DataTable } from '../../../extensions/array.toDataTable';

//models
import { ClientAPI, APIParameter, ParameterType, PostDataValueType } from '../../../models/client-api.model';
import { SBOType } from '../../../models/log.model';

//service
import { ClientAPIService } from '../../../services/client-api.service';
import { SBOApiService } from '../../../services/sbo-api.service';

@Component({
  selector: 'app-client-api',
  templateUrl: './client-api.component.html',
  styleUrls: ['./client-api.component.css']
})
export class ClientApiComponent implements OnInit {

  NewParameter: APIParameter = new APIParameter();
  Type: typeof SBOType = SBOType;
  ValueType: typeof PostDataValueType = PostDataValueType;
  TransactionType: Array<any>; //= Object.keys(SBOType).filter(itm => !isNaN(Number.parseInt(itm)));
  
  Model: ClientAPI = new ClientAPI();
  APIs:DataTable ;

  constructor(private apiService: ClientAPIService, private sboApiService: SBOApiService) { }

  ngOnInit() {
    $.material.options.autofill = true;
    $.material.init();
    this.loadData();
  }

  async loadData(){
    this.APIs = (await this.apiService.getAll()).toDataTable(10);
    this.TransactionType = (await this.sboApiService.getSBOModels()).filter(x => !x["IsDetail"]);
  }

  getAbbrType(type:string) :string {
    if(type == '' || type == undefined){
      return '';
    }
    
    if(this.TransactionType== undefined){
      return '';
    }
    return this.TransactionType.filter(x => x["Name"] == type)[0]["SBOType"];
  }

  onAddNewParam() {
    this.Model.Params.push(this.NewParameter);
    this.NewParameter = new APIParameter();
  }

  onDeleteParam(param: APIParameter){
    var index = this.Model.Params.indexOf(param, 0);
    if (index > -1) {
       this.Model.Params.splice(index, 1);
    }
  }

  onClear(){
    this.Model = new ClientAPI();
    this.NewParameter = new APIParameter();
  }

  async onSave(){
    let result = await this.apiService.saveAPI(this.Model);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>Client API</b> - " + result

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
      this.Model = new ClientAPI();
      this.NewParameter = new APIParameter();
      this.loadData();
      $.notify({
        icon: "notifications",
        message: "<b>Success</b> - New api is saved."

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

  async deleteApi(id: number){
    let result = await this.apiService.deleteAPI(id);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>Client API</b> - " + result

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
      this.NewParameter = new APIParameter();
      this.loadData();
    }
  }

  changeModel(api: ClientAPI){
    this.Model = api;
  }
}
