using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopAPI.Data;
using WebShopAPI.Helpers;
using WebShopAPI.Models;

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

            if (model.ProductItems == null/* || !model.ProductItems.Any()*/)
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

                var product = new Product
                {
                    IdPro = idPro,
                    IdCate = model.IdCate,
                    Name = model.Name,
                    Price = model.Price,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    CreateDate = DateTime.Now,
                    StatusProduct = 1, 
                };

                await _context.products.AddAsync(product);
                var productItem = new ProductItem
                {
                    IdPro = idPro,
                    IdProItem = GenerateNextProductItemsId(),
                    Size = model.ProductItems.Size,
                    Color = model.ProductItems.Color,
                    Quantity = model.ProductItems.Quantity,
                    StatusProItem = 1
                };
                await _context.productItems.AddAsync(productItem);

               /* foreach (var proItemModel in model.ProductItems)
                {
                    var productItem = new ProductItem
                    {
                        IdPro = idPro,
                        IdProItem = GenerateNextProductItemsId(),
                        Size = proItemModel.Size,
                        Color = proItemModel.Color,
                        Quantity = proItemModel.Quantity,
                        StatusProItem = 1
                    };
                    await _context.productItems.AddAsync(productItem);
                }*/

                foreach (var imgFile in model.ImgFiles)
                {
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
                        IdImg = GenerateNextIMGId(),
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


        public async Task<List<Product>> GetAll(string? searchString, string? IdCate, float? from, float? to)
        {
            var lsProduct = _context.products.AsNoTracking()
                .Where(x => x.StatusProduct == 1).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                lsProduct = lsProduct.Where(x => x.Name.ToLower().Contains(searchString));
            }
            if (!string.IsNullOrEmpty(IdCate)) 
            {
                lsProduct = lsProduct.Where(x => x.IdCate == IdCate);
            }
            if (from.HasValue)
            {
                lsProduct = lsProduct.Where(x => x.Price >= from);
            }
            if (to.HasValue)
            {
                lsProduct = lsProduct.Where(x => x.Price <= to);
            }
            var result = await lsProduct.OrderByDescending(p => p.UpdateDate != null ? p.UpdateDate : p.CreateDate).ToListAsync();

            return result;
        }
     


     
        public Task Update(ProductModel model, string id)
        {
            throw new NotImplementedException();
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
            product.StatusProduct = 0; // Đã xóa
            _context.Update(product);
            foreach (var pi in product.ProductItems)
            {
                pi.StatusProItem = 0;
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
                    nextNumber = numericValue + 1;
                }
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

       
    }
}
