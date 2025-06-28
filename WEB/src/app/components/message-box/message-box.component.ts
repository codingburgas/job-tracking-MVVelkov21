import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-message-box',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="message" class="p-4 rounded-lg shadow-md mb-4 flex justify-between items-center"
         [ngClass]="{'bg-blue-100 text-blue-800': type === 'info',
                     'bg-red-100 text-red-800': type === 'error',
                     'bg-green-100 text-green-800': type === 'success'}">
      <span class="font-medium">{{ message }}</span>
      <button *ngIf="close" (click)="onCloseClick()"
              class="ml-4 p-1 rounded-full hover:bg-opacity-50 focus:outline-none focus:ring-2 focus:ring-offset-2"
              [ngClass]="{'text-blue-800 focus:ring-blue-800': type === 'info',
                           'text-red-800 focus:ring-red-800': type === 'error',
                           'text-green-800 focus:ring-green-800': type === 'success'}">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12"></path>
        </svg>
      </button>
    </div>
  `
})
export class MessageBoxComponent {
  @Input() message: string | null = null;
  @Input() type: 'info' | 'error' | 'success' = 'info';
  @Output() close = new EventEmitter<void>();

  onCloseClick() {
    this.message = null;
    this.close.emit();
  }
}