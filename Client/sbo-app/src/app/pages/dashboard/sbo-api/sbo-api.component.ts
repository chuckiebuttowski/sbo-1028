import { Component, OnInit } from '@angular/core';

//models
import { APIControllerDocumentation } from '../../../models/sbo-api.model';

//services
import { SBOApiService } from '../../../services/sbo-api.service';

@Component({
  selector: 'app-sbo-api',
  templateUrl: './sbo-api.component.html',
  styleUrls: ['./sbo-api.component.css']
})
export class SboApiComponent implements OnInit {

  APIs: APIControllerDocumentation[] = [];
  SBOModels: string[];

  constructor(private service: SBOApiService) { }

  ngOnInit() {
    this.loadData();
  }

  async loadData() {
    this.APIs = await this.service.exploreAPI();
    this.SBOModels = await this.service.getSBOModels();
  }

}
