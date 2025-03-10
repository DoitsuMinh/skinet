import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { map, Observable } from 'rxjs';
import { Type } from 'src/app/shared/models/type';
import { Brand } from 'src/app/shared/models/brand';
import { FilteredProduct } from 'src/app/shared/models/filteredProduct';
import { Pagination } from 'src/app/shared/models/pagination';
import { Product } from 'src/app/shared/models/product';
import { ShopParams } from 'src/app/shared/models/shopParams';


@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private apiUrl = environment.apiUrl;
  private http = inject(HttpClient);
  types: Type[] = [];
  brands: Brand[] = [];

  getProducts(shopParams: ShopParams): Observable<Pagination<Product>> {
    let params = new HttpParams();

    if (shopParams.brandids.length > 0) {
      params = params.append('brandIds', shopParams.brandids.join(','));
    }

    if (shopParams.typeids.length > 0) {
      params = params.append('typeIds', shopParams.typeids.join(','));
    }

    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort);
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }

    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Pagination<Product>>(`${this.apiUrl}/products`, { params });
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/products/${id}`);
  }

  getTypes(): Observable<Type[]> {
    return this.http.get<Type[]>(`${this.apiUrl}/products/types`).pipe(
      map((response: Type[]) => this.types = response)
    );
  }

  getBrands(): Observable<Brand[]> {
    return this.http.get<Brand[]>(`${this.apiUrl}/products/brands`).pipe(
      map((brands: Brand[]) => this.brands = brands)
    );
  }

  getFilteredProduct(searchValue: string): Observable<FilteredProduct[]> {
    return this.http.get<FilteredProduct[]>(`${this.apiUrl}/products/filtered-products?searchValue=${searchValue}`);
  }

  clearMemory(): void {
    this.brands = [];
    this.types = [];
  }

}
