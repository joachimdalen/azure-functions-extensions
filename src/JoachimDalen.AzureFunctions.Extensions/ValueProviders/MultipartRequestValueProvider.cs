using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JoachimDalen.AzureFunctions.Extensions.Attributes;
using JoachimDalen.AzureFunctions.Extensions.Models;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JoachimDalen.AzureFunctions.Extensions.ValueProviders
{
    public class MultipartRequestValueProvider<T> : IValueProvider
    {
        private readonly MultipartRequestAttribute _attribute;
        private readonly HttpRequestMessage _request;
        private readonly ILogger _logger;

        public MultipartRequestValueProvider(MultipartRequestAttribute attribute, HttpRequestMessage request, ILogger logger)
        {
            _attribute = attribute;
            _request = request;
            _logger = logger;
        }

        public async Task<object> GetValueAsync()
        {
            try
            {
                var contents = (await _request.Content.ReadAsMultipartAsync()).Contents;
                var dataContent = contents.Where(x => x.HasData())?.FirstOrDefault();

                T dataResult = default;
                if (dataContent != null)
                {
                    var stringContent = await dataContent.ReadAsStringAsync();
                    dataResult = JsonConvert.DeserializeObject<T>(stringContent);
                }

                var filesContents = contents.Where(content => content.HasFiles(_attribute.FileName));

                var files = new List<MultipartFile>();

                foreach (var uploadedFile in filesContents)
                {
                    var fileName = uploadedFile.Headers?.ContentDisposition?.GetEscapedContentDispositionFileName();
                    var fileContents = await uploadedFile.ReadAsByteArrayAsync();
                    files.Add(new MultipartFile
                    {
                        FileName = fileName,
                        Content = fileContents
                    });
                }

                return new MultipartRequestData<T>
                {
                    Data = dataResult,
                    Files = files?.ToArray()
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error deserializing object from body");
                throw ex;
            }
        }

        public Type Type => typeof(object);
        public string ToInvokeString() => string.Empty;
    }
}