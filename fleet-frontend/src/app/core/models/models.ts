// ─── Auth ────────────────────────────────────────────────────────────────────

export interface LoginDto {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  fullName: string;
  role: string;
  expiresAt: string;
}

// ─── Lookups ─────────────────────────────────────────────────────────────────

export interface DcFuelType {
  fuelTypeId: number;
  code: string;
  label: string;
  isElectric: boolean;
  isActive: boolean;
}

export interface DcVehicleMake {
  makeId: number;
  name: string;
  isActive: boolean;
}

export interface DcVehicleModel {
  modelId: number;
  makeId: number;
  name: string;
  isActive: boolean;
}

export interface DcVehicleCategory {
  categoryId: number;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface DcLicenseCategory {
  licenseCategoryId: number;
  code: string;
  description?: string;
  isActive: boolean;
}

export interface DcMaintenanceType {
  maintenanceTypeId: number;
  name: string;
  description?: string;
  isActive: boolean;
}

// ─── Vehicle ──────────────────────────────────────────────────────────────────

export interface Vehicle {
  vehicleId: number;
  registrationNumber: string;
  vin: string;
  make: string;
  model: string;
  category: string;
  fuelType: string;
  year: number;
  color?: string;
  status: 'active' | 'service' | 'retired' | 'sold';
  currentOdometerKm: number;
  notes?: string;
}

// ─── Employee & Driver ────────────────────────────────────────────────────────

export interface Employee {
  employeeId: number;
  firstName: string;
  lastName: string;
  department?: string;
  email: string;
  phone?: string;
  isActive: boolean;
}

export interface Driver {
  driverId: number;
  employeeId: number;
  fullName: string;
  licenseNumber: string;
  licenseExpiry: string;
  licenseCategories: string[];
  notes?: string;
}

// ─── Insurance Policy ────────────────────────────────────────────────────────

export interface InsurancePolicy {
  policyId: number;
  vehicleId: number;
  registrationNumber: string;
  policyNumber: string;
  insurer: string;
  validFrom: string;
  validTo: string;
  premium: number;
  coverageNotes?: string;
  isActive: boolean;
}

export interface CreateInsurancePolicyDto {
  vehicleId: number;
  policyNumber: string;
  insurer: string;
  validFrom: string;
  validTo: string;
  premium: number;
  coverageNotes?: string;
}

export interface UpdateInsurancePolicyDto {
  insurer?: string;
  validFrom?: string;
  validTo?: string;
  premium?: number;
  coverageNotes?: string;
}

// ─── Registration Record ─────────────────────────────────────────────────────

export interface RegistrationRecord {
  registrationId: number;
  vehicleId: number;
  vehicleRegistrationNumber: string;
  registrationNumber: string;
  validFrom: string;
  validTo: string;
  fee?: number;
  notes?: string;
  isActive: boolean;
}

export interface CreateRegistrationRecordDto {
  vehicleId: number;
  registrationNumber: string;
  validFrom: string;
  validTo: string;
  fee?: number;
  notes?: string;
}

export interface UpdateRegistrationRecordDto {
  registrationNumber?: string;
  validFrom?: string;
  validTo?: string;
  fee?: number;
  notes?: string;
}

// ─── Inspection ───────────────────────────────────────────────────────────────

export interface Inspection {
  inspectionId: number;
  vehicleId: number;
  registrationNumber: string;
  inspectedAt: string;
  validTo?: string;
  result: 'passed' | 'failed' | 'conditional';
  notes?: string;
  odometerKm?: number;
  isValid: boolean;
}

export interface CreateInspectionDto {
  vehicleId: number;
  inspectedAt: string;
  validTo?: string;
  result: string;
  notes?: string;
  odometerKm?: number;
}

export interface UpdateInspectionDto {
  inspectedAt?: string;
  validTo?: string;
  result?: string;
  notes?: string;
  odometerKm?: number;
}

// ─── Fine ─────────────────────────────────────────────────────────────────────

export interface Fine {
  fineId: number;
  vehicleId: number;
  registrationNumber: string;
  driverId?: number;
  driverName?: string;
  occurredAt: string;
  amount: number;
  reason: string;
  paidAt?: string;
  paymentMethod?: string;
  isPaid: boolean;
  notes?: string;
}

export interface CreateFineDto {
  vehicleId: number;
  driverId?: number;
  occurredAt: string;
  amount: number;
  reason: string;
  notes?: string;
}

export interface UpdateFineDto {
  driverId?: number;
  occurredAt?: string;
  amount?: number;
  reason?: string;
  notes?: string;
}

export interface MarkFinePaidDto {
  paidAt: string;
  paymentMethod?: string;
}

// ─── Accident ─────────────────────────────────────────────────────────────────

export interface Accident {
  accidentId: number;
  vehicleId: number;
  registrationNumber: string;
  driverId?: number;
  driverName?: string;
  occurredAt: string;
  severity: 'minor' | 'major' | 'total';
  description: string;
  damageEstimate?: number;
  policeReport?: string;
  notes?: string;
}

export interface CreateAccidentDto {
  vehicleId: number;
  driverId?: number;
  occurredAt: string;
  severity: string;
  description: string;
  damageEstimate?: number;
  policeReport?: string;
  notes?: string;
}

export interface UpdateAccidentDto {
  driverId?: number;
  occurredAt?: string;
  severity?: string;
  description?: string;
  damageEstimate?: number;
  policeReport?: string;
  notes?: string;
}

// ─── Shared ───────────────────────────────────────────────────────────────────

export type UserRole = 'Admin' | 'FleetManager' | 'ReadOnly';

export interface PagedResult<T> {
  items: T[];
  total: number;
}