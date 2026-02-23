export interface Product {
  productId: number;
  categoryId: number;
  categoryName: string;
  productName: string;
  productDescreption: string | null;
  price: number;
  imgUrl: string | null;
}
