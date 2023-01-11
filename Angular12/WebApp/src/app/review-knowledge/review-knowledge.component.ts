import { Component, OnInit } from '@angular/core';
import { Author } from './review-knowledge-child/Author';

@Component({
  selector: 'app-review-knowledge',
  templateUrl: './review-knowledge.component.html',
  styleUrls: ['./review-knowledge.component.css']
})
export class ReviewKnowledgeComponent implements OnInit {

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

  deleteObject1(id: number)
  {
    this.listAuthor = this.listAuthor.splice(0, this.listAuthor.findIndex(x=>x.id === id));
  }
}
