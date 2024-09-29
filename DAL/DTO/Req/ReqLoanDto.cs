using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqLoanDto
    {
        
        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage ="amount must be a positive value")]

        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Interest is required")]

        public decimal InterestRate { get; set; }



    }
}
