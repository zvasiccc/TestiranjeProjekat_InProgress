import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  BehaviorSubject,
  Observable,
  catchError,
  exhaustMap,
  map,
  of,
  tap,
} from 'rxjs';
import { Turnir } from 'src/app/shared/models/turnir';

import * as TurnirActions from 'src/app/shared/state/turnir/turnir.actions';
import { StoreService } from '../store.service';

@Injectable({
  providedIn: 'root',
})
export class TurnirService {
  constructor(
    private store: Store,
    private http: HttpClient,
    private storeService: StoreService
  ) {}
  private turnirUrl = 'http://localhost:5101/turnir/';

  private refreshSubject = new BehaviorSubject<Turnir[]>([]);

  getTurniriBaza(): Observable<Turnir[]> {
    const url = this.turnirUrl + 'sviTurniri';
    return this.http
      .get<any>(url)
      .pipe(map((response) => response.$values || []));
  }
  getMojiTurniri(idKorisnika: number): Observable<Turnir[]> {
    return this.refreshSubject.pipe(
      exhaustMap(() => {
        const headers = this.storeService.pribaviHeaders();
        console.log('headers su');
        console.log(headers);
        const url = this.turnirUrl + `mojiTurniri/${idKorisnika}`;
        return this.http
          .get<any>(url, { headers })
          .pipe(map((response) => response.$values || []));
      })
    );
  }

  refresh() {
    this.refreshSubject.next([]);
  }

  kreirajTurnir(turnir: Turnir) {
    const headers = this.storeService.pribaviHeaders();
    const url = this.turnirUrl + 'dodajTurnir';
    return this.http.post<Turnir>(url, turnir, { headers });
  }
  async filtrirajTurnire(
    pretragaNaziv?: string,
    pretragaMesto?: string,
    pretragaPocetniDatum?: string,
    pretragaKrajnjiDatum?: string,
    pretragaPocetnaNagrada?: number,
    pretragaKrajnjaNagrada?: number
  ): Promise<Observable<Turnir[]>> {
    let url = this.turnirUrl + 'filtrirajTurnire?';
    if (pretragaNaziv !== undefined && pretragaNaziv !== '')
      url += `&pretragaNaziv=${pretragaNaziv}`;
    if (pretragaMesto !== undefined && pretragaMesto !== '')
      url += `&pretragaMesto=${pretragaMesto}`;
    if (pretragaPocetniDatum !== undefined && pretragaPocetniDatum !== '')
      url += `&pretragaPocetniDatum=${pretragaPocetniDatum}`;
    if (pretragaKrajnjiDatum !== undefined && pretragaKrajnjiDatum !== '')
      url += `&pretragaKrajnjiDatum=${pretragaKrajnjiDatum}`;
    if (pretragaPocetnaNagrada !== undefined && pretragaPocetnaNagrada !== 0)
      url += `&pretragaPocetnaNagrada=${pretragaPocetnaNagrada}`;
    if (pretragaKrajnjaNagrada !== undefined && pretragaKrajnjaNagrada !== 0)
      url += `&pretragaKrajnjaNagrada=${pretragaKrajnjaNagrada}`;

    return this.http.get<any>(url).pipe(
      map((response) => response.$values || []),
      tap((turniri) => {
        this.store.dispatch(TurnirActions.fetchTurniriUspesno({ turniri }));
      }),
      catchError((error) => {
        this.store.dispatch(TurnirActions.fetchTurniriNeuspesno({ error }));
        return of([]);
      })
    );
  }
  async obrisiTurnir(turnirId: number) {
    const headers = this.storeService.pribaviHeaders();
    const url = this.turnirUrl + `obrisiTurnir/${turnirId}`;
    return this.http.delete(url, { headers });
  }
}
