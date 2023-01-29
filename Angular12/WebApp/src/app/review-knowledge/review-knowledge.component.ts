import { AfterViewInit, Component, ElementRef, OnInit, QueryList, TemplateRef, ViewChild, ViewChildren } from '@angular/core';
import { Author } from './review-knowledge-child/Author';
import { ToggleComponent } from './toggle/toggle.component';

@Component({
  selector: 'app-review-knowledge',
  templateUrl: './review-knowledge.component.html',
  styleUrls: ['./review-knowledge.component.css']
})
export class ReviewKnowledgeComponent implements OnInit {

  @ViewChild("nameInput", {}) elementRef?: ElementRef;
  // @ViewChild("componentRef", {read: ElementRef}) componentRef? : ElementRef;
  @ViewChild("componentRef", {}) componentRef? : ToggleComponent;

  // Get form element
  @ViewChild('nameForm', {
    read: ElementRef,
    static: true,
  })
  form?: ElementRef<HTMLFormElement>;

  @ViewChild("name", {}) nameTempValue?: ElementRef;

  // Get list element of toggleComponent
  @ViewChildren(ToggleComponent) toggleList?: QueryList<ToggleComponent>;


  userInfo = [{
    Name: "Name1",
    Age: 11,
    Address: "Address1"
  }, {
    Name: "Name2",
    Age: 22,
    Address: "Address2"
  }, {
    Name: "Name3",
    Age: 33,
    Address: "Address3"
  }];

  listAuthor: Author[] = [
    {
      id : 1,
      email : "aa@gmail.com",
      firstName : "first name",
      lastName : "last name",
      gender: "M",
      ipAddress: "123.123.213.123"
    },
    {
      id : 2,
      email : "bb@gmail.com",
      firstName : "first name",
      lastName : "last name",
      gender: "M",
      ipAddress: "123.123.213.123"
    }
  ];

  checked = false;

  constructor() { }

  ngOnInit(): void {

  }

  DeleteObject1(id: number)
  {
    this.listAuthor = this.listAuthor.filter(x => x.id !== id);
  }

  ViewChild()
  {
    console.log(this.elementRef?.nativeElement);
    this.componentRef?.toggle();
    console.log(this.form);
    console.log(this.nameTempValue);
    console.log(this.toggleList?.last.checked);
  }
}
