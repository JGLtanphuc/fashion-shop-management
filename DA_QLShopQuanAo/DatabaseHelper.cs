// DatabaseHelper.cs
using System;
using System.Data;
using System.Data.SqlClient;

namespace DA_QLShopQuanAo.Helpers
{
    public class DatabaseHelper
    {
        private static string connectionString = @"Data Source=PHUC;Initial Catalog=QuanLyBanHang_GJGL;Integrated Security=True";

        // Phương thức để thiết lập connection string (nếu cần thay đổi)
        public static void SetConnectionString(string newConnectionString)
        {
            connectionString = newConnectionString;
        }

        // Lấy connection string
        public static string GetConnectionString()
        {
            return connectionString;
        }

        // Tạo và mở kết nối
        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        // Thực thi query trả về DataTable
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        // Thực thi query không trả về dữ liệu (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteNonQuery();
            }
        }

        // Thực thi query trả về một giá trị đơn
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteScalar();
            }
        }

        // Kiểm tra kết nối
        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Bắt đầu transaction
        public static SqlTransaction BeginTransaction()
        {
            var connection = GetConnection();
            return connection.BeginTransaction();
        }
    }
}