﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Res
{
    public class ResListLoadBorrowerAcc
    {
        public string LoanId { get; set; }
        public String BorrowerName { get; set; }
        public decimal Amount { get; set; }

        public decimal InterestRate { get; set; }

        public int Duration { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
