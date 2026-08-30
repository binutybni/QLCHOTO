namespace QLCHOTO.Models.DTOs.QuanLyBaoHanh
{
    public class CreateQLBH
    {
        public int? IdKh { get; set; }

        public int? IdNhaCungCap { get; set; }

        public int? IdSp { get; set; }

        public string? SoBaoHanh { get; set; }

        public string? NgayBaoHanh { get; set; }

        public string? NgayKetThucBaoHanh { get; set; }

        public string? TrangThai { get; set; }
    }
}
