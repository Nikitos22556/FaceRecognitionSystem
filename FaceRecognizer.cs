using System;
using System.IO;
using System.Xml.Serialization;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;

namespace ДипломNikitos.Services // Советую создать папку Services
{
    public class FaceRecognizerService
    {
        private LBPHFaceRecognizer _recognizer;
        private CascadeClassifier _faceCascade;
        private string _trainedModelPath = Path.Combine(Environment.CurrentDirectory, "trained_model.yml");

        public FaceRecognizerService()
        {
            string haarPath = Path.Combine(Environment.CurrentDirectory, "haarcascade_frontalface_default.xml");
            _faceCascade = new CascadeClassifier(haarPath);

            // Пытаемся загрузить уже обученную модель, если она есть
            if (File.Exists(_trainedModelPath))
            {
                _recognizer = LBPHFaceRecognizer.Create();
                _recognizer.Read(_trainedModelPath);
            }
            else
            {
                _recognizer = LBPHFaceRecognizer.Create();
            }
        }

        // Метод для "снятия" слепка с фото сотрудника и сохранения в БД
        public string ExtractFaceEmbedding(byte[] faceImageBytes)
        {
            using (var ms = new MemoryStream(faceImageBytes))
            using (var img = Mat.FromStream(ms, ImreadModes.Grayscale))
            {
                // Убедимся, что фото нужного размера (например, 100x100)
                Mat resizedImg = new Mat();
                Cv2.Resize(img, resizedImg, new OpenCvSharp.Size(100, 100));

                // LBPH ожидает "образец" для обучения.
                // Нам нужен только "слепок" (гистограмма). 
                // Здесь логика сложнее, но для начала мы сохраним само фото.
                // В будущем для реального распознавания нужно будет обучать модель целиком.

                // Для простоты в демо-версии можно пока сохранять фото.
                // Но для диплома покажу наметку:
                // _recognizer.Update(...) - команда для дообучения
                // После сохранения всех лиц, нужно будет вызывать _recognizer.Save(_trainedModelPath)

                // Пока возвращаем пустую строку, чтобы код скомпилировался.
                // В следующем шаге я дам полный рабочий код, если он понадобится.
                return string.Empty;
            }
        }

        // Метод для распознавания лица на кадре
        public (int userId, double confidence) RecognizeFace(byte[] faceImageBytes)
        {
            if (_recognizer == null) return (-1, 1000); // Если модель не загружена

            using (var ms = new MemoryStream(faceImageBytes))
            using (var img = Mat.FromStream(ms, ImreadModes.Grayscale))
            {
                Mat resizedImg = new Mat();
                Cv2.Resize(img, resizedImg, new OpenCvSharp.Size(100, 100));

                int label = -1;
                double confidence = 0;

                // Пытаемся предсказать, кто на фото
                _recognizer.Predict(resizedImg, out label, out confidence);

                // В LBPH чем МЕНЬШЕ confidence, тем ЛУЧШЕ. < 50 - отлично.
                return (label, confidence);
            }
        }

        // Метод для обучения модели по всем фото из БД
        public void TrainModelFromDatabase()
        {
            // Здесь нужен код, который:
            // 1. Загрузит из БД все фото (FaceImage) и ID сотрудников
            // 2. Преобразует их в Mat
            // 3. Вызовет _recognizer.Train(images, labels);
            // 4. Сохранит модель _recognizer.Save(_trainedModelPath);
        }
    }
}