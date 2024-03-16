import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BlogImage } from '../../models/blog-image.model';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) {
    
   }
   getallimages(): Observable<BlogImage[]>
   {
    return this.http.get<BlogImage[]>(`${environment.apiBaseUrl}/api/images`);
   }
   uploadImage(file:File, fileName: string, title: string ): Observable<BlogImage>
    {
      const formData = new FormData();

      formData.append('file',file);
      formData.append('fileName', fileName)
      formData.append('title', title)

      return this.http.post<BlogImage>(`${environment.apiBaseUrl}/api/images`, formData);
     
    }
}
