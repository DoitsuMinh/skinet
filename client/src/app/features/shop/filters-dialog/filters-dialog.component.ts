import { Component, inject } from '@angular/core';
import { ShopService } from 'src/app/core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatSelectionList, MatListOption } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  shopService = inject(ShopService);

  // Inject dialog reference - used to control the dialog (close it, etc.)
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);

  // Inject data passed to this dialog when it was opened
  // This contains the initial filter selections
  data = inject(MAT_DIALOG_DATA);

  // Initialize selected brand IDs from the data passed to the dialog
  // This maintains the current filter state when opening the dialog
  selectedBrandIds: number[] = this.data.selectedBrandIds;

  // Initialize selected type IDs from the data passed to the dialog
  // This maintains the current filter state when opening the dialog
  selectedTypeIds: number[] = this.data.selectedTypeIds;

  // Method called when user applies the filter selections
  applyFilters(): void {
    this.dialogRef.close({
      selectedBrandIds: this.selectedBrandIds,
      selectedTypeIds: this.selectedTypeIds
    });
  }
}
