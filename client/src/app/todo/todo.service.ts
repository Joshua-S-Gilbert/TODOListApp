import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../environments/environment.development';
import { TodoItem } from './todo.model';
import { firstValueFrom } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/api/todos`;

  GetAll(){
    return this.http.get<TodoItem[]>(this.baseUrl);
  }

  Add(Title: string){
    return this.http.post<TodoItem>(this.baseUrl, {Title});
  }

  Delete(Id: string){
    return this.http.delete(`${this.baseUrl}/${Id}`);
  }

  // testing purposes
  async addOnce(Title: string){
    return await firstValueFrom(this.Add(Title));
  }
}
