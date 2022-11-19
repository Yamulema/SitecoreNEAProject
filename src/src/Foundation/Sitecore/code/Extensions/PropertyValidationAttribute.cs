using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Neambc.Seiumb.Foundation.Sitecore.Extensions {
	public abstract class PropertyValidationAttribute : ValidationAttribute {
		#region Fields
		private object _value;
		#endregion

		#region Properties

		/// <summary>
		/// Gets whether or not <see cref="ValidationContext"/> is required.
		/// </summary>
		public override bool RequiresValidationContext => true;

		/// <summary>
		/// Gets the name of the other property.
		/// </summary>
		protected string PropertyName;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes new instance of the <see cref="PropertyValidationAttribute"/> class.
		/// </summary>
		/// <param name="propertyName">The name of the other property.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="propertyName"/> is <c>null</c>, empty or whitespace.</exception>
		protected PropertyValidationAttribute(string propertyName) {
			if (string.IsNullOrWhiteSpace(propertyName)) {
				throw new ArgumentNullException(nameof(propertyName));
			}
			PropertyName = propertyName;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the value of the other property.
		/// </summary>
		/// <param name="validationContext">The context of the validation.</param>
		/// <returns>A value of the other property.</returns>
		/// <exception cref="InvalidOperationException">If object type of the validation context does not contain <see cref="PropertyName"/> property.</exception>
		/// <exception cref="NotSupportedException">If property requires indexer parameters.</exception>
		protected object GetValue(ValidationContext validationContext) {
			var type = validationContext.ObjectType;
			var property = type.GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

			if (property == null) {
				throw new InvalidOperationException(
					$"Type {type.FullName} does not contains public instance property {PropertyName}.");
			}

			if (IsIndexerProperty(property)) {
				throw new NotSupportedException("Property with indexer parameters is not supported.");
			}

			_value = property.GetValue(validationContext.ObjectInstance);

			return _value;
		}

		private bool IsIndexerProperty(PropertyInfo property) {
			return property.GetIndexParameters().Length > 0;
		}

		#endregion
	}
}