import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, filter, map, tap } from 'rxjs';
import {
  selectPrijavljeniKorisnik,
  selectTokenPrijavljenogKorisnika,
} from '../shared/state/korisnik/korisnik.selector';
import { Store } from '@ngrx/store';
import { Igrac } from '../shared/models/igrac';
import { Turnir } from '../shared/models/turnir';
import { Organizator } from '../shared/models/organizator';
import { selectIzabraniTurnir } from '../shared/state/turnir/turnir.selector';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  jwtTokenString: string = '';
  headers: HttpHeaders = new HttpHeaders();
  trenutnoPrijavljeniKorisnik$: Observable<Igrac | Organizator | undefined> =
    new Observable();
  prijavljeniKorisnikId: number = 0;
  constructor(private store: Store) {
    this.trenutnoPrijavljeniKorisnik$ = this.store
      .select(selectPrijavljeniKorisnik)
      .pipe(map((p: any) => p?.prijavljeniKorisnik));
  }
  public pribaviHeaders(): HttpHeaders {
    let jwtTokenObservable = this.store
      .select(selectTokenPrijavljenogKorisnika)
      .pipe(map((p: any) => p.token));

    jwtTokenObservable.subscribe((token: string) => {
      this.jwtTokenString = token;
    });
    this.headers = new HttpHeaders({
      Authorization: `Bearer ${this.jwtTokenString}`,
    });
    return this.headers;
  }
  public pribaviTrenutnoPrijavljenogKorisnika() {
    this.trenutnoPrijavljeniKorisnik$.pipe().subscribe();
    return this.trenutnoPrijavljeniKorisnik$;
  }
  public pribaviIdPrijavljenogKorisnika(): number {
    this.trenutnoPrijavljeniKorisnik$.subscribe((p) => {
      this.prijavljeniKorisnikId = p?.id as number;
    });
    return this.prijavljeniKorisnikId;
  }
  vratiPrijavljeneIgrace(turnirId: number): Observable<Igrac[]> {
    return this.store
      .select(selectPrijavljeniIgraciZaTurnir)
      .pipe(map((p: any) => p.prijavljeniIgraci));
  }

  vratiPrijavljeniTurnir(): Observable<Turnir> {
    return this.store.select(selectIzabraniTurnir).pipe(
      filter((turnir) => !!turnir),

      map((p) => ({ ...p, $id: undefined } as Turnir))
    );
  }
}
function selectPrijavljeniIgraciZaTurnir(state: object): unknown {
  throw new Error('Function not implemented.');
}
