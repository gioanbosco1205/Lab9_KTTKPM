# Microservices Demo với API Gateway

## Giới thiệu

Dự án này demo kiến trúc microservices với 4 project:

1. **APIGateway** (Port 9000) - Cổng vào chính, định tuyến request đến các service
2. **CustomersAPI** (Port 9001) - Service quản lý khách hàng
3. **ProductsAPI** (Port 9002) - Service quản lý sản phẩm
4. **AuthServer** (Port 9003) - Service xác thực và phân quyền
5. **ClientApp** - Console app mô phỏng client

## Cấu trúc Port

| Service | Port | Mô tả |
|---------|------|-------|
| APIGateway | 9000 | Entry point chính |
| CustomersAPI | 9001 | API khách hàng |
| ProductsAPI | 9002 | API sản phẩm |
| AuthServer | 9003 | API xác thực |

## Cách chạy

### 1. Chạy từng service riêng lẻ

Mở 4 terminal riêng biệt và chạy:

```bash
# Terminal 1 - API Gateway
cd APIGateway
dotnet run

# Terminal 2 - Customers API
cd CustomersAPI
dotnet run

# Terminal 3 - Products API
cd ProductsAPI
dotnet run

# Terminal 4 - Auth Server
cd AuthServer
dotnet run
```

### 2. Chạy Client App để test

```bash
# Terminal 5 - Client App
cd ClientApp
dotnet run
```

## API Endpoints

### Qua API Gateway (Port 9000)

- `GET http://localhost:9000/customers` - Lấy danh sách khách hàng
- `GET http://localhost:9000/customers/{id}` - Lấy khách hàng theo ID
- `GET http://localhost:9000/api/products` - Lấy danh sách sản phẩm
- `POST http://localhost:9000/auth/login` - Đăng nhập
- `POST http://localhost:9000/auth/validate` - Validate token

### Trực tiếp đến service (không qua Gateway)

**CustomersAPI (Port 9001):**
- `GET http://localhost:9001/api/customers`
- `GET http://localhost:9001/api/customers/{id}`

**ProductsAPI (Port 9002):**
- `GET http://localhost:9002/api/products`

**AuthServer (Port 9003):**
- `POST http://localhost:9003/api/auth/login`
- `POST http://localhost:9003/api/auth/validate`

## Test với ClientApp

ClientApp cung cấp menu tương tác:

1. **Đăng nhập** - Test AuthServer
   - Username: `admin`
   - Password: `password`

2. **Lấy danh sách khách hàng** - Test CustomersAPI qua Gateway

3. **Lấy khách hàng theo ID** - Test CustomersAPI với parameter

4. **Lấy danh sách sản phẩm** - Test ProductsAPI qua Gateway

5. **Validate Token** - Test AuthServer validation

## Kiến trúc

```
ClientApp
    ↓
APIGateway (Ocelot) :9000
    ↓
    ├─→ CustomersAPI :9001
    ├─→ ProductsAPI :9002
    └─→ AuthServer :9003
```

## Công nghệ sử dụng

- **.NET 8.0**
- **Ocelot** - API Gateway
- **ASP.NET Core Web API**
- **Console App** cho client simulation

## Lưu ý

- Đảm bảo tất cả các service đang chạy trước khi test với ClientApp
- AuthServer sử dụng authentication đơn giản cho demo (không dùng JWT thật)
- Trong production cần implement JWT token thật và database
