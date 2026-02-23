import { Component } from '@angular/core';
import { HeaderService } from '../../services/header.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart.service';
import { UserService } from '../../services/user.service';
import { OrderItem } from '../../../models/order-item.model';
import { UserUpdate } from '../../../models/user.model';

@Component({
  selector: 'app-header',
  imports: [CommonModule, FormsModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  isLoggedIn = false;
  username = '';
  firstName = '';
  lastName = '';
  isAdmin = false;
  cartItems: OrderItem[] = [];
  showCartPreview = false;
  showUserModal = false;
  userForm: UserUpdate = {
    userName: '',
    firstName: '',
    lastName: '',
    isAdmin: false
  };

  constructor(
    private headerService: HeaderService,
    private router: Router,
    private authService: AuthService,
    public cartService: CartService,
    private userService: UserService
  ) {
    this.cartService.cart$.subscribe(items => {
      this.cartItems = items;
    });
    this.authService.isLoggedIn$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
      if (loggedIn) {
        this.username = this.authService.getUsername() || '';
        this.firstName = localStorage.getItem('firstName') || '';
        this.lastName = localStorage.getItem('lastName') || '';
        this.isAdmin = this.authService.isAdmin();
      } else {
        this.username = '';
        this.firstName = '';
        this.lastName = '';
        this.isAdmin = false;
      }
    });
  }

  goToHome() {
    this.router.navigate(['/home']);
  }

  login() {
    this.router.navigate(['/login']);
  }

  logout() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      // שמור את הסל הנוכחי ב-localStorage לפני התנתקות
      const currentCart = this.cartService.getItems();
      localStorage.setItem('cart', JSON.stringify(currentCart));
    }
    this.authService.logout();
    setTimeout(() => this.cartService.reloadCart(), 0);
    this.router.navigate(['/home']);
  }

  navigateToOrders(){
    this.router.navigate(['/orders']);
  }

  navigateToProducts(){
    this.router.navigate(['/products']);
  }

  navigateToUsers(){
    this.router.navigate(['/users']);
  }

  goToCart() {
    this.showCartPreview = false;
    this.router.navigate(['/cart']);
  }

  openUserProfile() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.userForm = {
        userName: this.username,
        firstName: this.firstName,
        lastName: this.lastName,
        isAdmin: this.isAdmin
      };
      this.showUserModal = true;
    }
  }

  closeUserModal() {
    this.showUserModal = false;
  }

  saveUserProfile() {
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.userService.updateUser(parseInt(userId), this.userForm).subscribe({
        next: () => {
          localStorage.setItem('username', this.userForm.userName);
          localStorage.setItem('firstName', this.userForm.firstName);
          localStorage.setItem('lastName', this.userForm.lastName);
          this.username = this.userForm.userName;
          this.firstName = this.userForm.firstName;
          this.lastName = this.userForm.lastName;
          this.closeUserModal();
          alert('Profile updated successfully!');
        },
        error: (error) => {
          console.error('Error updating profile:', error);
          alert('Failed to update profile');
        }
      });
    }
  }
}
