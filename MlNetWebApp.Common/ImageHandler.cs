using MlNetWebApp.Common.Dto;
using MlNetWebApp.Model.YoLoParser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace MlNetWebApp.Common
{
    public static class ImageHandler
    {
        public static string DrawBoundingBox(string inputImageLocation, string imageName, IList<YoLoBoundingBox> filteredBoundingBoxes)
        {
            string base64ImageContent = string.Empty;
            using (Image image = Image.FromFile(Path.Combine(inputImageLocation, imageName)))
            {
                var originalImageHeight = image.Height;
                var originalImageWidth = image.Width;
             
                foreach (var box in filteredBoundingBoxes)
                {
                    var x = (uint)Math.Max(box.Dimensions.X, 0);
                    var y = (uint)Math.Max(box.Dimensions.Y, 0);
                    var width = (uint)Math.Min(originalImageWidth - x, box.Dimensions.Width);
                    var height = (uint)Math.Min(originalImageHeight - y, box.Dimensions.Height);
                    x = (uint)originalImageWidth * x / OnnxModelScorer.ImageNetSettings.imageWidth;
                    y = (uint)originalImageHeight * y / OnnxModelScorer.ImageNetSettings.imageHeight;
                    width = (uint)originalImageWidth * width / OnnxModelScorer.ImageNetSettings.imageWidth;
                    height = (uint)originalImageHeight * height / OnnxModelScorer.ImageNetSettings.imageHeight;
                    string text = $"{box.Label} ({(box.Confidence * 100).ToString("0")}%)";


                    using (Graphics thumbnailGraphic = Graphics.FromImage(image))
                    {
                        thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Font drawFont = new Font("Arial", 12, FontStyle.Bold);
                        SizeF size = thumbnailGraphic.MeasureString(text, drawFont);
                        SolidBrush fontBrush = new SolidBrush(Color.Black);
                        Point atPoint = new Point((int)x, (int)y - (int)size.Height - 1);

                        // Define BoundingBox options
                        Pen pen = new Pen(box.BoxColor, 3.2f);
                        SolidBrush colorBrush = new SolidBrush(box.BoxColor);
                        thumbnailGraphic.FillRectangle(colorBrush, (int)x, (int)(y - size.Height - 1), (int)size.Width, (int)size.Height);
                        thumbnailGraphic.DrawString(text, drawFont, fontBrush, atPoint);
                        thumbnailGraphic.DrawRectangle(pen, x, y, width, height);

                        using (MemoryStream msImage = new MemoryStream())
                        {
                            image.Save(msImage, ImageFormat.Png);
                            byte[] imageBytes = msImage.ToArray();
                            base64ImageContent = Convert.ToBase64String(imageBytes);
                        }
                    }
                }
            }
            return base64ImageContent;
        }

        public static ImageAnalysis GetImageAnalysisData(string imageName,
                                                IList<YoLoBoundingBox> boundingBoxes)
        {
            ImageAnalysis imageAnalysis = new ImageAnalysis();
            Console.WriteLine($".....The objects in the image {imageName} are detected as below....");
            imageAnalysis.ImageAnalysisResults = new List<AnalysisResults>();
            if (boundingBoxes != null && boundingBoxes.Count != 0)
            {
                foreach (var box in boundingBoxes)
                {
                    Console.WriteLine($"{box.Label} and its Confidence score: {box.Confidence}");
                    imageAnalysis.ImageAnalysisResults.Add(new AnalysisResults() { Label = box.Label, PredictionConfidence = box.Confidence });
                }
            }
            return imageAnalysis;
        }

    }
}
