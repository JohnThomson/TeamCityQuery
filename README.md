# TeamCityQuery

This is a tiny, very unpolished program that demonstrates using .NET queries in C# to perform queries on TeamCity using the REST API.

In its present form it can retrieve a list of the configurations of a particular TeamCity URL (currently SIL's build.palaso.org) and perform an additional query on each configuration. Currently it obtains the most recent build data for each configuration and reports configurations that have no builds or where the latest build is failing and not in 2017.

No dependencies...just open the solution in Visual Studio and run it. Currently must use a debug build because it uses Debug.Writeline to send the results to the Visual Studio output window.

MIT license