using WebShopAPI.Data;

namespace WebShopAPI.Dtos
{
        public class ProductDto
        {
            public string IdPro { get; set; }
            public string IdCate { get; set; }
            public string Name { get; set; }
            public float Price { get; set; }
            public string ShortDescription { get; set; }
            public string LongDescription { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public int? BestSeller { get; set; }
            public List<ProductItemDto> ProductItems { get; set; }
            public List<ImgProDto> Images { get; set; }
        }
    }
