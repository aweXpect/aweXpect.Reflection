using BenchmarkDotNet.Attributes;

namespace aweXpect.T6e.Benchmarks;

/// <summary>
///     This is a dummy benchmark in the T6e template.
/// </summary>
public partial class HappyCaseBenchmarks
{
	[Benchmark]
	public TimeSpan Dummy_aweXpect()
		=> TimeSpan.FromSeconds(10);
}
