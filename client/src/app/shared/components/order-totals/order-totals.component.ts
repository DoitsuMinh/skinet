import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketTotals } from '../../models/basket';

@Component({
    selector: 'app-order-totals',
    templateUrl: './order-totals.component.html',
    styleUrls: ['./order-totals.component.scss'],
    standalone: false
})
export class OrderTotalsComponent implements OnInit {
  basketTotal$: Observable<IBasketTotals>

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.basketTotal$ = this.basketService.basketTotal$;
  }

}
