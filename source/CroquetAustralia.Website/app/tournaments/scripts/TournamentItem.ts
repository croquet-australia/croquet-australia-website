'use strict';

module App {
    export class TournamentItem {
        quantity: number;
        discountPercentage: number;

        constructor(public itemType: string, public id: string, public title: string, public unitPrice: number) {
            this.discountPercentage = 0;
        }

        totalPrice(): number {
            if (!this.quantity) {
                return null;
            }
            return this.quantity * this.unitPrice * (100 - this.discountPercentage) / 100;
        }
    }
}