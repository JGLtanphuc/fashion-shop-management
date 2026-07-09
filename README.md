🛍️ GJGL - Hệ thống Quản lý Shop Quần Áo
GJGL là một giải pháp phần mềm Desktop toàn diện dành cho mô hình kinh doanh bán lẻ thời trang. Hệ thống được thiết kế tối ưu cho các thao tác nghiệp vụ tại cửa hàng, cung cấp giao diện trực quan cho nhân viên và bộ công cụ quản lý, thống kê mạnh mẽ cho chủ shop.

🛠 Công nghệ sử dụng
Frontend/UI: Windows Forms.
Backend Logic: C#.
Database: Microsoft SQL Server.
Reporting & Export: SAP Crystal Reports.

⚙️ Các luồng nghiệp vụ cốt lõi
1. Quản lý Sản phẩm & Thể loại

Quản lý phân cấp: Thể loại (Áo thun, Quần Jean, Giày...) là dữ liệu cha, Sản phẩm là dữ liệu con. Mọi sản phẩm khi tạo mới bắt buộc phải thuộc về một Thể loại cụ thể.

Kiểm soát Tồn kho: Trường Số lượng tồn trên trang Quản lý Sản phẩm được thiết kế ở chế độ Read-only hoặc được kiểm soát nghiêm ngặt. Số lượng này không được sửa tùy tiện bằng tay mà tự động biến động dựa trên 2 luồng giao dịch thực tế: Tăng lên khi lập Phiếu Nhập và Giảm đi khi lập Hóa Đơn Bán.

Ràng buộc khóa ngoại: Khi Admin thao tác Xóa một Thể loại, hệ thống sẽ tự động kiểm tra xem Thể loại đó có đang chứa Sản phẩm nào không. Nếu có, hệ thống sẽ chặn thao tác xóa và phát ra cảnh báo.

2. Quản Nhà cung cấp & Khách hàn

Chuẩn hóa thông tin Nhập hàng: Dữ liệu từ trang Nhà Cung Cấp (Tên NCC, Số điện thoại, Địa chỉ, Email) là nguồn dữ liệu bắt buộc để bộ phận Kho lập Phiếu Nhập. Điều này giúp shop truy xuất nhanh nguồn gốc hàng hóa và đối soát công nợ khi cần.

Quản trị Thông tin Khách hàng: Hồ sơ Khách hàng được lưu trữ để phục vụ cho việc tạo đơn hàng. Khi nhân viên thu ngân nhập Số điện thoại tại màn hình Tạo Đơn Hàng, hệ thống sẽ tự động truy vấn từ bảng Khách Hàng và điền sẵn thông tin nếu là khách cũ, giúp rút ngắn thời gian thanh toán.

3. Quản lý Nhân viên & Phân quyền 

Cơ chế cấp phát tài khoản: Trang Quản lý Nhân viên không chỉ lưu thông tin hành chính mà còn liên kết trực tiếp với thông tin Đăng nhập.

Bảo mật giao diện: Dựa vào cột Quyền hệ thống sẽ tự động kích hoạt luồng kiểm tra . Các nút menu trên thanh Sidebar không thuộc thẩm quyền của nhân viên đó sẽ bị ẩn đi.

Xóa mềm & Lịch sử thao tác: Để tránh làm hỏng các báo cáo doanh thu củ, nhân viên đã từng lập Hóa đơn hoặc Phiếu nhập sẽ không thể bị xóa vĩnh viễn khỏi CSDL. Thay vào đó, Quản lý chỉ có thể thu hồi quyền đăng nhập hoặc đổi trạng thái làm việc.

4. Thống kê & Báo cáo 

Xử lý số liệu đa chiều: Hệ thống cho phép tổng hợp các chỉ số tài chính theo thời gian thực. Truy vấn các Hóa Đơn và Phiếu Nhập để đối chiếu dòng tiền.

Thuật toán bóc tách Lợi nhuận: Không chỉ dừng ở mức tính Doanh thu, thống kê còn tự động mapping với bảng Sản Phẩm và Phiếu Nhập để tính ra Giá vốn hàng bán, từ đó cung cấp chính xác Lợi nhuận ròng và Tỷ suất lợi nhuận cho chủ shop.

📈 Tối ưu hóa & Hiệu năng
Data Binding & UI Flow: Tối ưu hóa việc đổ dữ liệu từ SQL lên DataGridView, tự động dọn dẹp các TextBox và Reset form sau mỗi luồng thao tác hoàn tất (Thêm/Sửa/Xóa).

Format Data: Xử lý chuẩn hóa Tiền tệ (VNĐ - phân cách hàng nghìn) và Thời gian (dd/MM/yyyy) đồng bộ trên toàn bộ UI và Report.

Lưu ý triển khai: Vui lòng cấu hình lại chuỗi kết nối trỏ về thư mục chứa CSDL SQL Server cục bộ trong class DatabaseHelper trước khi Build và chạy ứng dụng.