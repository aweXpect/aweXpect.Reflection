using System;
using System.Linq;
using System.Threading.Tasks;
using aweXpect.Reflection.Collections;

namespace aweXpect.Reflection.Tests.Examples;

public sealed class IssueExampleTests
{
	[Fact]
	public async Task TaskExample_ShouldFilterForTaskReturningMethods()
	{
		// This demonstrates the exact example from the issue
		Filtered.Methods methods = In.Type<AsyncExampleClass>()
			.Methods().WhichReturn<Task>();

		await That(methods).IsEqualTo([
			typeof(AsyncExampleClass).GetMethod(nameof(AsyncExampleClass.AsyncMethodWithTask))!,
			typeof(AsyncExampleClass).GetMethod(nameof(AsyncExampleClass.AsyncMethodWithTaskAsync))!,
			typeof(AsyncExampleClass).GetMethod(nameof(AsyncExampleClass.GetTaskMethod))!,
		]).InAnyOrder();
		
		await That(methods.GetDescription())
			.IsEqualTo("methods which return Task in type")
			.AsPrefix();
	}

	[Fact]
	public async Task TaskExample_ShouldFilterForMethodsEndingWithAsync()
	{
		// This demonstrates filtering methods that return Task and have "Async" suffix
		Filtered.Methods methods = In.Type<AsyncExampleClass>()
			.Methods().WhichReturn<Task>();

		var asyncMethods = methods.Where(m => m.Name.EndsWith("Async")).ToArray();
		
		await That(asyncMethods).IsEqualTo([
			typeof(AsyncExampleClass).GetMethod(nameof(AsyncExampleClass.AsyncMethodWithTaskAsync))!,
		]).InAnyOrder();
	}

	private class AsyncExampleClass
	{
		public async Task AsyncMethodWithTask() => await Task.Delay(1);
		public async Task AsyncMethodWithTaskAsync() => await Task.Delay(1);
		public Task GetTaskMethod() => Task.CompletedTask;
		public void SyncMethod() { }
		public string GetString() => "test";
	}
}