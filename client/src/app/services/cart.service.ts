import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { OrderItem } from '../../models/order-item.model';
import { OrderService } from './order.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private items: OrderItem[] = [];
  private cartSubject = new BehaviorSubject<OrderItem[]>([]);
  cart$ = this.cartSubject.asObservable();
  private draftOrderId: number | null = null;

  constructor(
    private orderService: OrderService,
    private http: HttpClient
  ) {
    this.loadCart();
  }

  private loadCart() {
    const userId = localStorage.getItem('userId');
    const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
    const localCart = localStorage.getItem('cart');

    if (isLoggedIn && userId) {
      // משתמש מחובר
      if (localCart) {
        // יש סל מקומי - שמור אותו בשרת ונקה את localStorage
        this.items = JSON.parse(localCart);
        this.cartSubject.next([...this.items]);
        this.saveCartToServer(parseInt(userId));
        localStorage.removeItem('cart');
      } else {
        // אין סל מקומי - טען מהשרת
        this.loadCartFromServer(parseInt(userId));
      }
    } else {
      // משתמש לא מחובר - טען מ-localStorage
      this.loadCartFromLocalStorage();
    }
    }
  

  private loadCartFromServer(userId: number) {
    this.http.get<any>(`http://localhost:5034/api/Orders/user/${userId}/draft`).subscribe({
      next: (draftOrder) => {
        if (draftOrder && draftOrder.ordersItems) {
          this.draftOrderId = draftOrder.orderId;
          this.items = draftOrder.ordersItems.map((item: any) => ({
            productId: item.productId,
            quantity: item.quantity,
            product: item.product
          }));
          this.cartSubject.next([...this.items]);
        } else {
          this.items = [];
          this.cartSubject.next([]);
        }
      },
      error: () => {
        this.items = [];
        this.cartSubject.next([]);
      }
    });
  }

  private loadCartFromLocalStorage() {
    const saved = localStorage.getItem('cart');
    if (saved) {
      this.items = JSON.parse(saved);
      this.cartSubject.next([...this.items]);
    } else {
      this.items = [];
      this.cartSubject.next([]);
    }
  }

  private saveCart() {
    const userId = localStorage.getItem('userId');
    const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';

    if (isLoggedIn && userId) {
      // משתמש מחובר - שמור בשרת
      this.saveCartToServer(parseInt(userId));
    } else {
      // משתמש לא מחובר - שמור ב-localStorage
      localStorage.setItem('cart', JSON.stringify(this.items));
    }
  }

  private saveCartToServer(userId: number) {
    const orderData = {
      userId: userId,
      currentStatus: false, // טיוטה
      ordersSum: this.getTotal(),
      ordersItems: this.items.map(item => ({
        productId: item.productId,
        quantity: item.quantity
      }))
    };

    if (this.draftOrderId) {
      // עדכן הזמנה קיימת
      this.http.put(`http://localhost:5034/api/Orders/${this.draftOrderId}`, orderData).subscribe();
    } else {
      // צור הזמנה חדשה
      this.http.post<any>('http://localhost:5034/api/Orders', orderData).subscribe({
        next: (order) => {
          this.draftOrderId = order.orderId;
        }
      });
    }
  }

  addToCart(product: any, quantity: number = 1) {
    const existingItem = this.items.find(item => item.productId === product.productId);
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      this.items.push({ 
        productId: product.productId, 
        quantity,
        product 
      });
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
    this.saveCart();
  }

  reloadCart() {
    this.loadCart();
  }
}
