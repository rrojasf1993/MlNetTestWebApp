using Microsoft.AspNetCore.Http;
using Microsoft.ML;
using MlNetWebApp.Common.Dto;
using MlNetWebApp.Model;
using MlNetWebApp.Model.YoLoParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
namespace MlNetWebApp.Common
{
    public class MLService : IMLService
    {
        private string AssetsRelativePath
        {
            get
            {
                return @"assets";
            }
        }
        private string AssetsPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location),"appassets");
            }
        }

        private string ModelPath
        {
            get
            {
                return Path.Combine(AssetsPath, "Model", "Model.onnx");
            }
        }

        private string ImagesFolder
        {
            get
            {
                return Path.Combine(AssetsPath,AssetsRelativePath, "images");
            }
        }

       
        public MLContext MlContext { get; set; }

        public MLService()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.MlContext = new MLContext();
        }
        private static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(MLService).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;
            string fullPath = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }

       
        public List<ImageAnalysis> AnalyzePhotos()
        {
            List<ImageAnalysis> imageAnalyses = new List<ImageAnalysis>();
            try
            {
                IEnumerable<ImageNetData> images = ImageNetData.ReadFromFile(ImagesFolder);
                IDataView imageDataView = MlContext.Data.LoadFromEnumerable(images);
                var modelScorer = new OnnxModelScorer(ImagesFolder, ModelPath, MlContext);
                // Use model to score data
                IEnumerable<float[]> probabilities = modelScorer.Score(imageDataView);
                OutputParser parser = new OutputParser();
                var boundingBoxes =
                    probabilities
                    .Select(probability => parser.ParseOutputs(probability))
                    .Select(boxes => parser.FilterBoundingBoxes(boxes, 5, .5F));
             
                for (var i = 0; i < images.Count(); i++)
                {
                    string imageFileName = images.ElementAt(i).Label;
                    IList<YoLoBoundingBox> detectedObjects = boundingBoxes.ElementAt(i);
                    string analyzedImageContent=ImageHandler.DrawBoundingBox(ImagesFolder, imageFileName, detectedObjects);
                    ImageAnalysis analysis =  ImageHandler.GetImageAnalysisData(imageFileName, detectedObjects);
                    analysis.Base64ImageContent = analyzedImageContent;
                    imageAnalyses.Add(analysis);
                }
                DirectoryInfo imagesPathInfo = new DirectoryInfo(ImagesFolder);
                imagesPathInfo.GetFiles().ToList().ForEach((f) => f.Delete());
            }
            catch (Exception exc)
            { 
                throw exc;
            }
            return imageAnalyses;
        }

        public void UploadPhotos(List<IFormFile> fileData)
        {
            try
            {
                if (fileData!=null)
                {
                    string directoryPath = GetAbsolutePath(ImagesFolder);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    fileData.ForEach(async(file) =>
                    {
                        string filePath = Path.Combine(directoryPath, file.FileName);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms, System.Threading.CancellationToken.None);
                            if (!File.Exists(filePath))
                            {
                                File.Create(filePath);
                            }
                            ms.Position = 0;
                            await File.WriteAllBytesAsync(filePath, ms.ToArray());
                        }
                    });
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }  
        }
    }
}