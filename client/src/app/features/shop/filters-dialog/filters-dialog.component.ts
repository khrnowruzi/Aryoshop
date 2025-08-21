import { Component, inject } from '@angular/core';
import { MatDivider } from '@angular/material/divider';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle
} from '@angular/material/dialog';
import { MatButton } from "@angular/material/button";
import { ShopService } from '../../../core/services/shop.service';
import { FormsModule } from '@angular/forms';
import { MatListOption, MatSelectionList } from '@angular/material/list';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDivider,
    MatButton,
    MatSelectionList,
    MatListOption,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  shopService = inject(ShopService);
  readonly dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  readonly data = inject(MAT_DIALOG_DATA);
  selectedBrands: string[] = this.data.selectedBrands;
  selectedModels: string[] = this.data.selectedModels;

  applyFilters(): void {
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedModels: this.selectedModels
    });
  }
}
