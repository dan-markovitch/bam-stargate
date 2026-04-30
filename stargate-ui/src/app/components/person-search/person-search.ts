import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StargateService } from '../../services/stargate';
import { DutyHistoryComponent } from '../duty-history/duty-history';

@Component({
  selector: 'app-person-search',
  imports: [FormsModule, CommonModule, DutyHistoryComponent],
  templateUrl: './person-search.html',
  styleUrl: './person-search.css'
})
export class PersonSearchComponent {
  searchName: string = '';
  person: any = null;
  duties: any[] = [];
  loading: boolean = false;
  error: string = '';
  searched: boolean = false;

  constructor(private stargateService: StargateService) {}

  search() {
    if (!this.searchName.trim()) return;

    this.loading = true;
    this.error = '';
    this.person = null;
    this.duties = [];
    this.searched = true;

    this.stargateService.getAstronautDutiesByName(this.searchName).subscribe({
      next: (response) => {
        this.person = response.person;
        this.duties = response.astronautDuties || [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Person not found or an error occurred.';
        this.loading = false;
      }
    });
  }
}