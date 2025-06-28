import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { JobPostingService } from '../../services/job-posting.service';
import { JobApplicationService } from '../../services/job-application.service';
import { AuthService } from '../../services/auth.service';
import { JobPosting } from '../../models/job-posting.model';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { User } from '../../models/user.model';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-job-detail',
  standalone: true,
  imports: [CommonModule, MessageBoxComponent, RouterLink],
  templateUrl: './job-detail.component.html'
})
export class JobDetailComponent implements OnInit {
  jobPosting: JobPosting | null = null;
  message: string | null = null;
  messageType: 'info' | 'error' | 'success' = 'info';
  loading: boolean = false;
  applying: boolean = false;
  currentUser: User | null;

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private jobPostingService: JobPostingService,
    private jobApplicationService: JobApplicationService,
    public authService: AuthService
  ) {
    this.currentUser = this.authService.currentUserValue;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.fetchJobPostingDetails(id);
    } else {
      this.router.navigate(['/jobs']);
    }
  }

  fetchJobPostingDetails(id: string): void {
    this.loading = true;
    this.jobPostingService.getJobPostingById(id).subscribe({
      next: (data) => {
        this.jobPosting = data;
        this.loading = false;
      },
      error: (err) => {
        this.message = err.message || 'Failed to load job posting details.';
        this.messageType = 'error';
        this.loading = false;      
        this.router.navigate(['/jobs']);
      }
    });
  }

  canApply(): boolean {
    return this.authService.isLoggedIn() && !this.authService.isAdmin() && this.jobPosting?.status === 'Active';
  }

  async applyForJob(): Promise<void> {
    if (!this.canApply() || !this.jobPosting) {
      this.message = 'You are not authorized to apply or the job is not active.';
      this.messageType = 'error';
      return;
    }

    this.applying = true;
    this.message = null;

    try {
      await lastValueFrom(this.jobApplicationService.applyForJob({ jobPostingId: this.jobPosting.id }));
      this.message = 'Application submitted successfully!';
      this.messageType = 'success';      
      setTimeout(() => {
        this.router.navigate(['/my-applications']);
      }, 2000);
    } catch (error: any) {
      this.message = error.message || 'Failed to submit application.';
      this.messageType = 'error';
    } finally {
      this.applying = false;
    }
  }
}