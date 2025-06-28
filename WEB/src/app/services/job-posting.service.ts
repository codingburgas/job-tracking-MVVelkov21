import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { JobPosting, CreateJobPostingRequest, UpdateJobPostingRequest } from '../models/job-posting.model';

@Injectable({
  providedIn: 'root'
})
export class JobPostingService {
  private apiUrl = `${environment.apiBaseUrl}/JobPosting`;

  constructor(private http: HttpClient) { }

  getAllJobPostings(searchTerm?: string, status?: 'Active' | 'Inactive', pageNumber: number = 1, pageSize: number = 10): Observable<JobPosting[]> {
    let params = new HttpParams();
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (status) {
      params = params.set('status', status);
    }
    params = params.set('pageNumber', pageNumber.toString());
    params = params.set('pageSize', pageSize.toString());

    return this.http.get<JobPosting[]>(this.apiUrl, { params });
  }

  getJobPostingById(id: string): Observable<JobPosting> {
    return this.http.get<JobPosting>(`${this.apiUrl}/${id}`);
  }

  createJobPosting(request: CreateJobPostingRequest): Observable<JobPosting> {
    return this.http.post<JobPosting>(this.apiUrl, request);
  }

  updateJobPosting(id: string, request: UpdateJobPostingRequest): Observable<JobPosting> {
    return this.http.put<JobPosting>(`${this.apiUrl}/${id}`, request);
  }

  deleteJobPosting(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}