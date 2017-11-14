import { Component, OnInit } from '@angular/core';

declare var $: any;

//models
import { SAPProfile, ServerConfig } from '../../../models/config.model';

//service
import { ConfigService } from '../../../services/config.service';

@Component({
  selector: 'app-server-config',
  templateUrl: './server-config.component.html',
  styleUrls: ['./server-config.component.css']
})
export class ServerConfigComponent implements OnInit {

  Config: ServerConfig = new ServerConfig();
  SetActiveNewlyAddedServer: boolean = false;
  Profile: SAPProfile;

  RecentServers: ServerConfig[];

  constructor(private configService: ConfigService) { 
  }

  ngOnInit() {
    this.loadData();
  }

  async loadData(){
    this.configService.testAccessAuthorizeAPI(); 
    this.Profile = await this.configService.getSAPProfile();
    this.RecentServers = await this.configService.getRecentServers();
  }

  async onUpdateProfile() {
    var result = await this.configService.saveSAPProfile(this.Profile);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>SAP Profile Error</b> - " + result

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
      $.notify({
        icon: "notifications",
        message: "<b>Success</b> - SAP Profile is updated."

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

  async onSaveConfig() {
    let result = await this.configService.saveServer(this.Config, this.SetActiveNewlyAddedServer);
    if (result.toLowerCase() != 'success') {
      $.notify({
        icon: "notifications",
        message: "<b>Server Configuration Error</b> - " + result

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
      this.Config = new ServerConfig();
      $.notify({
        icon: "notifications",
        message: "<b>Success</b> - New configuration is saved."

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

  onEditConfig() {

  }

  async onActivateServer(id: number) {
    let result: string = await this.configService.activateServer(id);
    if(result.toLowerCase() != 'success'){
      $.notify({
        icon: "notifications",
        message: "<b>Server Configuration Error</b> - " + result

      }, {
          type: 'danger',
          timer: 500,
          placement: {
            from: 'top',
            align: 'right'
          }
        });
    }

    this.loadData();
  }

  async onDeleteConfig(id: number) {
    let result: string = await this.configService.deleteServer(id);
    if(result.toLowerCase() != 'success'){
      $.notify({
        icon: "notifications",
        message: "<b>Server Configuration Error</b> - " + result

      }, {
          type: 'danger',
          timer: 500,
          placement: {
            from: 'top',
            align: 'right'
          }
        });
    }

    this.loadData();
  }

}
