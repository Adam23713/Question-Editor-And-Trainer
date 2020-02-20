using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

namespace Common
{
    public class Version
    {
        public static readonly string VERSION = "1.0.0";
    }

    public class QuestionAndAnswer
    {
        public bool NoImage { get; set; } = true;

        public string Question { get; set; } = string.Empty;

        public BitmapImage Picture { get; set; } = null;
        public string PictureUrl { get; set; } = null;

        public List<Answer> Answers { get; set; } = null;

        public QuestionAndAnswer(string question, List<Answer> answers, string imageUrl)
        {
            NoImage = false;
            this.Question = question;
            this.Answers = new List<Answer>(answers);
            PictureUrl = imageUrl;
        }

        public QuestionAndAnswer(string question, List<Answer> answers, BitmapImage image)
        {
            NoImage = false;
            this.Question = question;
            this.Answers = new List<Answer>(answers);
            Picture = image;
        }

        public QuestionAndAnswer(string question, List<Answer> answers)
        {
            NoImage = true;
            this.Question = question;
            this.Answers = new List<Answer>(answers);
        }

        public QuestionAndAnswer()
        {
        }
    }

    public class Answer
    {
        public string answer = string.Empty;
        public bool right = false;

        public Answer(string answer, bool right)
        {
            this.answer = answer;
            this.right = right;
        }

        public Answer()
        {

        }
    }

    public class QuestionsAndAnswers
    {
        public enum ImageType { Base64, Url };
        private static string rightAttribute = "right";
        private static string imageTypeAttribute = "type";
        private static string rootStartTag = "questions";
        private static string questionStartTag = "question_";
        private static string theQuestionStartTag = "The_question";
        private static string answersStartTag = "Answers";
        private static string answerStartTag = "Answer_";
        private static string imageStartTag = "Image";

        private static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;
            return bmpReturn;
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private static string BitmapToBase64String(Bitmap bmp, ImageFormat imageFormat)
        {
            string base64String = string.Empty;


            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, imageFormat);


            memoryStream.Position = 0;
            byte[] byteBuffer = memoryStream.ToArray();


            memoryStream.Close();


            base64String = Convert.ToBase64String(byteBuffer);
            byteBuffer = null;


            return base64String;
        }

        private static string ConvertImageToBase64(BitmapImage image)
        {
            Bitmap img = BitmapImage2Bitmap(image);
            string base64String = BitmapToBase64String(img, ImageFormat.Png);
            return base64String;
        }

        public static List<QuestionAndAnswer> LoadFromXml(string fileName)
        {
            List<QuestionAndAnswer> list = new List<QuestionAndAnswer>();

            using (XmlReader reader = XmlReader.Create(fileName))
            {
                QuestionAndAnswer currentAnswerObject = null;
                List<Answer> currentAnswers = null;
                Answer currentAnswer = null;
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        string element = reader.Name.ToString();
                        if (element.Contains(questionStartTag))
                        {
                            currentAnswers = new List<Answer>();
                            currentAnswerObject = new QuestionAndAnswer();
                        }
                        else if (element == theQuestionStartTag)
                        {
                            currentAnswerObject.Question = reader.ReadString();
                        }
                        else if (element.Contains(answerStartTag))
                        {
                            currentAnswer = new Answer();
                            currentAnswer.right = reader.GetAttribute(rightAttribute) == "true" ? true : false;
                            currentAnswer.answer = reader.ReadString();
                            currentAnswers.Add(currentAnswer);
                        }
                        else if (element == imageStartTag)
                        {
                            currentAnswerObject.Answers = currentAnswers;
                            string type = reader.GetAttribute(imageTypeAttribute);
                            if (type == "base64")
                            {
                                string imgBase64 = reader.ReadString();
                                if (imgBase64 != string.Empty)
                                {
                                    currentAnswerObject.Picture = ToBitmapImage(Base64StringToBitmap(imgBase64));
                                    currentAnswerObject.NoImage = false;
                                }
                                else
                                {
                                    currentAnswerObject.NoImage = true;
                                }
                            }
                            list.Add(currentAnswerObject);
                        }
                    }
                }
            }
            return list;
        }

        public static void SaveToXML(string fileName, List<QuestionAndAnswer> questions, ImageType imageType)
        {
            XmlWriter xmlWriter = XmlWriter.Create(fileName);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement(rootStartTag);

            int counter = 0;
            foreach (var item in questions)
            {
                xmlWriter.WriteStartElement(questionStartTag + counter.ToString());
                xmlWriter.WriteStartElement(theQuestionStartTag);
                xmlWriter.WriteString(item.Question);
                xmlWriter.WriteEndElement(); //The_question

                int answerCounter = 0;
                xmlWriter.WriteStartElement(answersStartTag);
                foreach (var ans in item.Answers)
                {
                    string rightStr = (ans.right) ? "true" : "false";
                    xmlWriter.WriteStartElement(answerStartTag + answerCounter.ToString());
                    xmlWriter.WriteAttributeString(rightAttribute, rightStr);
                    xmlWriter.WriteString(ans.answer);
                    xmlWriter.WriteEndElement(); //Answer_

                    answerCounter++;
                }
                xmlWriter.WriteEndElement(); //Answers
                xmlWriter.WriteStartElement(imageStartTag);

                if (imageType == ImageType.Base64)
                {
                    xmlWriter.WriteAttributeString(imageTypeAttribute, "base64");
                    if (!item.NoImage)
                    {
                        xmlWriter.WriteString(ConvertImageToBase64(item.Picture));
                    }
                    else
                    {
                        xmlWriter.WriteString(string.Empty);
                    }
                }
                else if (imageType == ImageType.Url)
                {
                    xmlWriter.WriteAttributeString(imageTypeAttribute, "url");
                    xmlWriter.WriteString(item.PictureUrl);
                }
                xmlWriter.WriteEndElement(); //Image
                xmlWriter.WriteEndElement(); //question_

                counter++;
            }
            xmlWriter.WriteEndElement(); //questions
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
    }
}
