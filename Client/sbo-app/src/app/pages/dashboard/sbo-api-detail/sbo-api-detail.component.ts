import { Component, OnInit } from '@angular/core';
import {Location, LocationStrategy, PathLocationStrategy} from '@angular/common';

//models
import { APIControllerDocumentation, APIActionDocumentation } from '../../../models/sbo-api.model';

//services
import { SBOApiService } from '../../../services/sbo-api.service';

@Component({
  selector: 'app-sbo-api-detail',
  templateUrl: './sbo-api-detail.component.html',
  styleUrls: ['./sbo-api-detail.component.css']
})
export class SboApiDetailComponent implements OnInit {

  ControllerName: string;
  Model: APIControllerDocumentation = null;
  CurrentEndpoint: APIActionDocumentation = null;

  constructor(private service: SBOApiService, private location: Location) { 
    this.ControllerName = this.location.prepareExternalUrl(this.location.path()).split('/').pop();
  }

  ngOnInit() {
    this.loadData();
  }

  async loadData(){
    this.Model = await this.service.getAPIDetail(this.ControllerName);
    this.CurrentEndpoint = this.Model.Actions[0];
  }

  showDetail(action: APIActionDocumentation){
    this.CurrentEndpoint = action;
  }

}
