import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Home } from './components/home/home';
import { Login } from './components/login/login';
import { ProjectsComponent } from './components/projects/projects';
import { UsersComponent } from './components/users/users';
import { ProductsComponent } from './components/products/products';
import { CartComponent } from './components/cart/cart';
import { ProductDetailComponent } from './components/product-detail/product-detail';
import { PaymentComponent } from './components/payment.component';
import { CodeViewerComponent } from './components/code-viewer.component';
import { ContactComponent } from './components/contact/contact.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: Home },
  { path: 'login', component: Login },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'payment', component: PaymentComponent },
  { path: 'code-viewer', component: CodeViewerComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'orders', component: ProjectsComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'product/:id', component: ProductDetailComponent },
  { path: 'cart', component: CartComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'users', component: UsersComponent }
];
