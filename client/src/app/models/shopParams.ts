export class ShopParams {
    brandids: number[];
    typeids: number[];
    sort: string;
    pageNumber = 1;
    pageSize = 10;
    search = '';

    constructor(
        options?: {
            brandids?: number[],
            typeids?: number[],
            sort?: string,
            pageNumber?: number,
            pageSize?: number,
            searchValue?: string
        }
    ) {
        this.brandids = options?.brandids ?? [];
        this.typeids = options?.typeids ?? [];
        this.sort = options?.sort ?? 'name';
        this.pageNumber = options?.pageNumber ?? 1;
        this.pageSize = options?.pageSize ?? 10;
        this.search = options?.searchValue ?? '';
    }
}