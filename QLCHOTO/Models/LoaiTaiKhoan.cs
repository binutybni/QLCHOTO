using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class LoaiTaiKhoan
{
    public int IdLoaiTaiKhoan { get; set; }

    public string? TenLoaiTaiKhoan { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<CapQuyenTaiKhoan> CapQuyenTaiKhoans { get; set; } = new List<CapQuyenTaiKhoan>();
}
