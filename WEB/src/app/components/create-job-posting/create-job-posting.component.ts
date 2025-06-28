import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JobPostingService } from '../../services/job-posting.service';
import { CreateJobPostingRequest } from '../../models/job-posting.model';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-create-job-posting',
  standalone: true,
  imports: [CommonModule, FormsModule, MessageBoxComponent, RouterLink],
  templateUrl: './create-job-posting.component.html'
})
export class CreateJobPostingComponent {
  title: string = '';
  companyName: string = '';
  description: string = '';
  message: string | null = null;
  messageType: 'info' | 'error' | 'success' = 'info';
  loading: boolean = false;

  constructor(private jobPostingService: JobPostingService, public router: Router) { }

  async createJobPosting(): Promise<void> {
    this.loading = true;
    this.message = null;
    this.messageType = 'info';

    const request: CreateJobPostingRequest = {
      title: this.title,
      companyName: this.companyName,
      description: this.description
    };

    try {
      const response = await lastValueFrom(this.jobPostingService.createJobPosting(request));
      if (response && response.id) {
        this.message = 'Job posting created successfully!';
        this.messageType = 'success';    
        this.title = '';
        this.companyName = '';
        this.description = '';
        setTimeout(() => {
          this.router.navigate(['/admin/manage-applications']);
        }, 2000);
      } else {
        throw new Error('Failed to create job posting: Unexpected response.');
      }
    } catch (error: any) {
      this.message = error.message || 'An error occurred while creating job posting.';
      this.messageType = 'error';
    } finally {
      this.loading = false;
    }
  }
}