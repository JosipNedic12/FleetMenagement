import { Pipe, PipeTransform } from '@angular/core';

export function formatEu(
  value: number | null | undefined,
  minDecimals = 0,
  maxDecimals = 0
): string {
  if (value == null) return '—';
  return value.toLocaleString('de-DE', {
    minimumFractionDigits: minDecimals,
    maximumFractionDigits: maxDecimals,
  });
}

@Pipe({ name: 'euNumber', standalone: true })
export class EuNumberPipe implements PipeTransform {
  transform(value: number | null | undefined, digitsInfo = '1.0-0'): string {
    const match = digitsInfo.match(/^\d+\.(\d+)-(\d+)$/);
    const minDecimals = match ? parseInt(match[1], 10) : 0;
    const maxDecimals = match ? parseInt(match[2], 10) : 0;
    return formatEu(value, minDecimals, maxDecimals);
  }
}
