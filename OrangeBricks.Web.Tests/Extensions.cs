using System;
using System.Data.Entity;
using System.Linq;

using NSubstitute;

namespace OrangeBricks.Web.Tests
{
    public static class Extensions
    {
        /// <param name="getId">Function that returns the Id of T. 
        /// Specify when Find is used in the context.</param>
        public static IDbSet<T> Initialize<T>(this IDbSet<T> dbSet, IQueryable<T> data
                                            , Func<T, int> getId = null) where T : class
        {
            dbSet.Provider.Returns(data.Provider);
            dbSet.Expression.Returns(data.Expression);
            dbSet.ElementType.Returns(data.ElementType);
            dbSet.GetEnumerator().Returns(data.GetEnumerator());

            //http://www.jerriepelser.com/blog/unit-testing-nbuilder-fake-or-mock-dbset/
            if (getId != null)
            {
                dbSet.Find(Arg.Any<object[]>()).Returns(callinfo =>
                {
                    object[] idValues = callinfo.Arg<object[]>();
                    if (idValues != null && idValues.Length == 1)
                    {
                        var requestedId = (int)idValues[0];
                        return data.FirstOrDefault(d => getId(d) == requestedId);
                    }

                    return null;
                });
            }

            return dbSet;
        }
    }
}
