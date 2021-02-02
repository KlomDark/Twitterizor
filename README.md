# Welcome to Twitterizor!

This is my example solution for Jack Henry. Did not go with a classic architecture, using a different approach than normal just to show what I can do. (Lazy Singletons, etc, since I'm not using a database-backed solution) Hope you enjoy. I also constrained myself to using ONLY Microsoft NuGET packages, no third party libraries used except that ones that are contained in a new project by default. (Like jQuery, Bootstrap, etc.) All is written in .NET 5.0 rather than .NET Core or .NET Framework.

# API Keys	

API Keys should be stored in User Secrets in the following format:
<pre>
{
	"TwitterSecrets": {
	        "ApiKey": "API Key goes here",
	        "ApiSecretKey": "API Secret Key goes here",
	        "BearerToken": "Bearer Token goes heres"
        }
}
</pre>
(Really only the Bearer Token is used. So the others can be left empty strings.)

# Running the App

Set Visual Studio to start up both the Twitterizor.Web and Twitterizor.WebApi projects.

By default they will run using HTTPS on ports 44360 (Web) and port 44349 (WebApi), so if those are changed, will need to adjust the baseDir URL at the bottom of Views|Home|Index.cshtml in the Web project so the Javascript will know where to find the WebApi.