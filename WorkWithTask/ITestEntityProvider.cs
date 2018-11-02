using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithTask
{
    public delegate void AddTestEntity(int count);
    public interface ITestEntityProvider
    {
        void AddTestEntityRange(int count);
        Task AddTestEntityRangeAsync(int count);
        event AddTestEntity AddTestEntityEvent;
        bool CancelFlag { get; set; }
        bool StopAndSaveFlag { get; set; }
    }
}
