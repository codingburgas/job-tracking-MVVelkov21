import { Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AdminGuard } from './admin.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./components/login-register/login-register.component').then(m => m.LoginRegisterComponent) },
  { path: 'jobs', loadComponent: () => import('./components/job-list/job-list.component').then(m => m.JobListComponent) },
  { path: 'jobs/:id', loadComponent: () => import('./components/job-detail/job-detail.component').then(m => m.JobDetailComponent) },
  { path: 'my-applications', loadComponent: () => import('./components/my-applications/my-applications.component').then(m => m.MyApplicationsComponent), canActivate: [AuthGuard] },
  { path: 'admin', loadComponent: () => import('./components/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent), canActivate: [AdminGuard] },
  { path: 'admin/create-job', loadComponent: () => import('./components/create-job-posting/create-job-posting.component').then(m => m.CreateJobPostingComponent), canActivate: [AdminGuard] },
  { path: 'admin/manage-applications', loadComponent: () => import('./components/manage-applications/manage-applications.component').then(m => m.ManageApplicationsComponent), canActivate: [AdminGuard] },
  { path: '', redirectTo: '/jobs', pathMatch: 'full' },
  { path: '**', redirectTo: '/jobs' }
];