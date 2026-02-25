import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { ProductService } from '../../services/product.service';
import { OrderItem } from '../../../models/order-item.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class CartComponent implements OnInit {
  cartItems: OrderItem[] = [];
  total: number = 0;

  constructor(
    public cartService: CartService,
    private orderService: OrderService,
    private router: Router,
    private productService: ProductService
  ) {}

  getImageUrl(imgUrl: string | null): string {
    return this.productService.getImageUrl(imgUrl);
  }

  ngOnInit() {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
      this.total = this.cartService.getTotal();
    });
  }

  updateQuantity(productId: number, quantity: number) {
    this.cartService.updateQuantity(productId, quantity);
  }

  removeItem(productId: number) {
    this.cartService.removeFromCart(productId);
  }

  continueShopping() {
    this.router.navigate(['/products']);
  }

  checkout() {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      this.router.navigate(['/login']);
      return;
    }

    const order = {
      userId: parseInt(userId),
      status: 'draft' as const,
      totalAmount: this.total,
      ordersItems: this.cartItems.map(item => ({
        productId: item.productId,
        quantity: item.quantity,
        price: item.product.price
      }))
    };

    this.orderService.createOrder(order).subscribe({
      next: (createdOrder) => {
        localStorage.setItem('currentOrderId', createdOrder.orderId!.toString());
        this.router.navigate(['/dashboard']);
      },
      error: (err) => console.error('Error creating order:', err)
    });
  }
}
