import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../model/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogpostComponent implements OnInit, OnDestroy{

  id:string| null = null;
  routeSubscription?: Subscription;
  model?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[];
  updateBlogPostSubscription?: Subscription;
  getBlogPostSubscription?: Subscription;
  deleteBlogPostSubscription?: Subscription;
  isImageSelectorVisible?:boolean;

  constructor(private route: ActivatedRoute, private blogpostservice: BlogPostService, private categoryService: CategoryService, private router: Router){}
 

  ngOnInit(): void {
   this.categories$ = this.categoryService.getAllCategories();
    this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if (this.id) {
          this.getBlogPostSubscription = this.blogpostservice.getBlogPostById(this.id).subscribe({
            next: (response) =>{
              this.model = response;
              this.selectedCategories = response.categories.map(x =>x.id);
            }
          })
        }
      }
    })
  }
  onFormSubmit(): void{
    if(this.model && this.id)
    {
      var updateBlogPost: UpdateBlogPost= {
        author : this.model.author,
        content: this.model.content,
        shortDescription: this.model.shortDescription,
        featuredImageUrl: this.model.featuredImageUrl,
        isVisible: this.model.isVisible,
        publishedDate: this.model.publishedDate,
        title: this.model.title,
        urlHandle: this.model.urlHandle,
        categories: this.selectedCategories?? []
      };
      this.updateBlogPostSubscription = this.blogpostservice.updateBlogPost(this.id, updateBlogPost).subscribe({
        next: (response) =>{
          this.router.navigateByUrl('/admin/blogposts');
        }
      })
    }
  }
  onDelete(): void{
    if(this.id)
    {
      this.deleteBlogPostSubscription = this.blogpostservice.deleteBlogPostById(this.id).subscribe({
        next: (response) => {
          this.router.navigateByUrl('/admin/blogposts');
        }
      })
    }
  }
  openImageSelector(): void{
    this.isImageSelectorVisible = true;
  }
  closeImageSelectorVisible(): void{
    this.isImageSelectorVisible = false;
  }
  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.getBlogPostSubscription?.unsubscribe();
    this.deleteBlogPostSubscription?.unsubscribe();
  }

}
