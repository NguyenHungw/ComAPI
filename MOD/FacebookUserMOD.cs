using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD
{
    public class FacebookUserMOD
 
    {
        // Thông tin User
        public string Name { get; set; }
        public string Email { get; set; }

        // Thông tin External Login
        public string Provider { get; set; } = "Facebook";
        public string ProviderKey { get; set; }
    }
}
