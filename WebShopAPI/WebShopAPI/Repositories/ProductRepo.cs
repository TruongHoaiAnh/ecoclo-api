using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebShopAPI.Data;
using WebShopAPI.Dtos;
using WebShopAPI.Helpers;
using WebShopAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebShopAPI.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;//lấy đến folder wwwroot

        public ProductRepo(AppDbContext context, IMapper mapper, IWebHostEnvironment environment) 
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }
        public async Task<ApiResponse> Create(ProductModel model)
        {
			var allowedExtensions = new[] { ".gif", ".jpeg", ".jpg", ".tiff", ".png", ".webp", ".bmp" };
			var formatThumbnai = Path.GetExtension(model.ThumbnailImg.FileName).ToLowerInvariant();
			if (!allowedExtensions.Contains(formatThumbnai))
			{
				return new ApiResponse
				{
					Success = false,
					ErrorCode = "INVALID_IMAGE_FORMAT",
					Message = $"Invalid image thumbnai file format for file: {model.ThumbnailImg.FileName}. Allowed formats are: {string.Join(", ", allowedExtensions)}"
				};
			}
			if (model == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INVALID_DATA",
                    Message = "Product model cannot be null.",
                };
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_PRODUCT_NAME",
                    Message = "Product name is required."
                };
            }

            if (model.ImgFiles == null || model.ImgFiles.Count == 0)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_IMAGE",
                    Message = "At least one product image is required."
                };
            }

            if (model.ProductItems == null || !model.ProductItems.Any())
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_PRODUCT_ITEMS",
                    Message = "At least one product item is required."
                };
            }

            try
            {
                var idPro = GenerateNextProductId();
				//Thumbnail
				var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(model.ThumbnailImg.FileName);
				var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Product");
				var filePath = Path.Combine(folderPath, fileName);

				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.ThumbnailImg.CopyToAsync(stream);
				}



				var product = new Product
                {
                    IdPro = idPro,
                    IdCate = model.IdCate,
                    ThumbnailImg = fileName,
                    Name = model.Name,
                    Price = model.Price,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    CreateDate = DateTime.Now,
                    StatusProduct = 0, 
                };

                await _context.products.AddAsync(product);
				int countIdProItem = 0;
				foreach (var proItemModel in model.ProductItems)
                {
                    countIdProItem++;
                    var productItem = new ProductItem
                    {
                        IdPro = idPro,
                        IdProItem = (countIdProItem < 2) ? GenerateNextProductItemsId() : NextProductItemsIdWithMany(countIdProItem),
						Size = proItemModel.Size,
                        Color = proItemModel.Color,
                        Quantity = proItemModel.Quantity,
                        StatusProItem = 0
                    };
                    await _context.productItems.AddAsync(productItem);
                }
				int countIdImg = 0;
				foreach (var imgFile in model.ImgFiles)
                {
                    countIdImg++;
					var formatImgProduct = Path.GetExtension(imgFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(formatImgProduct))
                    {
                        return new ApiResponse
                        {
                            Success = false,
                            ErrorCode = "INVALID_IMAGE_FORMAT",
                            Message = $"Invalid image product file format for file: {imgFile.FileName}. Allowed formats are: {string.Join(", ", allowedExtensions)}"

                        };
                    }
					fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(imgFile.FileName);
                    folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Product");
                    filePath = Path.Combine(folderPath, fileName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(stream);
                    }

                    var imgPro = new ImgPro
                    {
						IdImg = (countIdImg < 2) ? GenerateNextIMGId() : NextIMGIdWithMany(countIdImg),
						IdPro = idPro,
                        LinkImg = fileName
                    };
                    await _context.imgPros.AddAsync(imgPro);
                }

                await _context.SaveChangesAsync();

                return new ApiResponse
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = new { ProductId = idPro }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INTERNAL_ERROR",
                    Message = $"An error occurred while creating the product: {ex.Message}"
                };
            }

        }
        public async Task<List<ProductDto>> GetAll(string? searchString, string? IdCate, float? from, float? to)
        {
            var query = _context.products.Where(p => p.StatusProduct == 0).AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(IdCate))
            {
                query = query.Where(x => x.IdCate == IdCate);
            }

            if (from.HasValue)
            {
                query = query.Where(x => x.Price >= from);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.Price <= to);
            }

            // Include related entities and convert to DTOs
            var products = await query
               .Where(p => p.StatusProduct == 0)
               .Select(p => new Product
               {
                   IdPro = p.IdPro,
                   IdCate = p.IdCate,
                   ThumbnailImg = p.ThumbnailImg,
                   Name = p.Name,
                   Price = p.Price,
                   ShortDescription = p.ShortDescription,
                   LongDescription = p.LongDescription,
                   CreateDate = p.CreateDate,
                   StatusProduct = p.StatusProduct,
                   ProductItems = p.ProductItems.Where(pi => pi.StatusProItem == 0).ToList(),
                   ImgPros = p.ImgPros
               })
               .ToListAsync();

            // Use AutoMapper to convert to DTOs
            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return productDtos;
        }
         
        public async Task<ProductDto> GetById(string idPro)
        {
            var product = await _context.products
                .Where(p => p.StatusProduct == 0)
                .Include(p => p.ProductItems)
                .Include(p => p.ImgPros)
                .FirstOrDefaultAsync(p => p.IdPro == idPro);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ApiResponse> Update(string idPro, ProductModel model)
        {
            if (model == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INVALID_DATA",
                    Message = "Product model cannot be null.",
                };
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_PRODUCT_NAME",
                    Message = "Product name is required."
                };
            }

            if (model.ImgFiles == null || model.ImgFiles.Count == 0)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_IMAGE",
                    Message = "At least one product image is required."
                };
            }

            if (model.ProductItems == null || !model.ProductItems.Any())
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "MISSING_PRODUCT_ITEMS",
                    Message = "At least one product item is required."
                };
            }

            try
            {
                // Lấy sản phẩm hiện tại bằng ID
                var existingProductDto = await GetById(idPro);
                if (existingProductDto == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        ErrorCode = "PRODUCT_NOT_FOUND",
                        Message = "Product not found."
                    };
                }

                // Cập nhật thông tin sản phẩm
                existingProductDto.IdCate = model.IdCate;
                existingProductDto.Name = model.Name;
                existingProductDto.Price = model.Price;
                existingProductDto.ShortDescription = model.ShortDescription;
                existingProductDto.LongDescription = model.LongDescription;
                existingProductDto.UpdateDate = DateTime.Now;

                // Cập nhật sản phẩm trong cơ sở dữ liệu
                var product = await _context.products.Where(p => p.StatusProduct == 0).FirstOrDefaultAsync(p => p.IdPro == idPro);
                if (product != null)
                {
                    _mapper.Map(existingProductDto, product);
                    _context.products.Update(product);
                }

                // Cập nhật ProductItems: Soft delete và xóa các mục cũ khỏi cơ sở dữ liệu
                //var existingProductItems = await _context.productItems.Where(pi => pi.IdPro == idPro && pi.StatusProItem == 0).ToListAsync();
                //foreach (var existingProductItem in existingProductItems)
                //{
                //    existingProductItem.StatusProItem = 1;
                //    _context.productItems.Update(existingProductItem);
                //}
                //await _context.SaveChangesAsync();
                foreach (var item in product.ProductItems)
                {
                    item.StatusProItem = 1; // Update the status as needed
                }

                // Save changes to the database
                await _context.SaveChangesAsync();


                // Thêm mới các ProductItems
                int countIdProItem = 0;
                foreach (var proItemModel in model.ProductItems)
                {
                    countIdProItem++;
                    var productItem = new ProductItem
                    {
                        IdPro = idPro,
                        IdProItem = (countIdProItem < 2) ? GenerateNextProductItemsId() : NextProductItemsIdWithMany(countIdProItem),
                        Size = proItemModel.Size,
                        Color = proItemModel.Color,
                        Quantity = proItemModel.Quantity,
                        StatusProItem = 0
                    };
                    await _context.productItems.AddAsync(productItem);
                }

                // Cập nhật ảnh sản phẩm (xóa các ảnh cũ và thêm ảnh mới)
                var existingImgPros = _context.imgPros.Where(img => img.IdPro == idPro).ToList();
                foreach (var existingImg in existingImgPros)
                {
                    var existingImgPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Product", existingImg.LinkImg);
                    if (File.Exists(existingImgPath))
                    {
                        File.Delete(existingImgPath); // Xóa ảnh cũ khỏi thư mục
                    }
                }
                _context.imgPros.RemoveRange(existingImgPros); // Xóa các ảnh cũ khỏi database
                await _context.SaveChangesAsync(); // Lưu thay đổi sau khi xóa ảnh cũ

                // Thêm ảnh mới
                int countIdImg = 0;
                foreach (var imgFile in model.ImgFiles)
                {
                    countIdImg++;

                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(imgFile.FileName);
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Product");
                    var filePath = Path.Combine(folderPath, fileName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(stream);
                    }

                    var imgPro = new ImgPro
                    {
                        IdImg = (countIdImg < 2) ? GenerateNextIMGId() : NextIMGIdWithMany(countIdImg),
                        IdPro = idPro,
                        LinkImg = fileName
                    };
                    await _context.imgPros.AddAsync(imgPro);
                }
                await _context.SaveChangesAsync(); // Lưu tất cả các thay đổi

                return new ApiResponse
                {
                    Success = true,
                    Message = "Product updated successfully",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "INTERNAL_ERROR",
                    Message = $"An error occurred while updating the product: {ex.Message}"
                };
            }
        }




        public async Task<ApiResponse> DeleteById(string id)
        {
            if(id == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "ID_NULL",
                    Message = "Id is null"
                };
            }
            var product = await _context.products
                .Include(pi => pi.ProductItems)
                .Include(pi => pi.ImgPros)
                .FirstOrDefaultAsync(x => x.IdPro.Equals(id));
            if(product == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = "PRODUCT_NOT_FOUND",
                    Message = "Product is not found"
                };
            }
            product.StatusProduct = 1; // Đã xóa
            _context.Update(product);
            foreach (var pi in product.ProductItems)
            {
                pi.StatusProItem = 1;
                _context.Update(pi);
            }
            foreach (var img in product.ImgPros)
            {
                //Xóa cứng luôn
                _context.imgPros.Remove(img);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Img", "Product", img.LinkImg);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            await _context.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Delete Product successfully"
            };
        }

      

        public string GenerateNextProductId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdPro = _context.products
                .Select(p => p.IdPro)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdPro))
            {
                string numericPart = maxIdPro.Substring(2); // Extract numeric part
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdPro = $"PR{nextNumber:D3}"; // Format the new Id_pro

            return nextIdPro;
        }

        public string GenerateNextProductItemsId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdProItem = _context.productItems
                .Select(p => p.IdProItem)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdProItem))
            {
                string numericPart = maxIdProItem.Substring(2); 
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdPro = $"PI{nextNumber:D3}"; 

            return nextIdPro;
        }
        public string NextProductItemsIdWithMany(int count)
        {
            // Retrieve the maximum existing Id_pro
            string maxIdPro = _context.productItems
                .Select(p => p.IdProItem)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            // Generate the next Id_pro
            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdPro))
            {
                string numericPart = maxIdPro.Substring(2);
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + count;
                }
            }
            else//Khi chưa có sản phẩm proitem nào
            {
                nextNumber = count;
            }

            string nextIdPro = $"PI{nextNumber:D3}";

            return nextIdPro;
        }
        public string GenerateNextIMGId()
        {
            // Retrieve the maximum existing Id_pro
            string maxIdIMG = _context.imgPros
                .Select(p => p.IdImg)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdIMG))
            {
                string numericPart = maxIdIMG.Substring(2); 
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + 1;
                }
            }

            string nextIdImg = $"IM{nextNumber:D3}"; 

            return nextIdImg;
        }

        public string NextIMGIdWithMany(int count)
        {
            // Retrieve the maximum existing Id_pro
            string maxIdIMG = _context.imgPros
                .Select(p => p.IdImg)
                .OrderByDescending(id => id)
                .FirstOrDefault();

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(maxIdIMG))
            {
                string numericPart = maxIdIMG.Substring(2);
                if (int.TryParse(numericPart, out int numericValue))
                {
                    nextNumber = numericValue + count;
                }
            }
            else//Khi chưa có sản phẩm proitem nào
            {
                nextNumber = count;
            }

            string nextIdImg = $"IM{nextNumber:D3}";

            return nextIdImg;
        }


    }
}
