module App {
    export class Tournament {
        constructor(
            public id: string,
            public title: string,
            public startsOn: Date,
            public endsOn: Date,
            public location: string,
            public events: TournamentItem[],
            public functions: TournamentItem[],
            public merchandise: TournamentItem[]) {
        }

        eventsAreOpen(): boolean {
            return false;
        }

        eventsAreClosed(): boolean {
            return !this.eventsAreOpen();
        }

        discountChanged(player: TournamentPlayer) {
            const discountPercentage = (player.fullTimeStudentUnder25 || player.under21) ? 50 : 0;

            this.events.forEach((event: TournamentItem) => {
                event.discountPercentage = discountPercentage;
            });
        }

        totalPayable(): number {
            return this.allTournamentItems()
                .filter(item => item.quantity > 0)
                .map(item => item.totalPrice())
                .reduce((previousValue, currentValue) => (previousValue + currentValue), 0);
        }

        private allTournamentItems(): TournamentItem[] {
            return this.events.concat(this.functions).concat(this.merchandise);
        }

    }
}