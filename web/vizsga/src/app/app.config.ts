import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';


//Kellendő modulok és szolgáltatások konfigurálása az Angular alkalmazás számára, ez nelkül nem töltődik be az alkalmazás
export const appConfig: ApplicationConfig = {
  providers: [   provideHttpClient(), provideRouter(routes)]
};
