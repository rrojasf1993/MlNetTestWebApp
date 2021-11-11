using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MlNetWebApp.Common;
using System.Linq;
using MlNetWebApp.Common.Dto;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BrunoZell.ModelBinding;

namespace MlNetWebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MLImageApiController : ControllerBase
    {
        public IMLService MLService { get; set; }
        public MLImageApiController(IMLService serviceInstance)
        {
            this.MLService = serviceInstance;
        }

        [Route("UploadFiles")]
        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFileCollection files)
        {
            IActionResult actionResult;
            List<FileItem> imageFiles = new List<FileItem>();
            try
            {
                MLService.UploadPhotos(files.ToList());
                actionResult = Ok();
            }
            catch (Exception exc)
            {
                actionResult = BadRequest(exc.Message);
            }
            return actionResult;
        }


        [Route("GetAnalysisResults")]
        [HttpGet]
        public IActionResult GetAnalysisResults()
        {
            IActionResult actionResult;
            try
            {
               List<ImageAnalysis> result= this.MLService.AnalyzePhotos();
               actionResult = Ok(result);
            }
            catch (Exception exc)
            {
                actionResult = BadRequest(exc.Message);
            }
            return actionResult;
        }
    }
}
