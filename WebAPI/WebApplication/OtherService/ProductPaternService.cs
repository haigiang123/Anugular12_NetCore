using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Common;
using WebData.EF;
using WebData.Entities;
using WebRepository.Infrastructure;
using WebRepository.Repository;
using WebUltilities;
using System.Linq;
using WebViewModel.Common;
using WebViewModel.OtherService;

namespace WebApplication.OtherService
{
    public interface IProductPaternService
    {
        Task<int> Create(ProductCreateRequest request);
        PagedResult<ProductVm> GetAllPaging(GetManageProductPagingRequest request);
    }

    public class ProductPaternService : IProductPaternService
    {
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductInCategoryRepository _productInCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductTranslationRepository _productTranslationRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICategoryTranslationRepository _categoryTranslationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        //private readonly WebAPIDbContext _context;

        public ProductPaternService(ICategoryRepository categoryRepository, IProductInCategoryRepository productInCategoryRepository,
            IProductRepository productRepository, IProductTranslationRepository productTranslationRepository,
            ILanguageRepository languageRepository, IProductImageRepository productImageRepository,
            ICategoryTranslationRepository categoryTranslationRepository,
            IUnitOfWork unitOfWork, IStorageService storageService
            //WebAPIDbContext context
            )
        {
            _categoryRepository = categoryRepository;
            _productInCategoryRepository = productInCategoryRepository;
            _productRepository = productRepository;
            _productTranslationRepository = productTranslationRepository;
            _languageRepository = languageRepository;
            _productImageRepository = productImageRepository;
            _categoryTranslationRepository = categoryTranslationRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            //_context = context;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
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
            _productImageRepository.Add(productImage);
            await _unitOfWork.SaveChange();
            return productImage.Id;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _productRepository.FindAsync(productId);
            product.ViewCount += 1;
            await _unitOfWork.SaveChange();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var languages = _languageRepository.GetAll().ToList();
            var translations = new List<ProductTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    translations.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    translations.Add(new ProductTranslation()
                    {
                        Name = SystemConstants.ProductConstants.NA,
                        Description = SystemConstants.ProductConstants.NA,
                        SeoAlias = SystemConstants.ProductConstants.NA,
                        LanguageId = language.Id
                    });
                }
            }
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = translations
            };
            //Save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _productRepository.Add(product);
            await _unitOfWork.SaveChange();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _productRepository.FindAsync(productId);
            if (product == null) throw new WebAPIException($"Cannot find a product: {productId}");

            var images = _productImageRepository.GetAll().Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _productRepository.Delete(product);

            return await _unitOfWork.SaveChange();
        }

        public PagedResult<ProductVm> GetAllPaging(GetManageProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _productRepository.GetAll()
                        join pt in _productTranslationRepository.GetAll() on p.Id equals pt.ProductId
                        join pic in _productInCategoryRepository.GetAll() on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _categoryRepository.GetAll() on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        join pi in _productImageRepository.GetAll() on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId && pi.IsDefault == true
                        select new { p, pt, pic, pi };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            //3. Paging
            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
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
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToList();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductVm>()
            {
                Total = totalRow,
                Size = request.PageSize,
                Page = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ProductVm> GetById(int productId, string languageId)
        {
            var product = await _productRepository.FindAsync(productId);
            var productTranslation = await _productTranslationRepository.GetSingleByConditionAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);

            var categories = (from c in _categoryRepository.GetAll()
                                    join ct in _categoryTranslationRepository.GetAll() on c.Id equals ct.CategoryId
                                    join pic in _productInCategoryRepository.GetAll() on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToList();

            var image = await _productImageRepository.GetSingleByConditionAsync(x => x.ProductId == productId && x.IsDefault == true);

            var productViewModel = new ProductVm()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId = productTranslation.LanguageId,
                Details = productTranslation != null ? productTranslation.Details : null,
                Name = productTranslation != null ? productTranslation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories,
                ThumbnailImage = image != null ? image.ImagePath : "no-image.jpg"
            };
            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _productImageRepository.FindAsync(imageId);
            if (image == null)
                throw new WebAPIException($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<IEnumerable<ProductImageViewModel>> GetListImages(int productId)
        {
            return (await _productImageRepository.GetMultiAsync(x => x.ProductId == productId))
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                });
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _productImageRepository.FindAsync(imageId);
            if (productImage == null)
                throw new WebAPIException($"Cannot find an image with id {imageId}");
            _productImageRepository.Delete(productImage);
            return await _unitOfWork.SaveChange();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _productRepository.FindAsync(request.Id);
            var productTranslations = await _productTranslationRepository.GetSingleByConditionAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);

            if (product == null || productTranslations == null) throw new WebAPIException($"Cannot find a product with id: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _productImageRepository.GetSingleByConditionAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _productImageRepository.Update(thumbnailImage);
                }
            }

            return await _unitOfWork.SaveChange();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _productImageRepository.FindAsync(imageId);
            if (productImage == null)
                throw new WebAPIException($"Cannot find an image with id {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _productImageRepository.Update(productImage);
            return await _unitOfWork.SaveChange();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _productRepository.FindAsync(productId);
            if (product == null) throw new WebAPIException($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _unitOfWork.SaveChange() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _productRepository.FindAsync(productId);
            if (product == null) throw new WebAPIException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            return await _unitOfWork.SaveChange() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public PagedResult<ProductVm> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _productRepository.GetAll()
                        join pt in _productTranslationRepository.GetAll() on p.Id equals pt.ProductId
                        join pic in _productInCategoryRepository.GetAll() on p.Id equals pic.ProductId
                        join c in _categoryRepository.GetAll() on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //2. filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
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
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToList();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductVm>()
            {
                Total = totalRow,
                Size = request.PageSize,
                Page = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        //public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        //{
        //    var user = await _productRepository.FindAsync(id);
        //    if (user == null)
        //    {
        //        return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
        //    }
        //    foreach (var category in request.Categories)
        //    {
        //        var productInCategory = await _productInCategoryRepository
        //            .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
        //            && x.ProductId == id);
        //        if (productInCategory != null && category.Selected == false)
        //        {
        //            _productInCategoryRepository.Remove(productInCategory);
        //        }
        //        else if (productInCategory == null && category.Selected)
        //        {
        //            await _productInCategoryRepository.AddAsync(new ProductInCategory()
        //            {
        //                CategoryId = int.Parse(category.Id),
        //                ProductId = id
        //            });
        //        }
        //    }
        //    await _unitOfWork.SaveChange();
        //    return new ApiSuccessResult<bool>();
        //}

        public List<ProductVm> GetFeaturedProducts(string languageId, int take)
        {
            //1. Select join
            var query = from p in _productRepository.GetAll()
                        join pt in _productTranslationRepository.GetAll() on p.Id equals pt.ProductId
                        join pic in _productInCategoryRepository.GetAll() on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _productImageRepository.GetAll() on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _categoryRepository.GetAll() on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        && p.IsFeatured == true
                        select new { p, pt, pic, pi };

            var data = query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVm()
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
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToList();

            return data;
        }

        public List<ProductVm> GetLatestProducts(string languageId, int take)
        {
            //1. Select join
            var query = from p in _productRepository.GetAll()
                        join pt in _productTranslationRepository.GetAll() on p.Id equals pt.ProductId
                        join pic in _productInCategoryRepository.GetAll() on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _productImageRepository.GetAll() on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _categoryRepository.GetAll() on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pi == null || pi.IsDefault == true)
                        select new { p, pt, pic, pi };

            var data = query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVm()
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
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = x.pi.ImagePath
                }).ToList();

            return data;
        }
    }
}
