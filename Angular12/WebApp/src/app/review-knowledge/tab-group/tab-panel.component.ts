import { Component, OnInit, OnDestroy, TemplateRef, Input, Output, ViewChild } from "@angular/core";
import { TabGroupComponent } from "./tab-group.component";

@Component({
  selector: "tab-panel",
  templateUrl: "./tab-panel.component.html"
})

export class TabPanelComponent {

  @ViewChild(TemplateRef, {static: true}) panelBody!: TemplateRef<unknown>;

  @Input("title") title!: string;

  constructor(private tabGroup: TabGroupComponent){}

  ngOnInit(){
    this.tabGroup.Add(this);
  }
}

