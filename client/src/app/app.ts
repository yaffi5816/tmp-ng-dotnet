import { Component } from '@angular/core';
import { Header } from './components/header/header';
import { RouterOutlet } from '@angular/router';
import { ContactButtonComponent } from './components/contact-button/contact-button.component';

@Component({
  selector: 'app-root',
  imports: [Header, RouterOutlet, ContactButtonComponent],
  template: `
    <app-header></app-header>
    <router-outlet></router-outlet>
    <app-contact-button></app-contact-button>
  `,
  styleUrl: './app.css'
})
export class App {}
