using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.DonViTinh;
using QLCHOTO.Models.DTOs.LoaiTaiKhoan;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiTaiKhoanController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public LoaiTaiKhoanController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-loataikhoan")]
        public async Task<IActionResult> GetAllLoaiTaiKhoan([FromBody] PaginationClass sr)
        {
            var query = db.LoaiTaiKhoans.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query.Where(x => x.TenLoaiTaiKhoan.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy loại tài khoản đó", success = false });
            }

            var ltk = await query
                .Select(x => new
                {
                    x.TenLoaiTaiKhoan,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(ltk);
        }

        [HttpPost]
        [Route("create-loataikhoan")]
        public async Task<IActionResult> CreateDonViTinh([FromBody] CreateLoaiTaiKhoan ltk)
        {
            if (string.IsNullOrWhiteSpace(ltk.TenLoaiTaiKhoan))
            {
                return Ok(new { msg = "Tên loại tài khoản không được để trống", success = false });
            }

            var check_name = await db.LoaiTaiKhoans.FirstOrDefaultAsync(x => x.TenLoaiTaiKhoan == ltk.TenLoaiTaiKhoan);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có loại tài khoản này rồi", success = false });
            }

            var newltk = new LoaiTaiKhoan
            {
                TenLoaiTaiKhoan = ltk.TenLoaiTaiKhoan,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newltk);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-loataikhoan")]
        public async Task<IActionResult> UpdateLoaiTaiKhoan(int id, [FromBody] UpdateLoaiTaiKhoan ltk)
        {
            var check_id = await db.LoaiTaiKhoans.FirstOrDefaultAsync(x => x.IdLoaiTaiKhoan == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có loại tài khoản này", success = false });
            }

            if (string.IsNullOrWhiteSpace(ltk.TenLoaiTaiKhoan))
            {
                return Ok(new { msg = "Tên loại tài khoản không được để trống", success = false });
            }

            var check_name = await db.LoaiTaiKhoans.FirstOrDefaultAsync(x => x.TenLoaiTaiKhoan == ltk.TenLoaiTaiKhoan && x.IdLoaiTaiKhoan != id);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có loại tài khoản này rồi", success = false });
            }

            check_id.TenLoaiTaiKhoan = ltk.TenLoaiTaiKhoan;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-loataikhoan")]
        public async Task<IActionResult> DeleteLoaiTaiKhoan(int id)
        {
            var check_id = await db.LoaiTaiKhoans.FirstOrDefaultAsync(x => x.IdLoaiTaiKhoan == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có loại tài khoản này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }
    }
}
