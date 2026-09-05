using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.DonViTinh;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonViTinhController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public DonViTinhController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-donvitinh")]
        public async Task<IActionResult> GetAllDonViTinh(//[FromBody] PaginationClass sr
                                                        )
        {
            var query = db.DonViTinhs.AsQueryable();
            //if (!string.IsNullOrEmpty(sr.SearchTerm))
            //{
            //    query = query.Where(x => x.TenDvt.Contains(sr.SearchTerm));
            //}

            //if (!query.Any())
            //{
            //    return Ok(new { msg = "không tìm thấy đơn vị tính đó", success = false });
            //}

            var dvt = await query
                .Select(x => new
                {
                    x.Id,
                    x.TenDvt,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(dvt);
        }

        [HttpPost]
        [Route("create-donvitinh")]
        public async Task<IActionResult> CreateDonViTinh([FromBody] CreateDonViTinh dvt)
        {
            if (string.IsNullOrWhiteSpace(dvt.TenDvt))
            {
                return Ok(new { msg = "Tên đơn vị không được để trống", success = false });
            }

            var check_name = await db.DonViTinhs.FirstOrDefaultAsync(x => x.TenDvt == dvt.TenDvt);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có đơn vị này rồi", success = false });
            }

            var newdonvitinh = new DonViTinh
            {
                TenDvt = dvt.TenDvt,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newdonvitinh);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-donvitinh")]
        public async Task<IActionResult> UpdateDonViTinh(int id,[FromBody] UpdateDonViTinh dvt)
        {
            var check_id = await db.DonViTinhs.FirstOrDefaultAsync(x => x.Id == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có đơn vị này", success = false });
            }

            if (string.IsNullOrWhiteSpace(dvt.TenDvt))
            {
                return Ok(new { msg = "Tên đơn vị không được để trống", success = false });
            }

            var check_name = await db.DonViTinhs.FirstOrDefaultAsync(x => x.TenDvt == dvt.TenDvt && x.Id != id);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có đơn vị này rồi", success = false });
            }

            check_id.TenDvt = dvt.TenDvt;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-donvitinh")]
        public async Task<IActionResult> DeleteDonViTinh(int id)
        {
            var check_id = await db.DonViTinhs.FirstOrDefaultAsync(x => x.Id == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có đơn vị này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }

    }
}
