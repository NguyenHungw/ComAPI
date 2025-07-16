using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using COM.MOD.SanPham;
using COM.MOD.Vnpay;
using Microsoft.AspNetCore.Http;

namespace COM.Services.Vnpay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(DonHangRequestModel model, HttpContext context);
        PaymentResponseMOD PaymentExecute(IQueryCollection collections);
    }
}
