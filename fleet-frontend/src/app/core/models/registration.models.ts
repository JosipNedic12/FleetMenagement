export interface RegistrationRecord {
  registrationId: number;
  vehicleId: number;
  vehicleRegistrationNumber: string;
  vehicleMake: string;
  vehicleModel: string;
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
