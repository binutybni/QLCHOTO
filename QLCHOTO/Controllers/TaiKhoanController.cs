using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.TaiKhoan;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public TaiKhoanController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-taikhoan")]
        public async Task<IActionResult> GetAllDonViTinh([FromBody] PaginationClass sr)
        {
            var query = db.TaiKhoans.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query.Where(x => x.TenTaiKhoan.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy tài khoản đó", success = false });
            }

            var tk = await query
                .Select(x => new
                {
                    x.TenTaiKhoan,
                    x.MatKhau,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(tk);
        }

        [HttpPost]
        [Route("create-taikhoan")]
        public async Task<IActionResult> CreateDonViTinh([FromBody] CreateTK tk)
        {
            if (string.IsNullOrWhiteSpace(tk.TenTaiKhoan))
            {
                return Ok(new { msg = "Tên tài khoản không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                return Ok(new { msg = "Mật khẩu không được để trống", success = false });
            }

            var check_name = await db.TaiKhoans.FirstOrDefaultAsync(x => x.TenTaiKhoan == tk.TenTaiKhoan);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có tên tài khoản này rồi", success = false });
            }

            if (string.IsNullOrEmpty(tk.MatKhau))
            {
                return Ok(new { msg = "không được bỏ trống mật khẩu", success = false });
            }

            if (string.IsNullOrEmpty(tk.TenTaiKhoan))
            {
                return Ok(new { msg = "không được bỏ trống tên tài khoản", success = false });
            }
            var newtk = new TaiKhoan
            {
                TenTaiKhoan = tk.TenTaiKhoan,
                MatKhau = tk.MatKhau,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newtk);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-taikhoan")]
        public async Task<IActionResult> UpdateDonViTinh(int id, [FromBody] UpdateTK tk)
        {
            var check_id = await db.TaiKhoans.FirstOrDefaultAsync(x => x.IdTaiKhoan == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có tài khoản này", success = false });
            }

            if (string.IsNullOrWhiteSpace(tk.TenTaiKhoan))
            {
                return Ok(new { msg = "Tên tài khoản không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                return Ok(new { msg = "Mật khẩu không được để trống", success = false });
            }

            var check_name = await db.TaiKhoans.FirstOrDefaultAsync(x => x.TenTaiKhoan == tk.TenTaiKhoan && x.IdTaiKhoan != id);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có tài khoản này rồi", success = false });
            }

            if (string.IsNullOrEmpty(tk.MatKhau))
            {
                return Ok(new { msg = "không được bỏ trống mật khẩu", success = false });
            }

            if (string.IsNullOrEmpty(tk.TenTaiKhoan))
            {
                return Ok(new { msg = "không được bỏ trống tên tài khoản", success = false });
            }
            check_id.TenTaiKhoan = tk.TenTaiKhoan;
            check_id.MatKhau = tk.MatKhau;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-taikhoan")]
        public async Task<IActionResult> DeleteDonViTinh(int id)
        {
            var check_id = await db.TaiKhoans.FirstOrDefaultAsync(x => x.IdTaiKhoan == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có tài khoản này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }
    }
}
