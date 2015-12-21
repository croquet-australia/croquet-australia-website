module App {
    declare var webConfig: any;

    export class TournamentsController {
        tournament: Tournament;
        player: TournamentPlayer;
        eventId: string;
        dietaryRequirements: number;
        submitted: boolean;
        state: string;
        absUrl: string;

        constructor(private $http, private uuid2, private $scope, private $location) {
            this.tournament = $location.absUrl().indexOf('/womens-open') > -1 ? this.getWomensOpen() : this.getMensOpen();
            this.player = new TournamentPlayer();
            this.state = 'show-form';

            this.absUrl = $location.absUrl().substr($location.absUrl().length - 15);

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

        private getMensOpen(): Tournament {
            const tournamentId = '813f8b2c-7d53-4af1-989b-164685584c83';
            const tournamentTitle = 'Australian AC Men\'s Singles';
            const events = [
                new TournamentItem('event', 'da76a935-1a5a-492d-9876-7dbc77149f48', 'Main and Consolation events', 85),
                new TournamentItem('event', 'ab655591-4947-42ae-839a-ed1ecd633f23', 'Main Event Only', 85),
                new TournamentItem('event', 'c379a10a-19c3-41a7-b73f-cff17541dd73', 'Plate Only', 42.50)
            ];

            return this.getOpenTournament(tournamentId, tournamentTitle, events);
        }

        private getWomensOpen(): Tournament {
            const tournamentId = '9cc639a0-764f-4247-ae14-338fac804ba3';
            const tournamentTitle = 'Australian AC Women\'s Singles';
            const events = [
                new TournamentItem('event', 'a9d8475a-cd63-460d-aed9-b5eb82cd06c6', 'Main and Consolation events', 85),
                new TournamentItem('event', 'eb5be945-b65a-4f35-8b31-ea298dce72ea', 'Main Event Only', 85),
                new TournamentItem('event', '697c794c-7fb7-4e7f-9397-2f7796525587', 'Plate Only', 42.50)
            ];

            return this.getOpenTournament(tournamentId, tournamentTitle, events);
        }

        private getOpenTournament(tournamentId: string, tournamentTitle: string, events: TournamentItem[]): Tournament {
            const functions = [
                new TournamentItem('function', 'e759a9cf-c2e1-4961-b6c6-4e2eefcc1a63', 'Eire Cup Teams Reception - 6:30pm Tuesday 15 March by invitation', 0),
                new TournamentItem('function', '40b86428-7a89-48b1-ac29-9f468440bc84', 'Eire Cup Presentation Dinner - 6:30pm Sunday 20 March', 50)
            ];

            const merchandise = [
            ];

            const tournament = new Tournament(
                tournamentId,
                tournamentTitle,
                new Date(2016, 3, 12),
                new Date(2016, 3, 15),
                'Victorian Croquet Centre, Cairnlea, VIC',
                events,
                functions,
                merchandise
            );

            return tournament;
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