import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  loading = false;
  loadingCount = 0;

  startLoading() {
    this.loadingCount++;
    this.loading = true;
  }

  stopLoading() {
    if (--this.loadingCount <= 0) {
      this.loadingCount = 0;
      this.loading = false;
    }
  }
}
