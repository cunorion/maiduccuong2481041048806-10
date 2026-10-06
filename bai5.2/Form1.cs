using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    // Class mô hình Dịch vụ
    public class Service
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Service(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return Name + " - " + Price.ToString("N0") + " đ";
        }
    }

    public partial class Form1 : Form
    {
        // Khai báo các Control và Dữ liệu
        ComboBox cboCategory;
        ListBox lstAvailableServices, lstSelectedServices;
        Button btnSelect, btnRemove, btnClearAll;
        Label lblTotal, lblPayment;
        NumericUpDown nudDiscount;

        Dictionary<string, List<Service>> data;

        public Form1()
        {
            InitializeComponent();
        }

        // Sự kiện Form1_Load tự động khởi tạo giao diện khi mở Form
        private void Form1_Load(object sender, EventArgs e)
        {
            // Thiết lập thông số cơ bản cửa sổ
            this.Text = "BẢNG TÍNH TIỀN DỊCH VỤ";
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Khởi tạo danh sách dữ liệu dịch vụ
            data = new Dictionary<string, List<Service>>();

            data["Khám bệnh"] = new List<Service>
            {
                new Service("Khám tổng quát", 200000),
                new Service("Khám chuyên khoa", 300000),
                new Service("Khám cấp cứu", 500000)
            };

            data["Xét nghiệm"] = new List<Service>
            {
                new Service("Xét nghiệm máu", 150000),
                new Service("Xét nghiệm nước tiểu", 100000),
                new Service("Xét nghiệm đường huyết", 120000)
            };

            data["Chụp X-Quang"] = new List<Service>
            {
                new Service("X-Quang ngực", 250000),
                new Service("X-Quang xương", 300000),
                new Service("X-Quang cột sống", 400000)
            };

            data["Vắc-xin"] = new List<Service>
            {
                new Service("Vắc-xin cúm", 300000),
                new Service("Vắc-xin viêm gan B", 250000),
                new Service("Vắc-xin HPV", 1500000)
            };

            // Tạo các thành phần giao diện
            Label lblCategory = new Label
            {
                Text = "Loại dịch vụ:",
                Location = new Point(30, 25),
                AutoSize = true
            };

            cboCategory = new ComboBox
            {
                Location = new Point(130, 20),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cboCategory.Items.AddRange(new object[]
            {
                "Khám bệnh",
                "Xét nghiệm",
                "Chụp X-Quang",
                "Vắc-xin"
            });

            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;

            Label lblAvailable = new Label
            {
                Text = "Dịch vụ có sẵn",
                Location = new Point(30, 70),
                AutoSize = true
            };

            lstAvailableServices = new ListBox
            {
                Location = new Point(30, 100),
                Size = new Size(300, 280)
            };

            lstAvailableServices.DoubleClick += LstAvailableServices_DoubleClick;

            Label lblSelected = new Label
            {
                Text = "Dịch vụ đã chọn",
                Location = new Point(490, 70),
                AutoSize = true
            };

            lstSelectedServices = new ListBox
            {
                Location = new Point(490, 100),
                Size = new Size(300, 280)
            };

            btnSelect = new Button
            {
                Text = ">",
                Location = new Point(370, 150),
                Size = new Size(80, 40)
            };

            btnRemove = new Button
            {
                Text = "<",
                Location = new Point(370, 210),
                Size = new Size(80, 40)
            };

            btnClearAll = new Button
            {
                Text = "<<",
                Location = new Point(370, 270),
                Size = new Size(80, 40)
            };

            btnSelect.Click += BtnSelect_Click;
            btnRemove.Click += BtnRemove_Click;
            btnClearAll.Click += BtnClearAll_Click;

            Label lblTotalText = new Label
            {
                Text = "Tổng tiền chưa giảm:",
                Location = new Point(30, 410),
                AutoSize = true
            };

            lblTotal = new Label
            {
                Text = "0 đ",
                Location = new Point(180, 410),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            Label lblDiscountText = new Label
            {
                Text = "Chiết khấu (%):",
                Location = new Point(30, 445),
                AutoSize = true
            };

            nudDiscount = new NumericUpDown
            {
                Location = new Point(180, 440),
                Width = 100,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            nudDiscount.ValueChanged += NudDiscount_ValueChanged;

            Label lblPaymentText = new Label
            {
                Text = "Thành tiền:",
                Location = new Point(490, 410),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            lblPayment = new Label
            {
                Text = "0 đ",
                Location = new Point(590, 410),
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold),
                ForeColor = Color.Red
            };

            this.Controls.AddRange(new Control[]
            {
                lblCategory, cboCategory,
                lblAvailable, lstAvailableServices,
                lblSelected, lstSelectedServices,
                btnSelect, btnRemove, btnClearAll,
                lblTotalText, lblTotal,
                lblDiscountText, nudDiscount,
                lblPaymentText, lblPayment
            });

            cboCategory.SelectedIndex = 0;
        }

        private void CboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstAvailableServices.Items.Clear();

            if (cboCategory.SelectedItem != null)
            {
                string category = cboCategory.SelectedItem.ToString();
                if (data.ContainsKey(category))
                {
                    foreach (Service service in data[category])
                    {
                        lstAvailableServices.Items.Add(service);
                    }
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            AddSelectedService();
        }

        private void LstAvailableServices_DoubleClick(object sender, EventArgs e)
        {
            AddSelectedService();
        }

        private void AddSelectedService()
        {
            if (lstAvailableServices.SelectedItem == null)
                return;

            Service service = (Service)lstAvailableServices.SelectedItem;
            lstSelectedServices.Items.Add(service);

            UpdateTotal();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lstSelectedServices.SelectedItem != null)
            {
                lstSelectedServices.Items.Remove(lstSelectedServices.SelectedItem);
                UpdateTotal();
            }
        }

        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            lstSelectedServices.Items.Clear();
            UpdateTotal();
        }

        private void NudDiscount_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotal();
        }

        // Hàm tính tổng tiền và thành tiền đã được sửa lỗi cú pháp
        private void UpdateTotal()
        {
            decimal total = 0;

            foreach (Service service in lstSelectedServices.Items)
            {
                total += service.Price;
            }

            // Tính số tiền được chiết khấu và thành tiền sau giảm giá
            decimal discountAmount = total * nudDiscount.Value / 100m;
            decimal payment = total - discountAmount;

            lblTotal.Text = total.ToString("N0") + " đ";
            lblPayment.Text = payment.ToString("N0") + " đ";
        }
    }
}