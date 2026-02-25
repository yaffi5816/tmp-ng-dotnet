import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Home } from './components/home/home';
import { Login } from './components/login/login';
import { ProjectsComponent } from './components/projects/projects';
import { UsersComponent } from './components/users/users';
import { ProductsComponent } from './components/products/products';
import { CartComponent } from './components/cart/cart';
import { ProductDetailComponent } from './components/product-detail/product-detail';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: Home },
  { path: 'login', component: Login },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'orders', component: ProjectsComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'product/:id', component: ProductDetailComponent },
  { path: 'cart', component: CartComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'users', component: UsersComponent }
];
