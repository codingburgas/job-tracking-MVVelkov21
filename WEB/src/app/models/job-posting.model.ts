export interface JobPosting {
  id: string;
  title: string;
  companyName: string;
  description: string;
  publicationDate: string;
  status: 'Active' | 'Inactive';
}

export interface CreateJobPostingRequest {
  title: string;
  companyName: string;
  description: string;
}

export interface UpdateJobPostingRequest {
  title?: string;
  companyName?: string;
  description?: string;
  status?: 'Active' | 'Inactive';
}