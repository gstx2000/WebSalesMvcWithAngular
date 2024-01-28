import { Component, OnInit } from '@angular/core';
import { LoadingService } from '../../Services/LoadingService';

@Component({
  selector: 'app-global-loading',
  templateUrl: './global-loading.component.html',
})
export class GlobalLoadingComponent implements OnInit {
  isLoading: boolean = false;
  constructor(private loadingService: LoadingService) { }

  ngOnInit() {
    this.loadingService.isLoading$.subscribe((isLoading) => {
      this.isLoading = isLoading;

      if (isLoading) {
        document.body.classList.add('no-scroll');
      } else {
        document.body.classList.remove('no-scroll');
      }
    });
  }
}
