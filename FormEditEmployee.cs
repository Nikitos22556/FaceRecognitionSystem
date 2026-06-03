using System;
using System.Windows.Forms;
using ДипломNikitos.DataBase;
using ДипломNikitos.Models;

namespace ДипломNikitos
{
    public partial class FormEditEmployee : Form
    {
        private Employee employee;
        private DatabaseHelper dbHelper;

        public FormEditEmployee(Employee emp)
        {
            InitializeComponent();
            employee = emp;
            dbHelper = new DatabaseHelper();
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            txtLastName.Text = employee.LastName;
            txtFirstName.Text = employee.FirstName;
            txtPatronymic.Text = employee.Patronymic ?? "";
            txtPhone.Text = employee.PhoneNumber;
            txtEmail.Text = employee.Email ?? "";
            chkActive.Checked = employee.IsActive;

            // Устанавливаем должность в ComboBox
            if (cmbRole.Items.Contains(employee.Role))
            {
                cmbRole.SelectedItem = employee.Role;
            }
            else
            {
                cmbRole.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите имя!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите телефон!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Обновляем данные сотрудника
            employee.LastName = txtLastName.Text.Trim();
            employee.FirstName = txtFirstName.Text.Trim();
            employee.Patronymic = string.IsNullOrWhiteSpace(txtPatronymic.Text) ? null : txtPatronymic.Text.Trim();
            employee.PhoneNumber = txtPhone.Text.Trim();
            employee.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            employee.Role = cmbRole.SelectedItem.ToString();
            employee.IsActive = chkActive.Checked;

            bool success = dbHelper.UpdateEmployee(employee);

            if (success)
            {
                MessageBox.Show("✅ Данные сотрудника успешно обновлены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("❌ Ошибка при сохранении данных!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Отменить изменения? Несохранённые данные будут потеряны.",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
