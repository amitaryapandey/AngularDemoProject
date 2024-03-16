import { Component, OnInit } from '@angular/core';
import { ImageService } from './image.service';
import { Observable } from 'rxjs';
import { BlogImage } from '../../models/blog-image.model';

@Component({
  selector: 'app-image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.css']
})
export class ImageSelectorComponent implements OnInit{
  private file?: File;
  fileName: string = '';
  title: string = '';
  images$?: Observable<BlogImage[]>;
constructor(private imageService: ImageService){

}
  ngOnInit(): void {
   this.getImages();
  }
onFileUploadChange(event: Event) {
  const element =  event.currentTarget as HTMLInputElement;
  this.file = element.files?.[0];
}
private getImages()
{
  this.images$ = this.imageService.getallimages();
}
uploadImage(): void{
  if (this.file && this.fileName !== '' && this.title !== '') {
    this.imageService.uploadImage(this.file, this.fileName, this.title)
    .subscribe({
      next: (response)=>{
        this.getImages();
      }
    })
  }
}

}
