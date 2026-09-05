using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.NhaCungCap;
using QLCHOTO.Models.DTOs.NhanVien;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public NhaCungCapController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-nhacungcap")]
        public async Task<IActionResult> GetAllNhaCungCap([FromBody] PaginationClass sr)
        {
            var query = db.NhaCungCaps.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query
                    .Where(x => x.TenNhaCungCap.Contains(sr.SearchTerm) ||
                           x.DiaChi.Contains(sr.SearchTerm) ||
                           x.Sdt.Contains(sr.SearchTerm) ||
                           x.MaNhaCungCap.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm thấy", success = false });
            }

            var ncc = await query
                .Select(x => new
                {
                    x.MaNhaCungCap,
                    x.TenNhaCungCap,
                    x.DiaChi,
                    x.Sdt,
                    x.NgayLienKet,
                    x.TrangThai,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(ncc);
        }

        [HttpPost]
        [Route("create-nhacungcap")]
        public async Task<IActionResult> CreateNhaCungCap([FromBody] CreateNhaCungCap ncc)
        {
            if (string.IsNullOrWhiteSpace(ncc.Sdt))
            {
                return Ok(new { msg = "Số điện thoại không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.MaNhaCungCap))
            {
                return Ok(new { msg = "mã nhà cung cấp không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.TenNhaCungCap))
            {
                return Ok(new { msg = "Tên nhà cung cấp không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.DiaChi))
            {
                return Ok(new { msg = "Địa chỉ không được để trống", success = false });
            }

            var check_sdt = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.Sdt == ncc.Sdt);
            if (check_sdt != null)
            {
                return Ok(new { msg = "đã có số điện thoại này rồi", success = false });
            }

            var check_ma = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.MaNhaCungCap == ncc.MaNhaCungCap);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có mã nhà cung cấp này rồi", success = false });
            }

            var newnv = new NhaCungCap
            {
                MaNhaCungCap = ncc.MaNhaCungCap,
                TenNhaCungCap = ncc.TenNhaCungCap,
                DiaChi = ncc.DiaChi,
                Sdt = ncc.Sdt,
                NgayLienKet = ncc.NgayLienKet,
                TrangThai = ncc.TrangThai,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newnv);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-nhacungcap")]
        public async Task<IActionResult> UpdateNhaCungCapn(int id, [FromBody] UpdateNhaCungCapcs ncc)
        {
            var check_id = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.IdNhaCungCap == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có nhà cung cấp này", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.Sdt))
            {
                return Ok(new { msg = "Số điện thoại không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.MaNhaCungCap))
            {
                return Ok(new { msg = "mã nhà cung cấp không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.TenNhaCungCap))
            {
                return Ok(new { msg = "Tên nhà cung cấp không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(ncc.DiaChi))
            {
                return Ok(new { msg = "Địa chỉ không được để trống", success = false });
            }

            var check_sdt = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.Sdt == ncc.Sdt && x.IdNhaCungCap != id);
            if (check_sdt != null)
            {
                return Ok(new { msg = "đã có số điện thoại này rồi", success = false });
            }

            var check_ma = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.MaNhaCungCap == ncc.MaNhaCungCap && x.IdNhaCungCap != id);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có số mã nhà cung cấp này rồi", success = false });
            }

            check_id.MaNhaCungCap = ncc.MaNhaCungCap;
            check_id.TenNhaCungCap = ncc.TenNhaCungCap;
            check_id.Sdt = ncc.Sdt;
            check_id.NgayLienKet = ncc.NgayLienKet;
            check_id.DiaChi = ncc.DiaChi;
            check_id.TrangThai = ncc.TrangThai;
            check_id.Sdt = ncc.Sdt;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-nhacungcap")]
        public async Task<IActionResult> DeleteNhanVien(int id)
        {
            var check_id = await db.NhaCungCaps.FirstOrDefaultAsync(x => x.IdNhaCungCap == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có nhà cung cấp này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }
    }
}
