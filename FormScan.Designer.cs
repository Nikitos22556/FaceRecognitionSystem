namespace ДипломNikitos
{
    partial class FormScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelVideo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panelConfidence = new System.Windows.Forms.Panel();
            this.lblConfidenceValue = new System.Windows.Forms.Label();
            this.progressConfidence = new System.Windows.Forms.ProgressBar();
            this.lblConfidenceTitle = new System.Windows.Forms.Label();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.panelEmployeeInfo = new System.Windows.Forms.Panel();
            this.pictureBoxFace = new System.Windows.Forms.PictureBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmailTitle = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblPhoneTitle = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblRoleTitle = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblFullNameTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRefreshModel = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelInfo.SuspendLayout();
            this.panelConfidence.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelEmployeeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelVideo
            // 
            this.panelVideo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVideo.Controls.Add(this.pictureBox1);
            this.panelVideo.Location = new System.Drawing.Point(12, 12);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(640, 480);
            this.panelVideo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(638, 478);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfo.Controls.Add(this.panelConfidence);
            this.panelInfo.Controls.Add(this.panelStatus);
            this.panelInfo.Controls.Add(this.panelEmployeeInfo);
            this.panelInfo.Controls.Add(this.lblTitle);
            this.panelInfo.Location = new System.Drawing.Point(660, 12);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(310, 480);
            this.panelInfo.TabIndex = 1;
            // 
            // panelConfidence
            // 
            this.panelConfidence.Controls.Add(this.lblConfidenceValue);
            this.panelConfidence.Controls.Add(this.progressConfidence);
            this.panelConfidence.Controls.Add(this.lblConfidenceTitle);
            this.panelConfidence.Location = new System.Drawing.Point(10, 335);
            this.panelConfidence.Name = "panelConfidence";
            this.panelConfidence.Size = new System.Drawing.Size(290, 40);
            this.panelConfidence.TabIndex = 5;
            // 
            // lblConfidenceValue
            // 
            this.lblConfidenceValue.Location = new System.Drawing.Point(250, 10);
            this.lblConfidenceValue.Name = "lblConfidenceValue";
            this.lblConfidenceValue.Size = new System.Drawing.Size(35, 20);
            this.lblConfidenceValue.TabIndex = 2;
            this.lblConfidenceValue.Text = "0%";
            this.lblConfidenceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressConfidence
            // 
            this.progressConfidence.Location = new System.Drawing.Point(95, 10);
            this.progressConfidence.Name = "progressConfidence";
            this.progressConfidence.Size = new System.Drawing.Size(185, 20);
            this.progressConfidence.TabIndex = 1;
            // 
            // lblConfidenceTitle
            // 
            this.lblConfidenceTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConfidenceTitle.Location = new System.Drawing.Point(5, 10);
            this.lblConfidenceTitle.Name = "lblConfidenceTitle";
            this.lblConfidenceTitle.Size = new System.Drawing.Size(90, 20);
            this.lblConfidenceTitle.TabIndex = 0;
            this.lblConfidenceTitle.Text = "Уверенность:";
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.LightYellow;
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.lblStatus);
            this.panelStatus.Controls.Add(this.lblStatusTitle);
            this.panelStatus.Location = new System.Drawing.Point(10, 260);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(290, 60);
            this.panelStatus.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(70, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(210, 45);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Ожидание запуска";
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatusTitle.Location = new System.Drawing.Point(5, 5);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(60, 20);
            this.lblStatusTitle.TabIndex = 0;
            this.lblStatusTitle.Text = "СТАТУС:";
            // 
            // panelEmployeeInfo
            // 
            this.panelEmployeeInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelEmployeeInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEmployeeInfo.Controls.Add(this.pictureBoxFace);
            this.panelEmployeeInfo.Controls.Add(this.lblEmail);
            this.panelEmployeeInfo.Controls.Add(this.lblEmailTitle);
            this.panelEmployeeInfo.Controls.Add(this.lblPhone);
            this.panelEmployeeInfo.Controls.Add(this.lblPhoneTitle);
            this.panelEmployeeInfo.Controls.Add(this.lblRole);
            this.panelEmployeeInfo.Controls.Add(this.lblRoleTitle);
            this.panelEmployeeInfo.Controls.Add(this.lblFullName);
            this.panelEmployeeInfo.Controls.Add(this.lblFullNameTitle);
            this.panelEmployeeInfo.Location = new System.Drawing.Point(10, 45);
            this.panelEmployeeInfo.Name = "panelEmployeeInfo";
            this.panelEmployeeInfo.Size = new System.Drawing.Size(290, 200);
            this.panelEmployeeInfo.TabIndex = 3;
            // 
            // pictureBoxFace
            // 
            this.pictureBoxFace.BackColor = System.Drawing.Color.LightGray;
            this.pictureBoxFace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFace.Location = new System.Drawing.Point(90, 135);
            this.pictureBoxFace.Name = "pictureBoxFace";
            this.pictureBoxFace.Size = new System.Drawing.Size(110, 50);
            this.pictureBoxFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFace.TabIndex = 8;
            this.pictureBoxFace.TabStop = false;
            // 
            // lblEmail
            // 
            this.lblEmail.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblEmail.Location = new System.Drawing.Point(100, 105);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(180, 20);
            this.lblEmail.TabIndex = 7;
            this.lblEmail.Text = "-";
            // 
            // lblEmailTitle
            // 
            this.lblEmailTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEmailTitle.Location = new System.Drawing.Point(10, 105);
            this.lblEmailTitle.Name = "lblEmailTitle";
            this.lblEmailTitle.Size = new System.Drawing.Size(80, 20);
            this.lblEmailTitle.TabIndex = 6;
            this.lblEmailTitle.Text = "Email:";
            // 
            // lblPhone
            // 
            this.lblPhone.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblPhone.Location = new System.Drawing.Point(100, 75);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(180, 20);
            this.lblPhone.TabIndex = 5;
            this.lblPhone.Text = "-";
            this.lblPhone.Click += new System.EventHandler(this.lblPhone_Click);
            // 
            // lblPhoneTitle
            // 
            this.lblPhoneTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPhoneTitle.Location = new System.Drawing.Point(10, 75);
            this.lblPhoneTitle.Name = "lblPhoneTitle";
            this.lblPhoneTitle.Size = new System.Drawing.Size(80, 20);
            this.lblPhoneTitle.TabIndex = 4;
            this.lblPhoneTitle.Text = "Телефон:";
            // 
            // lblRole
            // 
            this.lblRole.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblRole.Location = new System.Drawing.Point(100, 45);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(180, 13);
            this.lblRole.TabIndex = 3;
            this.lblRole.Text = "-";
            // 
            // lblRoleTitle
            // 
            this.lblRoleTitle.AutoSize = true;
            this.lblRoleTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRoleTitle.Location = new System.Drawing.Point(10, 45);
            this.lblRoleTitle.Name = "lblRoleTitle";
            this.lblRoleTitle.Size = new System.Drawing.Size(78, 13);
            this.lblRoleTitle.TabIndex = 2;
            this.lblRoleTitle.Text = "Должность:";
            // 
            // lblFullName
            // 
            this.lblFullName.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblFullName.Location = new System.Drawing.Point(100, 15);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(180, 20);
            this.lblFullName.TabIndex = 1;
            this.lblFullName.Text = "-";
            // 
            // lblFullNameTitle
            // 
            this.lblFullNameTitle.AutoSize = true;
            this.lblFullNameTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFullNameTitle.Location = new System.Drawing.Point(10, 15);
            this.lblFullNameTitle.Name = "lblFullNameTitle";
            this.lblFullNameTitle.Size = new System.Drawing.Size(41, 13);
            this.lblFullNameTitle.TabIndex = 0;
            this.lblFullNameTitle.Text = "ФИО:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(179, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "ИНФОРМАЦИЯ О СОТРУДНИКЕ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnRefreshModel);
            this.panel1.Controls.Add(this.btnRegister);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Location = new System.Drawing.Point(12, 500);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(958, 50);
            this.panel1.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.LightGray;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(830, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = " ❌ ВЫХОД";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRefreshModel
            // 
            this.btnRefreshModel.BackColor = System.Drawing.Color.LightYellow;
            this.btnRefreshModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshModel.Location = new System.Drawing.Point(420, 10);
            this.btnRefreshModel.Name = "btnRefreshModel";
            this.btnRefreshModel.Size = new System.Drawing.Size(150, 30);
            this.btnRefreshModel.TabIndex = 3;
            this.btnRefreshModel.Text = "🔄 ОБУЧИТЬ МОДЕЛЬ";
            this.btnRefreshModel.UseVisualStyleBackColor = false;
            this.btnRefreshModel.Click += new System.EventHandler(this.btnRefreshModel_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.LightBlue;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Location = new System.Drawing.Point(270, 10);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(140, 30);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "➕ РЕГИСТРАЦИЯ";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(140, 10);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 30);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "⏹ СТОП";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(10, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "▶ СТАРТ";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // FormScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelVideo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormScan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сканирование";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormScan_FormClosing);
            this.panelVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelConfidence.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.panelEmployeeInfo.ResumeLayout(false);
            this.panelEmployeeInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelVideo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelEmployeeInfo;
        private System.Windows.Forms.Label lblFullNameTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblRoleTitle;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblPhoneTitle;
        private System.Windows.Forms.Label lblEmailTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.PictureBox pictureBoxFace;
        private System.Windows.Forms.Panel panelConfidence;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblConfidenceValue;
        private System.Windows.Forms.ProgressBar progressConfidence;
        private System.Windows.Forms.Label lblConfidenceTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRefreshModel;
        private System.Windows.Forms.Button btnRegister;
    }
}