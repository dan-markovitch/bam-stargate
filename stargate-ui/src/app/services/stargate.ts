import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StargateService {
  private baseUrl = 'http://localhost:5204';

  constructor(private http: HttpClient) {}

  getPersonByName(name: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/Person/${name}`);
  }

  getAstronautDutiesByName(name: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/AstronautDuty/${name}`);
  }

  getAllPeople(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Person`);
  }
}