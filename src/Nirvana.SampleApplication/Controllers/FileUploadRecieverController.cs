using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nirvana.Mediation;
using Nirvana.SampleApplication.Services.Services;
using Nirvana.Util.Io;
using Nirvana.Web.Controllers;

namespace Nirvana.SampleApplication.Controllers
{

 public abstract class FileUploadRecieverController : CommandQueryApiControllerBase
    {

        private IHostingEnvironment _Env;
        public FileUploadRecieverController(IHostingEnvironment envrnmt, ISerializer serializer, IMediatorFactory mediator) : base(mediator,serializer)
        {
            _Env = envrnmt;
        }

       

        [HttpPost]
        public async Task<HttpResponseMessage> PostFile()
        {
            if (!Request.Form.Files.Any())
            {
                return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
            }


            var webRootInfo = _Env.WebRootPath;
            var root= System.IO.Path.Combine(webRootInfo, "/App_Data/Uploadfiles");
            Directory.CreateDirectory(root);
            foreach (var file in Request.Form.Files)
            {
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
            }
            return new HttpResponseMessage(HttpStatusCode.OK);

//            var action = result.FormData["key"];
//            var continueWith = result.FormData["continueWithAction"];
//
//            if (action == null || continueWith == null)
//            {
//                throw new HttpResponseException(HttpStatusCode.BadRequest);
//            }
//
//            //get the posted files  
//            foreach (var file in result.FileData)
//            {
//                if (File.Exists(file.LocalFileName))
//                {
//                    var command = MapAction(action,  continueWith, file);
//                    File.Delete(file.LocalFileName);
//                    return Command(command);
//                }
//            }
//
//            return Request.CreateResponse(HttpStatusCode.BadRequest, "Command could not be found");
        }
//
//        protected abstract FileUploadCommand MapAction(string action, string continueWith, MultipartFileData file);
//        
//
//        protected void AddFileMetaData(FileUploadCommand command, MultipartFileData file)
//        {
//            command.FileData = File.ReadAllBytes(file.LocalFileName);
//            command.FileName = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty); ;
//            var parts = command.FileName.Split('.');
//            command.FileMimeType = MimeTypeMap.GetMimeType(parts[parts.Length-1]);
//        }
    }
}
