using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class TaiKhoan
{
    public int IdTaiKhoan { get; set; }

    public string? TenTaiKhoan { get; set; }

    public string? MatKhau { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<CapQuyenTaiKhoan> CapQuyenTaiKhoans { get; set; } = new List<CapQuyenTaiKhoan>();
}
