import { Component, inject, OnDestroy } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { MatButton } from '@angular/material/button';
import { SignalrService } from 'src/app/core/services/signalr.service';
import { CommonModule } from '@angular/common';
import { PaymentPipe } from "../../../shared/pipes/payment.pipe";
import { AddressPipe } from "../../../shared/pipes/address.pipe";
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  standalone: true,
  selector: 'app-checkout-success',
  imports: [
    RouterLink,
    MatProgressSpinnerModule,
    MatButton,
    MatProgressSpinnerModule,
    CommonModule,
    PaymentPipe,
    AddressPipe
  ],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent implements OnDestroy {
  signalrService = inject(SignalrService);
  private orderService = inject(OrderService);

  ngOnDestroy(): void {
    this.orderService.orderComplete = true;
    this.signalrService.orderSignal.set(null);
  }
}
