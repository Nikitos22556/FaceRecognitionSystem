using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using ДипломNikitos.DataBase;
using ДипломNikitos.Models;

namespace ДипломNikitos
{
    public class FaceRecognitionService : IDisposable
    {
        private CascadeClassifier faceDetector;
        private LBPHFaceRecognizer faceRecognizer;
        private Dictionary<int, string> labelToEmployee;
        private bool isTrained;
        private DatabaseHelper dbHelper;

        public FaceRecognitionService()
        {
            string haarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");

            if (!File.Exists(haarPath))
            {
                throw new FileNotFoundException($"Haar-каскад не найден: {haarPath}");
            }

            faceDetector = new CascadeClassifier(haarPath);
            faceRecognizer = LBPHFaceRecognizer.Create();
            labelToEmployee = new Dictionary<int, string>();
            dbHelper = new DatabaseHelper();
            isTrained = false;
        }

        // Обучение модели на фото из базы

        public void TrainModelFromDatabase()
        {
            var trainingData = dbHelper.GetAllEmployeesWithFaces();

            if (trainingData.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("Нет данных для обучения");
                return;
            }

            List<Mat> faceMats = new List<Mat>();
            List<int> labels = new List<int>();

            int currentLabel = 0;
            Dictionary<string, int> fullNameToLabel = new Dictionary<string, int>();
            labelToEmployee.Clear();

            foreach (var data in trainingData)
            {
                try
                {
                    // ✅ ПРАВИЛЬНО: декодируем байты в Mat
                    using (Mat originalMat = Cv2.ImDecode(data.FaceImageBytes, ImreadModes.Color))
                    {
                        if (originalMat.Empty()) continue;

                        Mat grayMat = new Mat();
                        Cv2.CvtColor(originalMat, grayMat, ColorConversionCodes.BGR2GRAY);
                        Cv2.Resize(grayMat, grayMat, new OpenCvSharp.Size(100, 100));

                        faceMats.Add(grayMat.Clone());
                        grayMat.Dispose();

                        if (!fullNameToLabel.ContainsKey(data.FullName))
                        {
                            fullNameToLabel[data.FullName] = currentLabel;
                            labelToEmployee[currentLabel] = data.FullName;
                            currentLabel++;
                        }

                        labels.Add(fullNameToLabel[data.FullName]);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка обработки фото для {data.FullName}: {ex.Message}");
                }
            }

            if (faceMats.Count > 0)
            {
                faceRecognizer.Train(faceMats, labels);
                isTrained = true;

                foreach (var mat in faceMats)
                {
                    mat.Dispose();
                }

                System.Diagnostics.Debug.WriteLine($"Обучение завершено. Образцов: {faceMats.Count}, сотрудников: {currentLabel}");
            }
        }
        // Распознавание лица на изображении
        public RecognitionResult RecognizeFace(Image image)
        {
            if (!isTrained)
            {
                return new RecognitionResult
                {
                    IsRecognized = false,
                    Message = "Модель не обучена. Добавьте сотрудников с фото."
                };
            }

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Сохраняем Image в MemoryStream
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    byte[] imageBytes = ms.ToArray();

                    // ✅ ПРАВИЛЬНО: декодируем байты в Mat через ImDecode
                    using (Mat originalMat = Cv2.ImDecode(imageBytes, ImreadModes.Color))
                    {
                        if (originalMat.Empty())
                        {
                            return new RecognitionResult { IsRecognized = false, Message = "Не удалось загрузить изображение" };
                        }

                        Mat grayMat = new Mat();
                        Cv2.CvtColor(originalMat, grayMat, ColorConversionCodes.BGR2GRAY);
                        Cv2.Resize(grayMat, grayMat, new OpenCvSharp.Size(100, 100));

                        var faces = faceDetector.DetectMultiScale(grayMat, 1.1, 6);

                        if (faces.Length == 0)
                        {
                            grayMat.Dispose();
                            return new RecognitionResult { IsRecognized = false, Message = "Лицо не обнаружено" };
                        }

                        var faceRect = faces[0];
                        Mat faceMat = new Mat(grayMat, faceRect);

                        int predictedLabel = -1;
                        double confidence = 0;
                        faceRecognizer.Predict(faceMat, out predictedLabel, out confidence);

                        faceMat.Dispose();
                        grayMat.Dispose();

                        bool isMatch = confidence < 120;

                        if (isMatch && labelToEmployee.ContainsKey(predictedLabel))
                        {
                            string employeeName = labelToEmployee[predictedLabel];

                            string[] nameParts = employeeName.Split(' ');
                            string lastName = nameParts.Length > 0 ? nameParts[0] : "";
                            string firstName = nameParts.Length > 1 ? nameParts[1] : "";

                            var employee = dbHelper.FindEmployeeByFullName(lastName, firstName);

                            return new RecognitionResult
                            {
                                IsRecognized = true,
                                Employee = employee,
                                EmployeeName = employeeName,
                                Confidence = confidence,
                                Message = $"Узнан: {employeeName}"
                            };
                        }
                        else
                        {
                            return new RecognitionResult
                            {
                                IsRecognized = false,
                                Confidence = confidence,
                                Message = $"Лицо не распознано"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new RecognitionResult { IsRecognized = false, Message = $"Ошибка: {ex.Message}" };
            }
        }

        public void Dispose()
        {
            faceDetector?.Dispose();
            faceRecognizer?.Dispose();
        }
    }

    public class RecognitionResult
    {
        public bool IsRecognized { get; set; }
        public Employee Employee { get; set; }
        public string EmployeeName { get; set; }
        public double Confidence { get; set; }
        public string Message { get; set; }
    }
}