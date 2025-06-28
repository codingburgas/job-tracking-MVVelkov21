import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobApplicationService } from '../../services/job-application.service';
import { JobPostingService } from '../../services/job-posting.service';
import { AuthService } from '../../services/auth.service';
import { JobApplication } from '../../models/job-application.model';
import { JobPosting } from '../../models/job-posting.model';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-my-applications',
  standalone: true,
  imports: [CommonModule, MessageBoxComponent],
  templateUrl: './my-applications.component.html'
})
export class MyApplicationsComponent implements OnInit {
  applications: (JobApplication & { jobPosting?: JobPosting })[] = [];
  message: string | null = null;
  messageType: 'info' | 'error' | 'success' = 'info';
  loading: boolean = true;

  constructor(
    private jobApplicationService: JobApplicationService,
    private jobPostingService: JobPostingService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.fetchMyApplications();
  }

  fetchMyApplications(): void {
    this.loading = true;
    this.message = null;
    const currentUserId = this.authService.currentUserValue?.id || '';
    if (!currentUserId) {
        this.message = 'User not logged in or ID not found.';
        this.messageType = 'error';
        this.loading = false;
        return;
    }

    this.jobApplicationService.getUserApplications(currentUserId).subscribe({
      next: (data) => {
        if (data.length === 0) {
          this.applications = [];
          this.message = 'You have not submitted any applications yet.';
          this.messageType = 'info';
          this.loading = false;
          return;
        }

        const jobPostingRequests = data.map(app =>
          this.jobPostingService.getJobPostingById(app.jobPostingId)
        );

        forkJoin(jobPostingRequests).subscribe({
          next: (jobPostingsData) => {
            this.applications = data.map((app, index) => ({
              ...app,
              jobPosting: jobPostingsData[index]
            }));
            this.loading = false;
          },
          error: (jobPostingError) => {
            console.error('Error fetching job posting details for applications:', jobPostingError);
            this.message = 'Failed to load some job details for your applications.';
            this.messageType = 'error';
            this.applications = data;
            this.loading = false;
          }
        });
      },
      error: (err) => {
        this.message = err.message || 'Failed to fetch your applications.';
        this.messageType = 'error';
        this.loading = false;
      }
    });
  }
}