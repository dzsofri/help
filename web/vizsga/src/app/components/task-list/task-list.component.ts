import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../task.service';
import { Task } from '../../interfaces/task';

@Component({
  selector: 'app-task-list',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.scss'
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];
  newTask = { title: '' };
  editing: any = null;  // Ez tárolja azt a taskot, amit éppen szerkesztünk

  constructor(private taskService: TaskService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.taskService.getAll().subscribe(data => this.tasks = data);
  }

  add(taskTitle: string) {
    if (!taskTitle.trim()) return;
    this.taskService.create(this.newTask).subscribe(() => {
      this.newTask.title = '';
      this.load();
    });
  }

  // A szerkesztés megkezdése: a kiválasztott taskot másolatként eltároljuk az 'editing' változóba,
  // hogy a felhasználó módosíthassa a taskot, anélkül, hogy azonnal a listában lévő adat változna.
  edit(task: any) {
    this.editing = { ...task }; // objektum másolat készítése, hogy ne az eredeti változzon azonnal
  }

  // A módosítások mentése: elküldjük a szerkesztett taskot a szervernek az update metódussal,
  // majd ha sikeres, töröljük az 'editing' változót (bezárjuk a szerkesztést)
  // és újratöltjük a taskokat, hogy friss legyen a lista.
  save() {
    this.taskService.update(this.editing.id, this.editing).subscribe(() => {
      this.editing = null;  // megszüntetjük a szerkesztési állapotot
      this.load();          // frissítjük a taskokat a szerverről
    });
  }

  // A szerkesztés megszakítása: egyszerűen töröljük az 'editing' változót,
  // így a szerkesztő űrlap eltűnik, és a változtatások nem kerülnek mentésre.
  cancel() {
    this.editing = null;  // visszalépünk a normál nézetre, minden változás eldobódik
  }

  delete(id: number) {
    this.taskService.delete(id).subscribe(() => this.load());
  }

deleteAll() {
  if (confirm('Biztosan törölni szeretnéd az összes feladatot?')) {
    this.taskService.deleteAll().subscribe(() => this.load());
  }
}

}
