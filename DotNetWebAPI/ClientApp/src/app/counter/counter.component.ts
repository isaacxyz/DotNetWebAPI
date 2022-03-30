import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 1.0000000001;

  public incrementCounter() {
    this.currentCount *= this.currentCount;
  }
}
