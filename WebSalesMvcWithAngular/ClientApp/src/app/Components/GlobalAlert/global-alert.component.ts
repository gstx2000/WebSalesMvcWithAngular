import { Component, OnInit } from '@angular/core';
import { AlertService } from '../../Services/AlertService';
import { Alert } from '../../Models/Alert/Alert';
@Component({
  selector: 'app-alert',
  templateUrl: './global-alert.component.html',
  styleUrls: ['./global-alert.component.css']
})
export class GlobalAlertComponent implements OnInit {
  alerts: Alert[] = [];

  constructor(private alertService: AlertService) { }

  ngOnInit(): void {
    this.alertService.onAlert().subscribe(alert => {
      if (!alert) {
        this.alerts = [];
        return;
      }
      this.alerts.push(alert);
    });
  }

  closeAlert(alert: Alert) {
    this.alerts = this.alerts.filter(x => x !== alert);
  }
}
