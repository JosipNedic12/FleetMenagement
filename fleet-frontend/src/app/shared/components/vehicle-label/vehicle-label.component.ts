import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-vehicle-label',
  standalone: true,
  imports: [],
  template: `
    @if (layout === 'stacked') {
      <div class="vehicle-label-stacked">
        <span class="primary">{{ make }} {{ model }}</span>
        <span class="secondary">{{ registration }}</span>
      </div>
    } @else {
      <span class="vehicle-label-inline">{{ make }} {{ model }} – {{ registration }}</span>
    }
  `,
  styles: [`
    .vehicle-label-stacked {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }
    .primary {
      font-weight: 600;
      font-size: 13px;
    }
    .secondary {
      font-size: 11px;
      color: var(--text-muted);
      font-family: monospace;
    }
    .vehicle-label-inline {
      font-size: 13px;
    }
  `]
})
export class VehicleLabelComponent {
  @Input() make = '';
  @Input() model = '';
  @Input() registration = '';
  @Input() layout: 'stacked' | 'inline' = 'stacked';
}
