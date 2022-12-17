namespace TimeManager.IdP.Data.Response
{
    public class ApiException : IApiException
    {
        public int Status { get; }
        public string Description { get; }

        public ApiException(int status, string description)
        {
            Status = status;
            Description = description;
        }

    }
}
