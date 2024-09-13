import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Igrac } from '../shared/models/igrac';
import { Organizator } from '../shared/models/organizator';
import { catchError, Observable, Subscription, throwError } from 'rxjs';
import { StoreService } from './store.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class OrganizatorService {
  organizatorUrl: string = 'http://localhost:5101/Organizator/';
  idKorisnika: number | undefined;
  trenutnoPrijavljeniKorisnik$: Observable<Igrac | Organizator | undefined> =
    this.storeService.pribaviTrenutnoPrijavljenogKorisnika();
  private _snackBar: any;
  constructor(
    private store: Store,
    private http: HttpClient,
    private storeService: StoreService,
    private router: Router
  ) {
    this.trenutnoPrijavljeniKorisnik$.subscribe((korisnik) => {
      this.idKorisnika = korisnik?.id as number;
    });
  }

  registrujSeKaoOrganizator(organizator: Organizator): Subscription {
    const url = this.organizatorUrl + 'registrujOrganizatora';
    return this.http
      .post(url, organizator)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          // Proveri status kod greške
          console.log(error);
          if (error.status == 400) {
            // Proveri da li je status kod 400 Bad Request
            this._snackBar.open('Morate popuniti sva polja.', 'Zatvori', {
              duration: 4000,
            });
          } else if (error.status == 409) {
            // Proveri da li je status kod 409 Conflict
            this._snackBar.open(
              'Zeljeno korisnicko ime je vec u upotrebi.',
              'Zatvori',
              {
                duration: 4000,
              }
            );
          } else {
            this._snackBar.open(
              'Došlo je do greške. Pokušajte ponovo.',
              'Zatvori',
              {
                duration: 4000,
              }
            );
          }
          return throwError(error);
        })
      )
      .subscribe(() => {
        this._snackBar.open('Uspesno ste se registrovali', 'Zatvori', {
          duration: 2000,
        });
        this.router.navigateByUrl('');
      });
  }
  daLiJeOrganizatorTurnira(
    korisnikId: number | undefined | null,
    turnirId: number | undefined | null
  ): Observable<boolean> {
    const url =
      this.organizatorUrl +
      `daLiJeOrganizatorTurnira/${korisnikId}/${turnirId}`;
    const headers: HttpHeaders = this.storeService.pribaviHeaders();
    return this.http.get<boolean>(url, { headers });
  }
  izmeniPodatkeOOrganizatoru(
    organizator: Organizator
  ): Observable<Organizator> {
    const headers = this.storeService.pribaviHeaders();
    const url =
      this.organizatorUrl + `izmeniPodatkeOOrganizatoru/${this.idKorisnika}`;
    return this.http.put<Organizator>(url, organizator, { headers });
  }
}
