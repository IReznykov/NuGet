﻿using System.ComponentModel;
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
		public void SetDefaultValue_ShouldAssign_NullableProperty()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.State.Should().BeFalse();

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.State)));

			exception.Should().BeNull();
			result.Should().BeTrue();
			testObject.State.Should().NotHaveValue();
		}

		[Fact]
		public void SetDefaultValue_ShouldAssign_NameToDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Simple");

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.Name)));

			exception.Should().BeNull();
			result.Should().BeTrue();
			testObject.Name.Should().Be("Default");
		}

		[Fact]
		public void SetDefaultValue_ShouldKeep_NameWithoutDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.NameWithoutDefault.Should().Be("Name");

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.NameWithoutDefault)));

			exception.Should().BeNull();
			result.Should().BeFalse();
			testObject.NameWithoutDefault.Should().Be("Name");
		}

		[Fact]
		public void SetDefaultValue_ShouldAssign_CountToDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.Count.Should().Be(0);

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.Count)));

			exception.Should().BeNull();
			result.Should().BeTrue();
			testObject.Count.Should().Be(100);
		}

		[Fact]
		public void SetDefaultValue_ShouldAssign_ProtectedCountToDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.ProtectedCount.Should().Be(0);

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.ProtectedCount)));

			exception.Should().BeNull();
			result.Should().BeTrue();
			testObject.ProtectedCount.Should().Be(200);
		}

		[Fact]
		public void SetDefaultValue_ShouldAssign_PrivateCountToDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.PrivateCount.Should().Be(0);

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.PrivateCount)));

			exception.Should().BeNull();
			result.Should().BeTrue();
			testObject.PrivateCount.Should().Be(300);
		}

		[Fact]
		public void SetDefaultValue_ShouldKeep_CountWithoutDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.CountWithoutDefault.Should().Be(25);

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue(nameof(DefaultsObject4.CountWithoutDefault)));

			exception.Should().BeNull();
			result.Should().BeFalse();
			testObject.CountWithoutDefault.Should().Be(25);
		}

		[Fact]
		public void SetDefaultValue_ShouldAssign_DeltaToDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.Delta.Should().Be(0);

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue<short>(nameof(DefaultsObject4.Delta)));

			exception.Should().BeNull();
			result.Should().BeTrue();
			testObject.Delta.Should().Be(400);
		}

		[Fact]
		public void SetDefaultValue_ShouldKeep_DeltaWithoutDefault()
		{
			var testObject = new DefaultsObject4();
			testObject.Should().NotBeNull();
			testObject.DeltaWithoutDefault.Should().Be(50);

			bool? result = null;
			var exception = Record.Exception(() => result = testObject.SetDefaultValue<short>(nameof(DefaultsObject4.DeltaWithoutDefault)));

			exception.Should().BeNull();
			result.Should().BeFalse();
			testObject.DeltaWithoutDefault.Should().Be(50);
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
				this.SetDefaultValue(nameof(Name));
				this.SetDefaultValue(nameof(Count));
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
				this.SetDefaultValue(nameof(Name));
				// set value of Count property to default value, 100
				this.SetDefaultValue(nameof(Count));
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

		private class DefaultsObject4
		{
			[DefaultValue(null)]
			public bool? State { get; set; } = false;

			[DefaultValue("Default")]
			public string Name { get; set; } = "Simple";

			public string NameWithoutDefault { get; set; } = "Name";

			[DefaultValue(100)]
			public int Count { get; set; }

			[DefaultValue(200)]
			public int ProtectedCount { get; protected set; }

			[DefaultValue(300)]
			public int PrivateCount { get; private set; }

			public int CountWithoutDefault { get; set; } = 25;

			[DefaultValue(400)]
			public short Delta { get; set; }

			public short DeltaWithoutDefault { get; set; } = 50;

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
