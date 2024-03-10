import { Component, OnInit } from '@angular/core';
import { AlertService } from '../../Services/AlertService';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-alert',
  templateUrl: './global-alert.component.html',
  styleUrls: ['./global-alert.component.css']
})
export class GlobalAlertComponent implements OnInit {
  htmlContent!: SafeHtml;

  constructor(private alertService: AlertService, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.alertService.htmlAlertEmitter.subscribe(htmlContent => {
      this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(htmlContent);
    });
  }
}
