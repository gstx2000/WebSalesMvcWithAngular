import { Component, ViewChild } from '@angular/core';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  @ViewChild('menu') menu!: MatMenu ;
  @ViewChild(MatMenuTrigger) matMenuTrigger!: MatMenuTrigger;

  openMenu() {
    this.matMenuTrigger.openMenu();
  }

  closeMenu() {
    this.matMenuTrigger.closeMenu();
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
