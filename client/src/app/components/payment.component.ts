import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CartService } from '../services/cart.service';
import { ProductService } from '../services/product.service';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule, MatButtonModule, MatInputModule, MatFormFieldModule, MatProgressSpinnerModule],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent {
  cardNumber = '';
  cardName = '';
  expiryDate = '';
  cvv = '';
  processing = false;
  cartItems: any[] = [];
  totalAmount = 0;

  constructor(
    private router: Router,
    private cartService: CartService,
    private productService: ProductService,
    private orderService: OrderService
  ) {
    const savedItems = localStorage.getItem('orderItems');
    
    if (savedItems) {
      this.cartItems = JSON.parse(savedItems);
    } else {
      this.cartItems = this.cartService.getItems();
    }
    
    this.totalAmount = this.cartItems.reduce((sum, item) => sum + (item.product.price * item.quantity), 0);
  }

  getImageUrl(imgUrl: string | null): string {
    return this.productService.getImageUrl(imgUrl);
  }

  formatExpiryDate(event: any): void {
    let value = event.target.value.replace(/\D/g, '');
    if (value.length >= 2) {
      value = value.substring(0, 2) + '/' + value.substring(2, 4);
    }
    this.expiryDate = value;
  }

  isFormValid(): boolean {
    return this.cardNumber.trim().length >= 13 && 
           this.cardName.trim().length > 0 && 
           this.expiryDate.length === 5 && 
           this.cvv.length === 3;
  }

  processPayment(): void {
    this.processing = true;
    
    const orderId = localStorage.getItem('currentOrderId');
    const generatedCode = localStorage.getItem('generatedCode');
    const originalSchema = localStorage.getItem('originalSchema');
    const userId = localStorage.getItem('userId');
    
    if (orderId && userId) {
      const orderUpdate = {
        orderId: parseInt(orderId),
        userId: parseInt(userId),
        currentStatus: true,
        orderDate: new Date().toISOString().split('T')[0],
        ordersSum: this.totalAmount,
        originalSchema: originalSchema || '',
        generatedCode: generatedCode || '',
        ordersItems: this.cartItems.map(item => ({
          productId: item.productId,
          quantity: item.quantity,
          price: item.product.price
        }))
      };
      
      this.orderService.updateOrder(parseInt(orderId), orderUpdate).subscribe({
        next: () => {
          this.processing = false;
          localStorage.removeItem('orderItems');
          localStorage.removeItem('orderTotal');
          localStorage.removeItem('currentOrderId');
          localStorage.removeItem('originalSchema');
          localStorage.removeItem('generatedCode');
          this.cartService.clearCart();
          this.router.navigate(['/code-viewer']);
        },
        error: (err) => {
          console.error('Error updating order:', err);
          this.processing = false;
          alert('Payment processed but order update failed');
          this.router.navigate(['/code-viewer']);
        }
      });
    } else {
      this.processing = false;
      this.router.navigate(['/code-viewer']);
    }
  }
}
