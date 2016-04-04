'use strict';

module App {
    declare var webConfig: any;
    declare var angular: any;

    export class TournamentsController {

        tournament: Tournament;
        partner: TournamentPlayer;
        player: TournamentPlayer;
        eventId: string;
        dietaryRequirements: string;
        submitted: boolean;
        state: string;
        payingFor: string;

        constructor(private $http, private uuid2, private $scope, private $location, private moment) {

            this.player = new TournamentPlayer();
            this.partner = new TournamentPlayer();
            this.payingFor = '';

            this.getTournament($location.path());

            this.activate();
        }

        activate() {
            // future use
        }

        discountChanged() {
            this.updateDiscount();
        }

        eventChanged(selectedEventId: string) {
            this.updateEventQuantity(selectedEventId);
        }

        newEntry(slug: string): void {
            window.location.href = slug;
        }

        payByEFT() {
            this.payBy(0);
        }

        payByCheque() {
            this.payBy(1);
        }

        payingForChanged(): void {
            this.updateEventQuantity(this.eventId);
            this.updateDiscount();
        }

        payingForPartner(): boolean {
            return this.payingFor === 'myself-and-partner';
        }

        private getTournament(clientResource: string) {

            const apiUrl = `${webConfig.webApi.baseUri}${clientResource}`;

            this.$http.get(apiUrl)
                .then(response => {
                    this.tournament = Tournament.deserialize(response.data, this.moment);
                    this.state = 'show-form';
                });
        }

        private payBy(paymentMethod: number) {

            // submitted property is used by HTML form to know it may now show validation errors for pristine (un-edited) fields
            this.submitted = true;

            this.state = 'processing';

            try {
                const form = this.$scope.tournamentEntryForm;

                if (!form.$valid) {
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
                    dietaryRequirements: this.dietaryRequirements,
                    isDoubles: this.tournament.isDoubles,
                    payingForPartner: this.payingForPartner(),
                    player: {
                        firstName: this.player.firstName,
                        lastName: this.player.lastName,
                        email: this.player.email,
                        phone: this.player.phone,
                        handicap: this.player.handicap,
                        under21: this.player.under21,
                        fullTimeStudentUnder25: this.player.fullTimeStudentUnder25
                    },
                    partner: {
                        firstName: this.partner.firstName,
                        lastName: this.partner.lastName,
                        email: this.partner.email,
                        phone: this.partner.phone,
                        handicap: this.partner.handicap,
                        under21: this.partner.under21,
                        fullTimeStudentUnder25: this.partner.fullTimeStudentUnder25
                    }
                };

                this.$http.post(url, JSON.stringify(data)).then(
                    response => this.onPayByFulfilled(response),
                    response => this.onPayByEftRejected(response));

            } catch (e) {
                alert(`Your entry could not be saved. Please contact support quoting error the following error:\n\n${e}`);
                this.state = 'show-form';
            }
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

        private updateDiscount() {
            this.tournament.updateDiscount(this.player, this.partner, this.payingForPartner());
        }

        private updateEventQuantity(selectedEventId: string) {
            this.tournament.events.forEach((event: TournamentItem) => {
                if (event.id === selectedEventId) {
                    event.quantity = this.payingForPartner() ? 2 : 1;
                } else {
                    event.quantity = 0;
                }
            });
        }
    }

    angular.module('App').controller('TournamentsController', TournamentsController);
}