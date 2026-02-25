import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService, Order } from '../../services/order.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './projects.html',
  styleUrls: ['./projects.css']
})
export class ProjectsComponent implements OnInit {
  orders: Order[] = [];
  selectedOrder: Order | null = null;

  constructor(private orderService: OrderService) {}

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.orderService.getUserOrders(parseInt(userId)).subscribe({
        next: (orders) => this.orders = orders,
        error: (err) => console.error('Error loading orders:', err)
      });
    }
  }

  viewOrder(order: Order) {
    this.selectedOrder = order;
  }

  approveCode(orderId: number) {
    this.orderService.approveCode(orderId).subscribe({
      next: () => this.loadOrders(),
      error: (err) => console.error('Error approving code:', err)
    });
  }

  proceedToPayment(orderId: number) {
    this.orderService.completeOrder(orderId).subscribe({
      next: () => {
        alert('התשלום בוצע בהצלחה! הקוד נשלח למייל');
        this.loadOrders();
      },
      error: (err) => console.error('Error completing order:', err)
    });
  }

  suspendOrder(orderId: number) {
    this.orderService.suspendOrder(orderId).subscribe({
      next: () => {
        alert('ההזמנה הושהתה');
        this.loadOrders();
      },
      error: (err) => console.error('Error suspending order:', err)
    });
  }

  downloadCode(orderId: number) {
    this.orderService.downloadCode(orderId).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `dashboard-${orderId}.zip`;
        a.click();
        window.URL.revokeObjectURL(url);
        
        this.orderService.completeOrder(orderId).subscribe(() => this.loadOrders());
      },
      error: (err) => console.error('Error downloading code:', err)
    });
  }
}
