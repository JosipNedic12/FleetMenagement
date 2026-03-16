export interface Document {
  documentId: number;
  entityType: string;
  entityId: number;
  category?: string;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedBy: number;
  uploadedAt: string;
  notes?: string;
}

export interface VehicleDocument {
  vehicleDocumentId: number;
  vehicleId: number;
  documentId: number;
  documentTypeId: number;
  documentTypeName: string;
  createdAt: string;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedAt: string;
  notes?: string;
}
