using System.ComponentModel.DataAnnotations;

namespace TimeManager.IdP.Data.Token
{
    public class TokenKey
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
    }
}
