using COM.DAL;
using COM.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderSettingController : ControllerBase
    {
        private readonly ProviderSettingDAL _providerSettingDAL;

        public ProviderSettingController(IConfiguration config)
        {
            _providerSettingDAL = new ProviderSettingDAL(config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet("get-all")]
        public IActionResult GetAll(string name)
        {
            var secrets = _providerSettingDAL.GetByProvider(name);
            return Ok(secrets);
        }

        [HttpPost("add")]
        public IActionResult AddSecret([FromBody] ProviderSettingMOD item)
        {
            _providerSettingDAL.Insert(item);
            return Ok(new { message = "Thêm thành công" });
        }

        [HttpPut("update")]
        public IActionResult UpdateSecret([FromBody] ProviderSettingMOD item)
        {
            _providerSettingDAL.Update(item);
            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("delete/{provider}")]
        public IActionResult DeleteSecret(string provider)
        {
            _providerSettingDAL.Delete(provider);
            return Ok(new { message = "Xóa thành công" });
        }
    }
}
