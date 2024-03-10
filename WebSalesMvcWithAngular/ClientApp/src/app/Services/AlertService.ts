import { EventEmitter, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, Observable, filter } from 'rxjs';
import { Alert } from '../Models/Alert/Alert';
import { AlertDialogComponent } from '../Components/GlobalAlert/alert-dialog/alert-dialog.component';

@Injectable({ providedIn: 'root' })
export class AlertService {
  private subject = new BehaviorSubject<Alert | null>(null);
  private defaultId = 'default-alert';
  htmlAlertEmitter = new EventEmitter<string>();

  constructor(private dialog: MatDialog) { }

  onAlert(id = this.defaultId): Observable<Alert | null> {
    return this.subject.asObservable().pipe(
      filter(x => x !== null && x.id === id)
    );
  }

  private showAlert(alert: Alert) {
    const dialogRef = this.dialog.open(AlertDialogComponent, {
      width: '550px',
      height: '350px', 
      data: alert
    });
  }

  showHtmlAlert(message: string, htmlContent: string) {
    this.htmlAlertEmitter.emit(htmlContent);
  }

  success(message: string, options?: any) {
    this.alert(new Alert({ ...options, alertType: 'success', message }));
  }

  error(message: string, options?: any) {
    this.alert(new Alert({ ...options, alertType: 'error', message }));
  }

  info(message: string, options?: any) {
    this.alert(new Alert({ ...options, alertType: 'info', message }));
  }

  warn(message: string, options?: any) {
    this.alert(new Alert({ ...options, alertType: 'warning', message }));
  }

  alert(alert: Alert) {
    alert.id = alert.id || this.defaultId;
    this.subject.next(alert);
    this.showAlert(alert); 
  }

  clear(id = this.defaultId) {
    this.subject.next(new Alert({ id }));
  }
}
