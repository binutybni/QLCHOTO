using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class NhomSanPham
{
    public int IdNhomSanPham { get; set; }

    public string? TenNhomSanPham { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
