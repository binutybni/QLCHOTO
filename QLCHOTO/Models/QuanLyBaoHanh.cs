using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class QuanLyBaoHanh
{
    public int IdQuanLyBaoHanh { get; set; }

    public int? IdKh { get; set; }

    public int? IdNhaCungCap { get; set; }

    public int? IdSp { get; set; }

    public string? SoBaoHanh { get; set; }

    public string? NgayBaoHanh { get; set; }

    public string? NgayKetThucBaoHanh { get; set; }

    public string? TrangThai { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual KhachHang? IdKhNavigation { get; set; }

    public virtual NhaCungCap? IdNhaCungCapNavigation { get; set; }

    public virtual SanPham? IdSpNavigation { get; set; }
}
