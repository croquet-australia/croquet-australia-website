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
        dateOfBirth: /* nullable */ Date;
        nonResident: /* nullable */ boolean;
        country: string;
        gcDGrade: number;

        constructor() {
            this.dateOfBirth = null;
            this.nonResident = null;
        }
    }
}