namespace DBLibrary.EF.Context
{
    using DBLibrary.EF.Models;
    using System.Data.Entity;

    public class AsyncLoadEntity : DbContext
    {
        public AsyncLoadEntity()
            : base("name=AsyncLoadEntity")
        {
        }

        public virtual DbSet<TestEntity> TestEntities { get; set; }
    }
}