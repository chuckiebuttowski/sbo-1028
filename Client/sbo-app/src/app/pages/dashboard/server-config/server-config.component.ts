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
  Profile: SAPProfile = new SAPProfile();

  RecentServers: ServerConfig[];

  constructor(private configService: ConfigService) { 
      this.loadData();
  }

  ngOnInit() {
    
  }

  async loadData(){
    this.configService.testAccessAuthorizeAPI(); 
    this.RecentServers = await this.configService.getRecentServers();
  }

  onUpdateProfile() {
    this.configService.saveSAPProfile(this.Profile).subscribe(msg => {
      if (msg != 'Success') {
        $.notify({
          icon: "notifications",
          message: "<b>SAP Profile Error</b> - " + msg

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
    });
  }

  async onSaveConfig() {
    let result = await this.configService.saveServer(this.Config);
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

  onActivateServer() {

  }

  onDeleteConfig() {

  }

}
