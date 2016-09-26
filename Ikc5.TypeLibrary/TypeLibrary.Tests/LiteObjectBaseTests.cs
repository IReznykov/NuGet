using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace Ikc5.TypeLibrary.Tests
{
	public class LiteObjectBaseTests
	{
		#region Tests

		[Fact]
		public void LiteObjectBase_ShouldDoNothing_WithoutDefaultAttributes()
		{
			EmptyObject testObject = null;
			var exception = Record.Exception(() => testObject = new EmptyObject());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
		}

		[Fact]
		public void LiteObjectBase_ShouldKeepValues_WithoutDefaultAttributes()
		{
			SimpleObject testObject = null;
			var exception = Record.Exception(() => testObject = new SimpleObject());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Simple");
			testObject.Count.Should().Be(new int());
		}

		[Fact]
		public void LiteObjectBase_ShouldAssign_DefaultValues()
		{
			ExtendedSimpleObject testObject = null;
			var exception = Record.Exception(() => testObject = new ExtendedSimpleObject());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Extended");
			testObject.Title.Should().Be("ExtendedTitle");
			testObject.Count.Should().Be(10);
		}

		[Fact]
		public void LiteObjectBase_ShouldAssign_BaseDefaultValues()
		{
			DefaultsObject testObject = null;
			var exception = Record.Exception(() => testObject = new DefaultsObject());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Default");
			testObject.Title.Should().Be("DefaultTitle");
			testObject.Count.Should().Be(10);
		}

		[Fact]
		public void LiteObjectBase_ShouldAssign_OverridedDefaultValues()
		{
			ExtendedDefaultsObject testObject = null;
			var exception = Record.Exception(() => testObject = new ExtendedDefaultsObject());

			exception.Should().BeNull();
			testObject.Should().NotBeNull();
			testObject.Name.Should().Be("Default");
			testObject.Title.Should().Be("ExtendedDefaultTitle");
			testObject.Count.Should().Be(100);
		}

		#endregion

		#region Helper classes

		private class EmptyObject : LiteObjectBase
		{
		}

		private class SimpleObject : LiteObjectBase
		{
			public string Name { get; set; } = "Simple";
			public int Count { get; set; }
		}

		private class ExtendedSimpleObject : LiteObjectBase
		{
			[DefaultValue("Extended")]
			public string Name { get; set; } = "Simple";

			[DefaultValue("ExtendedTitle")]
			public string Title { get; set; }

			public int Count { get; set; } = 10;
		}

		private class DefaultsObject : LiteObjectBase
		{
			[DefaultValue("Default")]
			public virtual string Name { get; set; } = "Simple";

			[DefaultValue("DefaultTitle")]
			public virtual string Title { get; set; }

			public virtual int Count { get; set; } = 10;
		}

		private class ExtendedDefaultsObject : DefaultsObject
		{
			public override string Name { get; set; } = "ExtendedDefault";

			[DefaultValue("ExtendedDefaultTitle")]
			public override string Title { get; set; }

			[DefaultValue(100)]
			public override int Count { get; set; }
		}

		#endregion
	}
}
