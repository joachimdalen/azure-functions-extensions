namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    public class MultipartFile
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}