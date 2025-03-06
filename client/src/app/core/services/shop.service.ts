import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Pagination } from 'src/app/models/pagination';
import { Product } from 'src/app/models/product';
import { FilteredProduct } from 'src/app/models/filteredProduct';
import { ShopParams } from 'src/app/models/shopParams';
import { environment } from 'src/environments/environment';
import { EMPTY, map, Observable, tap } from 'rxjs';
import { Type } from 'src/app/models/type';
import { Brand } from 'src/app/models/brand';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private env = environment;
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

    return this.http.get<Pagination<Product>>(`${this.env.apiUrl}/products`, { params });
  }

  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.env.apiUrl}/products/${id}`);
  }

  getTypes(): Observable<Type[]> {
    return this.http.get<Type[]>(`${this.env.apiUrl}/products/types`).pipe(
      map((response: Type[]) => this.types = response)
    );
  }

  getBrands(): Observable<Brand[]> {
    return this.http.get<Brand[]>(`${this.env.apiUrl}/products/brands`).pipe(
      map((brands: Brand[]) => this.brands = brands)
    );
  }

  getFilteredProduct(searchValue: string): Observable<FilteredProduct[]> {
    return this.http.get<FilteredProduct[]>(`${this.env.apiUrl}/products/filtered-products?searchValue=${searchValue}`);
  }

  clearMemory(): void {
    this.brands = [];
    this.types = [];
  }

}
