# aweXpect.T6e

[![Nuget](https://img.shields.io/nuget/v/aweXpect.T6e)](https://www.nuget.org/packages/aweXpect.T6e)
[![Build](https://github.com/aweXpect/aweXpect.T6e/actions/workflows/build.yml/badge.svg)](https://github.com/aweXpect/aweXpect.T6e/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=aweXpect_aweXpect.T6e&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=aweXpect_aweXpect.T6e)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=aweXpect_aweXpect.T6e&metric=coverage)](https://sonarcloud.io/summary/overall?id=aweXpect_aweXpect.T6e)
[![Mutation testing badge](https://img.shields.io/endpoint?style=flat&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2FaweXpect%2FaweXpect.T6e%2Fmain)](https://dashboard.stryker-mutator.io/reports/github.com/aweXpect/aweXpect.T6e/main)

Template for extension projects for [aweXpect](https://github.com/aweXpect/aweXpect).

## Steps after creating a new project from this Template:

- Enable Sonarcloud analysis
	- Create the project at [sonarcloud](https://sonarcloud.io/projects/create)
	- Set "New Code" definition to "Previous version"
	- Add the `SONAR_TOKEN` as repository secret
	- Set the long-lived branch pattern to `(main|release/.*)`
	- Change the Quality Profile for C# and the Quality Gate to "aweXpect way"
- Enable Stryker Mutator
	- Enable the repository in the [Stryker Mutator Dashboard](https://dashboard.stryker-mutator.io/repos/aweXpect)
	- Add the API Key as `STRYKER_DASHBOARD_API_KEY` repository secret
- Take over settings from T6e project
	- General Settings
	- Protect the `main` branch
	- Create a "production" environment and add the `NUGET_API_KEY` secret
- Support [StrongName signing](https://learn.microsoft.com/en-us/dotnet/standard/assembly/sign-strong-name):
	- Create a
	  new [Public/Private Key-Pair](https://learn.microsoft.com/en-us/dotnet/standard/assembly/create-public-private-key-pair):
		- Open
		  the [developer command prompt](https://learn.microsoft.com/en-us/visualstudio/ide/reference/command-prompt-powershell?view=vs-2022#start-from-windows-menu)
		- Go to the project directory
		- Type `sn -k strongname.snk` to create a new key pair
		- Type `sn -p strongname.snk publicKey.snk` to extract the public key
		- Type `sn -tp publicKey.snk` to display the public key, put it in "Directory.Build.props" and enable the
		  corresponding `PropertyGroup`
		- Delete the "publicKey.snk" file
- Replace "T6e" with your suffix both in file names and in contents.
- Adapt the copyright and project information in Source/Directory.Build.props
- Adapt the README.md

