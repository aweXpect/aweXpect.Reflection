﻿using System.Reflection;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Collections;

public sealed partial class FilteredExtensions
{
	public class Methods
	{
		public sealed class With
		{
			public sealed class AttributeTests
			{
				[Fact]
				public async Task ShouldFilterForMethodsWithAttribute()
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<BarAttribute>();

					await That(methods).IsEqualTo([
						typeof(Dummy).GetMethod(nameof(Dummy.MyBarMethod)),
						typeof(DummyChild).GetMethod(nameof(DummyChild.MyBarMethod)),
					]).InAnyOrder();
					await That(methods.GetDescription())
						.IsEqualTo("methods with FilteredExtensions.Methods.With.BarAttribute")
						.AsPrefix();
				}

				[Fact]
				public async Task WhenInheritIsSetToFalse_ShouldFilterForTypesWithAttributeDirectlySet()
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<BarAttribute>(false);

					await That(methods).HasSingle().Which.IsEqualTo(typeof(Dummy).GetMethod(nameof(Dummy.MyBarMethod)));
					await That(methods.GetDescription())
						.IsEqualTo("methods with direct FilteredExtensions.Methods.With.BarAttribute")
						.AsPrefix();
				}

				[Theory]
				[MemberData(nameof(GetFooValues))]
				public async Task WithPredicate_ShouldFilterForTypesWithAttributeMatchingPredicate(int value,
					MethodInfo?[] expectedTypes)
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<FooAttribute>(foo => foo.Value == value);

					await That(methods).IsEqualTo(expectedTypes).InAnyOrder();
					await That(methods.GetDescription())
						.IsEqualTo(
							"methods with FilteredExtensions.Methods.With.FooAttribute matching foo => foo.Value == value")
						.AsPrefix();
				}

				[Fact]
				public async Task WithPredicate_WhenInheritIsSetToFalse_ShouldFilterForTypesWithAttributeDirectlySet()
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<FooAttribute>(foo => foo.Value == 2, false);

					await That(methods).HasSingle().Which
						.IsEqualTo(typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod2)));
					await That(methods.GetDescription())
						.IsEqualTo(
							"methods with direct FilteredExtensions.Methods.With.FooAttribute matching foo => foo.Value == 2")
						.AsPrefix();
				}

				public static TheoryData<int, MethodInfo?[]> GetFooValues()
					=> new()
					{
						{
							2, [
								typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod2)),
								typeof(DummyChild).GetMethod(nameof(DummyChild.MyFooMethod2)),
							]
						},
						{
							3, [typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod3)),]
						},
					};
			}

			public sealed class OrWithAttributeTests
			{
				[Fact]
				public async Task ShouldFilterForMethodsWithAttribute()
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<BarAttribute>().OrWith<FooAttribute>();

					await That(methods).IsEqualTo([
						typeof(Dummy).GetMethod(nameof(Dummy.MyBarMethod)),
						typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod2)),
						typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod3)),
						typeof(DummyChild).GetMethod(nameof(DummyChild.MyBarMethod)),
						typeof(DummyChild).GetMethod(nameof(DummyChild.MyFooMethod2)),
					]).InAnyOrder();
					await That(methods.GetDescription())
						.IsEqualTo(
							"methods with FilteredExtensions.Methods.With.BarAttribute or with FilteredExtensions.Methods.With.FooAttribute")
						.AsPrefix();
				}

				[Fact]
				public async Task WhenInheritIsSetToFalse_ShouldFilterForTypesWithAttributeDirectlySet()
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<BarAttribute>().OrWith<FooAttribute>(false);

					await That(methods).IsEqualTo([
						typeof(Dummy).GetMethod(nameof(Dummy.MyBarMethod)),
						typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod2)),
						typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod3)),
						typeof(DummyChild).GetMethod(nameof(DummyChild.MyBarMethod)),
					]).InAnyOrder();
					await That(methods.GetDescription())
						.IsEqualTo(
							"methods with FilteredExtensions.Methods.With.BarAttribute or with direct FilteredExtensions.Methods.With.FooAttribute")
						.AsPrefix();
				}

				[Theory]
				[MemberData(nameof(GetFooValues))]
				public async Task WithPredicate_ShouldFilterForTypesWithAttributeMatchingPredicate(int value,
					MethodInfo?[] expectedTypes)
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<BarAttribute>(false).OrWith<FooAttribute>(foo => foo.Value == value);

					await That(methods).IsEqualTo(expectedTypes).InAnyOrder();
					await That(methods.GetDescription())
						.IsEqualTo(
							"methods with direct FilteredExtensions.Methods.With.BarAttribute or with FilteredExtensions.Methods.With.FooAttribute matching foo => foo.Value == value")
						.AsPrefix();
				}

				[Fact]
				public async Task WithPredicate_WhenInheritIsSetToFalse_ShouldFilterForTypesWithAttributeDirectlySet()
				{
					Reflection.Collections.Filtered.Methods methods = In.AssemblyContaining<FilteredExtensions>()
						.Types().Methods().With<BarAttribute>(_ => false)
						.OrWith<FooAttribute>(foo => foo.Value == 2, false);

					await That(methods).HasSingle().Which
						.IsEqualTo(typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod2)));
					await That(methods.GetDescription())
						.IsEqualTo(
							"methods with FilteredExtensions.Methods.With.BarAttribute matching _ => false or with direct FilteredExtensions.Methods.With.FooAttribute matching foo => foo.Value == 2")
						.AsPrefix();
				}

				public static TheoryData<int, MethodInfo?[]> GetFooValues()
					=> new()
					{
						{
							2, [
								typeof(Dummy).GetMethod(nameof(Dummy.MyBarMethod)),
								typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod2)),
								typeof(DummyChild).GetMethod(nameof(DummyChild.MyFooMethod2)),
							]
						},
						{
							3, [
								typeof(Dummy).GetMethod(nameof(Dummy.MyBarMethod)),
								typeof(Dummy).GetMethod(nameof(Dummy.MyFooMethod3)),
							]
						},
					};
			}

			[AttributeUsage(AttributeTargets.Method)]
			private class BarAttribute : Attribute
			{
			}

			private class Dummy
			{
				[Bar]
				public virtual void MyBarMethod()
				{
				}

				[Foo(Value = 2)]
				public virtual void MyFooMethod2()
				{
				}

				[Foo(Value = 3)]
				public virtual void MyFooMethod3()
				{
				}
			}

			private class DummyChild : Dummy
			{
				public override void MyBarMethod()
				{
				}

				public override void MyFooMethod2()
				{
				}
			}

			[AttributeUsage(AttributeTargets.Method)]
			private class FooAttribute : Attribute
			{
				public int Value { get; set; }
			}
		}
	}
}
