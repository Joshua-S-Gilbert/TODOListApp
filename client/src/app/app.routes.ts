import { Routes } from '@angular/router';

export const routes: Routes = [
  {path:'', loadComponent: () => import('./todo/todo-list/todo-list').then(m => m.TodoListComponent)}
];
