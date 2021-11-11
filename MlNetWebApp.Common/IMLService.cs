using Microsoft.AspNetCore.Http;
using Microsoft.ML;
using MlNetWebApp.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MlNetWebApp.Common
{
    public interface IMLService
    {
        public MLContext MlContext { get; set; }

        public void UploadPhotos(List<IFormFile> files);

        public List<ImageAnalysis> AnalyzePhotos();

    }
}
