using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class KhachHang
{
    public int IdKh { get; set; }

    public string? TenKh { get; set; }

    public string? Sdt { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<QuanLyBaoHanh> QuanLyBaoHanhs { get; set; } = new List<QuanLyBaoHanh>();
}
