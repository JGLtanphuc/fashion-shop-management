-- 1. TẠO DATABASE
CREATE DATABASE QuanLyBanHang_GJGL
GO
USE QuanLyBanHang_GJGL
GO

-- =============================================
-- 2. TẠO CÁC BẢNG (TABLES)
-- =============================================

-- Bảng 1: THỂ LOẠI (Khớp với Menu 'Thể loại')
CREATE TABLE TheLoai (
    MaTheLoai char(10) PRIMARY KEY,
    TenTheLoai NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255)
);

-- Bảng 2: NHÀ CUNG CẤP (Khớp với Menu 'Nhà cung cấp')
CREATE TABLE NhaCungCap (
    MaNCC char(10) PRIMARY KEY,
    TenNCC NVARCHAR(150) NOT NULL,
    SDT VARCHAR(20),
    DiaChi NVARCHAR(255),
    Email VARCHAR(100)
);

-- Bảng 3: SẢN PHẨM (Khớp với Menu 'Sản phẩm')
CREATE TABLE SanPham (
    MaSP char(10) PRIMARY KEY,
    MaTheLoai char(10) NOT NULL,
    TenSP NVARCHAR(255) NOT NULL,
    ThuongHieu NVARCHAR(100),
    MauSac NVARCHAR(50),
    ChatLieu NVARCHAR(100),
    XuatXu NVARCHAR(100),
    GiaBan DECIMAL(18,2) NOT NULL DEFAULT 0, -- Giá bán ra
    SoLuongTon INT NOT NULL DEFAULT 0 CHECK(SoLuongTon >= 0), -- Số lượng trong kho (tự động đồng bộ từ LoHang)
   
    FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai)
);

-- Bảng 4: KHÁCH HÀNG (Khớp với Menu 'Khách hàng')
CREATE TABLE KhachHang (
    MaKH char(10) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT VARCHAR(20),
    DiaChi NVARCHAR(255)
);

-- Bảng 5: NHÂN VIÊN (Thông tin hồ sơ nhân viên)
CREATE TABLE NhanVien (
    MaNV char(10) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT VARCHAR(20),
    DiaChi NVARCHAR(255),
    Email VARCHAR(100),
    ChucVu NVARCHAR(50) -- Ví dụ: Quản lý, Nhân viên bán hàng
);

