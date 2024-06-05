using Abp.Extensions;
using esign.Url;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace esign.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PdfViewerController : esignControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        //Initialize the memory cache object   
        public IMemoryCache _cache;
        private readonly IWebUrlService _webUrlService;

        public PdfViewerController(IHostingEnvironment hostingEnvironment, IMemoryCache cache, IWebUrlService webUrlService)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            _webUrlService = webUrlService;
            Console.WriteLine("PdfViewerController initialized");
        }

        [HttpPost]
        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for loading the PDF documents 
        public async Task<IActionResult> Load([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            MemoryStream stream = new MemoryStream();
            object jsonResult = new object();

            if (jsonObject != null && jsonObject.ContainsKey("document"))
            {
                if (bool.Parse(jsonObject["isFileName"]))
                {
                    string documentPath = GetDocumentPath(jsonObject["document"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        //byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        //stream = new MemoryStream(bytes);
                        using (var client = new WebClient())
                        {
                            var serverRootProtocal = _webUrlService.ServerRootProtocolFormat;
                            var document = jsonObject["document"].Replace("http://", serverRootProtocal).Replace("https://", serverRootProtocal);
                            byte[] bytes = await Task.FromResult(client.DownloadData(document));
                            stream = new MemoryStream(bytes);
                        }
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                    stream = new MemoryStream(bytes);
                }
            }

            jsonResult = pdfviewer.Load(stream, jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_Bookmarks)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //[Route("[controller]/Bookmarks")]
        //Post action for processing the bookmarks from the PDF documents
        public IActionResult Bookmarks([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonResult = pdfviewer.GetBookmarks(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_RenderPdfPages)]

        //Post action for processing the PDF documents  
        public IActionResult RenderPdfPages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetPage(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_RenderPdfTexts)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for processing the PDF texts  
        public IActionResult RenderPdfTexts([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetDocumentText(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_RenderThumbnailImages)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for rendering the thumbnail images
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object result = pdfviewer.GetThumbnailImages(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        [AcceptVerbs("Post")]
        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_RenderAnnotationComments)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for rendering the annotations
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_ExportAnnotations)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action to export annotations
        public IActionResult ExportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = pdfviewer.ExportAnnotation(jsonObject);
            return Content(jsonResult);
        }
        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_ImportAnnotations)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action to import annotations
        public IActionResult ImportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = string.Empty;
            object JsonResult;
            if (jsonObject != null && jsonObject.ContainsKey("fileName"))
            {
                string documentPath = GetDocumentPath(jsonObject["fileName"]);
                if (!string.IsNullOrEmpty(documentPath))
                {
                    jsonResult = System.IO.File.ReadAllText(documentPath);
                }
                else
                {
                    return this.Content(jsonObject["document"] + " is not found");
                }
            }
            else
            {
                string extension = Path.GetExtension(jsonObject["importedData"]);
                if (extension != ".xfdf")
                {
                    JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                    return Content(JsonConvert.SerializeObject(JsonResult));
                }
                else
                {
                    string documentPath = GetDocumentPath(jsonObject["importedData"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        jsonObject["importedData"] = Convert.ToBase64String(bytes);
                        JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                        return Content(JsonConvert.SerializeObject(JsonResult));
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
            }
            return Content(jsonResult);
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_ExportFormFields)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action to export form fields
        public IActionResult ExportFormFields([FromBody] Dictionary<string, string> jsonObject)

        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = pdfviewer.ExportFormFields(jsonObject);
            return Content(jsonResult);
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_ImportFormFields)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        [Route("[controller]/ImportFormFields")]
        //Post action to import form fields
        public IActionResult ImportFormFields([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);

            object jsonResult = pdfviewer.ImportFormFields(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_Unload)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for unloading and disposing the PDF document resources  
        public IActionResult Unload([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            pdfviewer.ClearCache(jsonObject);
            return this.Content("Document cache is cleared");
        }


        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_Download)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for downloading the PDF documents
        public IActionResult Download([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
            return Content(documentBase);
        }

        [HttpPost]
        //[AbpAuthorizeAppPermissions.Pages_PdfViewer_PrintImages)]

        [Microsoft.AspNetCore.Cors.EnableCors("localhost")]
        //Post action for printing the PDF documents
        public IActionResult PrintImages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object pageImage = pdfviewer.GetPrintImage(jsonObject);
            return Content(JsonConvert.SerializeObject(pageImage));
        }

        //Returns the PDF document path
        private string GetDocumentPath(string document)
        {
            string documentPath = string.Empty;
            if (!System.IO.File.Exists(document))
            {
                var path = _hostingEnvironment.ContentRootPath;
                if (System.IO.File.Exists(path + "/Data/" + document))
                    documentPath = path + "/Data/" + document;
                else documentPath = document;
            }
            else
            {
                documentPath = document;
            }
            Console.WriteLine(documentPath);
            return documentPath;
        }

        //GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
    }
}
