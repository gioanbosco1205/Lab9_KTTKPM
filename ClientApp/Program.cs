using System.Net.Http.Json;
using System.Text.Json;

namespace ClientApp;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static string? authToken;

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Demo Client App ===");
        Console.WriteLine("Ứng dụng này mô phỏng client gọi API qua API Gateway\n");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1. Đăng nhập (Login)");
            Console.WriteLine("2. Lấy danh sách khách hàng (Get Customers)");
            Console.WriteLine("3. Lấy thông tin khách hàng theo ID (Get Customer by ID)");
            Console.WriteLine("4. Lấy danh sách sản phẩm (Get Products)");
            Console.WriteLine("5. Validate Token");
            Console.WriteLine("0. Thoát (Exit)");
            Console.Write("\nChọn chức năng: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await Login();
                    break;
                case "2":
                    await GetCustomers();
                    break;
                case "3":
                    await GetCustomerById();
                    break;
                case "4":
                    await GetProducts();
                    break;
                case "5":
                    await ValidateToken();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Tạm biệt!");
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ!");
                    break;
            }
        }
    }

    static async Task Login()
    {
        Console.Write("Username (mặc định: admin): ");
        string? username = Console.ReadLine();
        if (string.IsNullOrEmpty(username)) username = "admin";

        Console.Write("Password (mặc định: password): ");
        string? password = Console.ReadLine();
        if (string.IsNullOrEmpty(password)) password = "password";

        try
        {
            var loginData = new { Username = username, Password = password };
            var response = await client.PostAsJsonAsync("http://localhost:9003/api/auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                authToken = result.GetProperty("token").GetString();
                Console.WriteLine($"\n✓ Đăng nhập thành công!");
                Console.WriteLine($"Token: {authToken}");
            }
            else
            {
                Console.WriteLine($"\n✗ Đăng nhập thất bại: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Lỗi: {ex.Message}");
        }
    }

    static async Task GetCustomers()
    {
        try
        {
            var response = await client.GetAsync("http://localhost:9000/customers");
            
            if (response.IsSuccessStatusCode)
            {
                var customers = await response.Content.ReadFromJsonAsync<string[]>();
                Console.WriteLine("\n✓ Danh sách khách hàng:");
                if (customers != null)
                {
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"  - {customer}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"\n✗ Lỗi: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Lỗi: {ex.Message}");
            Console.WriteLine("Đảm bảo API Gateway và CustomersAPI đang chạy!");
        }
    }

    static async Task GetCustomerById()
    {
        Console.Write("Nhập Customer ID: ");
        string? id = Console.ReadLine();

        try
        {
            var response = await client.GetAsync($"http://localhost:9000/customers/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var customer = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n✓ Thông tin khách hàng: {customer}");
            }
            else
            {
                Console.WriteLine($"\n✗ Lỗi: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Lỗi: {ex.Message}");
        }
    }

    static async Task GetProducts()
    {
        try
        {
            var response = await client.GetAsync("http://localhost:9000/api/products");
            
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<string[]>();
                Console.WriteLine("\n✓ Danh sách sản phẩm:");
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        Console.WriteLine($"  - {product}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"\n✗ Lỗi: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Lỗi: {ex.Message}");
            Console.WriteLine("Đảm bảo API Gateway và ProductsAPI đang chạy!");
        }
    }

    static async Task ValidateToken()
    {
        if (string.IsNullOrEmpty(authToken))
        {
            Console.WriteLine("\n✗ Chưa có token! Vui lòng đăng nhập trước.");
            return;
        }

        try
        {
            var tokenData = new { Token = authToken };
            var response = await client.PostAsJsonAsync("http://localhost:9003/api/auth/validate", tokenData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n✓ Token hợp lệ!");
                Console.WriteLine($"Response: {result}");
            }
            else
            {
                Console.WriteLine($"\n✗ Token không hợp lệ: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Lỗi: {ex.Message}");
        }
    }
}
