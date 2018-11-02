using DBLibrary.EF.Context;
using System.Linq;
using System.Threading.Tasks;

namespace WorkWithTask
{
    public class ConnectionProvider
    {
        public delegate void ConnectionDelegate(AsyncLoadEntity context);
        public event ConnectionDelegate ConectedEvent;
        public ConnectionProvider() { }
        public Task ConnectRun()
        {
            return Task.Run(
                () =>
                {
                    AsyncLoadEntity _context = new AsyncLoadEntity();
                    _context.Configuration.AutoDetectChangesEnabled = false;

                    var c = _context.TestEntities.Count();

                    if (ConectedEvent != null)
                        ConectedEvent(_context);
                });
        }
    }
}
