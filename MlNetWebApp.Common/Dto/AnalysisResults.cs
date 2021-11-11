using System;
using System.Collections.Generic;
using System.Text;

namespace MlNetWebApp.Common.Dto
{
    public class AnalysisResults
    {
        public double PredictionConfidence { get; set; }
        public string Label { get; set; }
    }
}
