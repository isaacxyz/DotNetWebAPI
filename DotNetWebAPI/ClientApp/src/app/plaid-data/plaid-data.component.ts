import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { take, finalize } from 'rxjs/operators';


@Component({
  selector: 'app-plaid-data',
  templateUrl: './plaid-data.component.html'
})
export class PlaidDataComponent {

  private _http: HttpClient;
  private _baseUrl: string;

  public isLoading: boolean = false;

 
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;

    this.isLoading = true;
    http.post(baseUrl + 'plaid/token', null).pipe(
      finalize(() => this.isLoading = false)
    ).subscribe(_ => {}, error => console.error(error));
  }

  public getBalances() {
    this._http.get<BalanceResponse>(this._baseUrl + 'plaid/balances').subscribe(result => {
      console.log(result);
    }, error => console.error(error));
  }
}

interface BalanceResponse {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
