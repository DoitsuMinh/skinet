import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from "./layout/header/header.component";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'skinet';

  ngOnInit(): void { }
}
