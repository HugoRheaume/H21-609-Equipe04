import { QuizService } from './../../Quiz.service';
import { Component, OnInit } from '@angular/core';
import { HelloWorldObj } from 'src/models/HelloWorldObj';

@Component({
  selector: 'app-helloWorld',
  templateUrl: './helloWorld.component.html',
  styleUrls: ['./helloWorld.component.scss']
})
export class HelloWorldComponent implements OnInit {

  constructor(public service: QuizService) { }

  public obj: HelloWorldObj;

  ngOnInit(): void {
    this.obj = new HelloWorldObj();
    this.service.getBogusObject()
    .subscribe(response => {
      this.obj = response;
    })
  }

}
