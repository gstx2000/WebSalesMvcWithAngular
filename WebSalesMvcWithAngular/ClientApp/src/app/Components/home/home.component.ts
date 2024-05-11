import { MatDialog } from "@angular/material/dialog";
import { SalesRecordService } from "../../Services/SalesRecordService";
import { LoadingService } from "../../Services/LoadingService";
import { Component, OnInit } from '@angular/core';
import { Observable } from "rxjs";
import { SalesData } from "../../Models/Reports/SalesData";
import { Router } from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit{
  salesData$!: Observable<SalesData>;
  constructor(
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private salesService: SalesRecordService,
    private router: Router
  ) {  }
  ngOnInit(): void {
    this.salesData$ = this.salesService.getWeekEarnings();
    this.salesData$.subscribe(value => {
    });  }
}
