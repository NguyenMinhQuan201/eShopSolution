﻿
using eShopSolution.Application.Common;
using eShopSolution.Data.Entities;
using eShopSolution.Date.EF;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewMoldes.Catalog.ProductImage;
using eShopSolution.ViewMoldes.Catalog.ProductImages;
using eShopSolution.ViewMoldes.Catalog.Products;
using eShopSolution.ViewMoldes.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductIamgeCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
             await _context.SaveChangesAsync();
            return productImage.Id;
        } 

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name=request.Name,
                        Description=request.Description,
                        Details=request.Details,
                        SeoDescription=request.SeoDescription,
                        SeoAlias=request.SeoAlias,
                        SeoTitle=request.SeoTitle,
                        LanguageId=request.LanguageId,
                    }
                }

            };
            if (request.ThumbNailImage !=null)
            {                
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption= request.Name,
                        DateCreated=DateTime.Now,
                        FileSize=request.ThumbNailImage.Length,
                        ImagePath=await this.SaveFile(request.ThumbNailImage),
                        IsDefault=true,
                        SortOrder=1,
                    }
                };
            }
            _context.Products.Add(product);
             await _context.SaveChangesAsync();
            return product.Id;
        }
        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product:{productId}");
            var images =  _context.ProductImages.Where(i => i.ProductId == productId);
            foreach(var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);
           
            return await _context.SaveChangesAsync();
        }
        public async Task<PagedResult<ProductViewModle>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count > 0) 
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }
            //paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModle()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoTitle = x.pt.SeoTitle,
                    SeoDescription = x.pt.SeoDescription,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                }).ToListAsync();
            var pagedResult = new PagedResult<ProductViewModle>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pagedResult;
        }

        public async Task<ProductViewModle> GetById(int productId, string langugeId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(
                x => x.ProductId == productId
                && x.LanguageId==langugeId);
            var productViewModle = new ProductViewModle()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                Price=product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle= productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount=product.ViewCount,
            };
            return productViewModle;
        }

        public async Task<ProductImageViewModle> GetImageByID(int imageId)
        {
            var imgage = await _context.ProductImages.FindAsync(imageId);
            if (imgage == null)
            {
                throw new EShopException("Cannot find a imageId wtih id :{imageId}");
            }
                var viewmodle= new ProductImageViewModle()
                {
                    Caption = imgage.Caption,
                    DateCreated = imgage.DateCreated,
                    FileSize = imgage.FileSize,
                    Id = imgage.Id,
                    ImagePath = imgage.ImagePath,
                    IsDefault = imgage.IsDefault,
                    ProductId = imgage.ProductId,
                    SortOrder = imgage.SortOrder,
                };
            return viewmodle;
        }

        public async Task<List<ProductImageViewModle>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId).
                Select(i => new ProductImageViewModle()
                {
                    Caption=i.Caption,
                    DateCreated=i.DateCreated,
                    FileSize=i.FileSize,
                    Id=i.Id,
                    ImagePath=i.ImagePath,
                    IsDefault=i.IsDefault,
                    ProductId=i.ProductId,
                    SortOrder=i.SortOrder,
                }).ToListAsync();
        }

        public async Task<int> RemoveImage( int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new EShopException("Cannot find a imageId wtih id :{imageId}");
            }
            _context.ProductImages.Remove(productImage);
           return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null) throw new EShopException($"Cannot find a product wtih id :{request.Id}");
            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            if (request.ThumbNailImage != null)
            {
                var thumnailimage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true 
                && i.ProductId == request.Id);
                if (thumnailimage != null)
                {

                    thumnailimage.FileSize = request.ThumbNailImage.Length;
                    thumnailimage.ImagePath = await this.SaveFile(request.ThumbNailImage);
                    _context.ProductImages.Update(thumnailimage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductIamgeUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new EShopException("cannot find ID{imageId}");
            }
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int ProductId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) throw new EShopException($"Cannot find a product wtih id :{ProductId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product wtih id :{productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<List<ProductViewModle>> GetAll(string languageId)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            var data = await query.Select(x => new ProductViewModle()
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoTitle = x.pt.SeoTitle,
                SeoDescription = x.pt.SeoDescription,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
            }).ToListAsync();
            return data;
        }



        public async Task<PagedResult<ProductViewModle>> GetAllbyCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }
            //paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModle()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoTitle = x.pt.SeoTitle,
                    SeoDescription = x.pt.SeoDescription,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                }).ToListAsync();
            var pagedResult = new PagedResult<ProductViewModle>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pagedResult;
        }
    }
}
