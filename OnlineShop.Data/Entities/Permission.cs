using OnlineShop.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineShop.Data.Entities
{
    [Table("Permissions")]
    public class Permission : DomainEntity<int>
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [Column(TypeName ="varchar(128)")]
        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }
        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }
        public bool CanDelete { set; get; }


        [ForeignKey("RoleId")]
        public virtual AppRole AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }
    }
}
