import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from 'src/app/core/services/order.service';
import { Order } from 'src/app/shared/models/order';
import { AddressPipe } from "../../../shared/pipes/address.pipe";
import { PaymentPipe } from "../../../shared/pipes/payment.pipe";
import { MatButton } from '@angular/material/button';

@Component({
  standalone: true,
  selector: 'app-order-detailed',
  imports: [
    MatCard,
    MatButton,
    CommonModule,
    AddressPipe,
    PaymentPipe
  ],
  templateUrl: './order-detailed.component.html',
  styleUrl: './order-detailed.component.scss'
})
export class OrderDetailedComponent implements OnInit {
  private orderService = inject(OrderService);
  private ativatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  order?: Order;
  buttonText: string = 'Return to orders';

  ngOnInit(): void {
    this.loadOrder();
  }

  loadOrder() {
    const id = this.ativatedRoute.snapshot.paramMap.get('id');
    if (!id) return;
    this.orderService.getOrderDetailed(+id).subscribe({
      next: order => this.order = order
    })
  }

  onReturnClick() {
    this.router.navigateByUrl('/orders');
  }
}
