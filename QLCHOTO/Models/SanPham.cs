using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class SanPham
{
    public int IdSanPham { get; set; }

    public string? MaSp { get; set; }

    public string? TenSp { get; set; }

    public string? MoTa { get; set; }

    public int? SoLuong { get; set; }

    public string? TrangThai { get; set; }

    public int? IdNhomSanPham { get; set; }

    public int? IdDvt { get; set; }

    public double? GiaNhap { get; set; }

    public double? GiaBan { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<ChiTietHoaDonTra> ChiTietHoaDonTras { get; set; } = new List<ChiTietHoaDonTra>();

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual DonViTinh? IdDvtNavigation { get; set; }

    public virtual NhomSanPham? IdNhomSanPhamNavigation { get; set; }

    public virtual ICollection<MuonTraSanPham> MuonTraSanPhams { get; set; } = new List<MuonTraSanPham>();

    public virtual ICollection<QuanLyBaoHanh> QuanLyBaoHanhs { get; set; } = new List<QuanLyBaoHanh>();
}
