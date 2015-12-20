module App {
    declare var webConfig: any;

    export class TournamentsController {
        tournament: Tournament;
        player: TournamentPlayer;
        eventId: string;
        dietaryRequirements: number;
        submitted: boolean;
        state: string;

        constructor(private $http, private uuid2, private $scope) {
            const events = [
                new TournamentItem('event', 'da76a935-1a5a-492d-9876-7dbc77149f48', 'Main and Consolation events', 85),
                new TournamentItem('event', 'ab655591-4947-42ae-839a-ed1ecd633f23', 'Main Event Only', 85),
                new TournamentItem('event', 'c379a10a-19c3-41a7-b73f-cff17541dd73', 'Plate Only', 42.50)
            ];

            const functions = [
                new TournamentItem('function', 'e759a9cf-c2e1-4961-b6c6-4e2eefcc1a63', 'Welcome BBQ, Friday 11 March 2016', 10),
                new TournamentItem('function', '40b86428-7a89-48b1-ac29-9f468440bc84', 'Team member attending Eire Cup Teams Presentation, Tuesday 15 March 2016', 0),
                new TournamentItem('function', '71f07231-2a77-417a-8a96-d6d65fd21652', 'Guests attending Eire Cup Teams Presentation, Tuesday 15 March', 30),
                new TournamentItem('function', 'cbf3b788-0ee1-4f97-bf95-bc94985209ab', 'Team member attending Eire Cup presentation dinner, Sunday 20 March 2016', 0),
                new TournamentItem('function', '19def587-7bfb-414f-8e99-55a4a43f396d', 'Guests attending Eire Cup presentation dinner, Sunday 20 March 2016', 50)
            ];

            const products = [
                new TournamentItem('product', '5afa7b83-1be6-495b-a6b9-806628404ac7', 'todo: S tshirt', 1.5),
                new TournamentItem('product', 'f299644f-5de3-4b11-8353-a9ceb1f15cbb', 'todo: L tshirt', 3),
                new TournamentItem('product', 'a266fc9a-004c-4aea-97ed-729b3ed61727', 'todo: XL tshirt', 4.5)
            ];

            this.tournament = new Tournament(
                '813f8b2c-7d53-4af1-989b-164685584c83',
                'Australian AC Men\'s Singles',
                new Date(2016, 3, 12),
                new Date(2016, 3, 15),
                'Victorian Croquet Centre, Cairnlea, VIC',
                events,
                functions,
                products
            );

            this.player = new TournamentPlayer();
            this.state = 'show-form';

            this.activate();
        }

        activate() {
            // future use
        }

        discountChanged() {
            this.tournament.discountChanged(this.player);
        }

        eventChanged(selectedEventId: string) {
            this.tournament.events.forEach((event: TournamentItem) => {
                if (event.id === selectedEventId) {
                    event.quantity = 1;
                } else {
                    event.quantity = 0;
                }
            });
        }

        payByEFT() {
            this.payBy(0);
        }

        payByCheque() {
            this.payBy(1);
        }

        private payBy(paymentMethod: number) {
            this.submitted = true;
            this.state = 'processing';

            if (!this.$scope.tournamentEntryForm.$valid) {
                alert('One or more fields must be corrected first.');
                this.state = 'show-form';
                return;
            }

            const url = `${webConfig.webApi.baseUri}/tournament-entry/add-entry`;
            const data = {
                entityId: this.uuid2.newguid(),
                tournamentId: this.tournament.id,
                eventId: this.getEventId(),
                functions: this.getSelectedItems(this.tournament.functions),
                merchandise: this.getSelectedItems(this.tournament.merchandise),
                paymentMethod: paymentMethod,
                player: {
                    firstName: this.player.firstName,
                    lastName: this.player.lastName,
                    email: this.player.email,
                    phone: this.player.phone,
                    handicap: this.player.handicap,
                    under21: this.player.under21,
                    fullTimeStudentUnder25: this.player.fullTimeStudentUnder25
                }
            };

            this.$http.post(url, JSON.stringify(data)).then(
                response => this.onPayByFulfilled(response),
                response => this.onPayByEftRejected(response));
        }

        onPayByFulfilled(response) {
            this.state = 'processed';
        }

        onPayByEftRejected(response) {
            this.state = 'show-form';

            if (response.status === -1) {
                alert(`Your entry could not be saved. Please contact support quoting error 'Likely CORS configuration error.'.`);

            } else if (response.status === 500 && (response.data.ExceptionMessage)) {
                alert(`Your entry could not be saved. Please contact support quoting error '${response.data.ExceptionMessage}'.`);

            } else if (response.status === 400 && (response.data.ModelState)) {
                alert(`Your entry could not be saved because:\n\n${this.getModelStateErrorMessage(response.data.ModelState)}`);

            } else {
                alert(`Your entry could not be saved. Please contact support quoting error 'status = ${response.status}, ${response.statusText}'.`);
            }
        }

        showDietaryRequirements() {
            var show = false;

            this.tournament.functions.forEach((item: TournamentItem) => {
                if (item.quantity > 0) {
                    show = true;
                }
            });

            return show;
        };

        showHandicap() {
            return this.getSelectedItems(this.tournament.events).length > 0;
        }

        showPage() {
            return true;
        }

        private getEventId(): string {
            const items = this.getSelectedItems(this.tournament.events);

            if (items.length === 0) {
                return null;
            }

            return items[0].id;
        }

        private getSelectedItems(items: TournamentItem[]): TournamentItem[] {
            return items.filter(item => item.quantity > 0);
        }

        private getModelStateErrorMessage(modelStateDictionary: any): string {
            let errors = '';

            for (let key in modelStateDictionary) {
                if (modelStateDictionary.hasOwnProperty(key)) {
                    const modelState = modelStateDictionary[key];

                    for (let i = 0; i < modelState.length; i++) {
                        const error = modelState[i];

                        if (errors.indexOf(error) < 0) {
                            errors = (errors === '' ? '' : errors + '\n') + error;
                        }
                    }
                }
            }

            return errors;
        }

    }

    angular.module('App').controller('TournamentsController', TournamentsController);
}