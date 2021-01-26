import { HelloWorldObj } from './../models/HelloWorldObj';
import { QuizService } from './../Quiz.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  title = 'QuizPlayAngular';
  public obj: HelloWorldObj;

  ngOnInit(): void {
    this.obj = new HelloWorldObj();
    this.service.getBogusObject()
    .subscribe(response => {
      this.obj = response;
    })
  }

  constructor(public service: QuizService){}

}
