import { Injectable } from '@angular/core';
import { ClientOrder } from '../../models/client-project.model';
import { AdminOrder } from '../../models/admin-project.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../../models/order.model';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
    private apiUrl='http://localhost:5034/api/Orders';
    
    constructor(private http: HttpClient) {}

    getClientOrders(userId: number): Observable<ClientOrder[]> {
      return this.http.get<ClientOrder[]>(`${this.apiUrl}?userId=${userId}`);
    }

    getAllOrders(adminId: number): Observable<AdminOrder[]> {
      return this.http.get<AdminOrder[]>(`${this.apiUrl}/admin?currentUserId=${adminId}`);
    }

    getOrder(orderId: number): Observable<Order> {
      return this.http.get<Order>(`${this.apiUrl}/${orderId}`);
    }

    deleteOrder(orderId: number): Observable<void> {
      return this.http.delete<void>(`${this.apiUrl}/${orderId}`);
    }
}
