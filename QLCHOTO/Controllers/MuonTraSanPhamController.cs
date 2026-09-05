using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.MuonTraSanPham;
using System.Timers;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuonTraSanPhamController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public MuonTraSanPhamController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("getall-muontrasanpham")]
        public async Task<IActionResult> GetAllMuonTraSP([FromBody] PaginationClass sr)
        {
            var query = db.MuonTraSanPhams.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query.Where(x => x.IdSanPhamNavigation.TenSp.Contains(sr.SearchTerm) ||
                                         x.IdNhaCungCapNavigation.TenNhaCungCap.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy", success = false });
            }

            var muonsp = await query
                .Select(x => new
                {
                    x.IdNhaCungCapNavigation.TenNhaCungCap,
                    x.IdSanPhamNavigation.TenSp,
                    x.SoLuongMuon,
                    x.IdSanPhamNavigation.IdDvtNavigation.TenDvt,
                    x.NgayMuon,
                    x.NgayTra,
                    x.TrangThai
                }).ToListAsync();
            return Ok(muonsp);
        }

        [HttpPost]
        [Route("create-muontrasanpham")]
        public async Task<IActionResult> CreateMuonTraSP([FromBody] CreateMuonTraSP sp)
        {
            var check_sp = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == sp.IdSanPham);
            if (check_sp == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            var check_ncc = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.IdNhaCungCap == sp.IdNhaCungCap);
            if (check_ncc == null)
            {
                return Ok(new { msg = "không có nhà cung cấp này", success = false });
            }

            var newmuonsp = new MuonTraSanPham
            {
                IdNhaCungCap = sp.IdNhaCungCap,
                IdSanPham = sp.IdSanPham,
                SoLuongMuon = sp.SoLuongMuon,
                NgayMuon = sp.NgayMuon,
                NgayTra = sp.NgayTra,
                TrangThai = sp.TrangThai,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newmuonsp);
            await db.SaveChangesAsync();
            return Ok(new { msg = "thêm mới thành công", success = true });
        }

        [HttpPut]
        [Route("update-muontrasanpham")]
        public async Task<IActionResult> UpdateMuonTraSP(int id, [FromBody] UpdateMuonTraSP sp)
        {
            var check_id = await db.MuonTraSanPhams.FirstOrDefaultAsync(x => x.IdMuon == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có mượn trả sản phẩm này", success = false });
            }

            var check_sp = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == sp.IdSanPham);
            if (check_sp == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            var check_ncc = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.IdNhaCungCap == sp.IdNhaCungCap);
            if (check_ncc == null)
            {
                return Ok(new { msg = "không có nhà cung cấp này", success = false });
            }

            check_id.IdNhaCungCap = sp.IdNhaCungCap;
            check_id.IdSanPham = sp.IdSanPham;
            check_id.SoLuongMuon = sp.SoLuongMuon;
            check_id.NgayMuon = sp.NgayMuon;
            check_id.NgayTra = sp.NgayTra;
            check_id.TrangThai = sp.TrangThai;
            check_id.TimeUp = unixTimestamp;

            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-muontrasanpham")]
        public async Task<IActionResult> DeleteMuonTraSP(int id)
        {
            var check_id = await db.MuonTraSanPhams.FirstOrDefaultAsync(x => x.IdMuon == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có mượn trả sản phẩm này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "xóa thành công", success = true });
        }
    }
}
