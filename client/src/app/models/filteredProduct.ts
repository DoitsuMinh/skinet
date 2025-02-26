import { SafeHtml } from "@angular/platform-browser";

export type FilteredProduct = {
  id: number;
  name: string;
  htmlContent: SafeHtml;
}