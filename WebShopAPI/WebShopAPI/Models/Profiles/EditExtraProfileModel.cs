using System.ComponentModel.DataAnnotations;

namespace WebShopAPI.Models.Profiles
{
    public class EditExtraProfileModel
    {

            [Display(Name = "Tài Khoản")]
            public string UserName { get; set; }

            [Display(Name = "Email")]
            public string UserEmail { get; set; }

            [Display(Name = "Số Điện Thoại")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Địa Chỉ")]
            [StringLength(400)]
            public string HomeAdress { get; set; }

            [Display(Name = "Ngày Sinh")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [AgeValidation(ErrorMessage = "Just greater than 18 year olds!")]
            public DateTime? BirthDate { get; set; }

            [Display(Name = "Giới Tính")]
            public int? Gender { get; set; }

            [Display(Name = "Họ và Tên")]
            public string FullName { get; set; }

            public string Avt { get; set; }




        }
        public class AgeValidationAttribute : ValidationAttribute
        {
            /*public override bool IsValid(object value)
            {
                DateTime birthDate = (DateTime)value;
                int age = DateTime.Now.Year - birthDate.Year;
                return age >= 16;
            }*/
        }
}
