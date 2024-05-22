using AutoMapper;
using WebSalesMvc.Models;
using WebSalesMvcWithAngular.Controllers.ProductsController.Responses;
using WebSalesMvcWithAngular.DTOs;
using WebSalesMvcWithAngular.Models;
using WebSalesMvcWithAngular.Controllers.SuppliersController.Responses;
namespace WebSalesMvcWithAngular.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.BarCode))
                .ForMember(dest => dest.NCM, opt => opt.MapFrom(src => src.NCM))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.InventoryUnitMeas, opt => opt.MapFrom(src => src.InventoryUnitMeas))
                .ForMember(dest => dest.InventoryCost, opt => opt.MapFrom(src => src.InventoryCost))
                .ForMember(dest => dest.InventoryQuantity, opt => opt.MapFrom(src => src.InventoryQuantity))
                .ForMember(dest => dest.AcquisitionCost, opt => opt.MapFrom(src => src.AcquisitionCost))
                .ForMember(dest => dest.MinimumInventoryQuantity, opt => opt.MapFrom(src => src.MinimumInventoryQuantity))
                .ForMember(dest => dest.TotalInventoryValue, opt => opt.MapFrom(src => src.TotalInventoryValue))
                .ForMember(dest => dest.TotalInventoryCost, opt => opt.MapFrom(src => src.TotalInventoryCost))
                .ForMember(dest => dest.CMV, opt => opt.MapFrom(src => src.CMV))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => src.Margin))
                .ForMember(p => p.Suppliers, s => s.MapFrom(src => src.Suppliers.Select(ps => ps.Supplier)))
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductSupplier, ProductDTO>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name)) 
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
              .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.Product.BarCode))
              .ForMember(dest => dest.NCM, opt => opt.MapFrom(src => src.Product.NCM))
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.Name))
              .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.Product.Subcategory.Name))
              .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Product.Department.Name))
              .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Product.DepartmentId)) 
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Product.CategoryId)) 
              .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.Product.SubCategoryId)) 
              .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl)) 
              .ForMember(dest => dest.InventoryUnitMeas, opt => opt.MapFrom(src => src.Product.InventoryUnitMeas)) 
              .ForMember(dest => dest.InventoryCost, opt => opt.MapFrom(src => src.Product.InventoryCost)) 
              .ForMember(dest => dest.InventoryQuantity, opt => opt.MapFrom(src => src.Product.InventoryQuantity)) 
              .ForMember(dest => dest.AcquisitionCost, opt => opt.MapFrom(src => src.Product.AcquisitionCost)) 
              .ForMember(dest => dest.MinimumInventoryQuantity, opt => opt.MapFrom(src => src.Product.MinimumInventoryQuantity)) 
              .ForMember(dest => dest.TotalInventoryValue, opt => opt.MapFrom(src => src.Product.TotalInventoryValue)) 
              .ForMember(dest => dest.TotalInventoryCost, opt => opt.MapFrom(src => src.Product.TotalInventoryCost)) 
              .ForMember(dest => dest.CMV, opt => opt.MapFrom(src => src.Product.CMV)) 
              .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => src.Product.Margin))
              .ForMember(dest => dest.Suppliers, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<ProductSupplier, SupplierDTO>()
               .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.Supplier.SupplierId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Supplier.Name))
               .ForMember(dest => dest.CNPJ, opt => opt.MapFrom(src => src.Supplier.CNPJ))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Supplier.Email))
               .ForMember(dest => dest.DayToPay, opt => opt.MapFrom(src => src.Supplier.DayToPay))
               .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Supplier.Website))
               .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.Supplier.ContactPerson))
               .ForMember(dest => dest.SupplierType, opt => opt.MapFrom(src => src.Supplier.SupplierType))
               .ForMember(dest => dest.ShippingValue, opt => opt.MapFrom(src => src.Supplier.ShippingValue))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Supplier.Phone))
               .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Supplier.Addresses))
               .ForMember(dest => dest.Products, opt => opt.Ignore())
               .ReverseMap();
            
            CreateMap<Supplier, SupplierDTO>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CNPJ, opt => opt.MapFrom(src => src.CNPJ))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DayToPay, opt => opt.MapFrom(src => src.DayToPay))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.SupplierType, opt => opt.MapFrom(src => src.SupplierType))
                .ForMember(dest => dest.SupplyPrice, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingValue, opt => opt.MapFrom(src => src.ShippingValue))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .AfterMap((src, dest) =>
                {
                    dest.Products = src.Products?.Select(ps =>
                    new ProductDTO
                    {
                        Id = ps.ProductId,
                    }).ToList();
                })
                .ReverseMap();

            //INDEX FOR PRODUCTS:

            CreateMap<Product, IndexProductResponse>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
             .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
             .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
             .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            //INDEX FOR SUPPLIERS:
            CreateMap<Supplier, IndexSupplierResponse>()
               .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.SupplierType, opt => opt.MapFrom(src => src.SupplierType))
               .ForMember(dest => dest.ProductCount, opt => opt.Ignore());

        }
    }
    
}