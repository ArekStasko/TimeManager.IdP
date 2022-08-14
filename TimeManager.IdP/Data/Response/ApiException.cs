namespace TimeManager.IdP.Data.Response
{
    public class ApiException : IApiException
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public ApiException(Exception ex, string title)
        {
            Description = ex.Message;
            Title = title;
        }

    }
}
