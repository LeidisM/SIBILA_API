using Microsoft.EntityFrameworkCore;
using Moq;
using SIBILA_API.Data;

namespace SIBILA_API.Tests.Controllers
{
    // Clase de extensión para configurar DbSet mock
    public static class MockDbSetExtensions
    {
        public static Mock<DbSet<T>> SetupDbSet<T>(this Mock<AppDbContext> context, List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            context.Setup(c => c.Set<T>()).Returns(mockSet.Object);

            return mockSet;
        }
    }
}