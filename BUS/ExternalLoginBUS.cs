using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.DAL;
using COM.MOD;
using Microsoft.Extensions.Configuration;

namespace COM.BUS
{
    public class ExternalLoginBUS
    {
        private readonly ExternalLoginDAL _dal;

        public ExternalLoginBUS(IConfiguration config)
        {
            _dal = new ExternalLoginDAL(config);
        }

        public int HandleFacebookLogin(FacebookUserMOD user, string providerKey)
        {
            string provider = "Facebook";

            var existingUserId = _dal.GetUserIdByExternalLogin(provider, providerKey);
            if (existingUserId.HasValue)
            {
                return existingUserId.Value;
            }

            return _dal.CreateUserAndExternalLogin(user, provider, providerKey);
        }
    }
}
