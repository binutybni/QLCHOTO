using System;
using System.Collections.Generic;

namespace QLCHOTO.Models;

public partial class CapQuyenTaiKhoan
{
    public int IdCapQuyenTk { get; set; }

    public int? IdTaiKhoan { get; set; }

    public int? IdLoaiTaiKhoan { get; set; }

    public int? NgayCapQuyen { get; set; }

    public int? TimeCre { get; set; }

    public int? TimeUp { get; set; }

    public virtual LoaiTaiKhoan? IdLoaiTaiKhoanNavigation { get; set; }

    public virtual TaiKhoan? IdTaiKhoanNavigation { get; set; }
}
