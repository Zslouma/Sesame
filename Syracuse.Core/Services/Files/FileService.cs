using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public class FileService
    {
        private readonly Assembly assembly;

        public FileService() => this.assembly = IntrospectionExtensions.GetTypeInfo(this.GetType()).Assembly;

        protected async Task<string> GetRootRessource(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(nameof(fileName));
            try
            {
                var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{fileName}");

                if (stream == null)
                    throw new Exception($"Can not read file {fileName}");

                using (var reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("iso-8859-1")))
                {
                    return await reader.ReadToEndAsync();
                }
            } catch (Exception) {
                return string.Empty;
            }
        }

        /// <summary>
        /// Cette méthode récupére le fichier json contenant le departement et les library
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected async Task<string> GetDataRessource(string fileName , string url)
        {
            try
            {   
                HttpWebRequest request =(HttpWebRequest)WebRequest.Create(url);
                // Create POST data and convert it to a byte array.
                request.Method = "GET";
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json; charset=utf-8";
                string fileText;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    // Read the content.
                    fileText = await reader.ReadToEndAsync();
                }
                }
                return fileText;
            }
            catch
            {
                return await GetRootRessource(fileName);
            }
            
        }
        
    }
}
