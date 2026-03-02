import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from '../auth/api.service';
import {
  LoginDto, AuthResponse,
  InsurancePolicy, CreateInsurancePolicyDto, UpdateInsurancePolicyDto,
  RegistrationRecord, CreateRegistrationRecordDto, UpdateRegistrationRecordDto,
  Inspection, CreateInspectionDto, UpdateInspectionDto,
  Fine, CreateFineDto, UpdateFineDto, MarkFinePaidDto,
  Accident, CreateAccidentDto, UpdateAccidentDto,
  Vehicle, Driver
} from '../models/models';

// ─── Auth ─────────────────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class AuthApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  login(dto: LoginDto): Observable<AuthResponse> {
    return this.post<AuthResponse>('auth/login', dto);
  }
}

// ─── Vehicle (read-only reference) ───────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class VehicleApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<Vehicle[]> {
    return this.get<Vehicle[]>('vehicle');
  }
  getById(id: number): Observable<Vehicle> {
    return this.get<Vehicle>(`vehicle/${id}`);
  }
}

// ─── Driver (read-only reference) ────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class DriverApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<Driver[]> {
    return this.get<Driver[]>('driver');
  }
}

// ─── Insurance Policy ────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class InsurancePolicyApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<InsurancePolicy[]> {
    return this.get<InsurancePolicy[]>('insurancepolicy');
  }
  getActive(): Observable<InsurancePolicy[]> {
    return this.get<InsurancePolicy[]>('insurancepolicy/active');
  }
  getByVehicle(vehicleId: number): Observable<InsurancePolicy[]> {
    return this.get<InsurancePolicy[]>(`insurancepolicy/vehicle/${vehicleId}`);
  }
  getById(id: number): Observable<InsurancePolicy> {
    return this.get<InsurancePolicy>(`insurancepolicy/${id}`);
  }
  create(dto: CreateInsurancePolicyDto): Observable<InsurancePolicy> {
    return this.post<InsurancePolicy>('insurancepolicy', dto);
  }
  update(id: number, dto: UpdateInsurancePolicyDto): Observable<InsurancePolicy> {
    return this.put<InsurancePolicy>(`insurancepolicy/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.delete<void>(`insurancepolicy/${id}`);
  }
}

// ─── Registration Record ─────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class RegistrationApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<RegistrationRecord[]> {
    return this.get<RegistrationRecord[]>('registrationrecord');
  }
  getByVehicle(vehicleId: number): Observable<RegistrationRecord[]> {
    return this.get<RegistrationRecord[]>(`registrationrecord/vehicle/${vehicleId}`);
  }
  getById(id: number): Observable<RegistrationRecord> {
    return this.get<RegistrationRecord>(`registrationrecord/${id}`);
  }
  create(dto: CreateRegistrationRecordDto): Observable<RegistrationRecord> {
    return this.post<RegistrationRecord>('registrationrecord', dto);
  }
  update(id: number, dto: UpdateRegistrationRecordDto): Observable<RegistrationRecord> {
    return this.put<RegistrationRecord>(`registrationrecord/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.delete<void>(`registrationrecord/${id}`);
  }
}

// ─── Inspection ───────────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class InspectionApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<Inspection[]> {
    return this.get<Inspection[]>('inspection');
  }
  getByVehicle(vehicleId: number): Observable<Inspection[]> {
    return this.get<Inspection[]>(`inspection/vehicle/${vehicleId}`);
  }
  getById(id: number): Observable<Inspection> {
    return this.get<Inspection>(`inspection/${id}`);
  }
  create(dto: CreateInspectionDto): Observable<Inspection> {
    return this.post<Inspection>('inspection', dto);
  }
  update(id: number, dto: UpdateInspectionDto): Observable<Inspection> {
    return this.put<Inspection>(`inspection/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.delete<void>(`inspection/${id}`);
  }
}

// ─── Fine ─────────────────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class FineApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<Fine[]> {
    return this.get<Fine[]>('fine');
  }
  getUnpaid(): Observable<Fine[]> {
    return this.get<Fine[]>('fine/unpaid');
  }
  getByVehicle(vehicleId: number): Observable<Fine[]> {
    return this.get<Fine[]>(`fine/vehicle/${vehicleId}`);
  }
  getByDriver(driverId: number): Observable<Fine[]> {
    return this.get<Fine[]>(`fine/driver/${driverId}`);
  }
  getById(id: number): Observable<Fine> {
    return this.get<Fine>(`fine/${id}`);
  }
  create(dto: CreateFineDto): Observable<Fine> {
    return this.post<Fine>('fine', dto);
  }
  update(id: number, dto: UpdateFineDto): Observable<Fine> {
    return this.put<Fine>(`fine/${id}`, dto);
  }
  markPaid(id: number, dto: MarkFinePaidDto): Observable<Fine> {
    return this.post<Fine>(`fine/${id}/pay`, dto);
  }
  delete(id: number): Observable<void> {
    return this.delete<void>(`fine/${id}`);
  }
}

// ─── Accident ─────────────────────────────────────────────────────────────────

@Injectable({ providedIn: 'root' })
export class AccidentApiService extends ApiService {
  constructor(http: HttpClient) { super(http); }

  getAll(): Observable<Accident[]> {
    return this.get<Accident[]>('accident');
  }
  getByVehicle(vehicleId: number): Observable<Accident[]> {
    return this.get<Accident[]>(`accident/vehicle/${vehicleId}`);
  }
  getByDriver(driverId: number): Observable<Accident[]> {
    return this.get<Accident[]>(`accident/driver/${driverId}`);
  }
  getById(id: number): Observable<Accident> {
    return this.get<Accident>(`accident/${id}`);
  }
  create(dto: CreateAccidentDto): Observable<Accident> {
    return this.post<Accident>('accident', dto);
  }
  update(id: number, dto: UpdateAccidentDto): Observable<Accident> {
    return this.put<Accident>(`accident/${id}`, dto);
  }
  delete(id: number): Observable<void> {
    return this.delete<void>(`accident/${id}`);
  }
}