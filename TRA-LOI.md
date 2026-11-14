
## Câu 1a

1. **Đóng gói (Encapsulation):** Gói dữ liệu và hành vi vào trong lớp, ẩn các thuộc tính bằng `private set` để bên ngoài không sửa tùy tiện, chỉ thao tác qua phương thức hợp lệ.
   - Minh hoạ: `EduHub.Domain/Entities/Member.cs` — lớp `Member`.

2. **Trừu tượng (Abstraction):** Ẩn chi tiết xử lý bên trong, chỉ đưa ra giao diện (`interface`) mô tả những hành vi cần thiết cho bên ngoài gọi.
   - Minh hoạ: `EduHub.Domain/Interfaces/IEventRepository.cs` — interface `IEventRepository`.

3. **Kế thừa (Inheritance):** Lớp con thừa hưởng thuộc tính và phương thức từ lớp cha để tái sử dụng và mở rộng code.
   - Minh hoạ: `EduHub.API/Controllers/EventsController.cs` — lớp `EventsController` (kế thừa `ControllerBase`).

4. **Đa hình (Polymorphism):** Cùng một lời gọi phương thức từ giao diện chung nhưng các lớp con khác nhau sẽ thực hiện theo cách riêng.
   - Minh hoạ: `EduHub.Infrastructure/Notifications/EmailNotificationService.cs` (`EmailNotificationService`) và `SmsNotificationService.cs` (`SmsNotificationService`).

## Câu 1b

Hai vấn đề của lớp `Member` chưa đóng gói:
1. Các thuộc tính `Id` và `Email` để `public { get; set; }` nên bên ngoài có thể gán hoặc sửa trực tiếp dữ liệu sai quy tắc (như `m.Email = "abc"`).
2. Constructor `public Member() { }` cho phép khởi tạo đối tượng rỗng không hợp lệ từ bên ngoài (`Id` bị `Guid.Empty`, `Email` chưa có giá trị) mà không qua bước kiểm tra.


## Câu 2a

```mermaid
stateDiagram-v2
    [*] --> Pending : Create()
    Pending --> Confirmed : Confirm()
    Pending --> Cancelled : Cancel()
    Confirmed --> Cancelled : Cancel()
    Cancelled --> [*]
```
