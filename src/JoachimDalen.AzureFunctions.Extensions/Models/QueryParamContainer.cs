namespace JoachimDalen.AzureFunctions.Extensions.Models
{
    public class QueryParamContainer<T>
    {
        public T Params { get; set; }

        public QueryParamContainer()
        {
        }
    }
}