-- Bảng 6: TÀI KHOẢN (Dùng để Đăng Nhập - Tách riêng theo yêu cầu)
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(20) DEFAULT N'Nhân viên', -- 'Admin' hoặc 'Nhân viên'
    MaNV char(10) NOT NULL, -- Liên kết với nhân viên nào
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- Bảng 7: ĐƠN HÀNG (Hóa đơn bán ra)
CREATE TABLE DonHang (
    MaDH char(10) PRIMARY KEY,
    MaKH char(10), -- Có thể NULL nếu khách vãng lai
    MaNV char(10), -- Nhân viên lập đơn
    NgayLap DATETIME NOT NULL DEFAULT(GETDATE()),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    DiaChiGiaoHang NVARCHAR(255),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- Bảng 8: CHI TIẾT ĐƠN HÀNG (Sản phẩm trong đơn hàng)
CREATE TABLE ChiTietDonHang (
    MaDH char(10) NOT NULL,
    MaSP char(10) NOT NULL,
    TenSP nvarchar(255), -- Tăng lên 255 ký tự
    SoLuong INT NOT NULL CHECK(SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL, -- Giá tại thời điểm bán
    ThanhTien DECIMAL(18,2),
    PRIMARY KEY (MaDH, MaSP),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- Bảng 9: PHIẾU NHẬP (Thông tin chung về lần nhập hàng)
CREATE TABLE PhieuNhap (
    MaPN char(10) PRIMARY KEY,
    MaNCC char(10) NOT NULL, -- Nhà cung cấp sản phẩm này
    MaNV char(10) NOT NULL, -- Nhân viên lập phiếu nhập
    NgayNhap DATETIME NOT NULL DEFAULT(GETDATE()),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0, -- Tổng tiền nhập
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- Bảng 10: CHI TIẾT PHIẾU NHẬP (Các sản phẩm trong Phiếu Nhập)
CREATE TABLE ChiTietPhieuNhap (
    MaPN char(10) NOT NULL,
    MaSP char(10) NOT NULL,
    SoLuongNhap INT NOT NULL CHECK(SoLuongNhap > 0),
    GiaNhap DECIMAL(18,2) NOT NULL CHECK(GiaNhap > 0), -- Giá vốn tại thời điểm nhập
    ThanhTien DECIMAL(18,2), -- SoLuongNhap * GiaNhap
    PRIMARY KEY (MaPN, MaSP),
    FOREIGN KEY (MaPN) REFERENCES PhieuNhap(MaPN),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

-- Bảng 11: LÔ HÀNG
CREATE TABLE LoHang (
    MaLo INT IDENTITY(1,1) PRIMARY KEY,
    MaSP CHAR(10) NOT NULL,
    MaPN CHAR(10) NOT NULL,
    SoLuongNhap INT NOT NULL CHECK(SoLuongNhap > 0),
    SoLuongConLai INT NOT NULL CHECK(SoLuongConLai >= 0) DEFAULT 0,
    GiaNhap DECIMAL(18,2) NOT NULL CHECK(GiaNhap > 0),
    NgayNhap DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaPN) REFERENCES PhieuNhap(MaPN),
    CHECK (SoLuongConLai <= SoLuongNhap)
);

-- Bảng 12: CHI TIẾT BÁN THEO LÔ
CREATE TABLE ChiTietBanTheoLo (
    MaCT INT IDENTITY(1,1) PRIMARY KEY,
    MaDH CHAR(10) NOT NULL,
    MaSP CHAR(10) NOT NULL,
    MaLo INT NOT NULL,
    SoLuongBan INT NOT NULL CHECK(SoLuongBan > 0),
    GiaBanThucTe DECIMAL(18,2) NOT NULL,
    NgayBan DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP),
    FOREIGN KEY (MaLo) REFERENCES LoHang(MaLo)
);

GO

-- =============================================
-- 3. TẠO CÁC TRIGGER (ĐÃ SỬA HOÀN CHỈNH)
-- =============================================

-- ============ QUAN TRỌNG: CHẶN UPDATE/INSERT THỦ CÔNG SỐ LƯỢNG TỒN ============

-- Trigger CHẶN INSERT thủ công SoLuongTon
-- Thêm DEFAULT CONSTRAINT để LUÔN đặt SoLuongTon = 0 khi insert
ALTER TABLE SanPham
ADD CONSTRAINT DF_SanPham_SoLuongTon DEFAULT 0 FOR SoLuongTon;
GO

-- ============ TRIGGER TỰ ĐỘNG ĐỒNG BỘ KHO ============

-- Trigger 1: TỰ ĐỘNG TẠO LÔ HÀNG KHI NHẬP KHO
CREATE TRIGGER trg_TaoLoHangKhiNhap
ON ChiTietPhieuNhap
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- 1. TẠO LÔ HÀNG MỚI
    INSERT INTO LoHang (MaSP, MaPN, SoLuongNhap, SoLuongConLai, GiaNhap, NgayNhap)
    SELECT 
        i.MaSP,
        i.MaPN,
        i.SoLuongNhap,
        i.SoLuongNhap, -- Lúc mới nhập, số lượng còn lại = số lượng nhập
        i.GiaNhap,
        pn.NgayNhap
    FROM inserted i
    JOIN PhieuNhap pn ON i.MaPN = pn.MaPN;
    
    -- 2. TỰ ĐỘNG ĐỒNG BỘ SỐ LƯỢNG TỒN (trigger trên LoHang sẽ chạy tiếp)
    PRINT 'Đã tạo lô hàng mới. Số lượng tồn sẽ được đồng bộ tự động.';
END;
GO

-- Trigger 2: TỰ ĐỘNG ĐỒNG BỘ SỐ LƯỢNG TỒN TỪ LoHang -> SanPham
CREATE TRIGGER trg_DongBoTonKhoTuLoHang
ON LoHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Lấy danh sách sản phẩm bị ảnh hưởng
    DECLARE @SanPhamThayDoi TABLE (MaSP CHAR(10));
    
    INSERT INTO @SanPhamThayDoi (MaSP)
    SELECT DISTINCT MaSP FROM (
        SELECT MaSP FROM inserted
        UNION
        SELECT MaSP FROM deleted
    ) AS ChangedProducts;
    
    -- Cập nhật SoLuongTon trong SanPham từ tổng SoLuongConLai trong LoHang
    UPDATE sp
    SET SoLuongTon = ISNULL((
        SELECT SUM(SoLuongConLai)
        FROM LoHang lh
        WHERE lh.MaSP = sp.MaSP
    ), 0)
    FROM SanPham sp
    WHERE sp.MaSP IN (SELECT MaSP FROM @SanPhamThayDoi);
    
    PRINT 'Đã đồng bộ số lượng tồn từ LoHang cho ' + CAST(@@ROWCOUNT AS VARCHAR) + ' sản phẩm';
END;
GO

-- Trigger 3: TỰ ĐỘNG PHÂN BỔ LÔ KHI BÁN HÀNG (FIFO)
CREATE TRIGGER trg_PhanBoLoKhiBan
ON ChiTietDonHang
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @MaDH CHAR(10), @MaSP CHAR(10), @SoLuongBan INT, @GiaBan DECIMAL(18,2);
    DECLARE @MaLo INT, @SoLuongConLai INT, @GiaNhap DECIMAL(18,2);
    DECLARE @SoLuongCanLay INT;
    DECLARE @NgayBan DATETIME;
    
    -- Duyệt qua tất cả các sản phẩm vừa được bán
    DECLARE curInserted CURSOR FOR
    SELECT i.MaDH, i.MaSP, i.SoLuong, i.DonGia
    FROM inserted i;
    
    OPEN curInserted;
    FETCH NEXT FROM curInserted INTO @MaDH, @MaSP, @SoLuongBan, @GiaBan;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Lấy ngày của đơn hàng
        SELECT @NgayBan = NgayLap 
        FROM DonHang 
        WHERE MaDH = @MaDH;
        
        -- Phân bổ theo FIFO (lô cũ nhất trước)
        DECLARE curLoHang CURSOR FOR
        SELECT MaLo, SoLuongConLai, GiaNhap
        FROM LoHang
        WHERE MaSP = @MaSP AND SoLuongConLai > 0
        ORDER BY NgayNhap; -- FIFO: lô cũ nhất trước
        
        OPEN curLoHang;
        FETCH NEXT FROM curLoHang INTO @MaLo, @SoLuongConLai, @GiaNhap;
        
        WHILE @SoLuongBan > 0 AND @@FETCH_STATUS = 0
        BEGIN
            -- Tính số lượng lấy từ lô này
            SET @SoLuongCanLay = CASE 
                WHEN @SoLuongConLai >= @SoLuongBan THEN @SoLuongBan
                ELSE @SoLuongConLai
            END;
            
            -- Ghi nhận vào bảng ChiTietBanTheoLo
            INSERT INTO ChiTietBanTheoLo (MaDH, MaSP, MaLo, SoLuongBan, GiaBanThucTe, NgayBan)
            VALUES (@MaDH, @MaSP, @MaLo, @SoLuongCanLay, @GiaBan, ISNULL(@NgayBan, GETDATE()));
            
            -- Cập nhật số lượng còn lại trong lô
            UPDATE LoHang
            SET SoLuongConLai = SoLuongConLai - @SoLuongCanLay
            WHERE MaLo = @MaLo;
            
            -- Giảm số lượng cần bán
            SET @SoLuongBan = @SoLuongBan - @SoLuongCanLay;
            
            FETCH NEXT FROM curLoHang INTO @MaLo, @SoLuongConLai, @GiaNhap;
        END
        
        CLOSE curLoHang;
        DEALLOCATE curLoHang;
        
        FETCH NEXT FROM curInserted INTO @MaDH, @MaSP, @SoLuongBan, @GiaBan;
    END
    
    CLOSE curInserted;
    DEALLOCATE curInserted;
    
    PRINT 'Đã phân bổ lô hàng theo FIFO cho ' + CAST(@@ROWCOUNT AS VARCHAR) + ' sản phẩm đã bán';
END;
GO

-- Trigger 4: TỰ ĐỘNG CẬP NHẬT THÀNH TIỀN
CREATE TRIGGER trg_CapNhatThanhTien
ON ChiTietDonHang
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE ChiTietDonHang
    SET ThanhTien = SoLuong * DonGia
    WHERE MaDH IN (SELECT MaDH FROM inserted)
      AND MaSP IN (SELECT MaSP FROM inserted);
END;
GO

-- Trigger 5: TỰ ĐỘNG CẬP NHẬT TỔNG TIỀN ĐƠN HÀNG
CREATE TRIGGER trg_CapNhatTongTienDonHang
ON ChiTietDonHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật tổng tiền cho các đơn hàng bị ảnh hưởng
    UPDATE DonHang
    SET TongTien = ISNULL((
        SELECT SUM(ThanhTien)
        FROM ChiTietDonHang
        WHERE MaDH = DonHang.MaDH
    ), 0)
    WHERE MaDH IN (
        SELECT MaDH FROM inserted
        UNION
        SELECT MaDH FROM deleted
    );
END;
GO

-- Trigger 6: TỰ ĐỘNG CẬP NHẬT TỔNG TIỀN PHIẾU NHẬP
CREATE TRIGGER trg_CapNhatTongTienPhieuNhap
ON ChiTietPhieuNhap
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật tổng tiền phiếu nhập
    UPDATE pn
    SET TongTien = ISNULL((
        SELECT SUM(SoLuongNhap * GiaNhap)
        FROM ChiTietPhieuNhap ct
        WHERE ct.MaPN = pn.MaPN
    ), 0)
    FROM PhieuNhap pn
    WHERE pn.MaPN IN (
        SELECT MaPN FROM inserted
        UNION
        SELECT MaPN FROM deleted
    );
END;
GO

-- Trigger 7: TỰ ĐỘNG CẬP NHẬT THÀNH TIỀN PHIẾU NHẬP
CREATE TRIGGER trg_CapNhatThanhTienPhieuNhap
ON ChiTietPhieuNhap
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE ChiTietPhieuNhap
    SET ThanhTien = SoLuongNhap * GiaNhap
    WHERE MaPN IN (SELECT MaPN FROM inserted)
      AND MaSP IN (SELECT MaSP FROM inserted);
END;
GO

-- =============================================
-- 4. TẠO CÁC HÀM (FUNCTION)
-- =============================================

-- Hàm kiểm tra tồn kho theo lô
CREATE FUNCTION fn_CheckTonKhoTheoLo(@MaSP CHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @TonKho INT;
    
    SELECT @TonKho = SUM(SoLuongConLai)
    FROM LoHang
    WHERE MaSP = @MaSP;
    
    RETURN ISNULL(@TonKho, 0);
END;
GO

-- Hàm tính doanh thu theo sản phẩm
CREATE FUNCTION fn_TinhDoanhThuSP(@MaSP CHAR(10), @TuNgay DATE, @DenNgay DATE)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @DoanhThu DECIMAL(18,2);
    
    SELECT @DoanhThu = SUM(ThanhTien)
    FROM ChiTietDonHang ct
    JOIN DonHang dh ON ct.MaDH = dh.MaDH
    WHERE ct.MaSP = @MaSP 
      AND dh.NgayLap BETWEEN @TuNgay AND @DenNgay;
    
    RETURN ISNULL(@DoanhThu, 0);
END;
GO

-- =============================================
-- 5. NHẬP DỮ LIỆU MẪU (DATA TEST)
-- =============================================

-- =============================================
-- 5.1. THÊM 10 THỂ LOẠI
-- =============================================
INSERT INTO TheLoai VALUES
('ML01', N'Áo thun', N'Áo thun cổ tròn, cổ tim, polo'),
('ML02', N'Áo sơ mi', N'Sơ mi công sở, sơ mi đi biển'),
('ML03', N'Áo khoác', N'Áo gió, áo bomber, áo vest'),
('ML04', N'Quần Jean', N'Jean rách, jean trơn, slimfit'),
('ML05', N'Quần Tây', N'Quần âu văn phòng'),
('ML06', N'Quần Short', N'Quần đùi kaki, thun'),
('ML07', N'Váy đầm', N'Váy dự tiệc, váy ngủ'),
('ML08', N'Giày thể thao', N'Sneaker, giày chạy bộ'),
('ML09', N'Giày tây', N'Giày da, giày lười'),
('ML10', N'Phụ kiện', N'Thắt lưng, ví, nón, tất');

-- =============================================
-- 5.2. THÊM 10 NHÀ CUNG CẤP
-- =============================================
INSERT INTO NhaCungCap VALUES
('MCC01', N'May Mặc Việt Tiến', '0901000001', N'Tân Bình, TP.HCM', 'viettien@gmail.com'),
('MCC02', N'Thời Trang An Phước', '0901000002', N'Quận 3, TP.HCM', 'anphuoc@gmail.com'),
('MCC03', N'Giày Bitis', '0901000003', N'Quận 6, TP.HCM', 'bitis@gmail.com'),
('MCC04', N'Xưởng May Nhà Bè', '0901000004', N'Quận 7, TP.HCM', 'nhabe@gmail.com'),
('MCC05', N'Owen Fashion', '0901000005', N'Hà Nội', 'owen@gmail.com'),
('MCC06', N'Routine Store', '0901000006', N'Gò Vấp, TP.HCM', 'routine@gmail.com'),
('MCC07', N'Yame Shop', '0901000007', N'Quận 10, TP.HCM', 'yame@gmail.com'),
('MCC08', N'Coolmate', '0901000008', N'Hà Nội', 'coolmate@gmail.com'),
('MCC09', N'Ivy Moda', '0901000009', N'Đà Nẵng', 'ivymoda@gmail.com'),
('MCC10', N'Elise', '0901000010', N'Hải Phòng', 'elise@gmail.com');

-- =============================================
-- 5.3. THÊM 10 SẢN PHẨM
-- =============================================
INSERT INTO SanPham (MaSP, MaTheLoai, TenSP, ThuongHieu, MauSac, ChatLieu, XuatXu, GiaBan, SoLuongTon) VALUES
('SP01', 'ML01', N'Áo Thun Basic Trắng', N'Coolmate', N'Trắng', N'Cotton', N'Việt Nam', 150000, 100),
('SP02', 'ML02', N'Áo Polo Cá Sấu', N'Lacoste', N'Xanh', N'Thun cá sấu', N'Pháp', 500000, 80),
('SP03', 'ML03', N'Sơ Mi Trắng Công Sở', N'Việt Tiến', N'Trắng', N'Kate', N'Việt Nam', 350000, 50),
('SP04', 'ML04', N'Áo Khoác Bomber', N'Zara', N'Đen', N'Dù', N'Trung Quốc', 650000, 50),
('SP05', 'ML05', N'Quần Jean Rách Gối', N'Levis', N'Xanh nhạt', N'Jean', N'Mỹ', 850000, 40),
('SP06', 'ML06', N'Quần Tây Slimfit', N'Owen', N'Đen', N'Vải âu', N'Việt Nam', 450000, 70),
('SP07', 'ML07', N'Quần Short Kaki', N'Uniqlo', N'Be', N'Kaki', N'Nhật Bản', 300000, 90),
('SP08', 'ML08', N'Váy Hoa Nhí', N'H&M', N'Hồng', N'Voan', N'Việt Nam', 250000, 50),
('SP09', 'ML09', N'Giày Nike Air Force', N'Nike', N'Trắng', N'Da', N'Việt Nam', 2000000, 30),
('SP10', 'ML10', N'Thắt Lưng Da Bò', N'Gucci', N'Nâu', N'Da thật', N'Ý', 1200000, 20);

-- =============================================
-- 5.4. THÊM 10 NHÂN VIÊN
-- =============================================
INSERT INTO NhanVien VALUES
('MNV01', N'Nguyễn Văn An', '0988111001', N'Hà Nội', 'an.nguyen@shop.com', N'Quản lý'),
('MNV02', N'Trần Thị Bình', '0988111002', N'TP.HCM', 'binh.tran@shop.com', N'Thu ngân'),
('MNV03', N'Lê Văn Cường', '0988111003', N'Đà Nẵng', 'cuong.le@shop.com', N'Bán hàng'),
('MNV04', N'Phạm Thị Dung', '0988111004', N'Cần Thơ', 'dung.pham@shop.com', N'Bán hàng'),
('MNV05', N'Hoàng Văn Em', '0988111005', N'Hải Phòng', 'em.hoang@shop.com', N'Kho'),
('MNV06', N'Vũ Thị Phương', '0988111006', N'Nghệ An', 'phuong.vu@shop.com', N'Thu ngân'),
('MNV07', N'Đặng Văn Giang', '0988111007', N'Bình Dương', 'giang.dang@shop.com', N'Bảo vệ'),
('MNV08', N'Bùi Thị Hạnh', '0988111008', N'Đồng Nai', 'hanh.bui@shop.com', N'Kế toán'),
('MNV09', N'Đỗ Văn Hùng', '0988111009', N'Long An', 'hung.do@shop.com', N'Shipper'),
('MNV10', N'Ngô Thị Lan', '0988111010', N'Tiền Giang', 'lan.ngo@shop.com', N'Bán hàng');

-- =============================================
-- 5.5. THÊM 10 TÀI KHOẢN (Tương ứng với 10 Nhân viên trên)
-- =============================================
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Quyen, MaNV) VALUES
('admin', '123456', 'Admin', 'MNV01'),
('thungan01', '123456', 'NhanVien', 'MNV02'),
('sale01', '123456', 'NhanVien', 'MNV03'),
('sale02', '123456', 'NhanVien', 'MNV04'),
('kho01', '123456', 'NhanVien', 'MNV05'),
('thungan02', '123456', 'NhanVien', 'MNV06'),
('baove', '123456', 'NhanVien', 'MNV07'),
('ketoan', '123456', 'Admin', 'MNV08'),
('shipper', '123456', 'NhanVien', 'MNV09'),
('sale03', '123456', 'NhanVien', 'MNV10');

-- =============================================
-- 5.6. THÊM 10 KHÁCH HÀNG
-- =============================================
INSERT INTO KhachHang VALUES
('MKH01', N'Khách Hàng A', '0911222001', N'Quận 1, TP.HCM'),
('MKH02', N'Khách Hàng B', '0911222002', N'Quận 3, TP.HCM'),
('MKH03', N'Khách Hàng C', '0911222003', N'Thủ Đức'),
('MKH04', N'Khách Hàng D', '0911222004', N'Bình Thạnh'),
('MKH05', N'Khách Hàng E', '0911222005', N'Gò Vấp'),
('MKH06', N'Khách Hàng F', '0911222006', N'Hà Nội'),
('MKH07', N'Khách Hàng G', '0911222007', N'Đà Nẵng'),
('MKH08', N'Khách Hàng H', '0911222008', N'Huế'),
('MKH09', N'Khách Hàng I', '0911222009', N'Cần Thơ'),
('MKH10', N'Khách Hàng K', '0911222010', N'Hải Phòng');

-- =============================================
-- 5.7. THÊM PHIẾU NHẬP HÀNG (2022-2025)
-- =============================================

-- Phiếu nhập năm 2022
INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien) VALUES
('PN22001', 'MCC01', 'MNV05', '2022-01-15', 15000000),
('PN22002', 'MCC02', 'MNV05', '2022-03-20', 20000000),
('PN22003', 'MCC03', 'MNV05', '2022-06-10', 30000000);

INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuongNhap, GiaNhap, ThanhTien) VALUES
-- Phiếu 1
('PN22001', 'SP01', 100, 100000, 10000000),
('PN22001', 'SP02', 40, 350000, 14000000),
('PN22001', 'SP03', 50, 250000, 12500000),
-- Phiếu 2
('PN22002', 'SP04', 30, 400000, 12000000),
('PN22002', 'SP05', 20, 500000, 10000000),
('PN22002', 'SP06', 40, 300000, 12000000),
-- Phiếu 3
('PN22003', 'SP07', 60, 180000, 10800000),
('PN22003', 'SP08', 50, 150000, 7500000),
('PN22003', 'SP09', 15, 1200000, 18000000);

-- Phiếu nhập năm 2023
INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien) VALUES
('PN23001', 'MCC04', 'MNV05', '2023-02-14', 12000000),
('PN23002', 'MCC05', 'MNV05', '2023-05-18', 18000000),
('PN23003', 'MCC06', 'MNV05', '2023-08-22', 25000000);

INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuongNhap, GiaNhap, ThanhTien) VALUES
-- Phiếu 1
('PN23001', 'SP01', 80, 110000, 8800000),
('PN23001', 'SP03', 30, 260000, 7800000),
('PN23001', 'SP05', 25, 520000, 13000000),
-- Phiếu 2
('PN23002', 'SP02', 35, 360000, 12600000),
('PN23002', 'SP04', 25, 420000, 10500000),
('PN23002', 'SP06', 45, 320000, 14400000),
-- Phiếu 3
('PN23003', 'SP07', 70, 190000, 13300000),
('PN23003', 'SP08', 55, 160000, 8800000),
('PN23003', 'SP10', 20, 800000, 16000000);

-- Phiếu nhập năm 2024
INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien) VALUES
('PN24001', 'MCC07', 'MNV05', '2024-01-10', 15000000),
('PN24002', 'MCC08', 'MNV05', '2024-04-15', 22000000),
('PN24003', 'MCC09', 'MNV05', '2024-09-20', 18000000);

INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuongNhap, GiaNhap, ThanhTien) VALUES
-- Phiếu 1
('PN24001', 'SP01', 90, 120000, 10800000),
('PN24001', 'SP03', 40, 270000, 10800000),
('PN24001', 'SP09', 12, 1300000, 15600000),
-- Phiếu 2
('PN24002', 'SP02', 40, 370000, 14800000),
('PN24002', 'SP05', 30, 550000, 16500000),
('PN24002', 'SP07', 80, 200000, 16000000),
-- Phiếu 3
('PN24003', 'SP04', 35, 430000, 15050000),
('PN24003', 'SP06', 50, 330000, 16500000),
('PN24003', 'SP08', 60, 170000, 10200000);

