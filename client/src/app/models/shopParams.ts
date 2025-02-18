export class ShopParams {
    brandid: number;
    typeid: number;
    sort: string;
    pageNumber = 1;
    pageSize = 10;
    search = '';

    constructor(
        options?: {
            brandid?: number,
            typeid?: number,
            sort?: string,
            pageNumber?: number,
            pageSize?: number,
            seachValue?: string
        }
    ) {
        this.brandid = options?.brandid ?? 0;
        this.typeid = options?.typeid ?? 0;
        this.sort = options?.sort ?? 'name';
        this.pageNumber = options?.pageNumber ?? 1;
        this.pageSize = options?.pageSize ?? 1;
        this.search = options?.seachValue ?? '';
    }
}