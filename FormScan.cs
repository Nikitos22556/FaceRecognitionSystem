using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ДипломNikitos.DataBase;
using ДипломNikitos.Models;

namespace ДипломNikitos
{
    public partial class FormScan : Form
    {
        private VideoCapture capture;
        private Timer timer;
        private FaceRecognitionService faceService;
        private DatabaseHelper dbHelper;
        private CascadeClassifier faceDetector;
        private bool isProcessing = false;

        // ✅ ФЛАГ ДЛЯ ОДНОРАЗОВОГО РАСПОЗНАВАНИЯ
        private bool isRecognized = false;
        private string recognizedUserName = "";

        public FormScan()
        {
            InitializeComponent();
            InitializeSystem();
        }

        private void InitializeSystem()
        {
            dbHelper = new DatabaseHelper();
            faceService = new FaceRecognitionService();

            string haarPath = Application.StartupPath + @"\haarcascade_frontalface_default.xml";
            if (System.IO.File.Exists(haarPath))
            {
                faceDetector = new CascadeClassifier(haarPath);
                System.Diagnostics.Debug.WriteLine("Каскад загружен успешно!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"ОШИБКА: Каскад не найден по пути {haarPath}");
                MessageBox.Show($"Файл каскада не найден: {haarPath}", "Ошибка");
            }

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;

            LoadModel();
        }

        private void LoadModel()
        {
            try
            {
                faceService.TrainModelFromDatabase();
                SetStatus("Модель загружена", StatusType.Success);
            }
            catch (Exception ex)
            {
                SetStatus($"Ошибка загрузки модели: {ex.Message}", StatusType.Error);
            }
        }

        private void SetStatus(string message, StatusType type)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetStatus(message, type)));
                return;
            }

            if (lblStatus != null)
            {
                lblStatus.Text = message;

                switch (type)
                {
                    case StatusType.Success:
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.BackColor = Color.LightGreen;
                        break;
                    case StatusType.Error:
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.BackColor = Color.LightCoral;
                        break;
                    case StatusType.Info:
                        lblStatus.ForeColor = Color.Blue;
                        lblStatus.BackColor = Color.LightBlue;
                        break;
                    default:
                        lblStatus.ForeColor = Color.Black;
                        lblStatus.BackColor = Color.LightYellow;
                        break;
                }
            }
        }

        private void ClearEmployeeInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearEmployeeInfo));
                return;
            }

            lblFullName.Text = "—";
            lblRole.Text = "—";
            lblPhone.Text = "—";
            lblEmail.Text = "—";
            progressConfidence.Value = 0;
            lblConfidenceValue.Text = "0%";
            pictureBoxFace.Image = null;
        }

        private void SetEmployeeInfo(Employee employee, double confidence)
        {
            System.Diagnostics.Debug.WriteLine($"SetEmployeeInfo вызван! Сотрудник: {employee?.LastName}, Уверенность: {confidence}");

            if (InvokeRequired)
            {
                Invoke(new Action(() => SetEmployeeInfo(employee, confidence)));
                return;
            }

            if (employee != null)
            {
                string fullName = $"{employee.LastName} {employee.FirstName}";
                if (!string.IsNullOrEmpty(employee.Patronymic))
                    fullName += $" {employee.Patronymic}";

                lblFullName.Text = fullName;
                lblRole.Text = employee.Role ?? "—";
                lblPhone.Text = employee.PhoneNumber ?? "—";
                lblEmail.Text = employee.Email ?? "—";

                int confidencePercent = (int)(100 - confidence);
                progressConfidence.Value = Math.Max(0, Math.Min(100, confidencePercent));
                lblConfidenceValue.Text = $"{confidencePercent}%";

                if (confidencePercent >= 70)
                    progressConfidence.ForeColor = Color.Green;
                else if (confidencePercent >= 50)
                    progressConfidence.ForeColor = Color.Orange;
                else
                    progressConfidence.ForeColor = Color.Red;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // ✅ Если уже распознали - выходим
            if (isRecognized) return;

            if (capture == null || isProcessing) return;

            try
            {
                if (!capture.IsOpened())
                {
                    return;
                }
            }
            catch (ObjectDisposedException)
            {
                capture = null;
                return;
            }
            catch (Exception)
            {
                return;
            }

            isProcessing = true;

            try
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame);
                    if (frame.Empty())
                    {
                        isProcessing = false;
                        return;
                    }

                    using (Mat grayFrame = new Mat())
                    {
                        Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

                        var faces = faceDetector.DetectMultiScale(grayFrame, 1.1, 6);

                        System.Diagnostics.Debug.WriteLine($"Найдено лиц на кадре: {faces.Length}");

                        foreach (var face in faces)
                        {
                            Cv2.Rectangle(frame, face, new Scalar(0, 255, 0), 2);
                            Cv2.PutText(frame, "FACE", new OpenCvSharp.Point(face.X, face.Y - 5),
                                HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 255, 0), 1);
                        }

                        if (pictureBox1 != null)
                        {
                            pictureBox1.Image = BitmapConverter.ToBitmap(frame);
                        }

                        if (faces.Length > 0)
                        {
                            var face = faces[0];
                            using (Mat faceMat = new Mat(grayFrame, face))
                            {
                                Cv2.Resize(faceMat, faceMat, new OpenCvSharp.Size(100, 100));

                                using (var bitmap = BitmapConverter.ToBitmap(faceMat))
                                {
                                    var result = faceService.RecognizeFace(bitmap);

                                    System.Diagnostics.Debug.WriteLine($"Распознавание: IsRecognized={result.IsRecognized}, Message={result.Message}");

                                    if (result.IsRecognized && result.Employee != null)
                                    {
                                        // ✅ РАСПОЗНАЛИ! Устанавливаем флаг
                                        isRecognized = true;
                                        recognizedUserName = result.Employee.FullName;

                                        SetStatus($"✓ Добро пожаловать, {recognizedUserName}!", StatusType.Success);
                                        SetEmployeeInfo(result.Employee, result.Confidence);

                                        // Останавливаем таймер и камеру
                                        timer.Stop();
                                        if (capture != null)
                                        {
                                            capture.Release();
                                            capture.Dispose();
                                            capture = null;
                                        }

                                        // ✅ ПОКАЗЫВАЕМ ПРИВЕТСТВИЕ
                                        MessageBox.Show(
                                            $"Добро пожаловать, {recognizedUserName}!",
                                            "Доступ разрешён",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                                        // Закрываем форму
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                        return;
                                    }
                                    else
                                    {
                                        SetStatus("✗ Лицо не распознано", StatusType.Error);
                                        ClearEmployeeInfo();
                                    }
                                }
                            }
                        }
                        else
                        {
                            SetStatus("Лицо не обнаружено. Поместите лицо в кадр.", StatusType.Info);
                            ClearEmployeeInfo();
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debug.WriteLine("Камера была уничтожена во время обработки кадра");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в Timer_Tick: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
            }
        }

        // ========== КНОПКИ ==========

        private void btnStart_Click(object sender, EventArgs e)
        {
            // ✅ Сбрасываем флаг при новом запуске
            isRecognized = false;
            recognizedUserName = "";

            try
            {
                if (capture != null && capture.IsOpened())
                {
                    timer.Start();
                    SetStatus("Камера запущена. Сканирование...", StatusType.Success);

                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    btnRegister.Enabled = false;
                    btnRefreshModel.Enabled = false;
                    return;
                }

                if (capture != null)
                {
                    try
                    {
                        capture.Dispose();
                    }
                    catch { }
                    capture = null;
                }

                capture = new VideoCapture(0);
                if (!capture.IsOpened())
                {
                    MessageBox.Show("Не удалось открыть камеру!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    capture = null;
                    return;
                }

                timer.Start();
                SetStatus("Камера запущена. Сканирование...", StatusType.Success);

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnRegister.Enabled = false;
                btnRefreshModel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска камеры: {ex.Message}", "Ошибка");
                capture = null;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (timer != null && timer.Enabled)
                {
                    timer.Stop();
                }

                if (capture != null)
                {
                    if (capture.IsOpened())
                    {
                        capture.Release();
                    }
                    capture.Dispose();
                    capture = null;
                }

                if (pictureBox1 != null)
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                    }
                }

                SetStatus("Камера остановлена", StatusType.Info);

                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnRegister.Enabled = true;
                btnRefreshModel.Enabled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при остановке: {ex.Message}");
                capture = null;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            timer.Stop();

            using (FormRegister registerForm = new FormRegister())
            {
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    faceService.TrainModelFromDatabase();
                    SetStatus("Новый сотрудник зарегистрирован! Модель обновлена.", StatusType.Success);
                    MessageBox.Show("Новый сотрудник успешно зарегистрирован!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (capture != null && capture.IsOpened())
            {
                timer.Start();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            timer?.Stop();
            capture?.Release();
            capture?.Dispose();
            faceService?.Dispose();
            Application.Exit();
        }

        private void btnRefreshModel_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("Обучение модели...", StatusType.Info);

                if (faceService != null)
                {
                    faceService.TrainModelFromDatabase();
                    SetStatus("Модель успешно обучена!", StatusType.Success);
                    MessageBox.Show("Модель распознавания обновлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    SetStatus("Сервис распознавания не инициализирован!", StatusType.Error);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Ошибка обучения: {ex.Message}", StatusType.Error);
                MessageBox.Show($"Ошибка при обучении модели: {ex.Message}", "Ошибка");
            }
        }

        private void FormScan_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }

                if (capture != null)
                {
                    try
                    {
                        if (capture.IsOpened())
                        {
                            capture.Release();
                        }
                    }
                    catch { }

                    capture.Dispose();
                    capture = null;
                }

                if (faceService != null)
                {
                    faceService.Dispose();
                    faceService = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при закрытии формы: {ex.Message}");
            }
        }

        private void lblPhone_Click(object sender, EventArgs e)
        {
            // Пустой обработчик
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Пустой обработчик
        }
    }

    public enum StatusType
    {
        Success,
        Error,
        Info,
        Normal
    }
}