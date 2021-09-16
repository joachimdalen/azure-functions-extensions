namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    public class MultipartRequestData<T>
    {
        public T Data { get; set; }
        public MultipartFile[] Files { get; set; }
    }
}