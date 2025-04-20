using Microsoft.EntityFrameworkCore;
using Moq;
using SIBILA_API.Tests.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIBILA_API.Tests.Data
{
    public static class MockExtensions
    {
        public static Mock<DbSet<T>> BuildMockDbSet<T>(this IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IAsyncEnumerable<T>>()
                  .Setup(m => m.GetAsyncEnumerator(default))
                  .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                  .Setup(m => m.Provider)
                  .Returns(new TestAsyncQueryProvider<T>(data.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
    }
}
