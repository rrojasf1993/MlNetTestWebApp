using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MlNetWebApp.Common.Dto
{
    public class FileItem
    {
        public string FileName { get; set; }

        public IFormFile File { get; set; }
    }
}
