using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.DonViTinh;
using QLCHOTO.Models.DTOs.KhachHang;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public KhachHangController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-khachhang")]
        public async Task<IActionResult> GetAllKhachHang(//[FromBody] PaginationClass sr)
                                                        )
        {
            var query = db.KhachHangs.AsQueryable();
            //if (!string.IsNullOrEmpty(sr.SearchTerm))
            //{
            //    query = query.Where(x => x.TenKh.Contains(sr.SearchTerm) || x.Sdt.Contains(sr.SearchTerm));
            //}

            //if (!query.Any())
            //{
            //    return Ok(new { msg = "không tìm thấy", success = false });
            //}

            var kh = await query
                .Select(x => new
                {
                    x.IdKh,
                    x.TenKh,
                    x.Sdt,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(kh);
        }

        [HttpPost]
        [Route("create-khachhang")]
        public async Task<IActionResult> CreateKhachHang([FromBody] CreateKH kh)
        {
            if (string.IsNullOrWhiteSpace(kh.TenKh))
            {
                return Ok(new { msg = "Tên khách hàng không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(kh.Sdt))
            {
                return Ok(new { msg = "Số điện thoại không được để trống", success = false });
            }

            var check_sdt = await db.KhachHangs.FirstOrDefaultAsync(x => x.Sdt == kh.Sdt);
            if (check_sdt != null)
            {
                return Ok(new { msg = "đã có số điện thoại này rồi", success = false });
            }

            var newkh = new KhachHang
            {
                TenKh = kh.TenKh,
                Sdt = kh.Sdt,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newkh);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-khachhang")]
        public async Task<IActionResult> UpdateKhachHang(int id,[FromBody] UpdateKH kh)
        {
            var check_id = await db.KhachHangs.FirstOrDefaultAsync(x => x.IdKh == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có khách hàng này", success = false });
            }

            if (string.IsNullOrWhiteSpace(kh.TenKh))
            {
                return Ok(new { msg = "Tên khách hàng không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(kh.Sdt))
            {
                return Ok(new { msg = "Số điện thoại không được để trống", success = false });
            }

            var check_sdt = await db.KhachHangs.FirstOrDefaultAsync(x => x.Sdt == kh.Sdt && x.IdKh != id);
            if (check_sdt != null)
            {
                return Ok(new { msg = "đã có số điện thoại này rồi", success = false });
            }

            check_id.TenKh = kh.TenKh;
            check_id.Sdt = kh.Sdt;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-khachhang")]
        public async Task<IActionResult> DeleteKhachHang(int id)
        {
            var check_id = await db.KhachHangs.FirstOrDefaultAsync(x => x.IdKh == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có khách hàng này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }
    }
}
