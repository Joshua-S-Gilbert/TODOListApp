import { Component, inject, signal, effect, computed } from '@angular/core';
import { TodoService } from '../todo.service';
import { TodoItem } from '../todo.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-todo-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-list.html',
  styleUrl: './todo-list.css',
})
export class TodoListComponent {
  private api = inject(TodoService);
  readonly todos = signal<TodoItem[]>([]); 
  readonly title = signal('');

  constructor(){
    this.load();
  }

  load(){
    this.api.GetAll().subscribe(items => this.todos.set(items));
  }

  onAdd(){
    const newTitle = this.title();
    if (!newTitle) {
        return;
    }
    this.api.Add(newTitle).subscribe(item => {
      this.todos.update(list => [item, ...list]);
      this.title.set('');
    });
  }

  onDelete(DeleteId: string){
    this.api.Delete(DeleteId).subscribe(() => {
      this.todos.update(list => list.filter(i => i.id !== DeleteId));
    });
  }
}
