namespace TimeManager.IdP.Data.Response
{
    public interface IApiException
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