-- Phiếu nhập năm 2025
INSERT INTO PhieuNhap (MaPN, MaNCC, MaNV, NgayNhap, TongTien) VALUES
('PN25001', 'MCC10', 'MNV05', '2025-01-05', 20000000),
('PN25002', 'MCC01', 'MNV05', '2025-03-12', 16000000),
('PN25003', 'MCC02', 'MNV05', '2025-06-08', 14000000);

INSERT INTO ChiTietPhieuNhap (MaPN, MaSP, SoLuongNhap, GiaNhap, ThanhTien) VALUES
-- Phiếu 1
('PN25001', 'SP01', 100, 130000, 13000000),
('PN25001', 'SP03', 50, 280000, 14000000),
('PN25001', 'SP10', 25, 850000, 21250000),
-- Phiếu 2
('PN25002', 'SP02', 45, 380000, 17100000),
('PN25002', 'SP05', 35, 560000, 19600000),
('PN25002', 'SP07', 90, 210000, 18900000),
-- Phiếu 3
('PN25003', 'SP04', 40, 440000, 17600000),
('PN25003', 'SP06', 55, 340000, 18700000),
('PN25003', 'SP09', 18, 1350000, 24300000);

-- =============================================
-- 5.8. THÊM ĐƠN HÀNG (2022-2025)
-- =============================================

-- Đơn hàng năm 2022
INSERT INTO DonHang (MaDH, MaKH, MaNV, NgayLap, TongTien, DiaChiGiaoHang) VALUES
('DH22001', 'MKH01', 'MNV03', '2022-02-20', 1850000, N'Quận 1, TP.HCM'),
('DH22002', 'MKH02', 'MNV04', '2022-04-15', 3200000, N'Quận 3, TP.HCM'),
('DH22003', 'MKH03', 'MNV03', '2022-07-10', 4500000, N'Thủ Đức'),
('DH22004', 'MKH04', 'MNV04', '2022-09-05', 2800000, N'Bình Thạnh'),
('DH22005', 'MKH05', 'MNV10', '2022-11-20', 6200000, N'Gò Vấp');

INSERT INTO ChiTietDonHang (MaDH, MaSP, TenSP, SoLuong, DonGia, ThanhTien) VALUES
-- Đơn 1
('DH22001', 'SP01', N'Áo Thun Basic Trắng', 3, 150000, 450000),
('DH22001', 'SP03', N'Sơ Mi Trắng Công Sở', 2, 350000, 700000),
('DH22001', 'SP07', N'Quần Short Kaki', 2, 300000, 600000),
-- Đơn 2
('DH22002', 'SP02', N'Áo Polo Cá Sấu', 1, 500000, 500000),
('DH22002', 'SP04', N'Áo Khoác Bomber', 2, 650000, 1300000),
('DH22002', 'SP06', N'Quần Tây Slimfit', 2, 450000, 900000),
('DH22002', 'SP08', N'Váy Hoa Nhí', 1, 250000, 250000),
-- Đơn 3
('DH22003', 'SP05', N'Quần Jean Rách Gối', 3, 850000, 2550000),
('DH22003', 'SP09', N'Giày Nike Air Force', 1, 2000000, 2000000),
-- Đơn 4
('DH22004', 'SP01', N'Áo Thun Basic Trắng', 5, 150000, 750000),
('DH22004', 'SP07', N'Quần Short Kaki', 4, 300000, 1200000),
('DH22004', 'SP08', N'Váy Hoa Nhí', 2, 250000, 500000),
-- Đơn 5
('DH22005', 'SP03', N'Sơ Mi Trắng Công Sở', 3, 350000, 1050000),
('DH22005', 'SP06', N'Quần Tây Slimfit', 2, 450000, 900000),
('DH22005', 'SP10', N'Thắt Lưng Da Bò', 2, 1200000, 2400000),
('DH22005', 'SP09', N'Giày Nike Air Force', 1, 2000000, 2000000);

-- Đơn hàng năm 2023
INSERT INTO DonHang (MaDH, MaKH, MaNV, NgayLap, TongTien, DiaChiGiaoHang) VALUES
('DH23001', 'MKH06', 'MNV03', '2023-01-25', 3100000, N'Hà Nội'),
('DH23002', 'MKH07', 'MNV04', '2023-03-18', 4200000, N'Đà Nẵng'),
('DH23003', 'MKH08', 'MNV10', '2023-06-30', 1850000, N'Huế'),
('DH23004', 'MKH09', 'MNV03', '2023-08-22', 5400000, N'Cần Thơ'),
('DH23005', 'MKH10', 'MNV04', '2023-12-15', 7200000, N'Hải Phòng');

