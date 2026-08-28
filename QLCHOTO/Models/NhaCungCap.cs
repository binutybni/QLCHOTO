using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class NhaCungCap
{
    public int IdNhaCungCap { get; set; }

    public string? MaNhaCungCap { get; set; }

    public string? TenNhaCungCap { get; set; }

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public int? NgayLienKet { get; set; }

    public string? TrangThai { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<MuonTraSanPham> MuonTraSanPhams { get; set; } = new List<MuonTraSanPham>();

    public virtual ICollection<QuanLyBaoHanh> QuanLyBaoHanhs { get; set; } = new List<QuanLyBaoHanh>();
}
