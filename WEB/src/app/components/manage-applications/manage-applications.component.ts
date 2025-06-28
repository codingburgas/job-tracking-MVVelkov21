import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JobApplicationService } from '../../services/job-application.service';
import { JobPostingService } from '../../services/job-posting.service';
import { JobApplication } from '../../models/job-application.model';
import { JobPosting } from '../../models/job-posting.model';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { forkJoin, lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-manage-applications',
  standalone: true,
  imports: [
    CommonModule,     
    FormsModule,      
    RouterLink,       
    MessageBoxComponent 
  ],
  templateUrl: './manage-applications.component.html'
})
export class ManageApplicationsComponent implements OnInit {
  jobPostings: JobPosting[] = [];
  selectedJobPostingId: string | null = null;
  applications: (JobApplication & { user?: any })[] = []; 
  message: string | null = null;
  messageType: 'info' | 'error' | 'success' = 'info';
  loadingJobs: boolean = true;
  loadingApplications: boolean = false;
  updatingStatus: { [key: string]: boolean } = {}; 

  constructor(
    private jobPostingService: JobPostingService,
    private jobApplicationService: JobApplicationService,
    public router: Router 
  ) { }

  ngOnInit(): void {
    this.fetchAllJobPostings();
  }

  fetchAllJobPostings(): void {
    this.loadingJobs = true;
    this.jobPostingService.getAllJobPostings().subscribe({
      next: (data) => {
        this.jobPostings = data;
        this.loadingJobs = false;
        if (this.jobPostings.length > 0) {
          this.selectedJobPostingId = this.jobPostings[0].id; 
          this.fetchApplicationsForSelectedJob();
        }
      },
      error: (err) => {
        this.message = err.message || 'Failed to load job postings.';
        this.messageType = 'error';
        this.loadingJobs = false;
      }
    });
  }

  fetchApplicationsForSelectedJob(): void {
    if (!this.selectedJobPostingId) {
      this.applications = [];
      return;
    }

    this.loadingApplications = true;
    this.message = null;
    this.jobApplicationService.getAllApplicationsForJobPosting(this.selectedJobPostingId).subscribe({
      next: (data) => {
        if (data.length === 0) {
          this.applications = [];
          this.message = 'No applications found for this job posting.';
          this.messageType = 'info';
          this.loadingApplications = false;
          return;
        }
        
        this.applications = data.map(app => ({ ...app, user: null }));
        this.loadingApplications = false;
      },
      error: (err) => {
        this.message = err.message || 'Failed to load applications for selected job.';
        this.messageType = 'error';
        this.loadingApplications = false;
      }
    });
  }

  async updateApplicationStatus(application: JobApplication, newStatus: 'Submitted' | 'ApprovedForInterview' | 'Rejected'): Promise<void> {
    this.updatingStatus[application.id] = true;
    this.message = null;

    try {
      const request = { newStatus };
      const updatedApp = await lastValueFrom(this.jobApplicationService.updateApplicationStatus(application.id, request));
      if (updatedApp) {    
        const index = this.applications.findIndex(app => app.id === updatedApp.id);
        if (index > -1) {
          this.applications[index].status = updatedApp.status;
        }
        this.message = `Application status for ${application.id.substring(0, 8)}... updated to ${newStatus}.`;
        this.messageType = 'success';
      } else {
        throw new Error('Failed to update application status: Unexpected response.');
      }
    } catch (error: any) {
      this.message = error.message || 'Error updating application status.';
      this.messageType = 'error';
    } finally {
      this.updatingStatus[application.id] = false;
    }
  }
}