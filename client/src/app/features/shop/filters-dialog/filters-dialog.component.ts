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

  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  selectedBrandIds: string[] = this.data.selectedBrandIds;
  selectedTypeIds: string[] = this.data.selectedTypeIds;

  applyFilters(): void {
    this.dialogRef.close({
      selectedBrandIds: this.selectedBrandIds,
      selectedTypeIds: this.selectedTypeIds
    });
  }
}
