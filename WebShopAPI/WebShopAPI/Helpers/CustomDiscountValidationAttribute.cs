using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Helpers
{
	public class CustomDiscountValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			// Nếu giá trị không phải là float, trả về lỗi
			if (!(value is float discountAmount))
			{
				return new ValidationResult("Invalid discount amount format.");
			}

			// Kiểm tra nếu là giảm theo phần trăm (0.01 đến 0.99)
			if (discountAmount > 0 && discountAmount < 1)
			{
				return ValidationResult.Success; // Hợp lệ
			}

			// Kiểm tra nếu là giảm trừ số tiền (>= 1)
			if (discountAmount >= 1)
			{
				return ValidationResult.Success; // Hợp lệ
			}

			// Nếu không thuộc một trong hai trường hợp trên, trả về lỗi
			return new ValidationResult("Discount amount must be between 0.01 and 0.99 for percentage, or greater than or equal to 1 for flat discount.");
		}
	}
}
