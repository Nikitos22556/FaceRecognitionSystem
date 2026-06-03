using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ДипломNikitos.DataBase;
using ДипломNikitos.Models;

namespace ДипломNikitos
{
    public partial class FormAdmin : Form
    {
        private DatabaseHelper dbHelper;
        private DataTable employeesTable;

        public FormAdmin()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            dbHelper = new DatabaseHelper();

            // Настройка таблицы
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.MultiSelect = false;
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.ReadOnly = true;
            dgvEmployees.RowHeadersVisible = false;

            // Загружаем данные
            LoadEmployees();
        }

        private void LoadEmployees(string searchText = "")
        {
            try
            {
                var employees = dbHelper.GetAllEmployees();

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Исправлено: убираем StringComparison, используем простой ToLower()
                    employees = employees.Where(e =>
                        e.LastName.ToLower().Contains(searchText.ToLower()) ||
                        e.FirstName.ToLower().Contains(searchText.ToLower()) ||
                        (e.Patronymic != null && e.Patronymic.ToLower().Contains(searchText.ToLower())) ||
                        e.PhoneNumber.Contains(searchText)).ToList();

                    lblStatusLeft.Text = $"🔍 Найдено: {employees.Count} сотрудников";
                }
                else
                {
                    lblStatusLeft.Text = "✅ Готов к работе";
                }

                // Создаём DataTable для отображения
                employeesTable = new DataTable();
                employeesTable.Columns.Add("Id", typeof(int));
                employeesTable.Columns.Add("Фамилия", typeof(string));
                employeesTable.Columns.Add("Имя", typeof(string));
                employeesTable.Columns.Add("Отчество", typeof(string));
                employeesTable.Columns.Add("Телефон", typeof(string));
                employeesTable.Columns.Add("Email", typeof(string));
                employeesTable.Columns.Add("Должность", typeof(string));
                employeesTable.Columns.Add("Статус", typeof(string));
                employeesTable.Columns.Add("Фото", typeof(string));

                int activeCount = 0;
                int inactiveCount = 0;
                int hasFaceCount = 0;

                var faces = dbHelper.GetAllEmployeesWithFaces();

                foreach (var emp in employees)
                {
                    bool hasFace = faces.Any(f => f.EmployeeId == emp.Id);
                    if (hasFace) hasFaceCount++;

                    string status = emp.IsActive ? "● Активен" : "○ Уволен";
                    string faceStatus = hasFace ? "✅ Есть" : "❌ Нет";

                    if (emp.IsActive) activeCount++;
                    else inactiveCount++;

                    employeesTable.Rows.Add(
                        emp.Id, emp.LastName, emp.FirstName, emp.Patronymic ?? "—",
                        emp.PhoneNumber, emp.Email ?? "—", emp.Role, status, faceStatus);
                }

                dgvEmployees.DataSource = employeesTable;

                // Скрываем столбец Id
                dgvEmployees.Columns["Id"].Visible = false;

                // Настройка цветов строк
                foreach (DataGridViewRow row in dgvEmployees.Rows)
                {
                    if (row.Cells["Статус"].Value?.ToString() == "○ Уволен")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                    }
                }

                // Обновляем счётчик
                lblRecordCount.Text = $"👥 Активных: {activeCount} | Уволенных: {inactiveCount} | 📸 С фото: {hasFaceCount}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatusLeft.Text = "❌ Ошибка загрузки данных";
            }
        }

        // ========== ОБНОВЛЕНИЕ ВРЕМЕНИ ==========
        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        // ========== КНОПКИ ==========

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadEmployees();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadEmployees(txtSearch.Text.Trim());
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoadEmployees(txtSearch.Text.Trim());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FormRegister registerForm = new FormRegister())
            {
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    LoadEmployees();
                    MessageBox.Show("✅ Сотрудник успешно добавлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null)
            {
                MessageBox.Show("⚠️ Выберите сотрудника для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int employeeId = (int)dgvEmployees.CurrentRow.Cells["Id"].Value;
            var employee = dbHelper.GetAllEmployees().FirstOrDefault(emp => emp.Id == employeeId);

            if (employee != null)
            {
                using (FormEditEmployee editForm = new FormEditEmployee(employee))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadEmployees();
                        MessageBox.Show("✅ Данные обновлены!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow == null)
            {
                MessageBox.Show("⚠️ Выберите сотрудника для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int employeeId = (int)dgvEmployees.CurrentRow.Cells["Id"].Value;
            string fullName = $"{dgvEmployees.CurrentRow.Cells["Фамилия"].Value} {dgvEmployees.CurrentRow.Cells["Имя"].Value}";

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить сотрудника?\n\n{fullName}\n\nЭто действие нельзя отменить!",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                bool success = dbHelper.DeleteEmployee(employeeId);
                if (success)
                {
                    LoadEmployees();
                    MessageBox.Show("🗑 Сотрудник удалён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("❌ Ошибка при удалении!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Функция экспорта в Excel (будет добавлена позже)
            MessageBox.Show("📊 Экспорт в Excel будет добавлен в следующей версии", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            // Функция экспорта в CSV
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV Files|*.csv";
                    sfd.Title = "Сохранить CSV";
                    sfd.FileName = $"Сотрудники_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var writer = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // Заголовки
                            writer.WriteLine("ID;Фамилия;Имя;Отчество;Телефон;Email;Должность;Статус;Фото");

                            // Данные
                            foreach (DataGridViewRow row in dgvEmployees.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    string line = $"{row.Cells["Id"].Value};" +
                                                 $"{row.Cells["Фамилия"].Value};" +
                                                 $"{row.Cells["Имя"].Value};" +
                                                 $"{row.Cells["Отчество"].Value};" +
                                                 $"{row.Cells["Телефон"].Value};" +
                                                 $"{row.Cells["Email"].Value};" +
                                                 $"{row.Cells["Должность"].Value};" +
                                                 $"{row.Cells["Статус"].Value};" +
                                                 $"{row.Cells["Фото"].Value}";
                                    writer.WriteLine(line);
                                }
                            }
                        }
                        MessageBox.Show($"📄 CSV сохранён!\n{sfd.FileName}", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            MessageBox.Show("📋 Функция просмотра логов в разработке", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
