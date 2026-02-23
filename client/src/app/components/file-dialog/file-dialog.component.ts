import { Component } from '@angular/core';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-file-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>העלה קובץ Schema</h2>
    <mat-dialog-content>
      <div 
        class="drop-zone"
        (dragover)="onDragOver($event)"
        (drop)="onDrop($event)"
        (click)="fileInput.click()">
        <div *ngIf="!selectedFile" class="drop-message">
          <p>גרור קובץ לכאן או לחץ לבחירה</p>
          <p class="hint">JSON, SQL, TXT</p>
        </div>
        <div *ngIf="selectedFile" class="file-info">
          <p>✓ {{ selectedFile.name }}</p>
        </div>
        <input 
          #fileInput 
          type="file" 
          (change)="onFileSelected($event)" 
          accept=".json,.sql,.txt"
          style="display: none;">
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">ביטול</button>
      <button mat-raised-button color="primary" (click)="onSubmit()" [disabled]="!selectedFile">צור Dashboard</button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 500px;
      min-height: 300px;
    }
    .drop-zone {
      border: 2px dashed #ccc;
      border-radius: 8px;
      padding: 40px;
      text-align: center;
      cursor: pointer;
      transition: all 0.3s;
    }
    .drop-zone:hover {
      border-color: #3f51b5;
      background-color: #f5f5f5;
    }
    .drop-message p {
      margin: 10px 0;
      font-size: 16px;
    }
    .hint {
      color: #999;
      font-size: 14px;
    }
    .file-info {
      color: #4caf50;
      font-weight: bold;
    }
  `]
})
export class FileDialogComponent {
  selectedFile: File | null = null;

  constructor(private dialogRef: MatDialogRef<FileDialogComponent>) {}

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.selectedFile = files[0];
    }
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.dialogRef.close(this.selectedFile);
  }
}
