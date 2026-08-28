using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class MuonTraSanPham
{
    public int IdMuon { get; set; }

    public int? IdSanPham { get; set; }

    public int? IdNhaCungCap { get; set; }

    public int? NgayMuon { get; set; }

    public int? NgayTra { get; set; }

    public string? TrangThai { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual NhaCungCap? IdNhaCungCapNavigation { get; set; }

    public virtual SanPham? IdSanPhamNavigation { get; set; }
}
