import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Author } from './Author';

@Component({
  selector: 'app-review-knowledge-child',
  templateUrl: './review-knowledge-child.component.html',
  styleUrls: ['./review-knowledge-child.component.css']
})
export class ReviewKnowledgeChildComponent implements OnInit {

 @Input() backgroundColor = "";
 @Input() progressColor = "";
 @Input() progress = 0;
 @Input() listAuthorInput!: Author[];
 @Output() handleDeleteAction = new EventEmitter<number>();

  constructor() { }

  ngOnInit() {
  }

  deleteObject(id: number)
  {
    this.handleDeleteAction.emit(id);
  }

  // ngOnChanges(changes: SimpleChanges)
  // {
  //     console.log('chan');
  //     if('reviseProcess' in changes)
  //     {
  //        if(typeof changes['reviseProcess'].currentValue !== 'number' )
  //        {
  //           let reviseProcess = Number(changes['reviseProcess'].currentValue)
  //           if(Number.isNaN(reviseProcess))
  //           {
  //               this.progress = 0;
  //           }
  //           else
  //           {
  //               this.progress = reviseProcess;
  //           }
  //        }
  //     }
  // }

}
