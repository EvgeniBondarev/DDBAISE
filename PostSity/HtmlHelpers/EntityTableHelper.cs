using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace PostСity.HtmlHelpers
{
    public static class EntityTableHelper
    {
        public static HtmlString DisplayTable<T>(this IHtmlHelper html, IEnumerable<T> items, List<ColumnDefinition<T>> columnDefinitions)
        {
            if (items == null || columnDefinitions == null || columnDefinitions.Count == 0)
            {
                return new HtmlString(string.Empty);
            }

            var properties = typeof(T).GetProperties();

            string table = "<table class='table'>";
            string endTable = "</table>";

            string headerRow = "<tr>";
            foreach (var column in columnDefinitions)
            {
                headerRow += "<th>" + column.Header + "</th>";
            }
            headerRow += "</tr>";

            string bodyRows = "";
            foreach (var item in items)
            {
                string row = "<tr>";
                foreach (var column in columnDefinitions)
                {
                    var value = GetFormattedValue(item, column);
                    row += "<td>" + value + "</td>";
                }
                row += "</tr>";
                bodyRows += row;
            }

            string result = table + headerRow + bodyRows + endTable;

            return new HtmlString(result);
        }

        private static string GetFormattedValue<T>(T item, ColumnDefinition<T> column)
        {
            if (column.ValueExpression != null)
            {
                var value = column.ValueExpression.Compile()(item);
                return value?.ToString() ?? string.Empty;
            }
            else if (!string.IsNullOrEmpty(column.PropertyName))
            {
                var property = typeof(T).GetProperty(column.PropertyName);
                var value = property?.GetValue(item)?.ToString() ?? string.Empty;
                return value;
            }
            return string.Empty;
        }
    }


   
}
public class ColumnDefinition<T>
{
        public string Header { get; set; }
        public string PropertyName { get; set; }
        public Expression<Func<T, object>> ValueExpression { get; set; }
}