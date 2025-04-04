namespace aweXpect.Api.Tests;

public sealed class ApiAcceptance
{
	/// <summary>
	///     Execute this test to update the expected public API to the current API surface.
	/// </summary>
	[Fact(Explicit = true)]
	public async Task AcceptApiChanges()
	{
		string[] assemblyNames =
		[
			"aweXpect.T6e",
		];

		foreach (string assemblyName in assemblyNames)
		{
			foreach (string framework in Helper.GetTargetFrameworks())
			{
				string publicApi = Helper.CreatePublicApi(framework, assemblyName)
					.Replace("\n", Environment.NewLine);
				Helper.SetExpectedApi(framework, assemblyName, publicApi);
			}
		}

		await That(assemblyNames).IsNotEmpty();
	}
}
