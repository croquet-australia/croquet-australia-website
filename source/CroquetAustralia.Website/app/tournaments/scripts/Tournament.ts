'use strict';

module App {
    export class Tournament {
        isGcWorldQualifier2017EOI: boolean;
        isGC: boolean;
        isAC: boolean;
        isHandicap: boolean;

        constructor(
            public id: string,
            public title: string,
            public starts: any /*moment*/,
            public finishes: any /*moment*/,
            public location: string,
            public events: TournamentItem[],
            public eventsClose: any /*moment*/,
            public functions: TournamentItem[],
            public functionsClose: any /*moment*/,
            public merchandise: TournamentItem[],
            public merchandiseClose: any /*moment*/,
            public isDoubles: boolean,
            public isEOI: boolean,
            private moment: any /*moment*/,
            public isUnder21: boolean,
            public dateOfBirthRange: DateOfBirthRange,
            public isGateball: boolean,
            public discipline: string) {

            this.isGcWorldQualifier2017EOI = (id === 'e777de8c-cd14-4e9f-afda-b0fae09ef549');
            this.isGC = (discipline === 'gc');
            this.isAC = (discipline === 'ac');
            this.isHandicap = (id === '8942c912-5844-4053-b122-7f8c9a6953ed' || id === '00a1dd57-ac07-4905-a434-5b418edab8a0' || id === '8fb9cf7f-6105-47ec-9a0f-d10ff4325de5' || id === '2fc52538-87ff-4619-b81f-59c658173c75');
        }

        static deserialize(data: any, moment: any /*moment*/): Tournament {

            const id = data.id;
            const title = data.title;
            const starts = this.deserializeMoment(data.starts, moment);
            const finishes = this.deserializeMoment(data.finishes, moment);
            const location = data.location;
            const events = this.deserializeLineItems(data.events);
            const eventsClose = this.deserializeMoment(data.eventsClose, moment);
            const functions = this.deserializeLineItems(data.functions);
            const functionsClose = this.deserializeMoment(data.functionsClose, moment);
            const merchandise = this.deserializeLineItems(data.merchandise);
            const merchandiseClose = this.deserializeMoment(data.merchandiseClose, moment);
            const isDoubles = data.isDoubles;
            const isEOI = data.isEOI;
            const isUnder21 = data.isUnder21;
            const isGateball = data.isGateball;
            const dateOfBirthRange = this.deserializeDateOfBirthRange(data.dateOfBirthRange);
            const discipline = data.discipline;

            const tournament = new Tournament(
                id,
                title,
                starts,
                finishes,
                location,
                events,
                eventsClose,
                functions,
                functionsClose,
                merchandise,
                merchandiseClose,
                isDoubles,
                isEOI,
                moment,
                isUnder21,
                dateOfBirthRange,
                isGateball,
                discipline);

            return tournament;
        }

        private static deserializeDateOfBirthRange(data: any): DateOfBirthRange {
            // todo: handle timezone
            return (data) ? new DateOfBirthRange(new Date(data.minimum), new Date(data.maximum)) : null;
        }

        private static deserializeLineItems(data: any): TournamentItem[] {
            return data.map(dataItem => new TournamentItem(
                dataItem.itemType,
                dataItem.id,
                dataItem.title,
                dataItem.unitPrice,
                dataItem.isInformationOnly,
                dataItem.currency
            ));
        }

        private static deserializeMoment(data: any, moment: any /*moment*/): any {
            return moment.tz(data.dateTime, data.timeZone);
        }

        closedOn(): string {

            const time = 'h:mma';
            const closed = this.getMaxMoment([this.eventsClose, this.functionsClose, this.merchandiseClose]);

            const canberraTime = closed.tz('Australia/Canberra').format(time);
            const queenslandTime = closed.tz('Australia/Brisbane').format(time);
            const adelaideTime = closed.tz('Australia/Adelaide').format(time);
            const perthTime = closed.tz('Australia/Perth').format(time);
            const canberraDay = closed.tz('Australia/Canberra').format('dddd, Do MMMM YYYY');

            if (canberraTime === queenslandTime) {
                return `${canberraTime} on ${canberraDay} Eastern Standard Time, ${adelaideTime} in South Australia and ${perthTime} in Western Australia.`;
            } else {
                return `${canberraTime} on ${canberraDay} Eastern Summer Time, ${queenslandTime} in Queensland, ${adelaideTime} in South Australia and ${perthTime} in Western Australia.`;
            }
        }

        currencySymbol(): string {
            if (this.showCurrency()) {
                const events = this.events.filter(event => event.quantity > 0);

                if (events.length === 0) {
                    return '$';
                }

                return events[0].currencySymbol(true);
            }

            return '$';
        }

        updateDiscount(player: TournamentPlayer, partner: TournamentPlayer, payingForPartner: boolean) {

            let discountPercentage = 0;

            if (payingForPartner) {
                if (player.fullTimeStudentUnder25 || player.under21) {
                    discountPercentage = (partner.fullTimeStudentUnder25 || partner.under21) ? 50 : 25;
                } else if ((partner.fullTimeStudentUnder25 || partner.under21)) {
                    discountPercentage = 25;
                }
            } else {
                discountPercentage = (player.fullTimeStudentUnder25 || player.under21) ? 50 : 0;
            }

            this.events.forEach((event: TournamentItem) => {
                event.discountPercentage = discountPercentage;
            });
        }

        eventsAreOpen(): boolean {
            return this.areOpen(this.eventsClose);
        }

        functionsAreOpen(): boolean {
            return this.areOpen(this.functionsClose);
        }

        isClosed(): boolean {
            return this.isOpen() === false;
        }

        isOpen(): boolean {
            return this.eventsAreOpen() || this.functionsAreOpen() || this.merchandiseAreOpen();
        }

        showCurrency(): boolean {
            if (this.events.filter(event => event.currency !== 'AUD').length > 0) {
                return true;
            }

            if (this.functions.filter(event => event.currency !== 'AUD').length > 0) {
                return true;
            }

            if (this.merchandise.filter(event => event.currency !== 'AUD').length > 0) {
                return true;
            }

            return false;
        }

        showFunctions(): boolean {
            return this.functionsAreOpen() && this.functions.length > 0;
        }

        showMerchandise(): boolean {
            return this.merchandiseAreOpen() && this.merchandise.length > 0;
        }

        totalPayable(): number {
            return this.allTournamentItems()
                .filter(item => item.quantity > 0)
                .map(item => item.totalPrice())
                .reduce((previousValue, currentValue) => (previousValue + currentValue), 0);
        }

        merchandiseAreOpen(): boolean {
            return this.areOpen(this.merchandiseClose);
        }

        private allTournamentItems(): TournamentItem[] {
            return this.events.concat(this.functions).concat(this.merchandise);
        }

        private areOpen(whenClose: any /*moment*/): boolean {
            const open = this.getMoment() < whenClose;
            return open;
        }

        private getMaxMoment(closes: any[] /*moment[]*/) {
            const sorted = closes.sort((a, b) => b - a);
            return sorted[0];
        }

        private getMoment(): any /*moment*/ {
            // ReSharper disable once InconsistentNaming
            return new this.moment();
        }
    }
}