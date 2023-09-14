using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using Models;
using Newtonsoft.Json;
using System.Security.Principal;

namespace WebUploaderApp.Controllers
{
    public class DocsController : ApiController
    {
        private static String docPath = @"c:\uploads";

        // POST api/doc
        public void Post(HttpRequestMessage request)
        {
            var content = request.Content;
            string jsonContent = content.ReadAsStringAsync().Result;

            if (jsonContent != null)
            {
                Document doc = JsonConvert.DeserializeObject<Document>(jsonContent);

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, doc.Filename)))
                {
                   outputFile.WriteLine(JsonConvert.SerializeObject(doc.Body));
                }
            }
            else
            {
                Console.WriteLine("Missing document in payload body.");
            }
        }
    }
}
