using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Khai báo các thành phần trên Form
        TextBox txtUsername, txtPassword, txtConfirm;
        DateTimePicker dtpBirth;
        RadioButton radMale, radFemale;
        CheckBox chkTerms;
        Button btnRegister, btnReset;
        ErrorProvider epCheck;

        public Form1()
        {
            InitializeComponent();
        }

        // Sự kiện Form_Load tự động tạo giao diện khi mở Form
        private void Form1_Load(object sender, EventArgs e)
        {
            // Cấu hình cửa sổ Form
            this.Text = "ĐĂNG KÝ TÀI KHOẢN";
            this.Size = new Size(520, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            epCheck = new ErrorProvider();

            // 1. GroupBox Thông tin cá nhân
            GroupBox gbPersonal = new GroupBox
            {
                Text = "Thông tin cá nhân",
                Location = new Point(20, 20),
                Size = new Size(455, 180)
            };

            Label lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(20, 30), AutoSize = true };
            txtUsername = new TextBox { Location = new Point(150, 27), Width = 250 };

            Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(20, 70), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(150, 67), Width = 250, UseSystemPasswordChar = true };

            Label lblConfirm = new Label { Text = "Xác nhận mật khẩu:", Location = new Point(20, 110), AutoSize = true };
            txtConfirm = new TextBox { Location = new Point(150, 107), Width = 250, UseSystemPasswordChar = true };

            gbPersonal.Controls.AddRange(new Control[]
            {
                lblUser, txtUsername,
                lblPass, txtPassword,
                lblConfirm, txtConfirm
            });

            // 2. GroupBox Thông tin bổ sung
            GroupBox gbMore = new GroupBox
            {
                Text = "Thông tin bổ sung",
                Location = new Point(20, 215),
                Size = new Size(455, 150)
            };

            Label lblBirth = new Label { Text = "Ngày sinh:", Location = new Point(20, 30), AutoSize = true };
            dtpBirth = new DateTimePicker { Location = new Point(150, 27), Width = 250, Format = DateTimePickerFormat.Short };

            Label lblGender = new Label { Text = "Giới tính:", Location = new Point(20, 70), AutoSize = true };
            radMale = new RadioButton { Text = "Nam", Location = new Point(150, 67), Checked = true };
            radFemale = new RadioButton { Text = "Nữ", Location = new Point(230, 67) };

            chkTerms = new CheckBox { Text = "Tôi đồng ý với điều khoản dịch vụ", Location = new Point(150, 105), AutoSize = true };

            gbMore.Controls.AddRange(new Control[]
            {
                lblBirth, dtpBirth,
                lblGender, radMale, radFemale,
                chkTerms
            });

            // 3. Các Nút chức năng
            btnRegister = new Button { Text = "Đăng Ký", Location = new Point(140, 390), Size = new Size(100, 35) };
            btnReset = new Button { Text = "Làm Mới", Location = new Point(260, 390), Size = new Size(100, 35) };

            // Đăng ký sự kiện Click cho nút bấm
            btnRegister.Click += BtnRegister_Click;
            btnReset.Click += BtnReset_Click;

            // Thêm tất cả vào Form
            this.Controls.Add(gbPersonal);
            this.Controls.Add(gbMore);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnReset);
        }

        // Xử lý kiểm tra dữ liệu khi bấm nút "Đăng Ký"
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            epCheck.Clear();
            bool valid = true;

            // Kiểm tra Tên đăng nhập
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                epCheck.SetError(txtUsername, "Tên đăng nhập không được để trống!");
                valid = false;
            }

            // Kiểm tra Mật khẩu
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                epCheck.SetError(txtPassword, "Mật khẩu không được để trống!");
                valid = false;
            }

            // Kiểm tra Mật khẩu xác nhận
            if (txtPassword.Text != txtConfirm.Text)
            {
                epCheck.SetError(txtConfirm, "Mật khẩu nhập lại không khớp!");
                valid = false;
            }

            // Tính tuổi chính xác
            int age = DateTime.Today.Year - dtpBirth.Value.Year;
            if (dtpBirth.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            // Kiểm tra độ tuổi >= 18
            if (age < 18)
            {
                epCheck.SetError(dtpBirth, "Người đăng ký phải đủ 18 tuổi!");
                valid = false;
            }

            // Kiểm tra đồng ý điều khoản
            if (!chkTerms.Checked)
            {
                epCheck.SetError(chkTerms, "Bạn phải đồng ý với điều khoản dịch vụ!");
                valid = false;
            }

            // Hiển thị thông báo thành công
            if (valid)
            {
                MessageBox.Show(
                    "Đăng ký tài khoản thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        // Xử lý khi bấm nút "Làm Mới"
        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirm.Clear();

            dtpBirth.Value = DateTime.Today;
            radMale.Checked = true;
            chkTerms.Checked = false;

            epCheck.Clear();
            txtUsername.Focus();
        }
    }
}