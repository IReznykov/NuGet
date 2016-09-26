using System;
using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace Ikc5.TypeLibrary.Tests
{
	public class LiteObjectServiceTests
	{
		#region Tests

		#region GetLiteObjectType

		[Fact]
		public void GetLiteObjectType_Should_ReturnValue()
		{
			EmptyObject testObject = null;
			Type liteObjectType = null;

			var exception = Record.Exception(() =>
			{
				testObject = new EmptyObject();
				liteObjectType = (new LiteObjectService()).GetLiteObjectType(testObject);
			});

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			liteObjectType.Should().NotBeNull();
		}

		[Fact]
		public void GetLiteObjectType_Should_ReturnTheSameValue()
		{
			SimpleObject testObject = null;
			Type liteObjectType1 = null;
			Type liteObjectType2 = null;

			var exception = Record.Exception(() =>
			{
				testObject = new SimpleObject();
				var liteObjectService = new LiteObjectService();
				liteObjectType1 = liteObjectService.GetLiteObjectType(testObject);
				liteObjectType2 = liteObjectService.GetLiteObjectType(testObject);
			});

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			liteObjectType1.Should().NotBeNull();
			liteObjectType2.Should().NotBeNull();
			liteObjectType1.Should().BeSameAs(liteObjectType2);
		}

		[Fact]
		public void GetLiteObjectType_Should_ReturnDifferentValueFromDifferentServices()
		{
			SimpleObject testObject = null;
			Type liteObjectType1 = null;
			Type liteObjectType2 = null;

			var exception = Record.Exception(() =>
			{
				testObject = new SimpleObject();
				liteObjectType1 = (new LiteObjectService()).GetLiteObjectType(testObject);
				liteObjectType2 = (new LiteObjectService()).GetLiteObjectType(testObject);
			});

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			liteObjectType1.Should().NotBeNull();
			liteObjectType2.Should().NotBeNull();
			liteObjectType1.Should().NotBeSameAs(liteObjectType2);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void GetLiteObjectType_Should_ReturnObjectWithAllProperties(bool top)
		{
			Type liteObjectType = null;
			var propertyNames = new[] { "Name", "Count", "Title", "Indexed" };
			var propertyTypes = new[] { typeof(string), typeof(int), typeof(string), typeof(bool) };

			var exception = Record.Exception(() =>
			{
				var testObject = new DefaultsObject();
				liteObjectType = (new LiteObjectService()).GetLiteObjectType(testObject, top);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObjectType);

			var propertyInfos = TypeDescriptor.GetProperties(liteObjectType);
			Assert.Equal(propertyInfos.Count, propertyNames.Length);

			for (var pos = 0; pos < propertyNames.Length; pos++)
			{
				var property = propertyInfos[propertyNames[pos]];
				Assert.NotNull(property);
				Assert.Equal(property.PropertyType, propertyTypes[pos]);
			}
		}

		[Fact]
		public void GetLiteObjectType_Should_ReturnPropertiesFromDerivedType()
		{
			Type liteObjectType = null;
			var propertyNames = new[] { "Name", "Count", "LastDateTime" };
			var propertyTypes = new[] { typeof(string), typeof(int), typeof(DateTime) };

			var exception = Record.Exception(() =>
			{
				var testObject = new ExtendedDefaultsObject();
				liteObjectType = (new LiteObjectService()).GetLiteObjectType(testObject, true);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObjectType);

			var propertyInfos = TypeDescriptor.GetProperties(liteObjectType);
			Assert.Equal(propertyInfos.Count, propertyNames.Length);

			for (var pos = 0; pos < propertyNames.Length; pos++)
			{
				var property = propertyInfos[propertyNames[pos]];
				Assert.NotNull(property);
				Assert.Equal(property.PropertyType, propertyTypes[pos]);
			}
		}

		[Fact]
		public void GetLiteObjectType_Should_ReturnPropertiesFromDerivedAndBasedType()
		{
			Type liteObjectType = null;
			var propertyNames = new[] { "Name", "Count", "Title", "Indexed", "LastDateTime" };
			var propertyTypes = new[] { typeof(string), typeof(int), typeof(string), typeof(bool), typeof(DateTime) };

			var exception = Record.Exception(() =>
			{
				var testObject = new ExtendedDefaultsObject();
				liteObjectType = (new LiteObjectService()).GetLiteObjectType(testObject, false);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObjectType);

			var propertyInfos = TypeDescriptor.GetProperties(liteObjectType);
			Assert.Equal(propertyInfos.Count, propertyNames.Length);

			for (var pos = 0; pos < propertyNames.Length; pos++)
			{
				var property = propertyInfos[propertyNames[pos]];
				Assert.NotNull(property);
				Assert.Equal(property.PropertyType, propertyTypes[pos]);
			}
		}

		#endregion GetLiteObjectType

		#region GetLiteObject

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void GetLiteObject_Should_ReturnObjectWithAllProperties(bool top)
		{
			var testObject = new DefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle"
			};
			dynamic liteObject = null;

			var exception = Record.Exception(() =>
			{
				liteObject = (new LiteObjectService()).GetLiteObject(testObject, top);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);

			Assert.Equal(testObject.Name, liteObject.Name);
			Assert.Equal(testObject.Count, liteObject.Count);
			Assert.Equal(testObject.Title, liteObject.Title);
			Assert.Equal(testObject.Indexed, liteObject.Indexed);
		}

		[Fact]
		public void GetLiteObject_Should_ReturnPropertiesFromDerivedType()
		{
			var testObject = new ExtendedDefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle",
				Indexed = false
			};
			dynamic liteObject = null;

			var exception = Record.Exception(() =>
			{
				liteObject = (new LiteObjectService()).GetLiteObject(testObject, true);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);

			Assert.Equal(testObject.Name, liteObject.Name);
			Assert.Equal(testObject.Count, liteObject.Count);
			Assert.Equal(testObject.LastDateTime, liteObject.LastDateTime);
		}

		[Fact]
		public void GetLiteObject_Should_ReturnPropertiesFromDerivedAndBasedType()
		{
			var testObject = new ExtendedDefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle",
				Indexed = false
			};
			dynamic liteObject = null;

			var exception = Record.Exception(() =>
			{
				liteObject = (new LiteObjectService()).GetLiteObject(testObject, true);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(liteObject);

			Assert.Equal(testObject.Name, liteObject.Name);
			Assert.Equal(testObject.Count, liteObject.Count);
			Assert.Equal(testObject.LastDateTime, liteObject.LastDateTime);
		}

		#endregion GetLiteObject

		#region CopyLiteObjectValues


		[Fact]
		public void CopyLiteObjectValues_Should_ReturnNullOnWrongLiteType()
		{
			object parentObject = new DefaultsObject();

			var exception = Record.Exception(() =>
			{
				parentObject = (new LiteObjectService()).CopyLiteObjectValues(parentObject, new DefaultsObject());
			});

			// check correct properties
			Assert.Null(exception);
			Assert.Null(parentObject);
		}

		[Fact]
		public void CopyLiteObjectValues_Should_ReturnNullOnDifferentServices()
		{
			var testObject = new DefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle"
			};
			object parentObject = new DefaultsObject();

			var exception = Record.Exception(() =>
			{
				var liteObject = (new LiteObjectService()).GetLiteObject(testObject);
				parentObject = (new LiteObjectService()).CopyLiteObjectValues(parentObject, liteObject);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.Null(parentObject);
		}


		[Fact]
		public void CopyLiteObjectValues_Should_ReturnNullOnWrongParentObject()
		{
			var testObject = new ExtendedDefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle"
			};
			object parentObject = new DefaultsObject();

			var exception = Record.Exception(() =>
			{
				var service = new LiteObjectService();
				var liteObject = service.GetLiteObject(testObject);
				parentObject = service.CopyLiteObjectValues(parentObject, liteObject);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.Null(parentObject);
		}

		[Theory]
		[InlineData(false, true)]
		[InlineData(true, false)]
		public void CopyLiteObjectValues_Should_ReturnNullOnDifferentTopValue(bool top1, bool top2)
		{
			var testObject = new ExtendedDefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle",
				Indexed = false
			};
			var parentObject = new ExtendedDefaultsObject();

			var exception = Record.Exception(() =>
			{
				var service = new LiteObjectService();
				dynamic liteObject = service.GetLiteObject(testObject, top1);
				parentObject = service.CopyLiteObjectValues(parentObject, liteObject, top2);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.Null(parentObject);
		}
		
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void CopyLiteObjectValues_Should_ReturnObjectWithAllProperties(bool top)
		{
			var testObject = new DefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle"
			};
			var parentObject = new DefaultsObject();

			var exception = Record.Exception(() =>
			{
				var service = new LiteObjectService();
				dynamic liteObject = service.GetLiteObject(testObject, top);
				parentObject = service.CopyLiteObjectValues(parentObject, liteObject, top);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(parentObject);

			Assert.Equal(testObject.Name, parentObject.Name);
			Assert.Equal(testObject.Count, parentObject.Count);
			Assert.Equal(testObject.Indexed, parentObject.Indexed);
			Assert.Equal(testObject.Title, parentObject.Title);
			Assert.Equal(testObject.Id, parentObject.Id);
		}

		[Fact]
		public void CopyLiteObjectValues_Should_ReturnPropertiesFromDerivedType()
		{
			var testObject = new ExtendedDefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle",
				Indexed = false
			};
			var parentObject = new ExtendedDefaultsObject();

			var exception = Record.Exception(() =>
			{
				var service = new LiteObjectService();
				dynamic liteObject = service.GetLiteObject(testObject, true);
				parentObject = service.CopyLiteObjectValues(parentObject, liteObject, true);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(parentObject);

			Assert.Equal(testObject.Name, parentObject.Name);
			Assert.Equal(testObject.Count, parentObject.Count);
			Assert.Equal(testObject.LastDateTime, parentObject.LastDateTime);

			Assert.NotEqual(testObject.Title, parentObject.Title);
			Assert.NotEqual(testObject.Indexed, parentObject.Indexed);

			Assert.Equal(testObject.Id, parentObject.Id);
		}

		[Fact]
		public void CopyLiteObjectValues_Should_ReturnPropertiesFromDerivedAndBasedType()
		{
			var testObject = new ExtendedDefaultsObject
			{
				Name = "SomeName",
				Count = 30,
				Title = "SomeTitle",
				Indexed = false
			};
			var parentObject = new ExtendedDefaultsObject();

			var exception = Record.Exception(() =>
			{
				var service = new LiteObjectService();
				dynamic liteObject = service.GetLiteObject(testObject, false);
				parentObject = service.CopyLiteObjectValues(parentObject, liteObject, false);
			});

			// check correct properties
			Assert.Null(exception);
			Assert.NotNull(parentObject);

			Assert.Equal(testObject.Name, parentObject.Name);
			Assert.Equal(testObject.Count, parentObject.Count);
			Assert.Equal(testObject.Title, parentObject.Title);
			Assert.Equal(testObject.Indexed, parentObject.Indexed);
			Assert.Equal(testObject.Id, parentObject.Id);
			Assert.Equal(testObject.LastDateTime, parentObject.LastDateTime);
		}

		#endregion CopyLiteObjectValues

		#endregion

		#region Helper classes

		private class EmptyObject
		{
		}

		private class SimpleObject
		{
			public string Name { get; set; } = "Simple";
			public int Count { get; set; }
		}

		private class DefaultsObject
		{
			[DefaultValue("Default")]
			public virtual string Name { get; set; } = "Simple";

			[DefaultValue("DefaultTitle")]
			public virtual string Title { get; set; }

			public virtual int Count { get; set; } = 10;

			public bool Indexed { get; set; } = true;

			public int Id { get; } = 1;
		}

		private class ExtendedDefaultsObject : DefaultsObject
		{
			public override string Name { get; set; } = "ExtendedDefault";

			[DefaultValue(100)]
			public override int Count { get; set; }

			public DateTime LastDateTime { get; set; } = DateTime.UtcNow;
		}

		#endregion

	}
}
