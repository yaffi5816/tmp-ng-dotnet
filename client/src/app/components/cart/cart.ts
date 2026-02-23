import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { OrderItem } from '../../../models/order-item.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class CartComponent implements OnInit {
  cartItems: OrderItem[] = [];

  constructor(
    public cartService: CartService,
    private router: Router
  ) {}

  getImageUrl(imgUrl: string | null): string {
    if (!imgUrl) return 'https://images.unsplash.com/photo-1551434678-e076c223a692?w=800';
    if (imgUrl.startsWith('http')) return imgUrl;
    return `http://localhost:5034${imgUrl}`;
  }

  ngOnInit() {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
    });
  }

  updateQuantity(productId: number, quantity: number) {
    this.cartService.updateQuantity(productId, quantity);
  }

  removeItem(productId: number) {
    this.cartService.removeFromCart(productId);
  }

  checkout() {
    this.router.navigate(['/dashboard']);
  }

  continueShopping() {
    this.router.navigate(['/products']);
  }
}
