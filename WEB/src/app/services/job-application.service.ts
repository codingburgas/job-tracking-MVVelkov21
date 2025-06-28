import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { JobApplication, ApplyForJobRequest, UpdateApplicationStatusRequest } from '../models/job-application.model';

@Injectable({
  providedIn: 'root'
})
export class JobApplicationService {
  private apiUrl = `${environment.apiBaseUrl}/JobApplication`;

  constructor(private http: HttpClient) { }

  applyForJob(request: ApplyForJobRequest): Observable<JobApplication> {
    return this.http.post<JobApplication>(`${this.apiUrl}/apply`, request);
  }

  getUserApplications(userId: string): Observable<JobApplication[]> {
    return this.http.get<JobApplication[]>(`${this.apiUrl}/my-applications`);
  }

  updateApplicationStatus(applicationId: string, request: UpdateApplicationStatusRequest): Observable<JobApplication> {
    return this.http.patch<JobApplication>(`${this.apiUrl}/${applicationId}/status`, request);
  }

  getAllApplicationsForJobPosting(jobPostingId: string): Observable<JobApplication[]> {
    return this.http.get<JobApplication[]>(`${this.apiUrl}/job/${jobPostingId}`);
  }

  getJobApplicationById(applicationId: string): Observable<JobApplication> {
    return this.http.get<JobApplication>(`${this.apiUrl}/${applicationId}`);
  }
}