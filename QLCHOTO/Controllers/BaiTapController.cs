using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Data;
using QLCHOTO.Models;
using QLCHOTO.Models.DTOs.HoaDon;
using QLCHOTO.Models.DTOs.QuanLyBaoHanh;
using System.Text.Json;

namespace QLCHOTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaiTapController : ControllerBase
    {
        private readonly QLCHOtoDbContext db;
        private readonly int unixTimestamp;

        public BaiTapController(QLCHOtoDbContext _db)
        {
            this.db = _db;
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpGet]
        [Route("bài tập 1")]
        public async Task<IActionResult> Baitap1()
        {
            var sp = await db.NhomSanPhams
                .Select(x => new
                {
                    x.IdNhomSanPham,
                    x.TenNhomSanPham,
                }).ToListAsync();
            var newlist = new List<object>();
            foreach (var item in sp)
            {
                var sp2 = await db.SanPhams
                    .Where(x => x.IdNhomSanPham == item.IdNhomSanPham)
                    .Select(x => new
                    {
                        x.MaSp,
                        x.TenSp,
                        x.MoTa,
                        x.SoLuong,
                        x.IdDvtNavigation.TenDvt,
                        x.TrangThai,
                        x.GiaNhap,
                        x.GiaBan
                    }).ToListAsync();
                newlist.Add(new
                {
                    item.TenNhomSanPham,
                    sp2
                });
            }
            return Ok(newlist);
        }

        [HttpGet]
        [Route("bài tập 2")]  // nếu mà bảng muon tra san pham vậy thì thiếu số lượng mượn
        public async Task<IActionResult> Baitap2()
        {
            var sp = await db.MuonTraSanPhams
                .Select(x => new
                {
                    x.IdSanPham,
                    x.IdNhaCungCapNavigation.TenNhaCungCap,
                    x.NgayMuon,
                    x.NgayTra,
                    x.TrangThai
                }).ToListAsync();
            var newlist = new List<object>();
            foreach (var item in sp)
            {
                var sp2 = await db.SanPhams
                    .Where(x => x.IdSanPham == item.IdSanPham)
                    .Select(x => new
                    {
                        x.MaSp,
                        x.TenSp,
                        x.MoTa
                    }).ToListAsync();
                newlist.Add(new
                {
                    item.TenNhaCungCap,
                    item.NgayMuon,
                    item.NgayTra,
                    item.TrangThai,
                    sp2
                });
            }
            return Ok(newlist);
        }

        [HttpPost]
        [Route("bài tập 3")]  // bài 3 này sai vì không nghĩ ra được làm sao để cho cái đầu vào DTOs truyền vào được 2 bảng, mà chắc tư duy code phía dưới đúng 1 phần
        public async Task<IActionResult> Baitap3([FromBody] CreateHoaDon hd)
        {
            var check_ma = await db.HoaDons.FirstOrDefaultAsync(x => x.MaHd == hd.MaHd);
            if (check_ma != null)
            {
                return Ok(new { msg = "bị trùng mã hóa đơn", success = false });
            }

            if (hd.IdKh == 0)
            {
                hd.IdKh = null;
            }

            if (hd.IdNhaCungCap == 0)
            {
                hd.IdNhaCungCap = null;
            }
            var newhd = new HoaDon
            {
                MaHd = hd.MaHd,
                IdKh = hd.IdKh,
                IdNhaCungCap = hd.IdNhaCungCap,
                IdNv = hd.IdNv,
                TienCong = hd.TienCong,
                ThanhTien = hd.ThanhTien + hd.TienCong,  // bình thường thì khi chọn sản phẩm thì giá bán của sản phẩm sẽ vào ThanhTien nhưng này cho phép
                                                         // chỉnh sữa vậy thì sao giá bán tự nhập vào được
                NgayXuatHoaDon = unixTimestamp,
                TimeCre = unixTimestamp,
                TimeUp = unixTimestamp
            };

            await db.AddAsync(newhd);
            await db.SaveChangesAsync();

            var ct = await db.ChiTietHoaDons.FirstOrDefaultAsync(x => x.IdSp == hd.IdSp);
            if (ct == null)
            {
                return Ok(new { msg = "không có id sản phẩm đó", success = false });
            }

            var sp = await db.SanPhams.FirstOrDefaultAsync(x => x.IdSanPham == hd.IdSp);
            if (sp == null)
            {
                return Ok(new { msg = "không có sản phẩm này", success = false });
            }

            sp.SoLuong = sp.SoLuong - hd.SoLuong;

            var newct = new ChiTietHoaDon
            {
                IdHoaDon = newhd.IdHoaDon,
                IdSp = hd.IdSp,
                SoLuong = hd.SoLuong
            };
            await db.AddAsync(newct);


            await db.SaveChangesAsync();
            return Ok(new { msg = "thêm mới hóa đơn thành công", success = true });
        }

        [HttpGet]
        [Route("bài tập 4")]
        public async Task<IActionResult> Baitap4()
        {
            var hd = await db.HoaDons
                .Select(x => new
                {
                    x.IdHoaDon,
                    x.MaHd,
                    x.IdKh,
                    x.IdNhaCungCap,
                    x.IdNv,
                    x.NgayXuatHoaDon,
                    x.TienCong,
                    x.ThanhTien
                }).ToListAsync();

            var list = new List<object>();
            foreach (var item in hd)
            {
                var ct = await db.ChiTietHoaDons
                    .Where(x => x.IdHoaDon == item.IdHoaDon)
                    .Select(x => new
                    {
                        x.IdSpNavigation.TenSp,
                        x.IdSpNavigation.GiaBan,
                        Soluong = x.SoLuong,
                        Donvi = x.IdSpNavigation.IdDvtNavigation.TenDvt,
                        tongtien = x.SoLuong * x.IdSpNavigation.GiaBan
                    }).ToListAsync();
                var tien = ct.Sum(x => x.tongtien);
                list.Add(new
                {
                    item.MaHd,
                    sp = ct,
                    tongtienhoadon = tien + item.TienCong
                });
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("bài tập 5")]
        public async Task<IActionResult> Baitap5([FromBody] PaginationClass sr)
        {
            var query = db.QuanLyBaoHanhs.AsQueryable();
            if (!string.IsNullOrEmpty(sr.SearchTerm))
            {
                query = query.Where(x => x.SoBaoHanh.Contains(sr.SearchTerm));
            }

            if (!query.Any())
            {
                return Ok(new { msg = "không tìm  thấy mã bảo hành đó", success = false });
            }

            var bh = await query
                .Select(x => new
                {
                    x.SoBaoHanh,
                    x.IdKhNavigation.TenKh,                 // nếu có khách hàng thì nha cung cap null và ngược lại, vậy thì làm sao để cái null đó
                                                            // không hiện ra, hay API nó vẫn hiện mà bên frontend không cho hiện
                    x.IdNhaCungCapNavigation.TenNhaCungCap,
                    x.IdSpNavigation.TenSp,
                    x.NgayBaoHanh,
                    x.NgayKetThucBaoHanh,
                    x.TrangThai           // cái trang thai này làm cách nào để tự động cập nhật khi hết bảo hành thành "hết hạn bảo hành"

                })
                .ToListAsync();

            return Ok(bh);
        }

        [HttpGet]
        [Route("bài tập 6")]        // hình như bảng Hoadon thiếu cái mô tả cho tiền công, ví dụ như lắp rắp cái gì, thay cái gì, sữa cái gì
        public async Task<IActionResult> Baitap6()
        {
            var hd = await db.HoaDons
                .Select(x => new
                {
                    x.IdHoaDon,
                    x.MaHd,
                    x.IdKhNavigation.TenKh,
                    x.IdNhaCungCapNavigation.TenNhaCungCap,
                    x.IdNvNavigation.TenNv,
                    x.NgayXuatHoaDon,
                    x.TienCong,
                    x.ThanhTien
                }).ToListAsync();

            var list = new List<object>();

            foreach (var item in hd)
            {
                var ct = await db.ChiTietHoaDons
                    .Where(x => x.IdHoaDon == item.IdHoaDon)
                    .Select(x => new
                    {
                        TenSanPham = x.IdSpNavigation.TenSp,
                        SoLuongMua = x.SoLuong,
                        DonVi = x.IdSpNavigation.IdDvtNavigation.TenDvt
                    }).ToListAsync();

                var hdtr = await db.HoaDonTras
                        .Where(x => x.IdHd == item.IdHoaDon)
                        .Select(x => new
                        {
                            x.IdHdTra,
                            x.MaHdTra,
                            x.NgayXuatHoaDon,
                            x.TienCong,
                            x.ThanhTien,
                            x.IdHdNavigation.MaHd
                        }).ToListAsync();

                var list2 = new List<object>();
                foreach (var item2 in hdtr)
                {
                    var cttr = await db.ChiTietHoaDonTras
                        .Where(x => x.IdHdTra == item2.IdHdTra)
                        .Select(x => new
                        {
                            TenSanPhamTra = x.IdSpNavigation.TenSp,          // cái idsp của chitiethoadon trả này nối với idsp của bảng chitiethoadon gốc
                                                                             // hay idsp của bảng sản phẩm
                            SoLuongTra = x.SoLuong,
                            DonVi = x.IdSpNavigation.IdDvtNavigation.TenDvt
                        }).ToListAsync();
                    list2.Add(new
                    {
                        MaHDTra = item2.MaHdTra,
                        NgayXuatHoaDonTra = item2.NgayXuatHoaDon,
                        TienCongHoaDonTra = item2.TienCong,
                        TienTraLaiCuaHoaDonTra = item2.ThanhTien,
                        cttr
                    });
                }
                list.Add(new
                {
                    MaHoaDon = item.MaHd,
                    TenKhachHang = item.TenKh,
                    TenNhaCungCap = item.TenNhaCungCap,
                    NgayXuatHoaDonGoc = item.NgayXuatHoaDon,
                    TienCongHoaDonGoc = item.TienCong,
                    TienHoaDonGoc = item.ThanhTien,
                    ChiTietHoaDon = ct,
                    HoaDonTra = list2
                });
            }
            return Ok(list);
        }
    }
}

