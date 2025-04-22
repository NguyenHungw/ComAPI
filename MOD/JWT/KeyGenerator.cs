using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD.Jwt
{
    public class KeyGenerator
    {
        public static string GenerateHmacSha256Key()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] keyBytes = new byte[32]; // 256 bits
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
        }
    }
}
