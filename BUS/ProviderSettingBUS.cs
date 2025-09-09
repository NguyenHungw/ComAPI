using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM.DAL;
using COM.MOD;

namespace COM.BUS
{
    public class ProviderSettingBUS
    {
        private readonly ProviderSettingDAL _dal;

        public ProviderSettingBUS(string connectionString)
        {
            _dal = new ProviderSettingDAL(connectionString);
        }

        public ProviderSettingMOD GetProvider(string providerName)
        {
            return _dal.GetByProvider(providerName);
        }

        public void CreateProvider(ProviderSettingMOD item)
        {
            _dal.Insert(item);
        }

        public void UpdateProvider(ProviderSettingMOD item)
        {
            _dal.Update(item);
        }

        public void DeleteProvider(string providerName)
        {
            _dal.Delete(providerName);
        }
    }

}
