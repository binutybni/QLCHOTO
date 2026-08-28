using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class ChiTietHoaDon
{
    public int IdChiTietHoaDon { get; set; }

    public int? IdHoaDon { get; set; }

    public int? IdSp { get; set; }

    public int? SoLuong { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual HoaDon? IdHoaDonNavigation { get; set; }

    public virtual SanPham? IdSpNavigation { get; set; }
}
