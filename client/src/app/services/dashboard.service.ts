import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'http://localhost:5034/api/dashboard';

  constructor(private http: HttpClient) {}

  generateFromSchema(schema: string, products?: any[]): Observable<{ html: string }> {
    const formData = new FormData();
    formData.append('schema', schema);
    if (products && products.length > 0) {
      formData.append('products', JSON.stringify(products));
    }
    return this.http.post<{ html: string }>(`${this.apiUrl}/generate`, formData).pipe(
      catchError(error => {
        console.error('Dashboard generation error:', error);
        const errorMessage = error.error?.error || error.message || 'Unknown error occurred';
        const errorDetails = error.error?.details || '';
        throw new Error(`${errorMessage}${errorDetails ? ' - ' + errorDetails : ''}`);
      })
    );
  }

  generateFromFile(file: File, products?: any[]): Observable<{ html: string }> {
    const formData = new FormData();
    formData.append('schemaFile', file);
    if (products && products.length > 0) {
      formData.append('products', JSON.stringify(products));
    }
    return this.http.post<{ html: string }>(`${this.apiUrl}/generate`, formData);
  }

  refineCode(currentCode: string, refinementRequest: string): Observable<{ html: string }> {
    const formData = new FormData();
    formData.append('currentCode', currentCode);
    formData.append('refinementRequest', refinementRequest);
    return this.http.post<{ html: string }>(`${this.apiUrl}/refine`, formData);
  }
}
