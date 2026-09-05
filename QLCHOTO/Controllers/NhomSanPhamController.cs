using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.KhachHang;
using QLCHOTO.Models.DTOs.NhomSanPham;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhomSanPhamController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public NhomSanPhamController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-nhomsanpham")]
        public async Task<IActionResult> GetAllNhomSanPham(//[FromBody] PaginationClass sr
                                                           )
        {
            var query = db.NhomSanPhams.AsQueryable();
            //if (!string.IsNullOrEmpty(sr.SearchTerm))
            //{
            //    query = query.Where(x => x.TenNhomSanPham.Contains(sr.SearchTerm));
            //}

            //if (!query.Any())
            //{
            //    return Ok(new { msg = "không tìm thấy nhóm sản phẩm đó", success = false });
            //}

            var sp = await query
                .Select(x => new
                {
                    x.IdNhomSanPham,
                    x.TenNhomSanPham,
                    x.TimeCre,
                    x.TimeUp
                }).ToListAsync();
            return Ok(sp);
        }

        [HttpPost]
        [Route("create-nhomsanpham")]
        public async Task<IActionResult> CreateNhomSanPham([FromBody] CreateNhomSP sp)
        {
            if (string.IsNullOrWhiteSpace(sp.TenNhomSanPham))
            {
                return Ok(new { msg = "Tên nhóm sản phẩm không được để trống", success = false });
            }

            var check_name = await db.NhomSanPhams.FirstOrDefaultAsync(x => x.TenNhomSanPham == sp.TenNhomSanPham);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có nhóm sản phẩm này rồi", success = false });
            }

            var newnhomsp = new NhomSanPham
            {
                TenNhomSanPham = sp.TenNhomSanPham,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };
            await db.AddAsync(newnhomsp);
            await db.SaveChangesAsync();

            return Ok(new { msg = "thêm mới thành công", success = true });

        }

        [HttpPut]
        [Route("update-nhomsanpham")]
        public async Task<IActionResult> UpdateNhomSanPham(int id,[FromBody] UpdateNhomSP sp)
        {
            var check_id = await db.NhomSanPhams.FirstOrDefaultAsync(x => x.IdNhomSanPham == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có nhóm sản phẩm này", success = false });
            }

            if (string.IsNullOrWhiteSpace(sp.TenNhomSanPham))
            {
                return Ok(new { msg = "Tên nhóm sản phẩm không được để trống", success = false });
            }

            var check_name = await db.NhomSanPhams.FirstOrDefaultAsync(x => x.TenNhomSanPham == sp.TenNhomSanPham && x.IdNhomSanPham != id);
            if (check_name != null)
            {
                return Ok(new { msg = "đã có nhóm sản phẩm này rồi", success = false });
            }

            check_id.TenNhomSanPham = sp.TenNhomSanPham;
            check_id.TimeUp = unixTimestamp;
            await db.SaveChangesAsync();
            return Ok(new { msg = "cập nhật thành công", success = true });
        }

        [HttpDelete]
        [Route("delete-nhomsanpham")]
        public async Task<IActionResult> DeleteNhomSanPham(int id)
        {
            var check_id = await db.NhomSanPhams.FirstOrDefaultAsync(x => x.IdNhomSanPham == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có nhóm sản phẩm này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok(new { msg = "đã xóa thành công", success = true });
        }
    }
}
