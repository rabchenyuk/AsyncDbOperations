using DBLibrary.EF.Context;
using DBLibrary.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace WorkWithTask
{
    public class TestEntityProvider : ITestEntityProvider
    {
        private readonly AsyncLoadEntity _context;
        public bool CancelFlag { get; set; }
        public bool StopAndSaveFlag { get; set; }

        public TestEntityProvider(AsyncLoadEntity context)
        {
            _context = context;
        }

        public event AddTestEntity AddTestEntityEvent;

        public void AddTestEntityRange(int count)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        TestEntity test = new TestEntity
                        {
                            Age = 3,
                            Name = "Random data",
                            Male = true
                        };

                        _context.TestEntities.Add(test);
                        _context.SaveChanges();

                        if (AddTestEntityEvent != null)
                            AddTestEntityEvent.Invoke(i + 1);

                        if (CancelFlag == true)
                        {
                            CancelFlag = false;
                            throw new TransactionAbortedException();
                            //return;
                        }

                        if (StopAndSaveFlag == true)
                        {
                            MessageBox.Show($"Paused and saved on {i + 1}");
                            scope.Complete();
                            StopAndSaveFlag = false;
                            return;
                        }
                    }
                    scope.Complete();

                    MessageBox.Show("Done");

                }
                catch (TransactionAbortedException)
                {
                    MessageBox.Show("None of elements have been recorded to database");
                }
            }
        }

        public Task AddTestEntityRangeAsync(int count)
        {
            return Task.Run(() => AddTestEntityRange(count));
        }
    }
}
