using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD
{
    public class ProviderSettingMOD
    {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
