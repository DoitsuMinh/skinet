import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Order, OrderToCreate } from 'src/app/shared/models/order';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  orderComplete: boolean = false;

  createOrder(orderToCreate: OrderToCreate) {
    return this.http.post<Order>(`${this.baseUrl}/orders`, orderToCreate);
  }

  getOrdersForUser() {
    return this.http.get<Order[]>(`${this.baseUrl}/orders`);
  }

  getOrderDetailed(id: number) {
    return this.http.get<Order>(`${this.baseUrl}/orders/${id}`);
  }
}
