using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Presentation.Api.ContentFormatter;

public class CsvOutputFormatter : TextOutputFormatter
{
	public CsvOutputFormatter()
	{
		SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedEncodings.Add(Encoding.Unicode);
	}

	protected override bool CanWriteType(Type? type)
	{
		if (type == null) return false;

		// IEnumerable<T> check properly
		// Previously IsAssignableFrom didn't work correctly with generic type definitions
		if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
			return true;

		// Any class (single object)
		if (type.IsClass && type != typeof(string))
			return true;

		return false;
	}

	public override async Task WriteResponseBodyAsync(
		OutputFormatterWriteContext context, Encoding selectedEncoding)
	{
		var response = context.HttpContext.Response;
		var buffer = new StringBuilder();

		if (context.Object == null)
		{
			await response.WriteAsync(string.Empty);
			return;
		}

		var objectType = context.Object.GetType();

		if (context.Object is IEnumerable enumerable && objectType != typeof(string))
		{
			var items = enumerable.Cast<object>().ToList();

			if (items.Count == 0)
			{
				await response.WriteAsync(string.Empty);
				return;
			}

			//  item type from first element (most reliable)
			var itemType = items.First().GetType();
			WriteCsv(buffer, items, itemType);
		}
		else
		{
			WriteCsv(buffer, new List<object> { context.Object }, objectType);
		}

		await response.WriteAsync(buffer.ToString());
	}

	private static void WriteCsv(StringBuilder buffer, IEnumerable<object> items, Type type)
	{
		var properties = type
			.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.GetIndexParameters().Length == 0)
			// Simple types only (no nested objects)
			.Where(p => IsSimpleType(p.PropertyType))
			.ToArray();

		if (properties.Length == 0) return;

		// Header row
		buffer.AppendLine(string.Join(",", properties.Select(p => EscapeCsvValue(p.Name))));

		// Data rows
		foreach (var item in items)
		{
			var values = properties.Select(p => ValueAsString(p, item));
			buffer.AppendLine(string.Join(",", values));
		}
	}

	private static string ValueAsString(PropertyInfo property, object item)
	{
		try
		{
			var value = property.GetValue(item, null);
			if (value == null) return "";
			return EscapeCsvValue(value.ToString() ?? "");
		}
		catch
		{
			return "";
		}
	}

	/// <summary>
	/// CSV value escape — handles comma, quote, and newline characters
	/// </summary>
	private static string EscapeCsvValue(string value)
	{
		if (string.IsNullOrEmpty(value)) return "";

		// If value contains comma, quote, or newline, wrap it in double quotes
		if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
		{
			// Escape double quotes by doubling them
			value = value.Replace("\"", "\"\"");
			return $"\"{value}\"";
		}

		return value;
	}

	private static bool IsSimpleType(Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

		return underlyingType.IsPrimitive ||
				 underlyingType.IsEnum ||
				 underlyingType == typeof(string) ||
				 underlyingType == typeof(decimal) ||
				 underlyingType == typeof(DateTime) ||
				 underlyingType == typeof(DateTimeOffset) ||
				 underlyingType == typeof(TimeSpan) ||
				 underlyingType == typeof(Guid);
	}
}