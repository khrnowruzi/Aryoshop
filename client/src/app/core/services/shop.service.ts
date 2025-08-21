import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { ShopParams } from '../../shared/models/shopParams';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  brands: string[] = [];
  models: string[] = [];

  getProducts(shopParams: ShopParams): Observable<Pagination<Product>> {
    let params = new HttpParams();

    if (shopParams.brands.length > 0)
      params = params.append('brands', shopParams.brands.join(','));

    if (shopParams.models.length > 0)
      params = params.append('models', shopParams.models.join(','));

    if (shopParams.search)
      params = params.append('search', shopParams.search);

    if (shopParams.sort)
      params = params.append('sort', shopParams.sort);

    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(this.baseUrl + 'products', { params });
  }

  getProduct(id: string): Observable<Product> {
    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  getBrands(): void {
    if (this.brands.length > 0) return;
    this.http.get<string[]>(this.baseUrl + 'products/brands').subscribe({
      next: response => this.brands = response,
      error: error => console.log(error)
    });
  }

  getModels(): void {
    if (this.models.length > 0) return;
    this.http.get<string[]>(this.baseUrl + 'products/models').subscribe({
      next: response => this.models = response,
      error: error => console.log(error)
    });
  }
}
