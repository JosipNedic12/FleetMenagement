import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (visible) {
      <div class="modal-overlay" (click)="onCancel()">
        <div class="modal-box" (click)="$event.stopPropagation()">
          <div class="modal-icon">🗑</div>
          <h3 class="modal-title">{{ title }}</h3>
          <p class="modal-message">{{ message }}</p>
          <div class="modal-actions">
            <button class="btn btn-secondary" (click)="onCancel()">Cancel</button>
            <button class="btn btn-danger" (click)="onConfirm()">Delete</button>
          </div>
        </div>
      </div>
    }
  `,
  styles: [`
    .modal-overlay {
      position: fixed; inset: 0;
      background: rgba(0,0,0,0.4);
      display: flex; align-items: center; justify-content: center;
      z-index: 1000;
      animation: fadeIn 0.15s ease;
    }
    @keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
    .modal-box {
      background: white;
      border-radius: 12px;
      padding: 32px;
      max-width: 380px;
      width: 90%;
      text-align: center;
      box-shadow: 0 20px 60px rgba(0,0,0,0.15);
      animation: slideUp 0.15s ease;
    }
    @keyframes slideUp { from { transform: translateY(8px); opacity: 0; } to { transform: translateY(0); opacity: 1; } }
    .modal-icon { font-size: 36px; margin-bottom: 12px; }
    .modal-title { font-size: 18px; font-weight: 700; color: #111; margin: 0 0 8px; }
    .modal-message { font-size: 14px; color: #6b7280; margin: 0 0 24px; }
    .modal-actions { display: flex; gap: 10px; justify-content: center; }
    .btn { padding: 9px 22px; border-radius: 7px; font-size: 14px; font-weight: 600; border: none; cursor: pointer; transition: all 0.15s; }
    .btn-secondary { background: #f1f5f9; color: #374151; }
    .btn-secondary:hover { background: #e2e8f0; }
    .btn-danger { background: #ef4444; color: white; }
    .btn-danger:hover { background: #dc2626; }
  `]
})
export class ConfirmModalComponent {
  @Input() visible = false;
  @Input() title = 'Delete record';
  @Input() message = 'This action cannot be undone.';
  @Output() confirmed = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  onConfirm(): void { this.confirmed.emit(); }
  onCancel(): void  { this.cancelled.emit(); }
}