INSERT INTO ChiTietDonHang (MaDH, MaSP, TenSP, SoLuong, DonGia, ThanhTien) VALUES
-- Đơn 1
('DH23001', 'SP02', N'Áo Polo Cá Sấu', 2, 520000, 1040000),
('DH23001', 'SP04', N'Áo Khoác Bomber', 1, 680000, 680000),
('DH23001', 'SP07', N'Quần Short Kaki', 3, 320000, 960000),
('DH23001', 'SP08', N'Váy Hoa Nhí', 2, 270000, 540000),
-- Đơn 2
('DH23002', 'SP01', N'Áo Thun Basic Trắng', 4, 160000, 640000),
('DH23002', 'SP05', N'Quần Jean Rách Gối', 2, 890000, 1780000),
('DH23002', 'SP09', N'Giày Nike Air Force', 1, 2100000, 2100000),
-- Đơn 3
('DH23003', 'SP03', N'Sơ Mi Trắng Công Sở', 3, 370000, 1110000),
('DH23003', 'SP06', N'Quần Tây Slimfit', 1, 480000, 480000),
('DH23003', 'SP08', N'Váy Hoa Nhí', 1, 270000, 270000),
-- Đơn 4
('DH23004', 'SP02', N'Áo Polo Cá Sấu', 1, 520000, 520000),
('DH23004', 'SP04', N'Áo Khoác Bomber', 2, 680000, 1360000),
('DH23004', 'SP07', N'Quần Short Kaki', 4, 320000, 1280000),
('DH23004', 'SP10', N'Thắt Lưng Da Bò', 1, 1250000, 1250000),
('DH23004', 'SP09', N'Giày Nike Air Force', 1, 2100000, 2100000),
-- Đơn 5
('DH23005', 'SP01', N'Áo Thun Basic Trắng', 6, 160000, 960000),
('DH23005', 'SP05', N'Quần Jean Rách Gối', 3, 890000, 2670000),
('DH23005', 'SP06', N'Quần Tây Slimfit', 2, 480000, 960000),
('DH23005', 'SP10', N'Thắt Lưng Da Bò', 2, 1250000, 2500000);

-- Đơn hàng năm 2024
INSERT INTO DonHang (MaDH, MaKH, MaNV, NgayLap, TongTien, DiaChiGiaoHang) VALUES
('DH24001', 'MKH02', 'MNV10', '2024-02-14', 2850000, N'Quận 3, TP.HCM'),
('DH24002', 'MKH04', 'MNV03', '2024-05-20', 4100000, N'Bình Thạnh'),
('DH24003', 'MKH06', 'MNV04', '2024-07-08', 6200000, N'Hà Nội'),
('DH24004', 'MKH08', 'MNV10', '2024-09-30', 3350000, N'Huế'),
('DH24005', 'MKH01', 'MNV03', '2024-11-25', 7800000, N'Quận 1, TP.HCM');

INSERT INTO ChiTietDonHang (MaDH, MaSP, TenSP, SoLuong, DonGia, ThanhTien) VALUES
-- Đơn 1
('DH24001', 'SP03', N'Sơ Mi Trắng Công Sở', 2, 390000, 780000),
('DH24001', 'SP06', N'Quần Tây Slimfit', 3, 500000, 1500000),
('DH24001', 'SP08', N'Váy Hoa Nhí', 1, 290000, 290000),
('DH24001', 'SP07', N'Quần Short Kaki', 2, 340000, 680000),
-- Đơn 2
('DH24002', 'SP02', N'Áo Polo Cá Sấu', 3, 540000, 1620000),
('DH24002', 'SP04', N'Áo Khoác Bomber', 2, 700000, 1400000),
('DH24002', 'SP09', N'Giày Nike Air Force', 1, 2200000, 2200000),
-- Đơn 3
('DH24003', 'SP05', N'Quần Jean Rách Gối', 2, 920000, 1840000),
('DH24003', 'SP10', N'Thắt Lưng Da Bò', 3, 1300000, 3900000),
('DH24003', 'SP01', N'Áo Thun Basic Trắng', 4, 170000, 680000),
-- Đơn 4
('DH24004', 'SP03', N'Sơ Mi Trắng Công Sở', 3, 390000, 1170000),
('DH24004', 'SP06', N'Quần Tây Slimfit', 2, 500000, 1000000),
('DH24004', 'SP07', N'Quần Short Kaki', 3, 340000, 1020000),
('DH24004', 'SP08', N'Váy Hoa Nhí', 2, 290000, 580000),
-- Đơn 5
('DH24005', 'SP01', N'Áo Thun Basic Trắng', 8, 170000, 1360000),
('DH24005', 'SP05', N'Quần Jean Rách Gối', 4, 920000, 3680000),
('DH24005', 'SP09', N'Giày Nike Air Force', 2, 2200000, 4400000),
('DH24005', 'SP10', N'Thắt Lưng Da Bò', 1, 1300000, 1300000);

-- Đơn hàng năm 2025
INSERT INTO DonHang (MaDH, MaKH, MaNV, NgayLap, TongTien, DiaChiGiaoHang) VALUES
('DH25001', 'MKH03', 'MNV04', '2025-01-10', 2950000, N'Thủ Đức'),
('DH25002', 'MKH05', 'MNV10', '2025-03-18', 4800000, N'Gò Vấp'),
('DH25003', 'MKH07', 'MNV03', '2025-05-22', 6700000, N'Đà Nẵng'),
('DH25004', 'MKH09', 'MNV04', '2025-08-30', 3900000, N'Cần Thơ'),
('DH25005', 'MKH02', 'MNV10', '2025-11-15', 8500000, N'Quận 3, TP.HCM');

INSERT INTO ChiTietDonHang (MaDH, MaSP, TenSP, SoLuong, DonGia, ThanhTien) VALUES
-- Đơn 1
('DH25001', 'SP04', N'Áo Khoác Bomber', 2, 720000, 1440000),
('DH25001', 'SP07', N'Quần Short Kaki', 3, 360000, 1080000),
('DH25001', 'SP08', N'Váy Hoa Nhí', 2, 310000, 620000),
('DH25001', 'SP03', N'Sơ Mi Trắng Công Sở', 1, 410000, 410000),
-- Đơn 2
('DH25002', 'SP02', N'Áo Polo Cá Sấu', 4, 560000, 2240000),
('DH25002', 'SP06', N'Quần Tây Slimfit', 3, 520000, 1560000),
('DH25002', 'SP09', N'Giày Nike Air Force', 1, 2300000, 2300000),
-- Đơn 3
('DH25003', 'SP05', N'Quần Jean Rách Gối', 3, 950000, 2850000),
('DH25003', 'SP10', N'Thắt Lưng Da Bò', 2, 1350000, 2700000),
('DH25003', 'SP01', N'Áo Thun Basic Trắng', 5, 180000, 900000),
('DH25003', 'SP04', N'Áo Khoác Bomber', 1, 720000, 720000),
-- Đơn 4
('DH25004', 'SP03', N'Sơ Mi Trắng Công Sở', 4, 410000, 1640000),
('DH25004', 'SP06', N'Quần Tây Slimfit', 2, 520000, 1040000),
('DH25004', 'SP07', N'Quần Short Kaki', 3, 360000, 1080000),
('DH25004', 'SP08', N'Váy Hoa Nhí', 2, 310000, 620000),
-- Đơn 5
('DH25005', 'SP01', N'Áo Thun Basic Trắng', 10, 180000, 1800000),
('DH25005', 'SP05', N'Quần Jean Rách Gối', 5, 950000, 4750000),
('DH25005', 'SP09', N'Giày Nike Air Force', 2, 2300000, 4600000),
('DH25005', 'SP10', N'Thắt Lưng Da Bò', 2, 1350000, 2700000);

-- =============================================
-- 6. TẠO CHI TIẾT BÁN THEO LÔ (CHO DỮ LIỆU HIỆN CÓ)
-- =============================================

PRINT '=== TẠO DỮ LIỆU CHO ChiTietBanTheoLo ===';

-- Xóa dữ liệu cũ nếu có
DELETE FROM ChiTietBanTheoLo;

