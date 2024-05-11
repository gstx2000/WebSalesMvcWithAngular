import { Component, ViewChild } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
import { LoginService } from '../../Services/LoginService/login.service';
import { LoadingService } from '../../Services/LoadingService';

@Component({
 selector: 'app-nav-menu',
 templateUrl: './nav-menu.component.html',
 styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
 @ViewChild(MatMenuTrigger) matMenuTrigger!: MatMenuTrigger;

  isExpanded = false;

  constructor(private loginService: LoginService, private loadingService: LoadingService,
) { }

  logoutUser() {
    this.loadingService.showLoading();
    this.loginService.logout();
    this.loadingService.hideLoading();

  }

 toggle() {
    this.isExpanded = !this.isExpanded;
    if (this.isExpanded) {
      this.matMenuTrigger.openMenu();
    } else {
      this.matMenuTrigger.closeMenu();
    }
 }
}
