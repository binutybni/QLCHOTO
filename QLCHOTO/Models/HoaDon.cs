using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class HoaDon
{
    public int IdHoaDon { get; set; }

    public string? MaHd { get; set; }

    public int? IdKh { get; set; }

    public int? IdNhaCungCap { get; set; }

    public int? IdNv { get; set; }

    public int? NgayXuatHoaDon { get; set; }

    public double? TienCong { get; set; }

    public string? MotaTiencong { get; set; }

    public double? TienTru { get; set; }

    public double? ThanhTien { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<HoaDonTra> HoaDonTras { get; set; } = new List<HoaDonTra>();

    public virtual KhachHang? IdKhNavigation { get; set; }

    public virtual NhaCungCap? IdNhaCungCapNavigation { get; set; }

    public virtual NhanVien? IdNvNavigation { get; set; }
}
