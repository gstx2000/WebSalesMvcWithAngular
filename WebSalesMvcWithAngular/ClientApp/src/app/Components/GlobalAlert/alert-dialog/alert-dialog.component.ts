import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Alert } from '../../../Models/Alert/Alert';

@Component({
  selector: 'app-alert-dialog',
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.css']

})
export class AlertDialogComponent implements OnInit {
  public remainingTime!: number;
  private closeTimeout: any;
  private countdownInterval: any;

  constructor(
    public dialogRef: MatDialogRef<AlertDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Alert
  ) { }

  ngOnInit(): void {
    this.remainingTime = 5;
    this.startTimer();
    this.startCountdown();
  }

  startTimer(): void {
    this.closeTimeout = setTimeout(() => {
      this.dialogRef.close();
    }, this.remainingTime * 1000);
  }

  startCountdown(): void {
    this.countdownInterval = setInterval(() => {
      if (this.remainingTime > 0) {
        this.remainingTime--;
      } else {
        clearInterval(this.countdownInterval);
        this.dialogRef.close();
      }
    }, 1000);
  }

  close(): void {
    clearTimeout(this.closeTimeout);
    clearInterval(this.countdownInterval);
    this.dialogRef.close();
  }
}
