using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class HoaDonTra
{
    public int IdHdTra { get; set; }

    public string? MaHdTra { get; set; }

    public int? IdHd { get; set; }

    public int? NgayXuatHoaDon { get; set; }

    public double? TienCong { get; set; }

    public double? ThanhTien { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual ICollection<ChiTietHoaDonTra> ChiTietHoaDonTras { get; set; } = new List<ChiTietHoaDonTra>();

    public virtual HoaDon? IdHdNavigation { get; set; }
}
