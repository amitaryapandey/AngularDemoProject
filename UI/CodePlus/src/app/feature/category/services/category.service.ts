import { Injectable } from '@angular/core';
import { AddCategoryRequest } from '../../model/add-category-request.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Category } from '../../model/category.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }
  addCategory(model: AddCategoryRequest): Observable<void>{
    return this.http.post<void>(`${environment.apiBaseUrl}/api/categories`, model);
  }

  getAllCategories():Observable<Category[]>{
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/categories`);
  }
  getCategoryById(id:string):Observable<Category>{
    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }
  updateCategory(id:string, model: AddCategoryRequest): Observable<Category>{
    return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}`, model);
  }
  DeleteCategory(id:string):Observable<Category>{
    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}`);
  }
}
