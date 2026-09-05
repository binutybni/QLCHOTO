namespace QLCHOTO.Models.DTOs.QuanLyBaoHanh
{
    public class CreateQLBH
    {
        public int? IdKh { get; set; }

        public int? IdNhaCungCap { get; set; }

        public int? IdSp { get; set; }

        public string? SoBaoHanh { get; set; }

        public int? NgayBaoHanh { get; set; }

        public int? NgayKetThucBaoHanh { get; set; }

        public string? TrangThai { get; set; }

        public string? TenKh { get; set; }

        public string? Sdt { get; set; }
    }
}
