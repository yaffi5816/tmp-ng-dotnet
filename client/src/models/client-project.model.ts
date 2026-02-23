export interface ClientOrder {
  orderId: number;
  orderDate: string;
  orderSum: number;
  productId: number;
  productName: string;
  imgUrl: string;
  currentStatus: boolean;
}
