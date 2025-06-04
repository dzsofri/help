export interface Task {
  // Egyedi azonosító, szám típusú
  id: number;

  // A feladat címe, szöveg típusú
  title: string;
}

/*
  Ez az interface azt határozza meg, hogy milyen adatokat várunk egy Task objektumtól:
  - az 'id' mindig szám kell legyen,
  - a 'title' mindig string (szöveg).

  Az interface segít a TypeScript fordítónak, hogy ellenőrizze a típusokat,
  így fejlesztés közben hibákat tudunk elkerülni, és az IDE is segít a kódírásban.

  Fontos: az interface nem jelenik meg a futó JavaScript kódban,
  csak a fejlesztési időben használjuk típusellenőrzésre.
*/
