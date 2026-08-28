using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QLCHOTO.Models;

namespace QLCHOTO.Data;

public partial class QLCHOtoDbContext : DbContext
{
    public QLCHOtoDbContext(DbContextOptions<QLCHOtoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CapQuyenTaiKhoan> CapQuyenTaiKhoans { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietHoaDonTra> ChiTietHoaDonTras { get; set; }

    public virtual DbSet<DonViTinh> DonViTinhs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HoaDonTra> HoaDonTras { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiTaiKhoan> LoaiTaiKhoans { get; set; }

    public virtual DbSet<MuonTraSanPham> MuonTraSanPhams { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhomSanPham> NhomSanPhams { get; set; }

    public virtual DbSet<QuanLyBaoHanh> QuanLyBaoHanhs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CapQuyenTaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdCapQuyenTk);

            entity.ToTable("cap_quyen_tai_khoan");

            entity.Property(e => e.IdCapQuyenTk).HasColumnName("Id_cap_quyen_tk");
            entity.Property(e => e.IdLoaiTaiKhoan).HasColumnName("Id_loai_tai_khoan");
            entity.Property(e => e.IdTaiKhoan).HasColumnName("Id_tai_khoan");

            entity.HasOne(d => d.IdLoaiTaiKhoanNavigation).WithMany(p => p.CapQuyenTaiKhoans)
                .HasForeignKey(d => d.IdLoaiTaiKhoan)
                .HasConstraintName("FK_cap_quyen_tai_khoan_loai_tai_khoan");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithMany(p => p.CapQuyenTaiKhoans)
                .HasForeignKey(d => d.IdTaiKhoan)
                .HasConstraintName("FK_cap_quyen_tai_khoan_tai_khoan");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.IdChiTietHoaDon);

            entity.ToTable("chi_tiet_hoa_don");

            entity.Property(e => e.IdChiTietHoaDon).HasColumnName("Id_chi_tiet_hoa_don");
            entity.Property(e => e.IdHoaDon).HasColumnName("Id_hoa_don");
            entity.Property(e => e.IdSp).HasColumnName("Id_SP");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");

            entity.HasOne(d => d.IdHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.IdHoaDon)
                .HasConstraintName("FK_chi_tiet_hoa_don_hoa_don");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.IdSp)
                .HasConstraintName("FK_chi_tiet_hoa_don_san_pham");
        });

        modelBuilder.Entity<ChiTietHoaDonTra>(entity =>
        {
            entity.HasKey(e => e.IdChiTietHoaDonTra);

            entity.ToTable("chi_tiet_hoa_don_tra");

            entity.Property(e => e.IdChiTietHoaDonTra).HasColumnName("Id_chi_tiet_hoa_don_tra");
            entity.Property(e => e.IdHdTra).HasColumnName("Id_HD_tra");
            entity.Property(e => e.IdSp).HasColumnName("Id_SP");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");

            entity.HasOne(d => d.IdHdTraNavigation).WithMany(p => p.ChiTietHoaDonTras)
                .HasForeignKey(d => d.IdHdTra)
                .HasConstraintName("FK_chi_tiet_hoa_don_tra_hoa_don_tra");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.ChiTietHoaDonTras)
                .HasForeignKey(d => d.IdSp)
                .HasConstraintName("FK_chi_tiet_hoa_don_tra_san_pham");
        });

        modelBuilder.Entity<DonViTinh>(entity =>
        {
            entity.ToTable("don_vi_tinh");

            entity.Property(e => e.TenDvt)
                .HasMaxLength(50)
                .HasColumnName("TenDVT");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("TIme_Up");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.IdHoaDon);

            entity.ToTable("hoa_don");

            entity.Property(e => e.IdHoaDon).HasColumnName("Id_hoa_don");
            entity.Property(e => e.IdKh).HasColumnName("Id_KH");
            entity.Property(e => e.IdNhaCungCap).HasColumnName("Id_nha_cung_cap");
            entity.Property(e => e.IdNv).HasColumnName("Id_NV");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdKh)
                .HasConstraintName("FK_hoa_don_khach_hang");

            entity.HasOne(d => d.IdNhaCungCapNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdNhaCungCap)
                .HasConstraintName("FK_hoa_don_nha_cung_cap");

            entity.HasOne(d => d.IdNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IdNv)
                .HasConstraintName("FK_hoa_don_nhan_vien");
        });

        modelBuilder.Entity<HoaDonTra>(entity =>
        {
            entity.HasKey(e => e.IdHdTra);

            entity.ToTable("hoa_don_tra");

            entity.Property(e => e.IdHdTra).HasColumnName("Id_HD_tra");
            entity.Property(e => e.IdHd).HasColumnName("Id_HD");
            entity.Property(e => e.MaHdTra)
                .HasMaxLength(50)
                .HasColumnName("MaHD_tra");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");

            entity.HasOne(d => d.IdHdNavigation).WithMany(p => p.HoaDonTras)
                .HasForeignKey(d => d.IdHd)
                .HasConstraintName("FK_hoa_don_tra_hoa_don");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.IdKh);

            entity.ToTable("khach_hang");

            entity.Property(e => e.IdKh).HasColumnName("Id_kh");
            entity.Property(e => e.Sdt).HasMaxLength(50);
            entity.Property(e => e.TenKh)
                .HasMaxLength(50)
                .HasColumnName("TenKH");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
        });

        modelBuilder.Entity<LoaiTaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdLoaiTaiKhoan);

            entity.ToTable("loai_tai_khoan");

            entity.Property(e => e.IdLoaiTaiKhoan).HasColumnName("Id_loai_tai_khoan");
            entity.Property(e => e.TenLoaiTaiKhoan)
                .HasMaxLength(50)
                .HasColumnName("Ten_loai_tai_khoan");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
        });

        modelBuilder.Entity<MuonTraSanPham>(entity =>
        {
            entity.HasKey(e => e.IdMuon);

            entity.ToTable("muon_tra_san_pham");

            entity.Property(e => e.IdMuon).HasColumnName("Id_muon");
            entity.Property(e => e.IdNhaCungCap).HasColumnName("Id_nha_cung_cap");
            entity.Property(e => e.IdSanPham).HasColumnName("Id_san_pham");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdNhaCungCapNavigation).WithMany(p => p.MuonTraSanPhams)
                .HasForeignKey(d => d.IdNhaCungCap)
                .HasConstraintName("FK_muon_tra_san_pham_nha_cung_cap");

            entity.HasOne(d => d.IdSanPhamNavigation).WithMany(p => p.MuonTraSanPhams)
                .HasForeignKey(d => d.IdSanPham)
                .HasConstraintName("FK_muon_tra_san_pham_san_pham");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.IdNhaCungCap);

            entity.ToTable("nha_cung_cap");

            entity.Property(e => e.IdNhaCungCap).HasColumnName("Id_nha_cung_cap");
            entity.Property(e => e.MaNhaCungCap)
                .HasMaxLength(50)
                .HasColumnName("Ma_nha_cung_cap");
            entity.Property(e => e.Sdt).HasMaxLength(50);
            entity.Property(e => e.TenNhaCungCap)
                .HasMaxLength(50)
                .HasColumnName("Ten_nha_cung_cap");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.IdNv);

            entity.ToTable("nhan_vien");

            entity.Property(e => e.IdNv).HasColumnName("Id_NV");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.NamSinh).HasMaxLength(50);
            entity.Property(e => e.Sdt).HasMaxLength(50);
            entity.Property(e => e.TenNv)
                .HasMaxLength(50)
                .HasColumnName("TenNV");
            entity.Property(e => e.ThamTien).HasMaxLength(50);
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<NhomSanPham>(entity =>
        {
            entity.HasKey(e => e.IdNhomSanPham);

            entity.ToTable("nhom_san_pham");

            entity.Property(e => e.IdNhomSanPham).HasColumnName("Id_nhom_san_pham");
            entity.Property(e => e.TenNhomSanPham)
                .HasMaxLength(50)
                .HasColumnName("Ten_nhom_san_pham");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
        });

        modelBuilder.Entity<QuanLyBaoHanh>(entity =>
        {
            entity.HasKey(e => e.IdQuanLyBaoHanh);

            entity.ToTable("quan_ly_bao_hanh");

            entity.Property(e => e.IdQuanLyBaoHanh).HasColumnName("Id_quan_ly_bao_hanh");
            entity.Property(e => e.IdKh).HasColumnName("Id_KH");
            entity.Property(e => e.IdNhaCungCap).HasColumnName("Id_nha_cung_cap");
            entity.Property(e => e.IdSp).HasColumnName("Id_SP");
            entity.Property(e => e.NgayBaoHanh).HasMaxLength(50);
            entity.Property(e => e.NgayKetThucBaoHanh).HasMaxLength(50);
            entity.Property(e => e.SoBaoHanh).HasMaxLength(50);
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdKhNavigation).WithMany(p => p.QuanLyBaoHanhs)
                .HasForeignKey(d => d.IdKh)
                .HasConstraintName("FK_quan_ly_bao_hanh_khach_hang");

            entity.HasOne(d => d.IdNhaCungCapNavigation).WithMany(p => p.QuanLyBaoHanhs)
                .HasForeignKey(d => d.IdNhaCungCap)
                .HasConstraintName("FK_quan_ly_bao_hanh_nha_cung_cap");

            entity.HasOne(d => d.IdSpNavigation).WithMany(p => p.QuanLyBaoHanhs)
                .HasForeignKey(d => d.IdSp)
                .HasConstraintName("FK_quan_ly_bao_hanh_san_pham");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.IdSanPham);

            entity.ToTable("san_pham");

            entity.Property(e => e.IdSanPham).HasColumnName("Id_san_pham");
            entity.Property(e => e.GiaBan).HasColumnName("Gia_Ban");
            entity.Property(e => e.GiaNhap).HasColumnName("Gia_Nhap");
            entity.Property(e => e.IdDvt).HasColumnName("Id_dvt");
            entity.Property(e => e.IdNhomSanPham).HasColumnName("Id_nhom_san_pham");
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .HasColumnName("MaSP");
            entity.Property(e => e.TenSp)
                .HasMaxLength(50)
                .HasColumnName("TenSP");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("Time_Up");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.IdDvtNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.IdDvt)
                .HasConstraintName("FK_san_pham_don_vi_tinh");

            entity.HasOne(d => d.IdNhomSanPhamNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.IdNhomSanPham)
                .HasConstraintName("FK_san_pham_nhom_san_pham");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdTaiKhoan);

            entity.ToTable("tai_khoan");

            entity.Property(e => e.IdTaiKhoan).HasColumnName("Id_tai_khoan");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .HasColumnName("Mat_khau");
            entity.Property(e => e.TenTaiKhoan)
                .HasMaxLength(50)
                .HasColumnName("Ten_tai_khoan");
            entity.Property(e => e.TimeCre).HasColumnName("Time_Cre");
            entity.Property(e => e.TimeUp).HasColumnName("TIme_Up");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
