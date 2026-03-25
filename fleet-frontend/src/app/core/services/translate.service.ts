import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TranslateService {

  // ── Dashboard Stat Cards ──────────────────────────────────
  readonly vehicles          = $localize`:@@dashboard.stat.vehicles:Vehicles`;
  readonly openOrders        = $localize`:@@dashboard.stat.openOrders:Open Orders`;
  readonly kmThisMonth       = $localize`:@@dashboard.stat.kmThisMonth:KM This Month`;
  readonly expiredInsurance  = $localize`:@@dashboard.stat.expiredInsurance:Expired Insurance`;
  readonly inspectionsDue    = $localize`:@@dashboard.stat.inspectionsDue:Inspections Due`;
  readonly fines             = $localize`:@@dashboard.stat.fines:Fines`;
  readonly accidents         = $localize`:@@dashboard.stat.accidents:Accidents`;
  readonly fuelCostThisMonth = $localize`:@@dashboard.stat.fuelCostThisMonth:Fuel Cost This Month`;

  // ── Stat Card Sub-labels ──────────────────────────────────
  readonly subMaintenanceInProgress = $localize`:@@dashboard.stat.subMaintenanceInProgress:Maintenance in progress`;
  readonly subFromOdometerLogs      = $localize`:@@dashboard.stat.subFromOdometerLogs:From odometer logs`;
  readonly subPoliciesExpired       = $localize`:@@dashboard.stat.subPoliciesExpired:Policies expired`;
  readonly subWithin30Days          = $localize`:@@dashboard.stat.subWithin30Days:Within 30 days`;
  readonly subUnpaidFines           = $localize`:@@dashboard.stat.subUnpaidFines:Unpaid fines`;
  readonly subReportedIncidents     = $localize`:@@dashboard.stat.subReportedIncidents:Reported incidents`;
  readonly subEurSpentOnFuel        = $localize`:@@dashboard.stat.subEurSpentOnFuel:EUR spent on fuel`;

  activeCount(n: number): string {
    return $localize`:@@dashboard.stat.subActive:${n}:count: active`;
  }

  // ── Chart Dataset Labels ──────────────────────────────────
  readonly fuelCostEur        = $localize`:@@dashboard.chart.fuelCostEur:Fuel Cost (EUR)`;
  readonly maintenanceCostEur = $localize`:@@dashboard.chart.maintenanceCostEur:Maintenance Cost (EUR)`;
  readonly chartAccidents     = $localize`:@@dashboard.chart.accidents:Accidents`;
  readonly chartFines         = $localize`:@@dashboard.chart.fines:Fines`;

  // ── Doughnut Status Labels ────────────────────────────────
  readonly statusActive    = $localize`:@@dashboard.chart.statusActive:Active`;
  readonly statusInService = $localize`:@@dashboard.chart.statusInService:In Service`;
  readonly statusRetired   = $localize`:@@dashboard.chart.statusRetired:Retired`;
  readonly statusSold      = $localize`:@@dashboard.chart.statusSold:Sold`;

  get vehicleStatusLabels(): string[] {
    return [this.statusActive, this.statusInService, this.statusRetired, this.statusSold];
  }

  // ── Month Abbreviations ───────────────────────────────────
  readonly monthJan = $localize`:@@dashboard.month.jan:Jan`;
  readonly monthFeb = $localize`:@@dashboard.month.feb:Feb`;
  readonly monthMar = $localize`:@@dashboard.month.mar:Mar`;
  readonly monthApr = $localize`:@@dashboard.month.apr:Apr`;
  readonly monthMay = $localize`:@@dashboard.month.may:May`;
  readonly monthJun = $localize`:@@dashboard.month.jun:Jun`;
  readonly monthJul = $localize`:@@dashboard.month.jul:Jul`;
  readonly monthAug = $localize`:@@dashboard.month.aug:Aug`;
  readonly monthSep = $localize`:@@dashboard.month.sep:Sep`;
  readonly monthOct = $localize`:@@dashboard.month.oct:Oct`;
  readonly monthNov = $localize`:@@dashboard.month.nov:Nov`;
  readonly monthDec = $localize`:@@dashboard.month.dec:Dec`;

  get months(): string[] {
    return [
      this.monthJan, this.monthFeb, this.monthMar, this.monthApr,
      this.monthMay, this.monthJun, this.monthJul, this.monthAug,
      this.monthSep, this.monthOct, this.monthNov, this.monthDec,
    ];
  }

  // ── Tooltip ───────────────────────────────────────────────
  readonly currencySuffix = $localize`:@@dashboard.tooltip.currencySuffix:EUR`;

  // ── Common Chips / Filter Labels ─────────────────────────
  readonly chipAll        = $localize`:@@common.chip.all:All`;
  readonly chipActive     = $localize`:@@common.chip.active:Active`;
  readonly chipClosed     = $localize`:@@common.chip.closed:Closed`;
  readonly chipOpen       = $localize`:@@common.chip.open:Open`;
  readonly chipPending    = $localize`:@@common.chip.pending:Pending`;
  readonly chipInactive   = $localize`:@@common.chip.inactive:Inactive`;
  readonly chipCompleted  = $localize`:@@common.chip.completed:Completed`;
  readonly chipInProgress = $localize`:@@common.chip.inProgress:In Progress`;
  readonly chipExpired    = $localize`:@@common.chip.expired:Expired`;
  readonly chipCancelled  = $localize`:@@common.chip.cancelled:Cancelled`;
  readonly chipOverdue    = $localize`:@@common.chip.overdue:Overdue`;
  readonly chipScheduled  = $localize`:@@common.chip.scheduled:Scheduled`;
  readonly chipDraft      = $localize`:@@common.chip.draft:Draft`;

  // ── Profile Page ──────────────────────────────────────────
  readonly profileTitle           = $localize`:@@profile.title:Profile`;
  readonly profileFirstName       = $localize`:@@profile.firstName:First Name`;
  readonly profileLastName        = $localize`:@@profile.lastName:Last Name`;
  readonly profileEmail           = $localize`:@@profile.email:Email`;
  readonly profilePhone           = $localize`:@@profile.phone:Phone`;
  readonly profileSave            = $localize`:@@profile.save:Save`;
  readonly profileCancel          = $localize`:@@profile.cancel:Cancel`;
  readonly profileChangePassword  = $localize`:@@profile.changePassword:Change Password`;
  readonly profileCurrentPassword = $localize`:@@profile.currentPassword:Current Password`;
  readonly profileNewPassword     = $localize`:@@profile.newPassword:New Password`;
  readonly profileConfirmPassword = $localize`:@@profile.confirmPassword:Confirm Password`;

  // ── Settings Page ─────────────────────────────────────────
  readonly settingsTitle      = $localize`:@@settings.title:Settings`;
  readonly settingsGeneral    = $localize`:@@settings.general:General`;
  readonly settingsNotifications = $localize`:@@settings.notifications:Notifications`;
  readonly settingsLanguage   = $localize`:@@settings.language:Language`;
  readonly settingsTheme      = $localize`:@@settings.theme:Theme`;
  readonly settingsDark       = $localize`:@@settings.dark:Dark`;
  readonly settingsLight      = $localize`:@@settings.light:Light`;
  readonly settingsSystem     = $localize`:@@settings.system:System`;
  readonly settingsDateFormat = $localize`:@@settings.dateFormat:Date Format`;
  readonly settingsTimeZone   = $localize`:@@settings.timeZone:Time Zone`;

  // ── User Management Page ──────────────────────────────────
  readonly userMgmtTitle         = $localize`:@@userMgmt.title:User Management`;
  readonly userMgmtUsers         = $localize`:@@userMgmt.users:Users`;
  readonly userMgmtAddUser       = $localize`:@@userMgmt.addUser:Add User`;
  readonly userMgmtEditUser      = $localize`:@@userMgmt.editUser:Edit User`;
  readonly userMgmtDeleteUser    = $localize`:@@userMgmt.deleteUser:Delete User`;
  readonly userMgmtRole          = $localize`:@@userMgmt.role:Role`;
  readonly userMgmtRoleAdmin     = $localize`:@@userMgmt.roleAdmin:Admin`;
  readonly userMgmtRoleManager   = $localize`:@@userMgmt.roleManager:Manager`;
  readonly userMgmtRoleDriver    = $localize`:@@userMgmt.roleDriver:Driver`;
  readonly userMgmtRoleViewer    = $localize`:@@userMgmt.roleViewer:Viewer`;
  readonly userMgmtStatus        = $localize`:@@userMgmt.status:Status`;
  readonly userMgmtLastLogin     = $localize`:@@userMgmt.lastLogin:Last Login`;
  readonly userMgmtCreated       = $localize`:@@userMgmt.created:Created`;
  readonly userMgmtActions       = $localize`:@@userMgmt.actions:Actions`;
  readonly userMgmtSearchPlaceholder = $localize`:@@userMgmt.searchPlaceholder:Search users...`;
  readonly userMgmtNoUsersFound  = $localize`:@@userMgmt.noUsersFound:No users found`;
  readonly userMgmtConfirmDelete = $localize`:@@userMgmt.confirmDelete:Confirm Delete`;
  readonly userMgmtAreYouSure    = $localize`:@@userMgmt.areYouSure:Are you sure?`;
}
