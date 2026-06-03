using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ДипломNikitos.DataBase;
using ДипломNikitos.Models;

namespace ДипломNikitos
{
    public partial class FormRegister : Form
    {
        private VideoCapture capture;
        private Timer timer;
        private CascadeClassifier faceDetector;
        private DatabaseHelper dbHelper;

        private byte[] capturedFaceImage;
        private Panel panelCamera;
        private PictureBox pictureBoxCamera;
        private Panel panelCaptured;
        private PictureBox pictureBoxFace;
        private Label lblCapturedTitle;
        private Panel panelEmployee;
        private TextBox txtLastName;
        private Label lblLastName;
        private Label lblEmployeeTitle;
        private Label lblPatronymic;
        private Label lblFirstName;
        private TextBox txtPatronymic;
        private TextBox txtFirstName;
        private TextBox txtPhone;
        private Label lblPhone;
        private Label lblEmail;
        private Label lblRole;
        private TextBox txtEmail;
        private ComboBox cmbRole;
        private Panel panelButtons;
        private Button btnSave;
        private Button btnCapture;
        private Button btnCancel;
        private Button btnClear;
        private Label lblStatus;
        private bool faceCaptured;

        public FormRegister()
        {
            InitializeComponent();
            InitializeRegistration();

            // ЭТОТ КОД НУЖНО УДАЛИТЬ ИЗ InitializeComponent()
            Timer statusTimer = new Timer();
            statusTimer.Interval = 3000;
            statusTimer.Tick += (s, args) =>
            {
                if (lblStatus.Text != "Готов к регистрации. Заполните данные и нажмите 'Захватить лицо'")
                {
                    lblStatus.Text = "Готов к регистрации. Заполните данные и нажмите 'Захватить лицо'";
                    lblStatus.BackColor = Color.LightYellow;
                    lblStatus.ForeColor = Color.Black;
                }
                statusTimer.Stop();
                statusTimer.Dispose();
            };

        }

        private void InitializeRegistration()
        {
            dbHelper = new DatabaseHelper();

            string haarPath = Application.StartupPath + @"\haarcascade_frontalface_default.xml";
            faceDetector = new CascadeClassifier(haarPath);

            // Запускаем камеру
            StartCamera();
        }

