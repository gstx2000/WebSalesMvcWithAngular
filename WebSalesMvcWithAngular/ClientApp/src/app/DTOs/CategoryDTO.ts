export interface CategoryDTO {
  id?: number;
  name: string;
  departmentName: string;
  productCount: number;
  isSubCategory: boolean;
  subCategoryCount: number;
}
