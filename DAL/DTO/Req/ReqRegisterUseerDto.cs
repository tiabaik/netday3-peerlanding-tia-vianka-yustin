using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{

    public class ReqRegisterUseerDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string Password { get; set; } = "Password1";

        [Required(ErrorMessage = "Role is required")]
        [MaxLength(30, ErrorMessage = "Role cannot exceed 30 characters")]
        [RegularExpression("^(lender|borrower|admin)$", ErrorMessage = "Role must be either 'lender', 'borrower', or 'admin'")]
        public string Role { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value")]
        public decimal? Balance { get; set; } = 0; // Default balance is 0
    }

    //public class ReqRegisterUseerDto
    //{
    //    [Required(ErrorMessage = "name is required")]
    //    [MaxLength(30, ErrorMessage = "name cannot exceed 30 characters")]
    //    public string Name { get; set; }

    //    [Required(ErrorMessage = "email is required")]
    //    [MaxLength(50, ErrorMessage = "email cannot exceed 50 characters")]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = "password is required")]
    //    [MinLength(8, ErrorMessage = "password must be at least 8 characters long")]
    //    [MaxLength(50, ErrorMessage ="Password cannot exceed 50 characters")]
    //    public string Password { get; set; }

    //    [Required(ErrorMessage = "role is required")]
    //    [MaxLength(30, ErrorMessage = "role cannot exceed 30 characters")]

    //    public string Role { get; set; }

    //    [Range(0, double.MaxValue, ErrorMessage = "balance must be a positive value")]
    //    public decimal? Balance { get; set; }


    //}
}