        private void StartCamera()
        {
            try
            {
                capture = new VideoCapture(0);
                if (capture.IsOpened())
                {
                    timer = new Timer();
                    timer.Interval = 50;
                    timer.Tick += CameraTimer_Tick;
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("Не удалось открыть камеру!", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска камеры: {ex.Message}", "Ошибка");
            }
        }

        private void CameraTimer_Tick(object sender, EventArgs e)
        {
            if (capture == null || !capture.IsOpened()) return;

            using (Mat frame = new Mat())
            {
                capture.Read(frame);
                if (frame.Empty()) return;

                // Ищем лица и рисуем рамку
                using (Mat grayFrame = new Mat())
                {
                    Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);
                    var faces = faceDetector.DetectMultiScale(grayFrame, 1.1, 6);

                    foreach (var face in faces)
                    {
                        Cv2.Rectangle(frame, face, new Scalar(0, 255, 0), 2);
                    }

                    // Отображаем видео (если есть pictureBoxPreview)
                     pictureBoxCamera.Image = BitmapConverter.ToBitmap(frame);

                    // Если нужно захватить лицо
                    if (faceCaptured && faces.Length > 0)
                    {
                        using (Mat faceMat = new Mat(frame, faces[0]))
                        using (var bitmap = BitmapConverter.ToBitmap(faceMat))
                        using (var ms = new System.IO.MemoryStream())
                        {
                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            capturedFaceImage = ms.ToArray();
                        }

                        pictureBoxFace.Image = BitmapConverter.ToBitmap(new Mat(frame, faces[0]));

                        faceCaptured = false;
                        btnSave.Enabled = true;
                        MessageBox.Show("Лицо захвачено! Нажмите Сохранить.", "Успех");
                    }
                }
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите фамилию и имя сотрудника!", "Ошибка");
                return;
            }

            faceCaptured = true;
            lblStatus.Text = "Смотрите в камеру...";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (capturedFaceImage == null)
            {
                MessageBox.Show("Сначала захватите лицо!", "Ошибка");
                return;
            }

            var employee = new Employee
            {
                LastName = txtLastName.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                Patronymic = string.IsNullOrWhiteSpace(txtPatronymic.Text) ? null : txtPatronymic.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                Role = cmbRole.SelectedItem?.ToString() ?? "Сотрудник",
                IsActive = true
            };

            bool success = dbHelper.RegisterEmployeeWithFace(employee, capturedFaceImage);

            if (success)
            {
                MessageBox.Show($"Сотрудник {employee.LastName} {employee.FirstName} зарегистрирован!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка регистрации!", "Ошибка");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLastName.Clear();
            txtFirstName.Clear();
            txtPatronymic.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            cmbRole.SelectedIndex = 0;
            pictureBoxFace.Image = null;
            capturedFaceImage = null;
            btnSave.Enabled = false;
            btnCapture.Enabled = true;
            btnCapture.Text = "📸 ЗАХВАТИТЬ ЛИЦО";
            lblStatus.Text = "Все данные очищены. Готов к новой регистрации.";
            lblStatus.BackColor = Color.LightYellow;
            lblStatus.ForeColor = Color.Black;
            txtLastName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите отменить регистрацию?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void FormRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer?.Stop();
            capture?.Release();
            capture?.Dispose();
            faceDetector?.Dispose();
        }

        private void InitializeComponent()
        {
            this.panelCamera = new System.Windows.Forms.Panel();
            this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
            this.panelCaptured = new System.Windows.Forms.Panel();
            this.lblCapturedTitle = new System.Windows.Forms.Label();
            this.pictureBoxFace = new System.Windows.Forms.PictureBox();
            this.panelEmployee = new System.Windows.Forms.Panel();
            this.lblEmployeeTitle = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtPatronymic = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblPatronymic = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            this.panelCaptured.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).BeginInit();
            this.panelEmployee.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCamera
            // 
            this.panelCamera.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCamera.Controls.Add(this.pictureBoxCamera);
            this.panelCamera.Location = new System.Drawing.Point(12, 12);
            this.panelCamera.Name = "panelCamera";
            this.panelCamera.Size = new System.Drawing.Size(450, 350);
            this.panelCamera.TabIndex = 0;
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCamera.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(448, 348);
            this.pictureBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCamera.TabIndex = 0;
            this.pictureBoxCamera.TabStop = false;
            // 
            // panelCaptured
            // 
            this.panelCaptured.BackColor = System.Drawing.Color.LightGray;
            this.panelCaptured.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCaptured.Controls.Add(this.pictureBoxFace);
            this.panelCaptured.Controls.Add(this.lblCapturedTitle);
            this.panelCaptured.Location = new System.Drawing.Point(480, 12);
            this.panelCaptured.Name = "panelCaptured";
            this.panelCaptured.Size = new System.Drawing.Size(290, 170);
            this.panelCaptured.TabIndex = 1;
            // 
            // lblCapturedTitle
            // 
            this.lblCapturedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCapturedTitle.Location = new System.Drawing.Point(10, 5);
            this.lblCapturedTitle.Name = "lblCapturedTitle";
            this.lblCapturedTitle.Size = new System.Drawing.Size(270, 20);
            this.lblCapturedTitle.TabIndex = 0;
            this.lblCapturedTitle.Text = "ЗАХВАЧЕННОЕ ЛИЦО";
            this.lblCapturedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxFace
            // 
            this.pictureBoxFace.BackColor = System.Drawing.Color.White;
            this.pictureBoxFace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFace.Location = new System.Drawing.Point(90, 30);
            this.pictureBoxFace.Name = "pictureBoxFace";
            this.pictureBoxFace.Size = new System.Drawing.Size(110, 110);
            this.pictureBoxFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFace.TabIndex = 1;
            this.pictureBoxFace.TabStop = false;
            // 
            // panelEmployee
            // 
            this.panelEmployee.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelEmployee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEmployee.Controls.Add(this.cmbRole);
            this.panelEmployee.Controls.Add(this.lblEmail);
            this.panelEmployee.Controls.Add(this.lblRole);
            this.panelEmployee.Controls.Add(this.txtEmail);
            this.panelEmployee.Controls.Add(this.txtPhone);
            this.panelEmployee.Controls.Add(this.lblPhone);
            this.panelEmployee.Controls.Add(this.lblPatronymic);
            this.panelEmployee.Controls.Add(this.lblFirstName);
            this.panelEmployee.Controls.Add(this.txtPatronymic);
            this.panelEmployee.Controls.Add(this.txtFirstName);
            this.panelEmployee.Controls.Add(this.txtLastName);
            this.panelEmployee.Controls.Add(this.lblLastName);
            this.panelEmployee.Controls.Add(this.lblEmployeeTitle);
            this.panelEmployee.Location = new System.Drawing.Point(480, 200);
            this.panelEmployee.Name = "panelEmployee";
            this.panelEmployee.Size = new System.Drawing.Size(290, 280);
            this.panelEmployee.TabIndex = 2;
            // 
            // lblEmployeeTitle
            // 
            this.lblEmployeeTitle.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblEmployeeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEmployeeTitle.Location = new System.Drawing.Point(10, 5);
            this.lblEmployeeTitle.Name = "lblEmployeeTitle";
            this.lblEmployeeTitle.Size = new System.Drawing.Size(270, 25);
            this.lblEmployeeTitle.TabIndex = 0;
            this.lblEmployeeTitle.Text = "ДАННЫЕ СОТРУДНИКА";
            this.lblEmployeeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLastName.Location = new System.Drawing.Point(15, 45);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(80, 20);
            this.lblLastName.TabIndex = 1;
            this.lblLastName.Text = "Фамилия:";
            // 
            // txtLastName
            // 
            this.txtLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtLastName.Location = new System.Drawing.Point(105, 42);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(170, 22);
            this.txtLastName.TabIndex = 2;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(105, 77);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(170, 20);
            this.txtFirstName.TabIndex = 3;
            // 
            // txtPatronymic
            // 
            this.txtPatronymic.Location = new System.Drawing.Point(105, 112);
            this.txtPatronymic.Name = "txtPatronymic";
            this.txtPatronymic.Size = new System.Drawing.Size(170, 20);
            this.txtPatronymic.TabIndex = 4;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFirstName.Location = new System.Drawing.Point(15, 80);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(80, 20);
            this.lblFirstName.TabIndex = 5;
            this.lblFirstName.Text = "Имя:";
            // 
            // lblPatronymic
            // 
            this.lblPatronymic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPatronymic.Location = new System.Drawing.Point(15, 115);
            this.lblPatronymic.Name = "lblPatronymic";
            this.lblPatronymic.Size = new System.Drawing.Size(80, 20);
            this.lblPatronymic.TabIndex = 6;
            this.lblPatronymic.Text = "Отчество:";
            // 
            // lblPhone
            // 
            this.lblPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPhone.Location = new System.Drawing.Point(15, 150);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(80, 20);
            this.lblPhone.TabIndex = 7;
            this.lblPhone.Text = "Телефон:";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(105, 147);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(170, 20);
            this.txtPhone.TabIndex = 8;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(105, 182);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(170, 20);
            this.txtEmail.TabIndex = 9;
            // 
            // lblRole
            // 
            this.lblRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRole.Location = new System.Drawing.Point(15, 220);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(80, 20);
            this.lblRole.TabIndex = 10;
            this.lblRole.Text = "Должность:";
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEmail.Location = new System.Drawing.Point(15, 185);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(80, 20);
            this.lblEmail.TabIndex = 11;
            this.lblEmail.Text = "Email:";
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "\"Сотрудник\", \"Менеджер\", \"Администратор\", \"Директор\""});
            this.cmbRole.Location = new System.Drawing.Point(105, 217);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(170, 21);
            this.cmbRole.TabIndex = 12;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnClear);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnCapture);
            this.panelButtons.Location = new System.Drawing.Point(12, 383);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(459, 97);
            this.panelButtons.TabIndex = 3;
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.LightBlue;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCapture.Location = new System.Drawing.Point(72, 12);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(150, 30);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "📸 ЗАХВАТИТЬ ЛИЦО";
            this.btnCapture.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnSave.Enabled = false;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSave.Location = new System.Drawing.Point(242, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 СОХРАНИТЬ";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightYellow;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClear.Location = new System.Drawing.Point(72, 49);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(150, 30);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "🗑 ОЧИСТИТЬ";
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCancel.Location = new System.Drawing.Point(242, 49);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "❌ ОТМЕНА";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.LightYellow;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(12, 499);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(758, 30);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Готов к регистрации. Заполните данные и нажмите \'Захватить лицо\'";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormRegister
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelEmployee);
            this.Controls.Add(this.panelCaptured);
            this.Controls.Add(this.panelCamera);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Регистрация";
            this.panelCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            this.panelCaptured.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).EndInit();
            this.panelEmployee.ResumeLayout(false);
            this.panelEmployee.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // Привязываем событие закрытия формы
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegister_FormClosing);

            // Настраиваем ComboBox (заполняем элементами)
            // Вместо неправильного добавления Items, используйте это:
            this.cmbRole.Items.Clear();
            this.cmbRole.Items.Add("Сотрудник");
            this.cmbRole.Items.Add("Менеджер");
            this.cmbRole.Items.Add("Администратор");
            this.cmbRole.Items.Add("Директор");
            this.cmbRole.SelectedIndex = 0;

            // Запускаем таймер для статуса
            

        }
    }
}