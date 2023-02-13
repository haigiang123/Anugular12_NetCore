import { Component, EventEmitter, Input, Output } from "@angular/core";
import { TabPanelComponent } from "./tab-panel.component";

@Component({
  selector: "tab-group",
  templateUrl: "./tab-group.component.html"
})

export class TabGroupComponent{

  tabGroup: TabPanelComponent[] = [];

  tabIndex: number = 0;

  Add(tabPanel: TabPanelComponent)
  {
    this.tabGroup.push(tabPanel);
  }

  SelectPanel(idx: number){
    this.tabIndex = idx;
  }

  // removeTabPanel(idx: number){
  //   this.tabGroup.forEach((item, index) => {
  //       if(item === idx){

  //       }
  //   })
  // }
}
