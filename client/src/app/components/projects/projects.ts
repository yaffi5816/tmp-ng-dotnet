import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { OrdersService } from '../../services/projects.service';
import { CartService } from '../../services/cart.service';
import { AdminOrder } from '../../../models/admin-project.model';
import { ClientOrder } from '../../../models/client-project.model';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './projects.html',
  styleUrl: './projects.css',
})
export class OrdersComponent implements OnInit {
    clientOrders: ClientOrder[] = [];
    adminOrders: AdminOrder[] = [];
    isAdmin = false;
    loading = true;
    
    constructor(
      private orderService: OrdersService,
      private authService: AuthService,
      private cartService: CartService,
      private router: Router
    ) {}

    ngOnInit() {
      const userId = this.authService.getUsername();
      this.isAdmin = this.authService.isAdmin();
      this.loadOrders();
    }

    loadOrders() {
      const currentUserId = parseInt(localStorage.getItem('userId') || '0');
      if (this.isAdmin) {
        this.orderService.getAllOrders(currentUserId).subscribe({
          next: (data) => {
            this.adminOrders = data;
            this.loading = false;
          },
          error: (error) => {
            console.error('Error fetching orders:', error);
            this.loading = false;
          }
        });
      } else {
        this.orderService.getClientOrders(currentUserId).subscribe({
          next: (data) => {
            this.clientOrders = data;
            this.loading = false;
          },
          error: (error) => {
            console.error('Error fetching orders:', error);
            this.loading = false;
          }
        });
      }
    }

    createNewOrder() {
      // ניווט ליצירת הזמנה חדשה
    }

    deleteOrder(orderId: number) {
      if (confirm('Are you sure you want to delete this draft order?')) {
        this.orderService.deleteOrder(orderId).subscribe({
          next: () => {
            this.loadOrders();
          },
          error: (error) => console.error('Error deleting order:', error)
        });
      }
    }

    editOrder(order: any) {
      this.orderService.getOrder(order.orderId).subscribe({
        next: (orderData: any) => {
          console.log('Order data:', orderData);
          this.cartService.clearCart();
          
          if (orderData.ordersItems && orderData.ordersItems.length > 0) {
            orderData.ordersItems.forEach((item: any) => {
              console.log('Item:', item);
              if (item.product) {
                this.cartService.addToCart(item.product, item.quantity);
              }
            });
          }
          
          this.router.navigate(['/cart']);
        },
        error: (error) => {
          console.error('Error loading order:', error);
          alert('Failed to load order');
        }
      });
    }
}
