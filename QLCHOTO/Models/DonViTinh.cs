using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class DonViTinh
{
    public int Id { get; set; }

    public string? TenDvt { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
