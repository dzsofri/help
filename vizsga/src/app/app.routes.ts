import { Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { MasikOldalComponent } from './components/masik-oldal/masik-oldal.component';

// Az alkalmazás útvonalainak (route) definiálása
export const routes: Routes = [
  // Ha a böngészőben a címsorban a /tasks szerepel, 
  // akkor megjelenik a TaskListComponent komponens
  { path: 'tasks', component: TaskListComponent },

  // Ha az alapértelmezett útvonalat (azaz a gyökér URL-t, pl. http://localhost:4200) kérik,
  // akkor átirányítjuk a felhasználót a /tasks útvonalra
  { path: '', redirectTo: '/tasks', pathMatch: 'full' },

  { path: 'masik', component: MasikOldalComponent },

  
    // Bármilyen más, nem definiált útvonal esetén
  // szintén átirányítjuk a felhasználót a /tasks oldalra
    { path: '**', redirectTo: '/tasks' },
];
