using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;

namespace Test.Common
{
    public sealed class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class
    {
        public MockDbSet(List<TEntity> dataSource = null)
        {
            var data = (dataSource ?? new List<TEntity>());
            var queryable = data.AsQueryable();

            As<IQueryable<TEntity>>().Setup(e => e.Provider).Returns(queryable.Provider);
            As<IQueryable<TEntity>>().Setup(e => e.Expression).Returns(queryable.Expression);
            As<IQueryable<TEntity>>().Setup(e => e.ElementType).Returns(queryable.ElementType);
            As<IQueryable<TEntity>>().Setup(e => e.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        }
    }
}
