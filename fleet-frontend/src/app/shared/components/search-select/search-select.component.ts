/* eslint-disable @typescript-eslint/no-explicit-any */
import {
  Component,
  Input,
  OnInit,
  OnDestroy,
  forwardRef,
  ElementRef,
  HostListener,
  signal,
  computed,
} from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { LucideAngularModule, ChevronDown, X } from 'lucide-angular';

@Component({
  selector: 'app-search-select',
  standalone: true,
  imports: [LucideAngularModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SearchSelectComponent),
      multi: true,
    },
  ],
  template: `
    <div class="ss-host" [class.ss-disabled]="isDisabled()">
      <div class="ss-input-row" [class.ss-open]="isOpen()">
        <input
          class="ss-input"
          type="text"
          [value]="searchText()"
          [placeholder]="placeholder"
          [disabled]="isDisabled()"
          (focus)="onInputFocus()"
          (input)="onInputChange($any($event.target).value)"
          autocomplete="off"
          role="combobox"
          [attr.aria-expanded]="isOpen()"
        />
        @if (selectedItem() && !isDisabled()) {
          <button class="ss-clear" type="button" (click)="clearSelection($event)" tabindex="-1" aria-label="Clear">
            <lucide-icon [img]="xIcon" [size]="12" [strokeWidth]="2.5"></lucide-icon>
          </button>
        }
        <lucide-icon
          class="ss-chevron"
          [img]="chevronDownIcon"
          [size]="14"
          [strokeWidth]="2"
          [class.ss-chevron-up]="isOpen()"
        ></lucide-icon>
      </div>

      @if (isOpen()) {
        <div class="ss-dropdown" role="listbox">
          @if (filteredItems().length === 0) {
            <div class="ss-empty" i18n="@@shared.searchSelect.noResults">No results</div>
          }
          @for (item of filteredItems(); track $index) {
            <button
              class="ss-option"
              type="button"
              [class.ss-option-highlighted]="highlightedIndex() === $index"
              [class.ss-option-selected]="selectedItem()?.[valueField] === item[valueField]"
              (click)="selectItem(item)"
              (mouseenter)="highlightedIndex.set($index)"
              role="option"
              [attr.aria-selected]="selectedItem()?.[valueField] === item[valueField]"
            >
              {{ displayFn(item) }}
            </button>
          }
        </div>
      }
    </div>
  `,
  styles: [`
    :host { display: block; position: relative; }

    .ss-host { position: relative; width: 100%; }

    .ss-input-row { display: flex; align-items: center; position: relative; }

    .ss-input {
      width: 100%;
      padding: 9px 32px 9px 12px;
      border: 1.5px solid var(--border);
      border-radius: 7px;
      font-size: 13.5px;
      font-family: inherit;
      color: var(--text-primary);
      background: var(--input-bg);
      outline: none;
      transition: border-color 0.15s;
      cursor: pointer;
      box-sizing: border-box;
    }

    .ss-input:focus,
    .ss-input-row.ss-open .ss-input {
      border-color: var(--brand);
      box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
    }

    .ss-disabled .ss-input {
      background: #f8fafc;
      color: var(--text-muted);
      cursor: not-allowed;
    }

    .ss-clear {
      position: absolute;
      right: 26px;
      background: none;
      border: none;
      cursor: pointer;
      padding: 2px;
      color: var(--text-muted);
      display: flex;
      align-items: center;
      border-radius: 3px;
      transition: color 0.12s;
      line-height: 1;
    }

    .ss-clear:hover { color: var(--text-primary); }

    .ss-chevron {
      position: absolute;
      right: 10px;
      color: var(--text-muted);
      pointer-events: none;
      transition: transform 0.2s ease;
    }

    .ss-chevron-up { transform: rotate(180deg); }

    .ss-dropdown {
      position: absolute;
      top: calc(100% + 4px);
      left: 0;
      width: 100%;
      background: var(--card-bg, #fff);
      border: 1.5px solid var(--border);
      border-radius: 8px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.10), 0 2px 6px rgba(0, 0, 0, 0.06);
      z-index: 200;
      max-height: 240px;
      overflow-y: auto;
      padding: 4px 0;
    }

    .ss-option {
      display: block;
      width: 100%;
      text-align: left;
      padding: 8px 14px;
      background: none;
      border: none;
      cursor: pointer;
      font-size: 13.5px;
      font-family: inherit;
      color: var(--text-secondary);
      transition: background 0.1s;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }

    .ss-option:hover,
    .ss-option-highlighted {
      background: var(--hover-bg);
      color: var(--text-primary);
    }

    .ss-option-selected {
      color: var(--brand);
      font-weight: 600;
    }

    .ss-option-highlighted.ss-option-selected {
      background: #dbeafe;
    }

    .ss-empty {
      padding: 10px 14px;
      font-size: 13px;
      color: var(--text-muted);
    }
  `],
})
export class SearchSelectComponent implements ControlValueAccessor, OnInit, OnDestroy {
  readonly chevronDownIcon = ChevronDown;
  readonly xIcon = X;

