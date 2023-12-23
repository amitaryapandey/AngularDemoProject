import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../services/category.service';
import { Category } from '../../model/category.model';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categories?: Category[];
category: any;
  constructor(private categoryService: CategoryService){}
  ngOnInit(): void {
    this.categoryService.getAllCategories().subscribe({
      next:(Response)=>
      {
        this.categories = Response;
      }
    })
  }
}
