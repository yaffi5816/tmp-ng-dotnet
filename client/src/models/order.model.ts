export interface Order {
  orderId?: number;
  userId: number;
  currentStatus: boolean;
  orderDate?: string;
  ordersSum: number;
  originalSchema?: string;
  generatedCode?: string;
}
