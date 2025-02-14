import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product } from 'src/app/models/product';
import { ShopParams } from 'src/app/shared/models/shopParams';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private env = environment;
  private http = inject(HttpClient);

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

    params = params.append('pageSize', shopParams.pageSize);
    params = params.append('pageIndex', shopParams.pageNumber);

    return this.http.get<Product[]>(`${this.env.apiUrl}/products`, { params })
  }
}
