import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { JobPostingService } from '../../services/job-posting.service';
import { JobPosting } from '../../models/job-posting.model';
import { MessageBoxComponent } from '../message-box/message-box.component';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, MessageBoxComponent],
  templateUrl: './job-list.component.html'
})
export class JobListComponent implements OnInit {
  jobPostings: JobPosting[] = [];
  searchTerm: string = '';
  statusFilter: 'Active' | 'Inactive' | '' = 'Active';
  message: string | null = null;
  messageType: 'info' | 'error' | 'success' = 'info';
  loading: boolean = false;

  constructor(private jobPostingService: JobPostingService) { }

  ngOnInit(): void {
    this.fetchJobPostings();
  }

  fetchJobPostings(): void {
    this.loading = true;
    this.message = null;
    this.jobPostingService.getAllJobPostings(this.searchTerm, this.statusFilter || undefined).subscribe({
      next: (data) => {
        this.jobPostings = data;
        this.loading = false;
      },
      error: (err) => {
        this.message = err.message || 'Failed to fetch job postings.';
        this.messageType = 'error';
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.fetchJobPostings();
  }
}