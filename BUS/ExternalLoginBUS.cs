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
        //var clientId = "34380660482-4lcr5s6j4o75eha13a8ov38kus0p3686.apps.googleusercontent.com";
        //var clientSecret = "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";
        public int HandleGoogleLogin(GoogleUserMOD user, string providerKey)
        {
            string provider = "Google";
            //string providerKey = "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";
            var existingUserId = _dal.GetUserIdByExternalLogin(provider, providerKey);
            //var existingUserId = ()
            if (existingUserId.HasValue)
            {
                return existingUserId.Value;
            }

            return _dal.CreateUserAndExternalLoginGoogle(user, provider, providerKey);
        }
    }
}
