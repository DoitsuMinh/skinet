import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'skinet';

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    const basketId = localStorage.getItem('basket_id');

    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(
        //   () => {
        //   console.log('initialized basket');
        // }, error => {
        //   console.log(error);
        // }
        {
          next: () => console.log('itialized basket'),
          error: (err) => console.error(err)
        }
      );
    }
  }
}
