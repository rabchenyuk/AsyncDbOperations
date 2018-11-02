using DBLibrary.EF.Context;
using DBLibrary.EF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncLoadToDB.Model
{
    class TestDataRepository
    {
        private ObservableCollection<TestEntity> testData;
        public ObservableCollection<TestEntity> TestData
        {
            get
            {
                if (testData == null)
                    testData = GetTestData();
                return testData;
            }
        }

        private ObservableCollection<TestEntity> GetTestData()
        {
            ObservableCollection<TestEntity> data = new ObservableCollection<TestEntity>();

            using (AsyncLoadEntity _context = new AsyncLoadEntity())
            {
                var records = _context.TestEntities.ToList();

                foreach (var item in records)
                {
                    data.Add(item);
                }
            }

            return data;
        }
    }
}
