import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  template: `
    <div class="app-shell">
      <app-sidebar #sidebar />
      <main class="main-content" [style.margin-left]="sidebar.collapsed() ? '64px' : '240px'">
        <router-outlet />
      </main>
    </div>
  `,
  styles: [`
    .app-shell {
      min-height: 100vh;
      background: var(--page-bg);
    }
    .main-content {
      transition: margin-left 0.25s ease;
      min-height: 100vh;
      overflow-y: auto;
    }
  `]
})
export class AppShellComponent {}