import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReviewKnowledgeComponent } from './review-knowledge/review-knowledge.component';
import { ReviewKnowledgeChildComponent } from './review-knowledge/review-knowledge-child/review-knowledge-child.component';
import { ToggleComponent } from './review-knowledge/toggle/toggle.component';

@NgModule({
  declarations: [
    AppComponent,
    ReviewKnowledgeComponent,
    ReviewKnowledgeChildComponent,
    ToggleComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
