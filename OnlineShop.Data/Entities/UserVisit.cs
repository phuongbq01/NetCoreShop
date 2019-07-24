using OnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShop.Data.Entities
{
    [Table("UserVisits")]
    public class UserVisit : DomainEntity<int>
    {
        public int IpAddress { get; set; }

        public string UserName { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        public DateTime DtStart { get; set; }

        public DateTime? DtEnd { get; set; }
    }
}
