export interface CartItem {
  product: any;
  quantity: number;
}

export interface Cart {
  items: CartItem[];
  total: number;
}
