using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs;
using QLCHOTO.Models.DTOs.SanPham;
using System.Timers;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public SanPhamController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpPost]
        [Route("get-all-sanpham")]
        public async Task<IActionResult> GetAllSP(//PaginationClass sr)
                                                 )
        {
            var query = db.SanPhams.AsQueryable();
            //if (!string.IsNullOrEmpty(sr.SearchTerm))
            //{
            //    query = query.Where(x => x.TenSp.Contains(sr.SearchTerm) ||
            //                             x.MaSp.Contains(sr.SearchTerm) ||
            //                             x.MoTa.Contains(sr.SearchTerm) ||
            //                             x.TrangThai.Contains(sr.SearchTerm) //||
            //                             //x.SoLuong.Value.ToString().Contains(sr.SearchTerm) ||
            //                             //x.GiaBan.V.ToString().Contains(sr.SearchTerm) ||                   tìm kiếm kiểu số sao, hay là làm tìm kiếm riêng cho kiểu số
            //                             //x.GiaNhap.ToString().Contains(sr.SearchTerm)
            //                             );
            //}

            //if (!query.Any())
            //{
            //    return Ok(new { msg = "không tìm thấy dữ liệu", success = false });
            //}
            var sp = await db.SanPhams
                .Select(x => new
                {
                    x.IdSanPham,
                    x.MaSp,
                    x.TenSp,
                    x.MoTa,
                    x.SoLuong,
                    x.IdDvtNavigation.TenDvt,
                    x.GiaNhap,
                    x.GiaBan,
                    x.TrangThai,
                    x.IdNhomSanPhamNavigation.TenNhomSanPham
                }).ToListAsync();
            return Ok(sp);
        }

        [HttpPost]
        [Route("create-sanpham")]
        public async Task<IActionResult> CreateSP([FromBody] CreateSanPham sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSp))
            {
                return Ok(new { msg = "mã sản phẩm không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(sp.TenSp))
            {
                return Ok(new { msg = "Tên sản phẩm không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(sp.MoTa))
            {
                return Ok(new { msg = "Mô tả sản phẩm không được để trống", success = false });
            }

            var check_ma = await db.SanPhams.FirstOrDefaultAsync(x => x.MaSp == sp.MaSp);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có mã sản phẩm này rồi", success = false });
            }

            var check_ten = await db.SanPhams.FirstOrDefaultAsync(x => x.TenSp == sp.TenSp);
            if (check_ten != null)
            {
                return Ok(new { msg = "đã có tên sản phẩm này rồi", success = false });
            }

            var newsp = new SanPham
            {
                MaSp = sp.MaSp,
                TenSp = sp.TenSp,
                MoTa = sp.MoTa,
                SoLuong = sp.SoLuong,
                IdDvt = sp.IdDvt,
                GiaNhap = sp.GiaNhap,
                GiaBan = sp.GiaBan,
                TrangThai = sp.TrangThai,
                IdNhomSanPham = sp.IdNhomSanPham,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };

            await db.AddAsync(newsp);
            await db.SaveChangesAsync();

            return Ok(new {msg="thêm mới thành công", success = true});
        }

        [HttpPut]
        [Route("update-sanpham")]
        public async Task<IActionResult> UpdateSP(int id, [FromBody] UpdateSanPham sp)
        {
            var check_id = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            if (string.IsNullOrWhiteSpace(sp.MaSp))
            {
                return Ok(new { msg = "mã sản phẩm không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(sp.TenSp))
            {
                return Ok(new { msg = "Tên sản phẩm không được để trống", success = false });
            }

            if (string.IsNullOrWhiteSpace(sp.MoTa))
            {
                return Ok(new { msg = "Mô tả sản phẩm không được để trống", success = false });
            }

            var check_ma = await db.SanPhams.FirstOrDefaultAsync(x => x.MaSp == sp.MaSp && x.IdSanPham != id);
            if (check_ma != null)
            {
                return Ok(new { msg = "đã có mã sản phẩm này rồi", success = false });
            }

            var check_ten = await db.SanPhams.FirstOrDefaultAsync(x => x.TenSp == sp.TenSp && x.IdSanPham != id);
            if (check_ten != null)
            {
                return Ok(new { msg = "đã có tên sản phẩm này rồi", success = false });
            }

            check_id.MaSp = sp.MaSp;
            check_id.TenSp = sp.TenSp;
            check_id.MoTa = sp.MoTa;
            check_id.SoLuong = sp.SoLuong;
            check_id.IdDvt = sp.IdDvt;
            check_id.GiaNhap = sp.GiaNhap;
            check_id.GiaBan = sp.GiaBan;
            check_id.TrangThai = sp.TrangThai;
            check_id.IdNhomSanPham = sp.IdNhomSanPham;
            check_id.TimeUp = unixTimestamp;

            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("delete-sanpham")]
        public async Task<IActionResult> DeleteSP(int id)
        {
            var check_id = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == id);
            if (check_id == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            db.Remove(check_id);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}
