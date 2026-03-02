import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { AppShellComponent } from './app-shell.component';

export const routes: Routes = [
  // Public
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },

  // Protected — wrapped in shell (sidebar + layout)
  {
    path: '',
    component: AppShellComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'insurance',
        loadComponent: () =>
          import('./features/insurance/list/insurance-list.component').then(m => m.InsuranceListComponent)
      },
      {
        path: 'registration',
        loadComponent: () =>
          import('./features/registration/list/registration-list.component').then(m => m.RegistrationListComponent)
      },
      {
        path: 'inspections',
        loadComponent: () =>
          import('./features/inspections/list/inspections-list.component').then(m => m.InspectionsListComponent)
      },
      {
        path: 'fines',
        loadComponent: () =>
          import('./features/fines/list/fines-list.component').then(m => m.FinesListComponent)
      },
      {
        path: 'accidents',
        loadComponent: () =>
          import('./features/accidents/list/accidents-list.component').then(m => m.AccidentsListComponent)
      },
    ]
  },

  // Catch-all
  { path: '**', redirectTo: 'dashboard' }
];