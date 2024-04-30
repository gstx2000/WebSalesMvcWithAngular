import { Component, ViewChild } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
 selector: 'app-nav-menu',
 templateUrl: './nav-menu.component.html',
 styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
 @ViewChild(MatMenuTrigger) matMenuTrigger!: MatMenuTrigger;

 isExpanded = false;

 toggle() {
    this.isExpanded = !this.isExpanded;
    if (this.isExpanded) {
      this.matMenuTrigger.openMenu();
    } else {
      this.matMenuTrigger.closeMenu();
    }
 }
}
