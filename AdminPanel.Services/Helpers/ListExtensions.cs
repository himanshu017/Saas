using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AdminPanel.Services.Helpers
{
    public static class ListExtensions
    {
        /// <summary>
        /// Order a list by string expression name
        /// </summary>
        /// <typeparam name="T">Generic list type</typeparam>
        /// <param name="list">Input list</param>
        /// <param name="sortExpression">ProductName desc or any other property with asc or desc appended to it</param>
        /// <returns>List ordered by the specified property</returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
        {
            sortExpression += "";
            string[] parts = sortExpression.Split(' ');
            bool descending = false;
            string property = "";

            if (parts.Length > 0 && parts[0] != "")
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("esc");
                }

                PropertyInfo prop = typeof(T).GetProperty(property);

                if (prop == null)
                {
                    throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
                }

                if (descending)
                    return list.OrderByDescending(x => prop.GetValue(x, null));
                else
                    return list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }

        /// <summary>
        /// Generates random list with rander everytime
        /// </summary>
        /// <typeparam name="t">Generic list type</typeparam>
        /// <param name="list">Input list</param>
        /// <returns>Randomly ordered list</returns>
        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> list)
        {
            Random r = new Random();

            return list.OrderBy(x => (r.Next()));
        }

        /// <summary>
        /// Converts an enumeration of groupings into a Dictionary of those groupings.
        /// </summary>
        /// <typeparam name="TKey">Key type of the grouping and dictionary.</typeparam>
        /// <typeparam name="TValue">Element type of the grouping and dictionary list.</typeparam>
        /// <param name="groupings">The enumeration of groupings from a GroupBy() clause.</param>
        /// <returns>A dictionary of groupings such that the key of the dictionary is TKey type and the value is List of TValue type.</returns>
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.ToDictionary(group => group.Key, group => group.ToList());
        }

        /// <summary>
        /// Get date range between two dates
        /// </summary>
        /// <param name="self">Start date</param>
        /// <param name="toDate">End date</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
        {
            var range = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days);

            return from p in range
                   select self.Date.AddDays(p);
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            return list.GroupBy(keySelector).Select(grps => grps).Select(e => e.First());
        }

        /// <summary>
        /// Recursively create directory
        /// </summary>
        /// <param name="dirInfo">Folder path to create.</param>
        public static void CreateDirectory(this DirectoryInfo dirInfo)
        {
            if (dirInfo.Parent != null) CreateDirectory(dirInfo.Parent);
            if (!dirInfo.Exists) dirInfo.Create();
        }

        /// <summary>
        /// Foreach inline for the IEnumerable<T>.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// Order the IQueryable by the given property or field.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable being ordered.</typeparam>
        /// <param name="queryable">The IQueryable being ordered.</param>
        /// <param name="propertyOrFieldName">The name of the property or field to order by.</param>
        /// <param name="ascending">Indicates whether or not the order should be ascending (true) or descending (false.)</param>
        /// <returns>Returns an IQueryable ordered by the specified field.</returns>
        public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> queryable, string propertyOrFieldName, bool ascending = true)
        {
            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

            return queryable.Provider.CreateQuery<T>(orderByExpression);
        }

        public static Image resizeImage(this Image imgToResize, Size size)
        {
            //Get picture width
            int sourceWidth = imgToResize.Width;
            //Get picture height
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calculate the scaling of the width
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate the scaling of the height
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //Desired width
            int destWidth = (int)(sourceWidth * nPercent);
            //Desired height
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //Draw image
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return condition ? transform(source) : source;
        }

        public static IQueryable<T> If<T>(this IQueryable<T> source, Func<T, bool> conditionPredicate, Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return source.Where(entity => conditionPredicate(entity))
                         .SelectMany(entity => conditionPredicate(entity) ? transform(new[] { entity }.AsQueryable()) : new[] { entity }.AsQueryable());
        }

        public static IQueryable<T> If<T, P>(this IIncludableQueryable<T, P> source,bool condition,Func<IIncludableQueryable<T, P>, IQueryable<T>> transform) where T : class
        {
            return condition ? transform(source) : source;
        }

        public static IQueryable<T> If<T, P>(this IIncludableQueryable<T, IEnumerable<P>> source,bool condition,Func<IIncludableQueryable<T, IEnumerable<P>>, IQueryable<T>> transform) where T : class
        {
            return condition ? transform(source) : source;
        }
    }
}
