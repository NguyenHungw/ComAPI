using COM.MOD;
using COM.MOD.SanPham;

public interface IDonHangService
{
    /// <summary>
    /// Tạo đơn hàng và xử lý thanh toán qua VNPAY hoặc chuyển tiếp thanh toán khác.
    /// </summary>
    /// <param name="model">Thông tin đơn hàng cần tạo</param>
    /// <param name="userId">ID người dùng đang đăng nhập</param>
    /// <returns>URL thanh toán nếu là VNPAY, hoặc URL thành công</returns>
    string TaoDonHangVaThanhToan(DonHangRequestModel item);
}
