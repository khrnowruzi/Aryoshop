import { nanoid } from 'nanoid';

export type CartType = {
    id: string;
    items: CartItem[];
}

export type CartItem = {
    productId: string;
    productName: string;
    brand: string;
    model: string;
    price: number;
    quantity: number;
    pictureUrl: string;
}

export class Cart implements CartType {
    id: string = nanoid();
    items: CartItem[] = [];
}