# thach-thuc-2022
Tài liệu của quán quân 2022, lưu ý: chỉ có tính chất tham khảo, không có tính chất đoán đề

> Chương trình hỗ trợ generate keyword + khoá cho vòng Nối mạng toàn cầu
## Lưu ý
Chương trình còn rất _sơ khai_ nên sẽ không phục vụ hết use case, các bạn có thể tuỳ ý chỉnh sửa và nếu được thì tạo PR lên repo này nha.

## Cấu trúc file
- chr2bin.py: Helper giúp thành viên đầu tiên luyện tập dịch từ ký tự sang nhị phân

Usage: `python3 chr2bin.py`
- mapper.py: Helper giúp chuyển sang t33nc0d3 :)) Các bạn có thể update thêm nếu thích
- prepare-tt.py: Helper giúp download các thành ngữ phổ biến từ trang https://vi.wikiquote.org/wiki/Th%C3%A0nh_ng%E1%BB%AF_Vi%E1%BB%87t_Nam, sau đó sẽ strip hết dấu thanh và lưu vào file `words`
- tt.py: File chính để generate keyword

Usage: `python3 tt.py <khoá>`

Có những mode debug để biết được mình sai ở đâu
  - `python3 tt.py --enc <khoá>` để đọc plaintext từ file `keyword` và encrypt bằng `khoá`
  - `python3 tt.py --dec <khoá>` để đọc encryted text từ file `encrypted` và decrypt ra plaintext bằng `khoá`

Note: Có thể tuỳ chỉnh độ dài của từ khoá bằng biến `KEYWORD_LENGTH` ở đầu file
  
## TODO
Vì hiện tại khâu crawl đã strip hết dấu thanh, nên các từ khoá được generate cũng chưa hỗ trợ dấu thanh :))) sẽ thêm khi author rảnh :^)
