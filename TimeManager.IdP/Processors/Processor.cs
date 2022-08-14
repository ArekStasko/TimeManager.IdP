using TimeManager.IdP.Data;

namespace TimeManager.IdP.Processors
{
    public class Processor
    {
        protected readonly DataContext _context;

        public Processor(DataContext context)
        {
            _context = context;
        }
    }
}
