import { Component } from '@angular/core';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-schema-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatInputModule, FormsModule],
  template: `
    <h2 mat-dialog-title>הזן Schema</h2>
    <mat-dialog-content>
      <textarea 
        [(ngModel)]="schema" 
        placeholder="הדבק כאן JSON או SQL Schema..."
        rows="15"
        style="width: 100%; padding: 10px; font-family: monospace;">
      </textarea>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">ביטול</button>
      <button mat-raised-button color="primary" (click)="onSubmit()" [disabled]="!schema">צור Dashboard</button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 500px;
      min-height: 300px;
    }
    textarea {
      border: 1px solid #ccc;
      border-radius: 4px;
    }
  `]
})
export class SchemaDialogComponent {
  schema = '';

  constructor(private dialogRef: MatDialogRef<SchemaDialogComponent>) {}

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.dialogRef.close(this.schema);
  }
}
