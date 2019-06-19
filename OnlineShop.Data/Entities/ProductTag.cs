using OnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShop.Data.Entities
{
    public class ProductTag : DomainEntity<int>
    {
        public int ProductId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string TagId { set; get; }

        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { set; get; }
    }
}
