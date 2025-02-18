import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Pagination } from 'src/app/models/pagination';
import { Product } from 'src/app/models/product';
import { FilteredProduct } from 'src/app/models/filteredProduct';
import { ShopParams } from 'src/app/models/shopParams';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private env = environment;
  private http = inject(HttpClient);
  types: string[] = [];
  brands: string[] = [];

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandid > 0) {
      params = params.append('brandid', shopParams.brandid);
    }

    if (shopParams.typeid > 0) {
      params = params.append('typeid', shopParams.typeid);
    }

    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }

    params = params.append('pageSize', 20);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(`${this.env.apiUrl}/products`, { params });
  }

  getTypes(): void {
    if (this.types.length > 0) return;

    this.http.get<string[]>(`${this.env.apiUrl}/products/types`).subscribe({
      next: (response) => this.types = response,
      error: (error) => console.error(error)
    })
  }

  getBrands(): void {
    if (this.brands.length > 0) return;

    this.http.get<string[]>(`${this.env.apiUrl}/products/brands`).subscribe({
      next: (response) => this.brands = response,
      error: (error) => console.error(error)
    })
  }

  getFilteredProduct(searchValue: string): Observable<FilteredProduct[]> {
    return this.http.get<FilteredProduct[]>(`${this.env.apiUrl}/products/filtered-products?searchValue=${searchValue}`);
  }
}
