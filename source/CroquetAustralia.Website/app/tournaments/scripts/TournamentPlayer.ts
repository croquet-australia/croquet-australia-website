'use strict';

module App {
    export class TournamentPlayer {
        firstName: string;
        lastName: string;
        email: string;
        phone: string;
        handicap: number;
        under21: boolean;
        fullTimeStudentUnder25: boolean;
        yearOfBirth: /* nullable */ number;
        nonResident: /* nullable */ boolean;

        constructor() {
            this.yearOfBirth = null;
            this.nonResident = null;
        }
    }
}