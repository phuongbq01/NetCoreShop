using Microsoft.AspNetCore.Identity;
using OnlineShop.Data.Enums;
using OnlineShop.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public AppUser() {
        }
        public AppUser(Guid id, string fullName, string userName,
            string email, string phoneNumber, string avatar, Status status)
        {
            Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Avatar = avatar;
            Status = status;
        }

        // Thêm các thuộc tính cho User
        public string FullName { get; set; }

        public DateTime? BirthDay { set; get; }

        public decimal Balance { get; set; }

        public string Avatar { get; set; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
