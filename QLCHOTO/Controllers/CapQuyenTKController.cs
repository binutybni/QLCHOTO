using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.CapQuyenTK;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapQuyenTKController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public CapQuyenTKController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("getall-capquyentk")]
        public async Task<IActionResult> GetAllCapQuyenTK([FromBody] PaginationClass sr)
        {
            var query = db.CapQuyenTaiKhoans.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query.Where(x => x.IdLoaiTaiKhoanNavigation.TenLoaiTaiKhoan.Contains(sr.SearchTerm) ||
                                         x.IdTaiKhoanNavigation.TenTaiKhoan.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy", success = false });
            }

            var tk = await query
                .Select(x => new
                {
                    x.IdTaiKhoanNavigation.TenTaiKhoan,
                    x.IdTaiKhoanNavigation.MatKhau,
                    x.IdLoaiTaiKhoanNavigation.TenLoaiTaiKhoan,
                    x.NgayCapQuyen,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(tk);
        }

        [HttpPost]
        [Route("create-capquyentaikhoan")]
        public async Task<IActionResult> CreateCapQuyenTK(CreateCapQuyenTK tk)
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

            var check_loaitk = await db.LoaiTaiKhoans.FirstOrDefaultAsync(x => x.IdLoaiTaiKhoan == tk.IdLoaiTaiKhoan);
            if (check_loaitk == null)
            {
                return Ok(new { msg = "không có loại tài khoản này", success = false });
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

            var newcapquyentk = new CapQuyenTaiKhoan
            {
                IdTaiKhoan = newtk.IdTaiKhoan,
                IdLoaiTaiKhoan = tk.IdLoaiTaiKhoan,
                NgayCapQuyen = unixTimestamp,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newcapquyentk);
            await db.SaveChangesAsync();
            return Ok(new { msg = "thêm mới thành công", success = true });
        }

        [HttpPut]
        [Route("update-capquyentaikhoan")]
        public async Task<IActionResult> UpdateCapQuyenTK(int id, [FromBody] UpdateCapQuyenTk tk)
        {
            var check_id = await db.CapQuyenTaiKhoans.FirstOrDefaultAsync(x => x.IdCapQuyenTk == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có dữ liệu", success = false });
            }

            if (string.IsNullOrWhiteSpace(tk.TenTaiKhoan))
            {
                return Ok(new { msg = "Tên tài khoản không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                return Ok(new { msg = "Mật khẩu không được để trống", success = false });
            }

            var check_tentk = await db.TaiKhoans.FirstOrDefaultAsync(x => x.TenTaiKhoan == tk.TenTaiKhoan && x.IdTaiKhoan != check_id.IdTaiKhoan);
            if (check_tentk != null)
            {
                return Ok(new { msg = "đã có tên tài khoản này rồi", success = false });
            }

            var check_loaitk = await db.LoaiTaiKhoans.FirstOrDefaultAsync(x => x.IdLoaiTaiKhoan == tk.IdLoaiTaiKhoan);
            if (check_loaitk == null)
            {
                return Ok(new { msg = "không có loại tài khoản này", success = false });
            }

            var taiKhoan = await db.TaiKhoans.FirstOrDefaultAsync(x => x.IdTaiKhoan == check_id.IdTaiKhoan);

            taiKhoan.TenTaiKhoan = tk.TenTaiKhoan;
            taiKhoan.MatKhau = tk.MatKhau;
            check_id.IdLoaiTaiKhoan = tk.IdLoaiTaiKhoan;
            check_id.TimeUp = unixTimestamp;

            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-capquyentaikhoan")]
        public async Task<IActionResult> DeleteCapQuyenTK(int id)
        {
            var check_id = await db.CapQuyenTaiKhoans.FirstOrDefaultAsync(x => x.IdCapQuyenTk == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có dữ liệu", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "xóa thành công", success = true });
        }
    }
}
