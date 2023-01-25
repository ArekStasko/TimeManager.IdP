using TimeManager.IdP.Data;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.services.MessageQueuer;

namespace TimeManager.IdP.Processors
{
    public class Processor<T>
    {
        protected readonly DataContext _context;
        protected readonly ILogger<T> _logger;
        protected readonly IMQManager _mqManager;

        public Processor(DataContext context, ILogger<T> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public Processor(DataContext context, ILogger<T> logger, IMQManager mqManager)
        {
            _context = context;
            _logger = logger;
            _mqManager = mqManager;
        }
    }
}
