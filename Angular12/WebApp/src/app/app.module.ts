import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReviewKnowledgeComponent } from './review-knowledge/review-knowledge.component';
import { ReviewKnowledgeChildComponent } from './review-knowledge/review-knowledge-child/review-knowledge-child.component';
import { ToggleComponent } from './review-knowledge/toggle/toggle.component';
import { TabContainerComponent } from './review-knowledge/tab-container/tab-container.component';

@NgModule({
  declarations: [
    AppComponent,
    ReviewKnowledgeComponent,
    ReviewKnowledgeChildComponent,
    ToggleComponent,
    TabContainerComponent
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
