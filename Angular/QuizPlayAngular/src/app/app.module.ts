import { MaterialModule } from './material/material.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateQuizComponent, CreateQuizConfirmDialog } from './create-quiz/create-quiz.component'

@NgModule({
  declarations: [
    AppComponent,
    CreateQuizComponent,
    CreateQuizConfirmDialog
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MaterialModule,
    BrowserAnimationsModule
  ],
  entryComponents: [
    CreateQuizComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
