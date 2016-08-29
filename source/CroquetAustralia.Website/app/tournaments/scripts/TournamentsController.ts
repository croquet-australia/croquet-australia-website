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
        datepickerOptions: any;

        constructor(private $http, private uuid2, private $scope, private $location, private moment) {

            this.player = new TournamentPlayer();
            this.partner = new TournamentPlayer();
            this.payingFor = '';
            this.datepickerOptions = {};

            this.getTournament($location.path());

            this.activate();
        }

        activate() {
            // future use
        }

        dateOfBirthIsInvalid(): boolean {
            if (!this.showDateOfBirth()) {
                console.debug('DoB is valid because DoB is not visible');
                return false;
            }

            if (!(this.player.dateOfBirth)) {
                console.log(`DoB is invalid because DoB is ${this.player.dateOfBirth}`);
                return true;
            }

            if (this.player.dateOfBirth.toString() === 'undefined') {
                console.debug('DoB is invalid because DoB is undefined');
                return true;
            }

            if (this.player.dateOfBirth < this.tournament.dateOfBirthRange.minimum) {
                console.log(`DoB is invalid because ${this.player.dateOfBirth} < ${this.tournament.dateOfBirthRange.minimum}`);
                return true;
            }

            if (this.player.dateOfBirth > this.tournament.dateOfBirthRange.maximum) {
                console.log(`DoB is invalid because ${this.player.dateOfBirth} > ${this.tournament.dateOfBirthRange.maximum}`);
                return true;
            }

            console.log(`DoB is valid`);
            return false;
        }

        discountChanged() {
            this.updateDiscount();
        }

        eventChanged(selectedEventId: string) {
            this.updateEventQuantity(selectedEventId);
        }

        eventIsRequired() {
            return (this.tournament !== null) && (this.tournament.functions.length === 0) && !(this.eventId);
        }

        newEntry(slug: string): void {
            window.location.href = slug;
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

        onPayByFulfilled(response) {
            this.state = 'processed';
        }

        payByCash() {
            this.payBy(2);
        }

        payByCheque() {
            this.payBy(1);
        }

        payByEFT() {
            this.payBy(0);
        }

        payingForChanged(): void {
            this.updateEventQuantity(this.eventId);
            this.updateDiscount();
        }

        payingForPartner(): boolean {
            return this.payingFor === 'myself-and-partner';
        }

        requiresPayment() {
            return (this.tournament == null) || (!this.tournament.isEOI);
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

        sendEOI() {
            this.payBy(3);
        }

        showDiscount() {
            if (this.tournament == null) {
                return false;
            }

            return !this.tournament.isUnder21;
        }

        showHandicap() {
            if (this.tournament == null) {
                return false;
            }

            const events = this.tournament.events;
            const selectedItems = this.getSelectedItems(events);
            const result = selectedItems.length > 0;

            return result;
        }

        showNonResident() {
            if (this.tournament == null) {
                return false;
            }

            return this.tournament.isUnder21 && !this.tournament.isEOI;
        }

        showPage() {
            return true;
        }

        showPayByCash() {
            if (this.tournament == null) {
                return false;
            }

            return this.tournament.isUnder21 && this.player.nonResident;
        }

        showPayByCheque() {
            if (this.tournament == null) {
                return true;
            }

            return !this.tournament.isUnder21;
        }

        showPayByEFT() {
            return !this.showPayByCash();
        }

        showDateOfBirth() {
            if (this.tournament == null) {
                return false;
            }

            return this.tournament.isUnder21;
        }

        private getEventId(): string {
            const items = this.getSelectedItems(this.tournament.events);

            if (items.length === 0) {
                return null;
            }

            return items[0].id;
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

        private getSelectedItems(items: TournamentItem[]): TournamentItem[] {
            return items.filter(item => item.quantity > 0);
        }

        private getTournament(clientResource: string) {

            const apiUrl = `${webConfig.webApi.baseUri}${clientResource}`;

            this.$http.get(apiUrl)
                .then(response => {
                    this.tournament = Tournament.deserialize(response.data, this.moment);

                    if (this.tournament.isUnder21) {
                        this.player.nonResident = false;
                    }

                    if (this.showDateOfBirth()) {
                        this.datepickerOptions = {
                            minDate: this.tournament.dateOfBirthRange.minimum,
                            maxDate: this.tournament.dateOfBirthRange.maximum
                        };
                    }

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
                        fullTimeStudentUnder25: this.player.fullTimeStudentUnder25,
                        dateOfBirth: this.player.dateOfBirth,
                        nonResident: this.player.nonResident
                    },
                    partner: {
                        firstName: this.partner.firstName,
                        lastName: this.partner.lastName,
                        email: this.partner.email,
                        phone: this.partner.phone,
                        handicap: this.partner.handicap,
                        under21: this.partner.under21,
                        fullTimeStudentUnder25: this.partner.fullTimeStudentUnder25,
                        dateOfBirth: this.partner.dateOfBirth,
                        nonResident: this.partner.nonResident
                    }
                };

                this.$http.post(url, JSON.stringify(data))
                    .then(
                        response => this.onPayByFulfilled(response),
                        response => this.onPayByEftRejected(response));

            } catch (e) {
                alert(`Your entry could not be saved. Please contact support quoting error the following error:\n\n${e}`);
                this.state = 'show-form';
            }
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