import { Component, inject, input } from '@angular/core';
import { CartItem } from '../../../shared/models/cart';
import { RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { CartService } from '../../../core/services/cart.service';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [
    RouterLink,
    CurrencyPipe,
    MatButton,
    MatIcon
  ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.scss'
})
export class CartItemComponent {
  cartItem = input.required<CartItem>();
  cartService = inject(CartService);

  incrementQuantity() {
    this.cartService.addItemToCart(this.cartItem());
  }

  decrementQuantity() {
    this.cartService.removeItemFromCart(this.cartItem().productId);
  }

  removeItemFromCart() {
    this.cartService.removeItemFromCart(this.cartItem().productId, this.cartItem().quantity);
  }
}
