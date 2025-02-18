import { Product } from "./product";

export type Pagination<Product> = {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: Product[];
}