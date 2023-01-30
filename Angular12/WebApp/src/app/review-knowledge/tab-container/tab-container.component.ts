import { Component, Input, TemplateRef } from "@angular/core";

@Component({
  selector: "tab-container",
  templateUrl: "./tab-container.component.html"
})

export class TabContainerComponent
{
    @Input() headerTemplate?: TemplateRef<any>;

    User = [{FirstName : "F123", LastName : "L123"}, {FirstName : "F456", LastName : "L456"}];
    isLoad: boolean = false;

    DisplayList(): void{
      this.isLoad = !this.isLoad;
    }
}
