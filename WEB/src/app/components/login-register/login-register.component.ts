import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserLoginRequest, UserRegistrationRequest } from '../../models/user.model';
import { MessageBoxComponent } from '../message-box/message-box.component';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-login-register',
  standalone: true,
  imports: [CommonModule, FormsModule, MessageBoxComponent],
  templateUrl: './login-register.component.html'
})
export class LoginRegisterComponent implements OnInit {
  isLogin: boolean = true;
  firstName: string = '';
  lastName: string = '';
  username: string = '';
  password: string = '';
  message: string | null = null;
  messageType: 'info' | 'error' | 'success' = 'info';
  loading: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/jobs']);
    }
  }

  toggleForm(): void {
    this.isLogin = !this.isLogin;
    this.resetForm();
  }

  resetForm(): void {
    this.firstName = '';
    this.lastName = '';
    this.username = '';
    this.password = '';
    this.message = null;
    this.messageType = 'info';
    this.loading = false;
  }

  async handleSubmit(): Promise<void> {
    this.loading = true;
    this.message = null;
    this.messageType = 'info';

    try {
      if (this.isLogin) {
        const loginRequest: UserLoginRequest = { username: this.username, password: this.password };
        const response = await lastValueFrom(this.authService.login(loginRequest));
        if (response && response.token) {
          this.message = 'Login successful!';
          this.messageType = 'success';          
          if (this.authService.isAdmin()) {
            this.router.navigate(['/admin']);
          } else {
            this.router.navigate(['/jobs']);
          }
        } else {
          throw new Error('Login failed: Invalid credentials or unexpected response.');
        }
      } else {
        const registrationRequest: UserRegistrationRequest = {
          firstName: this.firstName,
          lastName: this.lastName,
          username: this.username,
          password: this.password
        };
        const response = await lastValueFrom(this.authService.register(registrationRequest));
        if (response && response.id) {
          this.message = 'Registration successful! Please log in.';
          this.messageType = 'success';
          this.isLogin = true;
          this.username = registrationRequest.username;
          this.password = '';
        } else {
          throw new Error('Registration failed: Unexpected response.');
        }
      }
    } catch (error: any) {
      this.message = error.message || 'An unknown error occurred.';
      this.messageType = 'error';
    } finally {
      this.loading = false;
    }
  }
}