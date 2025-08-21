import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { ShopParams } from '../../shared/models/shopParams';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class ShopComponent implements OnInit {
  shopService = inject(ShopService);
  readonly dialogService = inject(MatDialog);
  productsData?: Pagination<Product>;
  shopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical A-Z', value: 'name' },
    { name: 'Alphabetical Z-A', value: 'nameDesc' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDesc' },
  ]
  pageSizeOptions = [5, 10, 25];

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    this.shopService.getBrands();
    this.shopService.getModels();
    this.getProducts();
  }

  getProducts(): void {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => this.productsData = response,
      error: error => console.log(error)
    });
  }

  handlePageEvent($event: PageEvent): void {
    this.shopParams.pageNumber = $event.pageIndex + 1;
    this.shopParams.pageSize = $event.pageSize;
    this.getProducts();
  }

  openFiltersDialog(): void {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      data: {
        selectedBrands: this.shopParams.brands,
        selectedModels: this.shopParams.models
      }
    });

    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.shopParams.pageNumber = 1;
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.models = result.selectedModels;
          this.getProducts();
        }
      }
    });
  }

  onSortChange($event: MatSelectionListChange): void {
    const selectedOption = $event.options[0];
    if (selectedOption) {
      this.shopParams.pageNumber = 1;
      this.shopParams.sort = selectedOption.value;
      this.getProducts();
    }
  }

  onSearchChange(): void {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
}
