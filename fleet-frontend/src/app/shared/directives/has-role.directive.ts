import { Directive, Input, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';
import { UserRole } from '../../core/models/models';
/**
 * Usage:
 *   <button *hasRole="'Admin'">Delete</button>
 *   <button *hasRole="['Admin','FleetManager']">Edit</button>
 */
@Directive({
  selector: '[hasRole]',
  standalone: true
})
export class HasRoleDirective implements OnInit {
  @Input('hasRole') roles: UserRole | UserRole[] = [];

  constructor(
    private templateRef: TemplateRef<unknown>,
    private vcr: ViewContainerRef,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    const allowed = Array.isArray(this.roles) ? this.roles : [this.roles];
    if (this.auth.hasRole(...allowed)) {
      this.vcr.createEmbeddedView(this.templateRef);
    }
  }
}