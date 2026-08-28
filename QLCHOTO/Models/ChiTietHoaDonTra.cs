using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class ChiTietHoaDonTra
{
    public int IdChiTietHoaDonTra { get; set; }

    public int? IdHdTra { get; set; }

    public int? IdSp { get; set; }

    public double? SoLuong { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual HoaDonTra? IdHdTraNavigation { get; set; }

    public virtual SanPham? IdSpNavigation { get; set; }
}