-- Reset số lượng còn lại trong LoHang
UPDATE LoHang SET SoLuongConLai = SoLuongNhap;

-- Tạo dữ liệu ChiTietBanTheoLo từ các đơn hàng đã có
DECLARE @MaDH CHAR(10), @MaSP CHAR(10), @SoLuongBan INT, @DonGia DECIMAL(18,2), @NgayLap DATETIME;
DECLARE @MaLo INT, @SoLuongConLai INT, @GiaNhap DECIMAL(18,2);
DECLARE @SoLuongCanLay INT;

DECLARE curChiTietDonHang CURSOR FOR
SELECT 
    ct.MaDH,
    ct.MaSP,
    ct.SoLuong,
    ct.DonGia,
    dh.NgayLap
FROM ChiTietDonHang ct
JOIN DonHang dh ON ct.MaDH = dh.MaDH
ORDER BY dh.NgayLap, ct.MaDH, ct.MaSP;

OPEN curChiTietDonHang;
FETCH NEXT FROM curChiTietDonHang INTO @MaDH, @MaSP, @SoLuongBan, @DonGia, @NgayLap;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Phân bổ theo FIFO
    DECLARE curLoHang CURSOR FOR
    SELECT MaLo, SoLuongConLai, GiaNhap
    FROM LoHang
    WHERE MaSP = @MaSP AND SoLuongConLai > 0
    ORDER BY NgayNhap;
    
    OPEN curLoHang;
    FETCH NEXT FROM curLoHang INTO @MaLo, @SoLuongConLai, @GiaNhap;
    
    WHILE @SoLuongBan > 0 AND @@FETCH_STATUS = 0
    BEGIN
        SET @SoLuongCanLay = CASE 
            WHEN @SoLuongConLai >= @SoLuongBan THEN @SoLuongBan
            ELSE @SoLuongConLai
        END;
        
        INSERT INTO ChiTietBanTheoLo (MaDH, MaSP, MaLo, SoLuongBan, GiaBanThucTe, NgayBan)
        VALUES (@MaDH, @MaSP, @MaLo, @SoLuongCanLay, @DonGia, @NgayLap);
        
        UPDATE LoHang
        SET SoLuongConLai = SoLuongConLai - @SoLuongCanLay
        WHERE MaLo = @MaLo;
        
        SET @SoLuongBan = @SoLuongBan - @SoLuongCanLay;
        
        FETCH NEXT FROM curLoHang INTO @MaLo, @SoLuongConLai, @GiaNhap;
    END
    
    CLOSE curLoHang;
    DEALLOCATE curLoHang;
    
    FETCH NEXT FROM curChiTietDonHang INTO @MaDH, @MaSP, @SoLuongBan, @DonGia, @NgayLap;
END

CLOSE curChiTietDonHang;
DEALLOCATE curChiTietDonHang;

PRINT 'Đã tạo xong dữ liệu ChiTietBanTheoLo';

-- =============================================
-- 7. KIỂM TRA VÀ ĐỒNG BỘ DỮ LIỆU
-- =============================================

PRINT '=== KIỂM TRA VÀ ĐỒNG BỘ DỮ LIỆU ===';

-- Đồng bộ tất cả số lượng tồn từ LoHang
UPDATE SanPham
SET SoLuongTon = ISNULL((
    SELECT SUM(SoLuongConLai)
    FROM LoHang
    WHERE LoHang.MaSP = SanPham.MaSP
), 0);

PRINT 'Đã đồng bộ tất cả số lượng tồn từ LoHang';

-- Kiểm tra dữ liệu
PRINT '=== KIỂM TRA DỮ LIỆU ĐÃ NHẬP ===';
SELECT '1. Số lượng sản phẩm:' AS ThongTin, COUNT(*) AS SoLuong FROM SanPham
UNION ALL
SELECT '2. Số lượng phiếu nhập:', COUNT(*) FROM PhieuNhap
UNION ALL
SELECT '3. Số lượng lô hàng:', COUNT(*) FROM LoHang
UNION ALL
SELECT '4. Số lượng đơn hàng:', COUNT(*) FROM DonHang
UNION ALL
SELECT '5. Số lượng chi tiết bán theo lô:', COUNT(*) FROM ChiTietBanTheoLo;

-- Kiểm tra đồng bộ số lượng tồn
PRINT '=== KIỂM TRA ĐỒNG BỘ SỐ LƯỢNG TỒN ===';
SELECT 
    sp.MaSP,
    sp.TenSP,
    sp.SoLuongTon AS TonKho_SanPham,
    ISNULL(SUM(lh.SoLuongConLai), 0) AS TonKho_LoHang,
    CASE 
        WHEN sp.SoLuongTon = ISNULL(SUM(lh.SoLuongConLai), 0) THEN N'ĐỒNG BỘ ✓'
        ELSE N'CHƯA ĐỒNG BỘ ✗'
    END AS TrangThai
FROM SanPham sp
LEFT JOIN LoHang lh ON sp.MaSP = lh.MaSP
GROUP BY sp.MaSP, sp.TenSP, sp.SoLuongTon
ORDER BY sp.MaSP;

PRINT '=== KẾT THÚC TẠO DATABASE ===';
GO

----Proc
CREATE PROCEDURE sp_ThongKeDoanhThuNam
    @Nam INT
AS
BEGIN
    SELECT 
        YEAR(dh.NgayLap) AS 'Nam',
        COUNT(DISTINCT dh.MaDH) AS 'SoDonHang',
        SUM(dh.TongTien) AS 'TongDoanhThu',
        AVG(dh.TongTien) AS 'DoanhThuTrungBinh',
        SUM(dh.TongTien) / 12 AS 'DoanhThuTrungBinhThang'
    FROM DonHang dh
    WHERE YEAR(dh.NgayLap) = @Nam
    GROUP BY YEAR(dh.NgayLap);
    
    -- Doanh thu theo từng tháng trong năm
    SELECT 
        MONTH(dh.NgayLap) AS 'Thang',
        COUNT(DISTINCT dh.MaDH) AS 'SoDonHang',
        SUM(dh.TongTien) AS 'DoanhThu',
        AVG(dh.TongTien) AS 'DonHangTrungBinh',
        RANK() OVER (ORDER BY SUM(dh.TongTien) DESC) AS 'XepHang'
    FROM DonHang dh
    WHERE YEAR(dh.NgayLap) = @Nam
    GROUP BY MONTH(dh.NgayLap)
    ORDER BY MONTH(dh.NgayLap);
END;
GO
----
CREATE PROCEDURE sp_ThongKeDoanhThuThang
    @Thang INT,
    @Nam INT
AS
BEGIN
    SELECT 
        YEAR(dh.NgayLap) AS 'Nam',
        MONTH(dh.NgayLap) AS 'Thang',
        COUNT(DISTINCT dh.MaDH) AS 'SoDonHang',
        SUM(dh.TongTien) AS 'TongDoanhThu',
        AVG(dh.TongTien) AS 'DoanhThuTrungBinh',
        SUM(dh.TongTien) / DAY(EOMONTH(CAST(@Nam AS VARCHAR) + '-' + 
            RIGHT('0' + CAST(@Thang AS VARCHAR), 2) + '-01')) AS 'DoanhThuTrungBinhNgay'
    FROM DonHang dh
    WHERE YEAR(dh.NgayLap) = @Nam 
        AND MONTH(dh.NgayLap) = @Thang
    GROUP BY YEAR(dh.NgayLap), MONTH(dh.NgayLap);
    
    -- Doanh thu theo từng ngày trong tháng
    SELECT 
        DAY(dh.NgayLap) AS 'Ngay',
        COUNT(DISTINCT dh.MaDH) AS 'SoDonHang',
        SUM(dh.TongTien) AS 'DoanhThu',
        AVG(dh.TongTien) AS 'DonHangTrungBinh'
    FROM DonHang dh
    WHERE YEAR(dh.NgayLap) = @Nam 
        AND MONTH(dh.NgayLap) = @Thang
    GROUP BY DAY(dh.NgayLap)
    ORDER BY DAY(dh.NgayLap);
