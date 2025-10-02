using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD.Jwt
{

    public class jwtmod
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int ID { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TimeOut { get; set; }

        public List<string> ChucNangVaQuyen { get; set; }
        /* public class ChucNangQuyen
         {
             public string TenChucNang { get; set; }
             public List<string> Quyen { get; set; }
         }*/
    }
    public class jwtRefreshMod
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }


    public class refreshJwtClaimInfo
    {
        public int UserID { get; set; }
        public string Email { get; set; }
     
    }
    public class TokenInfo
    {
        public int UserID { get; set; }
        public string Name {  get; set; } 
        public string Email { get; set; }
        public List<Claim> Claims { get; set; }
        public string Role { get; set; }
        //public bool IsAuthenticated { get; set; }
    }

}
