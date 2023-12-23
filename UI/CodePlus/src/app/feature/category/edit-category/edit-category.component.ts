import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../../model/category.model';
import { UpdateCategoryRequest } from '../../model/update-category-request.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy {
[x: string]: any;

  id:string| null = null;
  paramsSubscribe?: Subscription;
  editCategorySubscribe?: Subscription;
  category?:Category;
constructor(private route: ActivatedRoute, private categoryService: CategoryService, private router: Router){
}

  ngOnDestroy(): void {
    this.paramsSubscribe?.unsubscribe();
    this.editCategorySubscribe?.unsubscribe();
  }
  onFormSubmit() :void{
    const updateCategoryRequest:UpdateCategoryRequest ={
      name:this.category?.name??'',
      urlHandle:this.category?.urlHandle??''
    };
    if(this.id)
    {
      this.editCategorySubscribe = this.categoryService.updateCategory(this.id, updateCategoryRequest).subscribe({next:(Response)=>
      {
        this.router.navigateByUrl('/admin/categories')
      }})
    }
  }

  onDelete(): void
  {
    if(this.id)
     this.categoryService.DeleteCategory(this.id).subscribe({next:(Response)=>{
     this.router.navigateByUrl('/admin/categories');
    }})
  }
  ngOnInit(): void {
    this.paramsSubscribe = this.route.paramMap.subscribe({next: params =>
    {
      this.id = params.get('id');
      if (this.id) {
        this.categoryService.getCategoryById(this.id).subscribe(
          {
            next:(Response) =>{
              this.category = Response;
            }
          }
        );
      }
    }})
  }

}
