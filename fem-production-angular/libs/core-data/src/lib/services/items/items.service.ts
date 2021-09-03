import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Item } from 'libs/api-interfaces/src/lib/api-interfaces';

@Injectable({
  providedIn: 'root'
})
export class ItemsService {
  model = 'items';

  constructor(private http: HttpClient) {}

  all() {
    return this.http.get<Item[]>(this.getUrl());
  }

  find(id: string) {
    return this.http.get<Item>(this.getUrlWithId(id));
  }

  create(Item: Item) {
    return this.http.post(this.getUrl(), Item);
  }

  update(Item: Item) {
    return this.http.put(this.getUrlWithId(Item.id), Item);
  }

  delete(Item: Item) {
    return this.http.delete(this.getUrlWithId(Item.id));
  }

  private getUrl() {
    return `${environment.apiEndpoint}${this.model}`;
  }

  private getUrlWithId(id) {
    return `${this.getUrl()}/${id}`;
  }
}