END;
GO
----
CREATE PROCEDURE sp_ThongKeDoanhThuNgay
    @Ngay DATE
AS
BEGIN
    SELECT 
        CONVERT(DATE, dh.NgayLap) AS 'Ngay',
        COUNT(DISTINCT dh.MaDH) AS 'SoDonHang',
        SUM(dh.TongTien) AS 'TongDoanhThu',
        AVG(dh.TongTien) AS 'DoanhThuTrungBinh',
        MIN(dh.TongTien) AS 'DonHangNhoNhat',
        MAX(dh.TongTien) AS 'DonHangLonNhat',
        COUNT(DISTINCT dh.MaKH) AS 'SoKhachHang'
    FROM DonHang dh
    WHERE CONVERT(DATE, dh.NgayLap) = @Ngay
    GROUP BY CONVERT(DATE, dh.NgayLap);
    
    -- Chi tiết từng đơn hàng trong ngày
    SELECT 
        dh.MaDH,
        dh.MaKH,
        kh.HoTen AS 'TenKhachHang',
        dh.TongTien,
        dh.NgayLap,
        COUNT(ct.MaSP) AS 'SoSanPham',
        SUM(ct.SoLuong) AS 'TongSoLuong'
    FROM DonHang dh
    LEFT JOIN KhachHang kh ON dh.MaKH = kh.MaKH
    LEFT JOIN ChiTietDonHang ct ON dh.MaDH = ct.MaDH
    WHERE CONVERT(DATE, dh.NgayLap) = @Ngay
    GROUP BY dh.MaDH, dh.MaKH, kh.HoTen, dh.TongTien, dh.NgayLap
    ORDER BY dh.TongTien DESC;
END;
GO

CREATE PROCEDURE sp_LayDanhSachNamThongKe
AS
BEGIN
    SELECT DISTINCT YEAR(NgayLap) AS 'Nam'
    FROM DonHang
    UNION
    SELECT DISTINCT YEAR(NgayNhap) AS 'Nam'
    FROM PhieuNhap
    ORDER BY 'Nam' DESC;
END;
GO

CREATE PROCEDURE sp_LayDanhSachThangTheoNam
    @Nam INT
AS
BEGIN
    SELECT DISTINCT MONTH(NgayLap) AS 'Thang'
    FROM DonHang
    WHERE YEAR(NgayLap) = @Nam
    UNION
    SELECT DISTINCT MONTH(NgayNhap) AS 'Thang'
    FROM PhieuNhap
    WHERE YEAR(NgayNhap) = @Nam
    ORDER BY 'Thang';
END;
GO

CREATE PROCEDURE sp_LayDanhSachNgayTheoThang
    @Thang INT,
    @Nam INT
AS
BEGIN
    SELECT DISTINCT DAY(NgayLap) AS 'Ngay'
    FROM DonHang
    WHERE YEAR(NgayLap) = @Nam AND MONTH(NgayLap) = @Thang
    ORDER BY 'Ngay';
END;
GO

CREATE PROCEDURE sp_LoiNhuanChiTietSP
    @LoaiThongKe NVARCHAR(10) = 'NAM',
    @Ngay INT = NULL,
    @Thang INT = NULL,
    @Nam INT = NULL
AS
BEGIN
    DECLARE @TuNgay DATE, @DenNgay DATE;
    
    -- Xác định khoảng thời gian
    IF @LoaiThongKe = 'NGAY'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, @Thang, @Ngay);
        SET @DenNgay = DATEADD(DAY, 1, @TuNgay);
    END
    ELSE IF @LoaiThongKe = 'THANG'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, @Thang, 1);
        SET @DenNgay = DATEADD(MONTH, 1, @TuNgay);
    END
    ELSE IF @LoaiThongKe = 'NAM'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, 1, 1);
        SET @DenNgay = DATEFROMPARTS(@Nam + 1, 1, 1);
    END
    
    -- Tạo bảng tạm để tính toán đúng lợi nhuận
    CREATE TABLE #TempLoiNhuan (
        MaSP CHAR(10),
        TenSP NVARCHAR(255),
        SoDonHang INT,
        SoLuongDaBan INT,
        DoanhThu DECIMAL(18,2),
        GiaVon DECIMAL(18,2),
        LoiNhuan DECIMAL(18,2),
        TyLeLoiNhuan DECIMAL(18,2)
    );
    
    -- Lấy danh sách sản phẩm đã bán trong khoảng thời gian
    INSERT INTO #TempLoiNhuan (MaSP, TenSP, SoDonHang, SoLuongDaBan, DoanhThu)
    SELECT 
        sp.MaSP,
        sp.TenSP,
        COUNT(DISTINCT ctbl.MaDH) AS SoDonHang,
        SUM(ctbl.SoLuongBan) AS SoLuongDaBan,
        SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe) AS DoanhThu
    FROM ChiTietBanTheoLo ctbl
    JOIN SanPham sp ON ctbl.MaSP = sp.MaSP
    WHERE ctbl.NgayBan >= @TuNgay AND ctbl.NgayBan < @DenNgay
    GROUP BY sp.MaSP, sp.TenSP;
    
    -- Tính giá vốn theo FIFO cho từng sản phẩm
    UPDATE #TempLoiNhuan
    SET GiaVon = ISNULL((
        -- Tính giá vốn theo FIFO
        SELECT SUM(ctbl.SoLuongBan * lh.GiaNhap)
        FROM ChiTietBanTheoLo ctbl
        JOIN LoHang lh ON ctbl.MaLo = lh.MaLo
        WHERE ctbl.MaSP = #TempLoiNhuan.MaSP
          AND ctbl.NgayBan >= @TuNgay 
          AND ctbl.NgayBan < @DenNgay
    ), 0);
    
    -- Tính lợi nhuận và tỷ lệ
    UPDATE #TempLoiNhuan
    SET 
        LoiNhuan = DoanhThu - GiaVon,
        TyLeLoiNhuan = CASE 
            WHEN DoanhThu > 0 THEN ROUND((DoanhThu - GiaVon) * 100.0 / DoanhThu, 2)
            ELSE 0 
        END;
    
    -- Trả kết quả
    SELECT 
        MaSP,
        TenSP,
        SoDonHang,
        SoLuongDaBan,
        DoanhThu,
        GiaVon,
        LoiNhuan,
        TyLeLoiNhuan,
        CASE @LoaiThongKe 
            WHEN 'NGAY' THEN CONVERT(VARCHAR, @TuNgay, 103)
            WHEN 'THANG' THEN 'Tháng ' + CAST(@Thang AS VARCHAR) + '/' + CAST(@Nam AS VARCHAR)
            WHEN 'NAM' THEN 'Năm ' + CAST(@Nam AS VARCHAR)
        END AS 'ThoiGian'
    FROM #TempLoiNhuan
    ORDER BY LoiNhuan DESC;
    
    DROP TABLE #TempLoiNhuan;
END;
GO

CREATE PROCEDURE sp_LoiNhuanTongHop
    @LoaiThongKe NVARCHAR(10) = 'NAM', -- NGAY, THANG, NAM
    @Ngay INT = NULL,
    @Thang INT = NULL,
    @Nam INT = NULL
