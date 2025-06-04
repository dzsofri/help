import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TaskService {
  // Backend API alap URL-je
  private api = 'http://localhost:3000';

  // HttpClient injektálása, hogy HTTP kéréseket tudjunk küldeni
  constructor(private http: HttpClient) {}

  // Minden task lekérése a szerverről
  getAll() {
    // GET kérés a /tasks végpontra, visszatér egy Observable tömbbel (task lista)
    return this.http.get<any[]>(this.api + "/tasks");
  }

  // Új task létrehozása a szerveren
  create(task: any) {
    // POST kérés a /tasks végpontra, a task objektumot elküldve
    return this.http.post(this.api + "/tasks", task);
  }

  // Egy meglévő task frissítése azonosító alapján
  update(id: number, task: any) {
    // PUT kérés a /tasks/id végpontra, az új task adatokat küldve
    return this.http.put(`${this.api}/tasks/${id}`, task);
  }

  // Egy task törlése azonosító alapján
  delete(id: number) {
    // DELETE kérés a /tasks/id végpontra
    return this.http.delete(`${this.api}/tasks/${id}`);
  }

  // Minden task törlése egyszerre
  deleteAll() {
    // DELETE kérés a /tasks végpontra
    return this.http.delete(`${this.api}/tasks`);
  }
}
