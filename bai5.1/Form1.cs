using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // Class mô hình Sản phẩm
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
    }

    public partial class Form1 : Form
    {
        // Khai báo các Control và Dữ liệu
        private TextBox txtId, txtName, txtPrice, txtQuantity, txtSearch;
        private ComboBox cboCategory;
        private Button btnAdd, btnEdit, btnDelete, btnSearch, btnRefresh;
        private DataGridView dgvProducts;

        private BindingList<Product> products;
        private BindingSource bindingSource;

        public Form1()
        {
            InitializeComponent();
        }

        // Sự kiện Form_Load tự động chạy khi mở Form
        private void Form1_Load(object sender, EventArgs e)
        {
            // Cấu hình thuộc tính cơ bản của Form
            this.Text = "QUẢN LÝ DANH SÁCH SẢN PHẨM";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Khởi tạo nguồn dữ liệu
            products = new BindingList<Product>();
            bindingSource = new BindingSource { DataSource = products };

            // Tạo giao diện người dùng tự động
            BuildUI();

            // Gán dữ liệu vào DataGridView và định dạng cột
            dgvProducts.DataSource = bindingSource;
            FormatGridViewHeaders();
        }

        // Tạo toàn bộ Control và gán vào Form bằng Code
        private void BuildUI()
        {
            // 1. GroupBox Thông tin sản phẩm
            GroupBox gbInfo = new GroupBox
            {
                Text = "Thông tin sản phẩm",
                Location = new Point(20, 20),
                Size = new Size(940, 150)
            };

            Label lblId = MakeLabel("Mã SP:", 20, 30);
            txtId = MakeTextBox(100, 27);

            Label lblName = MakeLabel("Tên SP:", 320, 30);
            txtName = MakeTextBox(400, 27);

            Label lblPrice = MakeLabel("Đơn giá:", 20, 75);
            txtPrice = MakeTextBox(100, 72);

            Label lblQuantity = MakeLabel("Số lượng:", 320, 75);
            txtQuantity = MakeTextBox(400, 72);

            Label lblCategory = MakeLabel("Danh mục:", 620, 30);

            cboCategory = new ComboBox
            {
                Location = new Point(700, 27),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboCategory.Items.AddRange(new object[]
            {
                "Điện thoại",
                "Laptop",
                "Phụ kiện",
                "Thiết bị khác"
            });
            cboCategory.SelectedIndex = 0;

            gbInfo.Controls.AddRange(new Control[]
            {
                lblId, txtId,
                lblName, txtName,
                lblPrice, txtPrice,
                lblQuantity, txtQuantity,
                lblCategory, cboCategory
            });

            // 2. GroupBox Chức năng
            GroupBox gbFunction = new GroupBox
            {
                Text = "Chức năng",
                Location = new Point(20, 185),
                Size = new Size(940, 70)
            };

            btnAdd = MakeButton("Thêm", 20, 25);
            btnEdit = MakeButton("Sửa", 115, 25);
            btnDelete = MakeButton("Xóa", 210, 25);
            btnRefresh = MakeButton("Làm mới", 305, 25);

            txtSearch = new TextBox
            {
                Location = new Point(500, 27),
                Width = 220
            };

            btnSearch = MakeButton("Tìm kiếm", 730, 23);

            // Gán sự kiện Click
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSearch.Click += BtnSearch_Click;

            gbFunction.Controls.AddRange(new Control[]
            {
                btnAdd, btnEdit, btnDelete, btnRefresh,
                txtSearch, btnSearch
            });

            // 3. DataGridView hiển thị danh sách
            dgvProducts = new DataGridView
            {
                Location = new Point(20, 275),
                Size = new Size(940, 310),
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvProducts.CellClick += DgvProducts_CellClick;

            // Thêm các thành phần vào Form
            this.Controls.Add(gbInfo);
            this.Controls.Add(gbFunction);
            this.Controls.Add(dgvProducts);
        }

        // Hàm hỗ trợ khởi tạo nhanh Label, TextBox, Button
        private Label MakeLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true };
        }

        private TextBox MakeTextBox(int x, int y)
        {
            return new TextBox { Location = new Point(x, y), Width = 200 };
        }

        private Button MakeButton(string text, int x, int y)
        {
            return new Button { Text = text, Location = new Point(x, y), Size = new Size(85, 35) };
        }

        // Định dạng tiêu đề cột
        private void FormatGridViewHeaders()
        {
            if (dgvProducts.Columns["ProductId"] != null)
                dgvProducts.Columns["ProductId"].HeaderText = "Mã SP";

            if (dgvProducts.Columns["ProductName"] != null)
                dgvProducts.Columns["ProductName"].HeaderText = "Tên Sản Phẩm";

            if (dgvProducts.Columns["UnitPrice"] != null)
            {
                dgvProducts.Columns["UnitPrice"].HeaderText = "Đơn Giá";
                dgvProducts.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            }

            if (dgvProducts.Columns["Quantity"] != null)
                dgvProducts.Columns["Quantity"].HeaderText = "Số Lượng";

            if (dgvProducts.Columns["Category"] != null)
                dgvProducts.Columns["Category"].HeaderText = "Danh Mục";
        }

        // Kiểm tra dữ liệu đầu vào
        private bool ValidateInput(out decimal price, out int quantity)
        {
            price = 0;
            quantity = 0;

            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out price) || price < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ (phải là số dương)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ (phải là số nguyên dương)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // Chức năng Thêm
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out decimal price, out int quantity))
                return;

            string id = txtId.Text.Trim();

            if (products.Any(x => x.ProductId.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Product p = new Product
            {
                ProductId = id,
                ProductName = txtName.Text.Trim(),
                UnitPrice = price,
                Quantity = quantity,
                Category = cboCategory.SelectedItem?.ToString() ?? cboCategory.Text
            };

            products.Add(p);
            ClearInput();
        }

        // Chức năng Sửa
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Product p = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (p == null) return;

            if (!ValidateInput(out decimal price, out int quantity))
                return;

            string newId = txtId.Text.Trim();
            if (!p.ProductId.Equals(newId, StringComparison.OrdinalIgnoreCase) &&
                products.Any(x => x.ProductId.Equals(newId, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm mới bị trùng với sản phẩm khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            p.ProductId = newId;
            p.ProductName = txtName.Text.Trim();
            p.UnitPrice = price;
            p.Quantity = quantity;
            p.Category = cboCategory.SelectedItem?.ToString() ?? cboCategory.Text;

            bindingSource.ResetBindings(false);
            ClearInput();
        }

        // Chức năng Xóa
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Product p = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (p == null) return;

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm [{p.ProductName}]?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                products.Remove(p);
                ClearInput();
            }
        }

        // Chức năng Tìm kiếm
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                bindingSource.DataSource = products;
                return;
            }

            var filteredList = products
                .Where(p => p.ProductName.ToLower().Contains(keyword) || p.ProductId.ToLower().Contains(keyword))
                .ToList();

            bindingSource.DataSource = new BindingList<Product>(filteredList);
        }

        // Chức năng Làm mới
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            bindingSource.DataSource = products;
            ClearInput();
        }

        // Click trên dòng DataGridView để hiển thị ngược lại dữ liệu
        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Product p = dgvProducts.Rows[e.RowIndex].DataBoundItem as Product;
            if (p == null) return;

            txtId.Text = p.ProductId;
            txtName.Text = p.ProductName;
            txtPrice.Text = p.UnitPrice.ToString("0.##");
            txtQuantity.Text = p.Quantity.ToString();

            if (cboCategory.Items.Contains(p.Category))
            {
                cboCategory.SelectedItem = p.Category;
            }
            else
            {
                cboCategory.Text = p.Category;
            }
        }

        // Xóa trắng ô nhập liệu
        private void ClearInput()
        {
            txtId.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = 0;
            dgvProducts.ClearSelection();
        }
    }
}