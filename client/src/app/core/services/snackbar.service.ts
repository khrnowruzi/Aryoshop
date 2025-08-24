import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  private _snackBar = inject(MatSnackBar);

  success(message: string): void {
    this._snackBar.open(message, 'close', {
      duration: 5000,
      panelClass: ['snack-success']
    });
  }

  error(message: string): void {
    this._snackBar.open(message, 'close', {
      duration: 5000,
      panelClass: ['snack-error']
    });
  }
}
