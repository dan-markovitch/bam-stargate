import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-duty-history',
  imports: [CommonModule],
  templateUrl: './duty-history.html',
  styleUrl: './duty-history.css'
})
export class DutyHistoryComponent {
  @Input() person: any = null;
  @Input() duties: any[] = [];
}