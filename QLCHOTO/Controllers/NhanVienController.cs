using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.KhachHang;
using QLCHOTO.Models.DTOs.NhanVien;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public NhanVienController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-nhanvien")]
        public async Task<IActionResult> GetAllNhanVien([FromBody] PaginationClass sr)
        {
            var query = db.NhanViens.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query
                    .Where(x => x.TenNv.Contains(sr.SearchTerm) ||
                           x.MaNv.Contains(sr.SearchTerm) ||
                           x.Sdt.Contains(sr.SearchTerm) ||
                           x.NamSinh.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy", success = false });
            }

            var nv = await query
                .Select(x => new
                {
                    x.MaNv,
                    x.TenNv,
                    x.ThamNien,
                    x.Sdt,
                    x.NamSinh,
                    x.NgayVaoLam,
                    x.TrangThai,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(nv);
        }

        [HttpPost]
        [Route("create-nhanvien")]
        public async Task<IActionResult> CreateNhanVien([FromBody] CreateNV nv)
        {
            if (string.IsNullOrWhiteSpace(nv.MaNv))
            {
                return Ok(new { msg = "mã nhân viên không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.TenNv))
            {
                return Ok(new { msg = "Tên nhân viên không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.Sdt))
            {
                return Ok(new { msg = "Số điện thoại không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.NamSinh))
            {
                return Ok(new { msg = "Năm sinh không được để trống", success = false });
            }

            var check_sdt = await db.NhanViens.FirstOrDefaultAsync(x => x.Sdt == nv.Sdt);
            if (check_sdt != null)
            {
                return Ok(new { msg = "đã có số điện thoại này rồi", success = false });
            }

            var check_ma = await db.NhanViens.FirstOrDefaultAsync(x => x.MaNv == nv.MaNv);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có mã nhân viên này rồi", success = false });
            }

            var newnv = new NhanVien
            {
                MaNv = nv.MaNv,
                TenNv = nv.TenNv,
                ThamNien = nv.ThamNien,
                Sdt = nv.Sdt,
                NamSinh = nv.NamSinh,
                TrangThai = nv.TrangThai,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newnv);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-nhanvien")]
        public async Task<IActionResult> UpdateNhanVien(int id, [FromBody] UpdateNV nv)
        {
            var check_id = await db.NhanViens.FirstOrDefaultAsync(x => x.IdNv == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có nhân viên này", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.MaNv))
            {
                return Ok(new { msg = "mã nhân viên không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.TenNv))
            {
                return Ok(new { msg = "Tên nhân viên không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.Sdt))
            {
                return Ok(new { msg = "Số điện thoại không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(nv.NamSinh))
            {
                return Ok(new { msg = "Năm sinh không được để trống", success = false });
            }

            var check_sdt = await db.NhanViens.FirstOrDefaultAsync(x => x.Sdt == nv.Sdt && x.IdNv != id);
            if (check_sdt != null)
            {
                return Ok(new { msg = "đã có số điện thoại này rồi", success = false });
            }

            var check_ma = await db.NhanViens.FirstOrDefaultAsync(x => x.MaNv == nv.MaNv && x.IdNv != id);
            if (check_ma!= null)
            {
                return Ok(new { msg = "đã có số mã nhân viên này rồi", success = false });
            }

            check_id.TenNv = nv.TenNv;
            check_id.MaNv = nv.MaNv;
            check_id.ThamNien = nv.ThamNien;
            check_id.NamSinh = nv.NamSinh;
            check_id.NgayVaoLam = nv.NgayVaoLam;
            check_id.TrangThai = nv.TrangThai;
            check_id.Sdt = nv.Sdt;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-nhanvien")]
        public async Task<IActionResult> DeleteNhanVien(int id)
        {
            var check_id = await db.NhanViens.FirstOrDefaultAsync(x => x.IdNv == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có nhân viên này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }
    }
}
