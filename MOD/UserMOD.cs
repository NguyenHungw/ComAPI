using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace COM.MOD
{

    public class LoginUSER
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterUSER
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    //public class User
    //{
    //    public int Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Username { get; set; }

    //    [JsonIgnore]
    //    public string Password { get; set; }
    //}
    public class ChiTietUserMOD
    {
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? CreateAt { get; set; }
        public int? isActive { get; set; }
    }
    public class UserMOD
    {
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? isActive { get; set; }
        public string? TenNND { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? CreateAt { get; set; }
    }
    public class UserUpdateMOD
    {
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? isActive { get; set; }
        public int? IDNND { get; set; }
       
    }
}


