using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Linq;

namespace Movies.Contracts.Attributes
{
	public class EnumArrayDataTypeWithNoDuplicatesAttribute : ValidationAttribute
	{
		public bool AllowDuplicate { get; set; } = false;

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is null) return ValidationResult.Success;

			if (value is not IList list)
				throw new InvalidOperationException("Must be of either type array or list");

			var enumType = GetEnumType(validationContext.ObjectType, validationContext.MemberName);

			var set = new HashSet<object>();

			foreach (var item in list)
			{
				var (isValid, errorMessage) = ValidateEnumValue(item, enumType, validationContext);
				if (!isValid)
				{
					return new ValidationResult(errorMessage);
				}

				if (!AllowDuplicate && !set.Add(item))
				{
					return new ValidationResult("The list cannot contain duplicate values.");
				}
			}

			return ValidationResult.Success;
		}

		private static Type GetEnumType(Type objectType, string memberName)
		{
			Type type = null;

			var propertyInfo = objectType.GetProperty(memberName);
			if (propertyInfo != null)
			{
				var propertyType = propertyInfo.PropertyType;

				if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
				{
					type = propertyType.GetGenericArguments()[0];
				}
				else if (propertyType.IsArray)
				{
					type = propertyType.GetElementType();
				}
			}

			if (type is null || !type.IsEnum)
			{
				throw new InvalidOperationException("No valid enum type found.");
			}

			return type;
		}

		private static (bool isValid, string errorMessage) ValidateEnumValue(object value, Type enumType, ValidationContext validationContext)
		{
			var enumDataTypeAttribute = new EnumDataTypeAttribute(enumType);

			var results = new List<ValidationResult>();
			var context = new ValidationContext(value) { MemberName = validationContext.MemberName };

			if (!Validator.TryValidateValue(value, context, results, new List<EnumDataTypeAttribute> { enumDataTypeAttribute }))
			{
				var validationErrors = results.Select(r => r.ErrorMessage);
				return (false, string.Join(", ", validationErrors));
			}

			return (true, null);
		}
	}
}