AS
BEGIN
    DECLARE @TuNgay DATE, @DenNgay DATE;
    
    -- Xác định khoảng thời gian
    IF @LoaiThongKe = 'NGAY'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, @Thang, @Ngay);
        SET @DenNgay = DATEADD(DAY, 1, @TuNgay);
    END
    ELSE IF @LoaiThongKe = 'THANG'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, @Thang, 1);
        SET @DenNgay = DATEADD(MONTH, 1, @TuNgay);
    END
    ELSE IF @LoaiThongKe = 'NAM'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, 1, 1);
        SET @DenNgay = DATEFROMPARTS(@Nam + 1, 1, 1);
    END
    
    -- KẾT QUẢ: TỔNG HỢP LỢI NHUẬN
    SELECT 
        CASE @LoaiThongKe 
            WHEN 'NGAY' THEN CONVERT(VARCHAR, @TuNgay, 103)
            WHEN 'THANG' THEN 'Tháng ' + CAST(@Thang AS VARCHAR) + '/' + CAST(@Nam AS VARCHAR)
            WHEN 'NAM' THEN 'Năm ' + CAST(@Nam AS VARCHAR)
        END AS 'ThoiGian',
        COUNT(DISTINCT ctbl.MaDH) AS 'SoDonHang',
        SUM(ctbl.SoLuongBan) AS 'TongSoLuongBan',
        SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe) AS 'TongDoanhThu',
        SUM(ctbl.SoLuongBan * lh.GiaNhap) AS 'TongGiaVon',
        SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe) - SUM(ctbl.SoLuongBan * lh.GiaNhap) AS 'TongLoiNhuan',
        ROUND(
            (SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe) - SUM(ctbl.SoLuongBan * lh.GiaNhap)) * 100.0 / 
            NULLIF(SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe), 0), 2
        ) AS 'TyLeLoiNhuanTrungBinh'
    FROM ChiTietBanTheoLo ctbl
    JOIN LoHang lh ON ctbl.MaLo = lh.MaLo
    WHERE ctbl.NgayBan >= @TuNgay AND ctbl.NgayBan < @DenNgay;
END;
GO

CREATE PROCEDURE sp_LoiNhuanTheoLoHang
    @LoaiThongKe NVARCHAR(10) = 'NAM', -- NGAY, THANG, NAM
    @Ngay INT = NULL,
    @Thang INT = NULL,
    @Nam INT = NULL
AS
BEGIN
    DECLARE @TuNgay DATE, @DenNgay DATE;
    
    -- Xác định khoảng thời gian
    IF @LoaiThongKe = 'NGAY'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, @Thang, @Ngay);
        SET @DenNgay = DATEADD(DAY, 1, @TuNgay);
    END
    ELSE IF @LoaiThongKe = 'THANG'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, @Thang, 1);
        SET @DenNgay = DATEADD(MONTH, 1, @TuNgay);
    END
    ELSE IF @LoaiThongKe = 'NAM'
    BEGIN
        SET @TuNgay = DATEFROMPARTS(@Nam, 1, 1);
        SET @DenNgay = DATEFROMPARTS(@Nam + 1, 1, 1);
    END
    
    -- KẾT QUẢ: LỢI NHUẬN THEO TỪNG LÔ HÀNG
    SELECT 
        lh.MaLo,
        sp.TenSP,
        lh.GiaNhap,
        AVG(ctbl.GiaBanThucTe) AS 'GiaBanTrungBinh',
        SUM(ctbl.SoLuongBan) AS 'SoLuongDaBanTuLo',
        SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe) AS 'DoanhThuTuLo',
        SUM(ctbl.SoLuongBan * lh.GiaNhap) AS 'GiaVonTuLo',
        SUM(ctbl.SoLuongBan * ctbl.GiaBanThucTe) - SUM(ctbl.SoLuongBan * lh.GiaNhap) AS 'LoiNhuanTuLo',
        -- Thông tin thời gian
        CASE @LoaiThongKe 
            WHEN 'NGAY' THEN CONVERT(VARCHAR, @TuNgay, 103)
            WHEN 'THANG' THEN 'Tháng ' + CAST(@Thang AS VARCHAR) + '/' + CAST(@Nam AS VARCHAR)
            WHEN 'NAM' THEN 'Năm ' + CAST(@Nam AS VARCHAR)
        END AS 'ThoiGian'
    FROM ChiTietBanTheoLo ctbl
    JOIN LoHang lh ON ctbl.MaLo = lh.MaLo
    JOIN SanPham sp ON lh.MaSP = sp.MaSP
    WHERE ctbl.NgayBan >= @TuNgay AND ctbl.NgayBan < @DenNgay
    GROUP BY lh.MaLo, sp.TenSP, lh.GiaNhap
    ORDER BY lh.MaLo;
END;
GO

CREATE PROCEDURE sp_KhachHangThanThiet
    @SoLuongTop INT = 10
AS
BEGIN
    SELECT TOP(@SoLuongTop)
        kh.MaKH,
        kh.HoTen,
        kh.SDT,
        kh.DiaChi,
        COUNT(dh.MaDH) AS 'SoDonDaMua',
        SUM(dh.TongTien) AS 'TongChiTieu',
        AVG(dh.TongTien) AS 'DonHangTrungBinh',
        DATEDIFF(DAY, MAX(dh.NgayLap), GETDATE()) AS 'SoNgayChuaMua',
        MAX(dh.NgayLap) AS 'LanMuaGanNhat'
    FROM KhachHang kh
    LEFT JOIN DonHang dh ON kh.MaKH = dh.MaKH
    WHERE dh.MaDH IS NOT NULL
    GROUP BY kh.MaKH, kh.HoTen, kh.SDT, kh.DiaChi
    ORDER BY SUM(dh.TongTien) DESC;
    
    -- Phân loại khách hàng theo RFM
    WITH RFM AS (
        SELECT 
            kh.MaKH,
            kh.HoTen,
            DATEDIFF(DAY, MAX(dh.NgayLap), GETDATE()) AS Recency,
            COUNT(dh.MaDH) AS Frequency,
            SUM(dh.TongTien) AS Monetary
        FROM KhachHang kh
        LEFT JOIN DonHang dh ON kh.MaKH = dh.MaKH
        WHERE dh.MaDH IS NOT NULL
        GROUP BY kh.MaKH, kh.HoTen
    )
    SELECT *,
        CASE 
            WHEN Recency <= 30 AND Frequency >= 5 AND Monetary >= 5000000 THEN N'VIP Cao Cấp'
            WHEN Recency <= 60 AND Frequency >= 3 AND Monetary >= 2000000 THEN N'VIP'
            WHEN Recency <= 90 AND Frequency >= 1 THEN N'Khách thường xuyên'
            ELSE N'Khách ít hoạt động'
        END AS 'PhanLoai'
    FROM RFM
    ORDER BY Monetary DESC;
END;
GO

CREATE PROCEDURE sp_TopSanPhamBanChay
    @SoLuongTop INT = 10,
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL
AS
BEGIN
    IF @TuNgay IS NULL SET @TuNgay = DATEADD(MONTH, -1, GETDATE())
    IF @DenNgay IS NULL SET @DenNgay = GETDATE()
    
    SELECT TOP(@SoLuongTop)
        sp.MaSP,
        sp.TenSP,
        sp.ThuongHieu,
        tl.TenTheLoai,
        SUM(ct.SoLuong) AS 'TongSoLuongBan',
        SUM(ct.ThanhTien) AS 'TongDoanhThu',
        AVG(ct.DonGia) AS 'GiaBanTrungBinh',
        ROUND(CAST(SUM(ct.SoLuong) AS FLOAT) / 
              (SELECT SUM(SoLuong) FROM ChiTietDonHang ct2 
               JOIN DonHang dh2 ON ct2.MaDH = dh2.MaDH
               WHERE dh2.NgayLap BETWEEN @TuNgay AND @DenNgay) * 100, 2) AS 'PhanTram'
    FROM ChiTietDonHang ct
    JOIN DonHang dh ON ct.MaDH = dh.MaDH
    JOIN SanPham sp ON ct.MaSP = sp.MaSP
    JOIN TheLoai tl ON sp.MaTheLoai = tl.MaTheLoai
    WHERE dh.NgayLap BETWEEN @TuNgay AND @DenNgay
    GROUP BY sp.MaSP, sp.TenSP, sp.ThuongHieu, tl.TenTheLoai
    ORDER BY SUM(ct.SoLuong) DESC;
END;
GO