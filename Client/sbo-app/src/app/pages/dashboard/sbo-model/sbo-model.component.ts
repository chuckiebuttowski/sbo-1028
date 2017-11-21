import { Component, OnInit } from '@angular/core';
import {Location, LocationStrategy, PathLocationStrategy} from '@angular/common';

import { SBOApiService } from '../../../services/sbo-api.service';

@Component({
  selector: 'app-sbo-model',
  templateUrl: './sbo-model.component.html',
  styleUrls: ['./sbo-model.component.css']
})
export class SboModelComponent implements OnInit {

  ModelName:string = "";
  Fields: any[];
  constructor(private location: Location, private service: SBOApiService) { 
    this.ModelName = this.location.prepareExternalUrl(this.location.path()).split('/').pop();
    
  }

  ngOnInit() {
    this.loadData();
  }

  async loadData(){
    this.Fields = await this.service.getSBOModelFields(this.ModelName);
  }
}
