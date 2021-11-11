using System;
using System.Collections.Generic;
using System.Text;

namespace MlNetWebApp.Common.Dto
{
    public class ImageAnalysis
    {
        public string Base64ImageContent { get; set; }
        public List<AnalysisResults> ImageAnalysisResults { get; set; }
    }
}
