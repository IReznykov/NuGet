using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace Ikc5.TypeLibrary.Tests
{
	public class TypeExtensionsTests
	{
		#region Tests

		[Fact]
		public void SetDefaultValue_ShouldKeepValues_WithoutDefaultAttributes()
		{
			SimpleObject1 testObject = null;
			var exception = Record.Exception(() => testObject = new SimpleObject1());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Simple");
			testObject.Count.Should().Be(new int());
		}


		[Fact]
		public void GetDefaultValueObject_ShouldKeepValues_WithoutDefaultAttributes()
		{
			SimpleObject2 testObject = null;
			var exception = Record.Exception(() => testObject = new SimpleObject2());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Simple");
			testObject.Count.Should().Be(5);
		}

		[Fact]
		public void GetDefaultValueType_ShouldKeepValues_WithoutDefaultAttributes()
		{
			SimpleObject3 testObject = null;
			var exception = Record.Exception(() => testObject = new SimpleObject3());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Simple");
			testObject.Count.Should().Be(5);
		}

		[Fact]
		public void SetDefaultValue_ShouldAssign_DefaultValues()
		{
			DefaultsObject1 testObject = null;
			var exception = Record.Exception(() => testObject = new DefaultsObject1());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Default");
			testObject.Count.Should().Be(100);
		}


		[Fact]
		public void GetDefaultValueObject_ShouldAssign_DefaultValues()
		{
			DefaultsObject2 testObject = null;
			var exception = Record.Exception(() => testObject = new DefaultsObject2());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Default");
			testObject.Count.Should().Be(100);
		}

		[Fact]
		public void GetDefaultValueType_ShouldAssign_DefaultValues()
		{
			DefaultsObject3 testObject = null;
			var exception = Record.Exception(() => testObject = new DefaultsObject3());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Default");
			testObject.Count.Should().Be(100);
		}

		[Fact]
		public void CopyValuesFrom_ShouldCopy_PropertyValues()
		{
			PropertyObject1 testObject1 = null;
			PropertyObject2 testObject2 = null;
			var exception = Record.Exception(() =>
			{
				testObject1 = new PropertyObject1
				{
					Name = "Name1",
					Count = 10,
					Index = 5
				};
				testObject2 = new PropertyObject2
				{
					Name = "Name2",
					Count = 20,
					Title = "Title2"
				};
				testObject1.CopyValuesFrom(testObject2);
			});

			exception.Should().BeNull();
			testObject1.Should().NotBeNull();
			testObject2.Should().NotBeNull();

			testObject1.Name.Should().Be("Name2");
			testObject1.Count.Should().Be(20);
			testObject1.Index.Should().Be(5);

			testObject2.Name.Should().Be("Name2");
			testObject2.Count.Should().Be(20);
			testObject2.Title.Should().Be("Title2");
		}

		[Fact]
		public void CopyValuesTo_ShouldCopy_PropertyValues()
		{
			PropertyObject1 testObject1 = null;
			PropertyObject2 testObject2 = null;
			var exception = Record.Exception(() =>
			{
				testObject1 = new PropertyObject1
				{
					Name = "Name1",
					Count = 10,
					Index = 5
				};
				testObject2 = new PropertyObject2
				{
					Name = "Name2",
					Count = 20,
					Title = "Title2"
				};
				testObject1.CopyValuesTo(testObject2);
			});

			exception.Should().BeNull();
			testObject1.Should().NotBeNull();
			testObject2.Should().NotBeNull();

			testObject1.Name.Should().Be("Name1");
			testObject1.Count.Should().Be(10);
			testObject1.Index.Should().Be(5);

			testObject2.Name.Should().Be("Name1");
			testObject2.Count.Should().Be(10);
			testObject2.Title.Should().Be("Title2");
		}

		#endregion

		#region Helper classes

		private class SimpleObject1
		{
			public SimpleObject1()
			{
				this.SetDefaultValue<string>(nameof(Name));
				this.SetDefaultValue<int>(nameof(Count));
			}

			public string Name { get; set; } = "Simple";
			public int Count { get; set; }
		}

		private class SimpleObject2
		{
			public SimpleObject2()
			{
				// set value of Name property to default value, but it is not defined
				this.GetDefaultValue(ref _name, nameof(Name));
				// set value of Count property to provided value, 5
				this.GetDefaultValue(ref _count, 5, nameof(Count));
			}

			private string _name = "Simple";

			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			private int _count;

			public int Count
			{
				get { return _count; }
				set { _count = value; }
			}
		}

		private class SimpleObject3
		{
			public SimpleObject3()
			{
				// set value of Name property to default value, but it is not defined
				GetType().GetDefaultValue(ref _name, nameof(Name));
				// set value of Count property to provided value, 5
				GetType().GetDefaultValue(ref _count, 5, nameof(Count));
			}

			private string _name = "Simple";

			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			private int _count;

			public int Count
			{
				get { return _count; }
				set { _count = value; }
			}
		}

		private class DefaultsObject1
		{
			public DefaultsObject1()
			{
				// set value of Name property to default value, "Default"
				this.SetDefaultValue<string>(nameof(Name));
				// set value of Count property to default value, 100
				this.SetDefaultValue<int>(nameof(Count));
			}

			[DefaultValue("Default")]
			public string Name { get; set; } = "Simple";

			[DefaultValue(100)]
			public int Count { get; set; }
		}

		private class DefaultsObject2
		{
			public DefaultsObject2()
			{
				// set value of Name property to default value, "Default"
				this.GetDefaultValue(ref _name, nameof(Name));
				// set value of Count property to default value, 100
				this.GetDefaultValue(ref _count, 5, nameof(Count));
			}

			private string _name = "Simple";

			[DefaultValue("Default")]
			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			private int _count;

			[DefaultValue(100)]
			public int Count
			{
				get { return _count; }
				set { _count = value; }
			}
		}

		private class DefaultsObject3
		{
			public DefaultsObject3()
			{
				// set value of Name property to default value, "Default"
				GetType().GetDefaultValue(ref _name, nameof(Name));
				// set value of Count property to default value, 100
				GetType().GetDefaultValue(ref _count, 5, nameof(Count));
			}

			private string _name = "Simple";

			[DefaultValue("Default")]
			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			private int _count;

			[DefaultValue(100)]
			public int Count
			{
				get { return _count; }
				set { _count = value; }
			}
		}

		private class PropertyObject1
		{
			public string Name { get; set; } = "Simple";
			public int Count { get; set; } = 10;
			public int Index { get; set; } = 1;
		}

		private class PropertyObject2
		{
			public string Name { get; set; } = "Simple";
			public int Count { get; set; } = 100;
			public string Title { get; set; } = "Title";
		}


		#endregion
	}
}
