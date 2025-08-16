using aweXpect.Reflection.Collections;
using Xunit.Sdk;

namespace aweXpect.Reflection.Tests;

public sealed partial class ThatMethods
{
	public sealed class Return
	{
		public sealed class GenericTests
		{
			[Fact]
			public async Task ShouldFailWhenSomeMethodsDoNotReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return<string>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name.StartsWith("Get") in type ThatMethods.Return.TestClass
					             all return string,
					             but it contained not matching methods [*]
					             """).AsWildcard();
			}

			[Fact]
			public async Task ShouldSucceedWhenAllMethodsReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString));

				async Task Act()
					=> await That(methods).Return<string>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task ShouldSucceedWithInheritance()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

				async Task Act()
					=> await That(methods).Return<DummyBase>();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class TypeTests
		{
			[Fact]
			public async Task ShouldFailWhenSomeMethodsDoNotReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return(typeof(string));

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name.StartsWith("Get") in type ThatMethods.Return.TestClass
					             all return string,
					             but it contained not matching methods [*]
					             """).AsWildcard();
			}

			[Fact]
			public async Task ShouldSucceedWhenAllMethodsReturnSpecifiedType()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString));

				async Task Act()
					=> await That(methods).Return(typeof(string));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task ShouldSucceedWithInheritance()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

				async Task Act()
					=> await That(methods).Return(typeof(DummyBase));

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class OrReturnTests
		{
			[Fact]
			public async Task WithMultipleOrReturn_ShouldSupportChaining()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				await That(methods).Return<int>().OrReturn<string>().OrReturn(typeof(bool)).OrReturn<DummyBase>();
			}

			[Fact]
			public async Task WithOrReturn_WhenAllMethodsReturnOneOfTheTypes_ShouldSucceed()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name is nameof(TestClass.GetString) or nameof(TestClass.GetInt));

				await That(methods).Return<string>().OrReturn(typeof(int));
			}

			[Fact]
			public async Task WithOrReturn_WhenSomeMethodsDoNotReturnAnyOfTheTypes_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).Return<bool>().OrReturn<Task>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name.StartsWith("Get") in type ThatMethods.Return.TestClass
					             all return bool or Task,
					             but it contained not matching methods [*]
					             """).AsWildcard();
			}
		}

		public sealed class NegatedTests
		{
			public sealed class GenericTests
			{
				[Fact]
				public async Task WhenAllMethodsReturnSpecifiedType_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetString));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return<string>());

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name == nameof(TestClass.GetString) in type ThatMethods.Return.TestClass
						             not all return string,
						             but it only contained matching methods [
						               System.String GetString()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WhenMethodsReturnInheritedTypes_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return<DummyBase>());

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name == nameof(TestClass.GetDummy) in type ThatMethods.Return.TestClass
						             not all return ThatMethods.Return.DummyBase,
						             but it only contained matching methods [
						               Dummy GetDummy()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WhenSomeMethodsDoNotReturnSpecifiedType_ShouldSucceed()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name.StartsWith("Get"));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return<string>());

					await That(Act).DoesNotThrow();
				}
			}

			public sealed class TypeTests
			{
				[Fact]
				public async Task WhenAllMethodsReturnSpecifiedType_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetString));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return(typeof(string)));

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name == nameof(TestClass.GetString) in type ThatMethods.Return.TestClass
						             not all return string,
						             but it only contained matching methods [
						               System.String GetString()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WhenMethodsReturnInheritedTypes_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return(typeof(DummyBase)));

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name == nameof(TestClass.GetDummy) in type ThatMethods.Return.TestClass
						             not all return ThatMethods.Return.DummyBase,
						             but it only contained matching methods [
						               Dummy GetDummy()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WhenSomeMethodsDoNotReturnSpecifiedType_ShouldSucceed()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name.StartsWith("Get"));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return(typeof(string)));

					await That(Act).DoesNotThrow();
				}
			}

			public sealed class OrReturnTests
			{
				[Fact]
				public async Task WithMultipleOrReturn_ShouldSupportChaining()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.AsyncMethod));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they
							=> they.Return<int>().OrReturn<string>().OrReturn(typeof(bool)).OrReturn<DummyBase>());

					await That(Act).DoesNotThrow();
				}

				[Fact]
				public async Task WithOrReturn_WhenAllMethodsReturnOneOfTheTypes_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name is nameof(TestClass.GetString) or nameof(TestClass.GetInt));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return<string>().OrReturn(typeof(int)));

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name is nameof(TestClass.GetString) or nameof(TestClass.GetInt) in type ThatMethods.Return.TestClass
						             not all return string or int,
						             but it only contained matching methods [
						               System.String GetString(),
						               Int32 GetInt()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WithOrReturn_WhenSomeMethodsDoNotReturnAnyOfTheTypes_ShouldSucceed()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name.StartsWith("Get"));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.Return<bool>().OrReturn<Task>());

					await That(Act).DoesNotThrow();
				}
			}
		}

	public sealed class ReturnExactly
	{
		public sealed class GenericTests
		{
			[Fact]
			public async Task WhenAllMethodsReturnSpecifiedType_ShouldSucceed()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString));

				async Task Act()
					=> await That(methods).ReturnExactly<string>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenSomeMethodsDoNotReturnSpecifiedType_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).ReturnExactly<string>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name.StartsWith("Get") in type ThatMethods.Return.TestClass
					             all return exactly string,
					             but it contained not matching methods [
					               Int32 GetInt(),
					               Boolean GetBool(),
					               DummyBase GetDummyBase(),
					               Dummy GetDummy()
					             ]
					             """)
					.AsWildcard();
			}

			[Fact]
			public async Task WhenMethodsReturnInheritedTypes_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

				async Task Act()
					=> await That(methods).ReturnExactly<DummyBase>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name == nameof(TestClass.GetDummy) in type ThatMethods.Return.TestClass
					             all return exactly ThatMethods.Return.DummyBase,
					             but it contained not matching methods [
					               Dummy GetDummy()
					             ]
					             """)
					.AsWildcard();
			}
		}

		public sealed class TypeTests
		{
			[Fact]
			public async Task WhenAllMethodsReturnSpecifiedType_ShouldSucceed()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString));

				async Task Act()
					=> await That(methods).ReturnExactly(typeof(string));

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenSomeMethodsDoNotReturnSpecifiedType_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).ReturnExactly(typeof(string));

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name.StartsWith("Get") in type ThatMethods.Return.TestClass
					             all return exactly string,
					             but it contained not matching methods [
					               Int32 GetInt(),
					               Boolean GetBool(),
					               DummyBase GetDummyBase(),
					               Dummy GetDummy()
					             ]
					             """)
					.AsWildcard();
			}

			[Fact]
			public async Task WhenMethodsReturnInheritedTypes_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

				async Task Act()
					=> await That(methods).ReturnExactly(typeof(DummyBase));

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name == nameof(TestClass.GetDummy) in type ThatMethods.Return.TestClass
					             all return exactly ThatMethods.Return.DummyBase,
					             but it contained not matching methods [
					               Dummy GetDummy()
					             ]
					             """)
					.AsWildcard();
			}
		}

		public sealed class OrReturnExactlyTests
		{
			[Fact]
			public async Task WhenAllMethodsReturnOneOfTheSpecifiedTypes_ShouldSucceed()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString) || m.Name == nameof(TestClass.GetInt));

				async Task Act()
					=> await That(methods).ReturnExactly<string>().OrReturnExactly<int>();

				await That(Act).DoesNotThrow();
			}

			[Fact]
			public async Task WhenSomeMethodsDoNotReturnAnyOfTheSpecifiedTypes_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name.StartsWith("Get"));

				async Task Act()
					=> await That(methods).ReturnExactly<string>().OrReturnExactly<int>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name.StartsWith("Get") in type ThatMethods.Return.TestClass
					             all return exactly string or int,
					             but it contained not matching methods [
					               Boolean GetBool(),
					               DummyBase GetDummyBase(),
					               Dummy GetDummy()
					             ]
					             """)
					.AsWildcard();
			}

			[Fact]
			public async Task WhenMethodsReturnInheritedTypes_ShouldFail()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

				async Task Act()
					=> await That(methods).ReturnExactly<DummyBase>().OrReturnExactly<string>();

				await That(Act).Throws<XunitException>()
					.WithMessage("""
					             Expected that methods matching m => m.Name == nameof(TestClass.GetDummy) in type ThatMethods.Return.TestClass
					             all return exactly ThatMethods.Return.DummyBase or string,
					             but it contained not matching methods [
					               Dummy GetDummy()
					             ]
					             """)
					.AsWildcard();
			}

			[Fact]
			public async Task WithMultipleOrReturnExactly_ShouldSupportChaining()
			{
				Filtered.Methods methods = In.Type<TestClass>()
					.Methods().Which(m => m.Name == nameof(TestClass.GetString) || m.Name == nameof(TestClass.GetInt) || m.Name == nameof(TestClass.GetBool));

				async Task Act()
					=> await That(methods).ReturnExactly<string>().OrReturnExactly(typeof(int)).OrReturnExactly<bool>();

				await That(Act).DoesNotThrow();
			}
		}

		public sealed class NegatedTests
		{
			public sealed class GenericTests
			{
				[Fact]
				public async Task WhenAllMethodsReturnSpecifiedType_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetString));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.ReturnExactly<string>());

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name == nameof(TestClass.GetString) in type ThatMethods.Return.TestClass
						             not all return exactly string,
						             but it only contained matching methods [
						               System.String GetString()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WhenMethodsReturnInheritedTypes_ShouldSucceed()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetDummy));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.ReturnExactly<DummyBase>());

					await That(Act).DoesNotThrow();
				}

				[Fact]
				public async Task WhenSomeMethodsDoNotReturnSpecifiedType_ShouldSucceed()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name.StartsWith("Get"));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.ReturnExactly<string>());

					await That(Act).DoesNotThrow();
				}
			}

			public sealed class OrReturnExactlyTests
			{
				[Fact]
				public async Task WhenAllMethodsReturnOneOfTheSpecifiedTypes_ShouldFail()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetString) || m.Name == nameof(TestClass.GetInt));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.ReturnExactly<string>().OrReturnExactly<int>());

					await That(Act).Throws<XunitException>()
						.WithMessage("""
						             Expected that methods matching m => m.Name == nameof(TestClass.GetString) || m.Name == nameof(TestClass.GetInt) in type ThatMethods.Return.TestClass
						             not all return exactly string or int,
						             but it only contained matching methods [
						               System.String GetString(),
						               Int32 GetInt()
						             ]
						             """)
						.AsWildcard();
				}

				[Fact]
				public async Task WhenSomeMethodsDoNotReturnAnyOfTheSpecifiedTypes_ShouldSucceed()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name.StartsWith("Get"));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.ReturnExactly<string>().OrReturnExactly<int>());

					await That(Act).DoesNotThrow();
				}

				[Fact]
				public async Task WithMultipleOrReturnExactly_ShouldSupportChaining()
				{
					Filtered.Methods methods = In.Type<TestClass>()
						.Methods().Which(m => m.Name == nameof(TestClass.GetString) || m.Name == nameof(TestClass.GetInt));

					async Task Act()
						=> await That(methods).DoesNotComplyWith(they => they.ReturnExactly<bool>().OrReturnExactly<Task>());

					await That(Act).DoesNotThrow();
				}
			}
		}
	}

#pragma warning disable CA1822 // Mark members as static
		// ReSharper disable UnusedMember.Local
		private class TestClass
		{
			public string GetString() => "test";
			public int GetInt() => 42;
			public bool GetBool() => true;
			public DummyBase GetDummyBase() => new();
			public Dummy GetDummy() => new();
			public async Task AsyncMethod() => await Task.CompletedTask;
		}
		// ReSharper restore UnusedMember.Local
#pragma warning restore CA1822

		private class DummyBase
		{
		}

		private class Dummy : DummyBase
		{
		}
	}
}
