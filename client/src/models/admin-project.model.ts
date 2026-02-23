export interface AdminOrder {
  orderId: number;
  userId: number;
  userName: string;
  orderDate: string;
  orderSum: number;
  productId: number;
  productName: string;
  imgUrl: string;
  currentStatus: boolean;
}
