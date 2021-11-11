using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MlNetWebApp.Model
{
    class ImageNetPrediction
    {
        [ColumnName("grid")]
        public float[] PredictedLabels;
    }
}
