import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { OrderItem } from '../../models/order-item.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private items: OrderItem[] = [];
  private cartSubject = new BehaviorSubject<OrderItem[]>([]);
  cart$ = this.cartSubject.asObservable();

  constructor() {
    this.loadCart();
  }

  private loadCart() {
    const userId = localStorage.getItem('userId');
    const cartUserId = localStorage.getItem('cartUserId');
    
    if (userId && cartUserId && userId !== cartUserId) {
      localStorage.removeItem('cart');
      localStorage.setItem('cartUserId', userId);
      this.items = [];
    } else {
      const saved = localStorage.getItem('cart');
      this.items = saved ? JSON.parse(saved) : [];
      if (userId) {
        localStorage.setItem('cartUserId', userId);
      }
    }
    this.cartSubject.next([...this.items]);
  }

  private saveCart() {
    localStorage.setItem('cart', JSON.stringify(this.items));
  }

  addToCart(product: any, quantity: number = 1) {
    const existingItem = this.items.find(item => item.productId === product.productId);
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      this.items.push({ productId: product.productId, quantity, product });
    }
    this.cartSubject.next([...this.items]);
    this.saveCart();
  }

  removeFromCart(productId: number) {
    this.items = this.items.filter(item => item.productId !== productId);
    this.cartSubject.next([...this.items]);
    this.saveCart();
  }

  updateQuantity(productId: number, quantity: number) {
    const item = this.items.find(item => item.productId === productId);
    if (item) {
      item.quantity = quantity;
      if (item.quantity <= 0) {
        this.removeFromCart(productId);
      } else {
        this.cartSubject.next([...this.items]);
        this.saveCart();
      }
    }
  }

  getItems(): OrderItem[] {
    return this.items;
  }

  getTotal(): number {
    return this.items.reduce((sum, item) => sum + (item.product.price * item.quantity), 0);
  }

  getItemCount(): number {
    return this.items.reduce((sum, item) => sum + item.quantity, 0);
  }

  clearCart() {
    this.items = [];
    this.cartSubject.next([]);
    localStorage.removeItem('cart');
  }

  reloadCart() {
    this.loadCart();
  }
}
