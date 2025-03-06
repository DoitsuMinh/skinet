import { Component, DestroyRef, inject, OnDestroy, signal } from '@angular/core';
import { ShopService } from 'src/app/core/services/shop.service';
import { Product } from 'src/app/models/product';
import { ShopParams } from 'src/app/models/shopParams';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDialog } from '@angular/material/dialog';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FilteredProduct } from 'src/app/models/filteredProduct';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu'
import { MatSelectionList, MatListOption, MatSelectionListChange } from '@angular/material/list'
import { catchError, debounceTime, distinctUntilChanged, EMPTY, filter, from, map, Observable, Subscription, switchMap, takeUntil, throwError } from 'rxjs';
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { MatButton, MatMiniFabButton } from '@angular/material/button';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from 'src/app/models/pagination';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports:
    [
      FormsModule,
      ProductItemComponent,
      MatFormField,
      MatInputModule,
      MatIcon,
      ReactiveFormsModule,
      MatAutocompleteModule,
      MatMenu,
      MatSelectionList,
      MatListOption,
      MatMenuTrigger,
      MatButton,
      MatMiniFabButton,
      MatPaginator
    ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnDestroy {
  products?: Pagination<Product>;
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  private snackbar = inject(SnackbarService);
  private destroyRef = inject(DestroyRef);

  filteredProducts = signal<FilteredProduct[]>([]);
  searchControl = new FormControl('');

  selectedBrandIds: number[] = [];
  selectedTypeIds: number[] = [];
  selectedSort: string = 'name';
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low-High', value: 'priceAsc' },
    { name: 'Price: High-Low', value: 'priceDesc' }
  ]

  pageNumber: number = 1;
  pageSize: number = 10;
  pageSizeOption = [5, 10, 15, 20];


  readonly DEBOUNCE_TIME = 300;

  private sanitizer = inject(DomSanitizer);

  ngOnDestroy(): void {
    this.shopService.clearMemory();
  }

  ngOnInit(): void {
    this.initializeShop();

    this.searchControl.valueChanges.pipe(
      filter(searchValue => searchValue.length > 1),
      debounceTime(this.DEBOUNCE_TIME),
      distinctUntilChanged(),
      switchMap((searchValue: string) => {
        const isExistedInFilteredProducts = this.filteredProducts().some(product =>
          product.name.toLowerCase() === searchValue.toLowerCase()
        );
        if (isExistedInFilteredProducts) {
          const shopParams = new ShopParams(
            { pageNumber: 1, pageSize: this.pageSize, searchValue: searchValue }
          );
          return this.loadProducts(shopParams); // Load products based on the selected product
        } else if (searchValue.length >= 2) {
          return this.handleSearch(searchValue || '')
        }
      })
    ).subscribe();
  }

  // This method initializes the shop by getting brands, types, and products.
  initializeShop() {
    this.shopService.getBrands().pipe(
      switchMap(() => this.shopService.getTypes()),
      switchMap(() => this.loadProducts()),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.selectedSort = selectedOption.value;
      const shopParams = new ShopParams({
        sort: this.selectedSort,
        brandids: this.selectedBrandIds ?? [],
        typeids: this.selectedTypeIds ?? [],
        pageNumber: 1,
        pageSize: this.pageSize
      })
      this.loadProducts(shopParams).subscribe();
    }
  }

  openFiltersDialog(): void {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrand: this.selectedBrandIds,
        selectedType: this.selectedTypeIds,
      }
    });
    dialogRef.afterClosed().pipe(
      filter((result) => !!result && (result.selectedBrandIds || result.selectedTypeIds)),
      map((result) => {
        console.log(result)
        this.selectedBrandIds = result.selectedBrandIds;
        this.selectedTypeIds = result.selectedTypeIds;
        return new ShopParams({
          brandids: this.selectedBrandIds ?? [],
          typeids: this.selectedTypeIds ?? [],
          pageNumber: 1,
          pageSize: this.pageSize
        });
      }),
      switchMap((shopParams: ShopParams) => this.loadProducts(shopParams)),
      catchError((err) => {
        this.snackbar.error(err.message);
        return throwError(() => err);
      })
    ).subscribe()
  }

  clearFilters(): void {
    this.selectedBrandIds = [];
    this.selectedTypeIds = [];
    this.loadProducts().subscribe();
  }

  // This method returns the filtered products signal value.
  loadProducts(params?: ShopParams): Observable<Pagination<Product>> {
    return this.shopService.getProducts(params ?? new ShopParams({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    })).pipe(
      map((response) => this.products = response)
    )
  }

  handlePageEvent(event: PageEvent) {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    const shopParams = new ShopParams({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    })
    this.loadProducts(shopParams).subscribe();
  }

  // This method is called when the user submits the search form.
  onSubmit(): void {
    const searchValue = this.searchControl.value || '';
    const shopParams = new ShopParams(
      { pageNumber: 1, pageSize: this.pageSize, searchValue: searchValue }
    );

    this.loadProducts(shopParams).subscribe();
  }


  // This method is called when the user clicks on a product in the autocomplete dropdown.
  private handleSearch(searchValue: string): Observable<void> {
    // If the search value is less than 2 characters, clear the filtered products.
    if (searchValue.length < 2) {
      this.filteredProducts.set([]);
      return EMPTY;
    }

    // Call the shop service to get filtered products.
    return this.shopService.getFilteredProduct(searchValue).pipe(
      map((response: FilteredProduct[]) => this.updateFilteredProducts(response, searchValue))
    );
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