  @Input() items: any[] = [];
  @Input() displayFn: (item: any) => string = (item) => String(item);
  @Input() valueField: string = 'id';
  @Input() placeholder = 'Select…';
  @Input() set disabled(val: boolean) { this.isDisabled.set(val); }

  searchText       = signal('');
  isOpen           = signal(false);
  highlightedIndex = signal(-1);
  isDisabled       = signal(false);
  selectedItem     = signal<any>(null);

  filteredItems = computed<any[]>(() => {
    const q = this.searchText().toLowerCase();
    if (!q) return this.items;
    return this.items.filter(i => this.displayFn(i).toLowerCase().includes(q));
  });

  private onChange: (v: any) => void = () => {};
  private onTouched: () => void = () => {};
  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(private elRef: ElementRef) {}

  ngOnInit(): void {
    this.searchSubject
      .pipe(debounceTime(300), takeUntil(this.destroy$))
      .subscribe(term => this.searchText.set(term));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ── ControlValueAccessor ──────────────────────────────────────────────────

  writeValue(value: any): void {
    if (value === null || value === undefined || value === 0) {
      this.selectedItem.set(null);
      this.searchText.set('');
      return;
    }
    const found = this.items.find(i => i[this.valueField] === value) ?? null;
    this.selectedItem.set(found);
    this.searchText.set(found ? this.displayFn(found) : '');
  }

  registerOnChange(fn: (v: any) => void): void { this.onChange = fn; }
  registerOnTouched(fn: () => void): void { this.onTouched = fn; }
  setDisabledState(b: boolean): void { this.isDisabled.set(b); }

  // ── Interaction ───────────────────────────────────────────────────────────

  onInputFocus(): void {
    if (this.isDisabled()) return;
    if (this.selectedItem()) {
      this.searchText.set('');
      this.selectedItem.set(null);
      this.onChange(null);
    }
    this.isOpen.set(true);
    this.highlightedIndex.set(-1);
  }

  onInputChange(value: string): void {
    this.searchSubject.next(value);
    if (!this.isOpen()) this.isOpen.set(true);
    this.highlightedIndex.set(-1);
    if (!value) {
      this.selectedItem.set(null);
      this.onChange(null);
    }
  }

  selectItem(item: any): void {
    this.selectedItem.set(item);
    this.searchText.set(this.displayFn(item));
    this.isOpen.set(false);
    this.highlightedIndex.set(-1);
    this.onChange(item[this.valueField]);
    this.onTouched();
  }

  clearSelection(e: MouseEvent): void {
    e.stopPropagation();
    this.selectedItem.set(null);
    this.searchText.set('');
    this.isOpen.set(false);
    this.onChange(null);
    this.onTouched();
  }

  // ── Keyboard navigation ───────────────────────────────────────────────────

  @HostListener('keydown', ['$event'])
  onKeydown(event: KeyboardEvent): void {
    if (!this.isOpen()) {
      if (event.key === 'ArrowDown' || event.key === 'Enter') {
        this.isOpen.set(true);
        event.preventDefault();
      }
      return;
    }
    const items = this.filteredItems();
    const idx = this.highlightedIndex();

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.highlightedIndex.set(Math.min(idx + 1, items.length - 1));
        break;
      case 'ArrowUp':
        event.preventDefault();
        this.highlightedIndex.set(Math.max(idx - 1, 0));
        break;
      case 'Enter':
        event.preventDefault();
        if (idx >= 0 && idx < items.length) this.selectItem(items[idx]);
        break;
      case 'Escape':
        event.preventDefault();
        this.close();
        break;
      case 'Tab':
        this.close();
        break;
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(e: MouseEvent): void {
    if (!this.elRef.nativeElement.contains(e.target)) this.close();
  }

  close(): void {
    this.isOpen.set(false);
    this.highlightedIndex.set(-1);
    const sel = this.selectedItem();
    this.searchText.set(sel ? this.displayFn(sel) : '');
    this.onTouched();
  }
}
