import { Component, inject, signal } from '@angular/core';
import { ShopService } from 'src/app/core/services/shop.service';
import { Product } from 'src/app/models/product';
import { ShopParams } from 'src/app/models/shopParams';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FilteredProduct } from 'src/app/models/filteredProduct';
import { debounceTime, distinctUntilChanged, filter } from 'rxjs';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports:
    [
      ProductItemComponent,
      MatFormField,
      MatInputModule,
      MatIcon,
      ReactiveFormsModule,
      MatAutocompleteModule,
    ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent {
  products: Product[] = [];
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog)

  filteredProducts = signal<FilteredProduct[]>([]);
  searchControl = new FormControl('');

  selectedBrandIds: string[] = [];
  selectedTypeIds: string[] = [];

  readonly DEBOUNCE_TIME = 300;
  readonly INIT_PAGE_NUMBER = 1;

  private sanitizer = inject(DomSanitizer);

  ngOnInit(): void {
    this.initializeShop();

    this.searchControl.valueChanges.pipe(
      filter(searchValue => searchValue.length > 1),
      debounceTime(this.DEBOUNCE_TIME),
      distinctUntilChanged()
    ).subscribe(searchValue => {
      const isExistedInFilteredProducts = this.filteredProducts().some(product =>
        product.name.toLowerCase() === searchValue.toLowerCase()
      );
      if (isExistedInFilteredProducts) {
        const shopParams = new ShopParams(
          { pageNumber: this.INIT_PAGE_NUMBER, searchValue: searchValue }
        );
        this.loadProducts(shopParams); // Load products based on the selected product
      } else if (searchValue.length >= 2) {
        this.handleSearch(searchValue || '')
      }
    });
  }

  // This method initializes the shop by getting brands, types, and products.
  initializeShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.loadProducts();
  }

  openFiltersDialog(): void {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrand: this.selectedBrandIds,
        selectedType: this.selectedTypeIds,
      }
    });
    dialogRef.afterClosed().subscribe(({
      next: (result) => {
        if (result) {
          console.log(result)
          this.selectedBrandIds = result.selectedBrandIds;
          this.selectedTypeIds = result.selectedBrandIds;
          const shopParams = new ShopParams({
            brandids: this.selectedBrandIds.join(',').toString(),
            typeids: this.selectedTypeIds.join(',').toString(),
            pageNumber: this.INIT_PAGE_NUMBER
          });
          this.loadProducts(shopParams);
        }
      },
      error: (error) => console.error(error)
    }))
  }

  // This method returns the filtered products signal value.
  loadProducts(params?: ShopParams): void {
    this.shopService.getProducts(params ?? new ShopParams()).subscribe({
      next: response => this.products = response.data
    })
  }

  // This method is called when the user submits the search form.
  onSubmit(): void {
    const searchValue = this.searchControl.value || '';
    const shopParams = new ShopParams(
      { pageNumber: this.INIT_PAGE_NUMBER, searchValue: searchValue }
    );

    this.loadProducts(shopParams);
  }

  // This method is called when the user clicks on a product in the autocomplete dropdown.
  private handleSearch(searchValue: string): void {
    // If the search value is less than 2 characters, clear the filtered products.
    if (searchValue.length < 2) {
      this.filteredProducts.set([]);
      return;
    }

    // Call the shop service to get filtered products.
    this.shopService.getFilteredProduct(searchValue).subscribe({
      next: (response: FilteredProduct[]) => {
        this.updateFilteredProducts(response, searchValue)

      },
      error: (error) => console.error(error)
    });
  }

  // This method updates the filtered products signal with the response from the service.
  private updateFilteredProducts(response: FilteredProduct[], searchValue: string): void {
    this.filteredProducts.update((products) => {
      // If the response is shorter than or the same length as the current products,
      // replace the current products with the response.
      if (response.length <= products.length) {
        response.map(x => { x.htmlContent = this.handleHtmlContent(x.name, searchValue) });
        return response;
      }

      // Filter the response to find only new products (those not already in products).
      const newProducts = response.filter(
        (newProduct) => !products.some((existingProduct) => existingProduct.id === newProduct.id)
      );
      newProducts.map(x => { x.htmlContent = this.handleHtmlContent(x.name, searchValue) });
      // Append the new products to the existing products and return the updated array.
      return [...products, ...newProducts];
    });
  }

  // This method highlights the search value in the product name.
  private handleHtmlContent(content: string, searchValue: string): SafeHtml {
    if (!searchValue.trim()) return content;

    const regex = new RegExp(`(${searchValue})`, 'gi');
    const highlightedContent = content.replace(regex, '<strong>$1</strong>');
    const htmlContent = `<span>${highlightedContent}</span>`;
    return this.sanitizer.bypassSecurityTrustHtml(htmlContent);
  }
}
