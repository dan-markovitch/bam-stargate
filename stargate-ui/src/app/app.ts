import { Component } from '@angular/core';
import { PersonSearchComponent } from './components/person-search/person-search';

@Component({
  selector: 'app-root',
  imports: [PersonSearchComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'stargate-ui';
}