# Specflow.RestSharp

### Resources

* [SpecFlow](http://specflow.org/docs/) 

# Setup - Windows
	Download and install Visual Studio

## Visual Studio Config
Open Visual Studio and configure the following;

### Install Add-ins


Tools => Extensions

* NUnit 3 Test Adapter
* Specflow flow Visual Studio
	
### Running
#### IDE
* Build
* Visual Studio: Tests => Windows => Test Explorer

#### Local Execution
Run tests

nunit3-console.exe "\pathToProject\Specflow.RestSharp.csproj" --where "cat=TagYouWantToRun"

	
