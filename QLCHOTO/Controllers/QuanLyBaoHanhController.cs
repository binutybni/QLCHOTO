using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.QuanLyBaoHanh;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyBaoHanhController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public QuanLyBaoHanhController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("getall-quanlybaohanh")]
        public async Task<IActionResult> GetAllQLBH([FromBody] PaginationClass sr)
        {
            var query = db.QuanLyBaoHanhs.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query
                    .Where(x => x.SoBaoHanh.Contains(sr.SearchTerm) ||
                           x.IdKhNavigation.TenKh.Contains(sr.SearchTerm) ||
                           x.IdNhaCungCapNavigation.TenNhaCungCap.Contains(sr.SearchTerm) ||
                           x.IdSpNavigation.TenSp.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy", success = false });
            }

            var qlbh = await query
                .Select(x => new
                {
                    x.SoBaoHanh,
                    x.IdKhNavigation.TenKh,
                    x.IdNhaCungCapNavigation.TenNhaCungCap,
                    x.IdSpNavigation.TenSp,
                    x.IdSpNavigation.IdDvtNavigation.TenDvt,
                    x.NgayBaoHanh,
                    x.NgayKetThucBaoHanh,
                    x.TrangThai
                }).ToListAsync();
            return Ok(qlbh);
        }

        [HttpPost]
        [Route("create-quanlybaohanh")]
        public async Task<IActionResult> CreateQLBH([FromBody] CreateQLBH bh)
        {
            var check_ma = await db.QuanLyBaoHanhs.FirstOrDefaultAsync(x => x.SoBaoHanh == bh.SoBaoHanh);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có số bảo hành này rồi", success = false });
            }

            var check_nhacungcap = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.IdNhaCungCap == bh.IdNhaCungCap);
            if (check_nhacungcap == null)
            {
                return Ok(new { msg = "không có nhà cung cấp này", success = false });
            }

            var check_sp = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == bh.IdSp);
            if (check_sp == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            var newkh = new KhachHang
            {
                TenKh = bh.TenKh,
                Sdt = bh.Sdt,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newkh);
            await db.SaveChangesAsync();

            var newbh = new QuanLyBaoHanh
            {
                SoBaoHanh = bh.SoBaoHanh,
                IdKh = newkh.IdKh,
                IdNhaCungCap = bh.IdNhaCungCap,
                IdSp = bh.IdSp,
                NgayBaoHanh = bh.NgayBaoHanh,
                NgayKetThucBaoHanh = bh.NgayKetThucBaoHanh,
                TrangThai = bh.TrangThai
            };
            await db.AddAsync(newbh);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });
        }

        [HttpPut]
        [Route("update-quanlybaphanh")]
        public async Task<IActionResult> UpdateQLBH(int id, [FromBody] UpdateQLBH bh)
        {
            var check_id = await db.QuanLyBaoHanhs.FirstOrDefaultAsync(x => x.IdQuanLyBaoHanh == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có quản lý bảo hành này", success = false });
            }

            if (string.IsNullOrWhiteSpace(bh.SoBaoHanh))
            {
                return Ok(new { msg = "Số bảo hành không được để trống", success = false });
            }

            var check_ma = await db.QuanLyBaoHanhs.FirstOrDefaultAsync(x => x.SoBaoHanh == bh.SoBaoHanh && x.IdQuanLyBaoHanh != id);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có số bảo hành này rồi", success = false });
            }

            var check_nhacungcap = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.IdNhaCungCap == bh.IdNhaCungCap);
            if (check_nhacungcap == null)
            {
                return Ok(new { msg = "không có nhà cung cấp này", success = false });
            }

            var check_kh = await db.KhachHangs.FirstOrDefaultAsync(x => x.IdKh == bh.IdKh);
            if (check_kh == null)
            {
                return Ok(new { msg = "không có khách hàng này", success = false });
            }

            var check_sp = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == bh.IdSp);
            if (check_sp == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            if (bh.NgayKetThucBaoHanh < bh.NgayBaoHanh)
            {
                return Ok(new { msg = "Ngày kết thúc bảo hành không được trước ngày bảo hành", success = false });
            }

            check_id.SoBaoHanh = bh.SoBaoHanh;
            check_id.IdKh = bh.IdKh;
            check_id.IdNhaCungCap = bh.IdNhaCungCap;
            check_id.IdSp = bh.IdSp;
            check_id.NgayBaoHanh = bh.NgayBaoHanh;
            check_id.NgayKetThucBaoHanh = bh.NgayKetThucBaoHanh;
            check_id.TrangThai = bh.TrangThai;

            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-quanlybaohanh")]
        public async Task<IActionResult> DeleteQLBH(int id)
        {
            var check_id = await db.QuanLyBaoHanhs.FirstOrDefaultAsync(x => x.IdQuanLyBaoHanh == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có quản lý bảo hành này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "xóa thành công", success = true });
        }

    }
}
