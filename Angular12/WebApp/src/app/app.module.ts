import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReviewKnowledgeComponent } from './review-knowledge/review-knowledge.component';
import { ReviewKnowledgeChildComponent } from './review-knowledge/review-knowledge-child/review-knowledge-child.component';
import { ToggleComponent } from './review-knowledge/toggle/toggle.component';
import { TabContainerComponent } from './review-knowledge/tab-container/tab-container.component';
import { TabGroupComponent } from './review-knowledge/tab-group/tab-group.component';
import { TabPanelComponent } from './review-knowledge/tab-group/tab-panel.component';
import { BsTabGroupComponent } from './review-knowledge/tab-group/bs-tab-group.component';

@NgModule({
  declarations: [
    AppComponent,
    ReviewKnowledgeComponent,
    ReviewKnowledgeChildComponent,
    ToggleComponent,
    TabContainerComponent,
    TabGroupComponent,
    TabPanelComponent,
    BsTabGroupComponent
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
