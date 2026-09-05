using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class NhanVien
{
    public int IdNv { get; set; }

    public string? MaNv { get; set; }

    public string? TenNv { get; set; }

    public string? ThamNien { get; set; }

    public string? Sdt { get; set; }

    public string? NamSinh { get; set; }

    public int? NgayVaoLam { get; set; }

    public string? TrangThai { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
