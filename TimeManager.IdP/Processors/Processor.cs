using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Processors
{
    public class Processor
    {
        protected readonly DataContext _context;
        protected readonly ILogger<TokenController> _logger;

        public Processor(DataContext context, ILogger<TokenController> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
