import { JobPosting } from './job-posting.model';
import { User } from './user.model';

export interface JobApplication {
  id: string;
  userId: string;
  jobPostingId: string;
  status: 'Submitted' | 'ApprovedForInterview' | 'Rejected';
  applicationDate: string; 
  jobPosting?: JobPosting;
  user?: User | null;
}

export interface ApplyForJobRequest {
  jobPostingId: string;
}

export interface UpdateApplicationStatusRequest {
  newStatus: 'Submitted' | 'ApprovedForInterview' | 'Rejected';
}