# Digital Apprenticeships Service

##  Login Service
Licensed under the [MIT license](https://github.com/SkillsFundingAgency/das-login-service/blob/master/LICENSE)

|               |               |
| ------------- | ------------- |
|![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png)|Login Service Web|
| Info | An [OpenID Connect](https://openid.net/connect/) implementation built using Identity Server 4 on Asp.Net Core 2.2 |
| Build | [![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Endpoint%20Assessment%20Organisation/das-login-service?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1496&branchName=master) |
| Web  | N/A  |

See [Support Site](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/1731559639/Login+Service+-+Developer+Overview) for EFSA developer details.

### Developer Setup

#### Requirements

- Install [.NET Core 2.2 SDK](https://www.microsoft.com/net/download)
- Install [Visual Studio 2019](https://www.visualstudio.com/downloads/) with these workloads:
    - ASP.NET and web development
- Install [SQL Server 2017 Developer Edition](https://go.microsoft.com/fwlink/?linkid=853016)
- Install [SQL Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on atleast v5.3)
- Install [Azure Storage Explorer](http://storageexplorer.com/) 

#### Setup

- Clone this repository
- Open Visual Studio as an administrator

##### Publish Database

- Build the solution SFA.DAS.LoginService.sln
- Either use Visual Studio's `Publish Database` tool to publish the database project SFA.DAS.LoginService.Database to name {{database name}} on {{local instance name}}

	or

- Create a database manually named {{database name}} on {{local instance name}} and run each of the `.sql` scripts in the SFA.DAS.LoginService.Database project.

##### Config

- Get the das-login-service configuration json file from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-login-service/SFA.DAS.LoginService.json); which is a non-public repository.
- Create a Configuration table in your (Development) local Azure Storage account.
- Add a row to the Configuration table with fields: PartitionKey: LOCAL, RowKey: SFA.DAS.LoginService_1.0, Data: {{The contents of the local config json file}}.
- Update Configuration SFA.DAS.LoginService_1.0, Data { "SqlConnectionstring":"Server={{local instance name}};Initial Catalog={{database name}};Trusted_Connection=True;" }

##### Complete Data Setup

For an example follow the [EPAO Dev Setup Guide](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/1731395918/EPAO+-+Data+Setup+Guide#Login-Service---Initial-Setup) to populate a local database; scripts show the Assessor service being added to the Login service as a client.

##### Run the Solution

- Navigate to src/SFA.DAS.LoginService.Web/
- run `dotnet restore`
- run `dotnet run`

The Login service is now running waiting to recevie OpenId requests to authenticate a client application on port 5001 by default.

Please not that if this is run from VS using IIS Express (not recomended) the port number is different and would need to be configured in a client app as the valid ranges of SSL ports in IIS Express are 44300 to 44399.

##  Getting Started with Programmatic Integration

If you want to integrate your client application with the Login service then the below basic guide is an overview of what is required.

### Database setup
In the data setup guide above is an example of the database entries required to declare that a client application depends on the Login service for authentication; the example above is for the Assessor service.

### Client setup
The following example client configuration is taken from the Assessor service when running locally and configured with a Login service running locally.

```json
{
  "MetadataAddress": "https://localhost:5001/.well-known/openid-configuration",
  "ClientId": "assessor",
  "ApiClientSecret": "",
  "ApiUri": "https://localhost:5001/Invitations/08372E20-BECD-415C-9925-4D33DDF67FAF",   <- This GUID needs to be the Id of the record in LoginService.Clients table
  "RedirectUri": "https://localhost:5015/Account/SignIn",
  "CallbackUri": "https://localhost:5015/Account/Callback",
  "SignOutRedirectUri": "https://localhost:5015/Account/SignedOut"
}
  ```

### Invitations

Users of the login service are added by invitation; that is a user registers details i.e email address and name; which prompts the login service to send an email; responding to a link in the email prompts the Login service to request a password; which completes the addition of the user. 

To invite users, your client app needs to POST the following JSON to the `ApiUrl` specified above.  

```json
{
  "sourceId": "[the id of a user in a client database]",
  "given_name" : "[Given name of user]",
  "family_name" : "[Family name of user]",
  "email": "[User's email address]",
  "userRedirect" : "[URL that the user should be redirected to on successful sign up]",
  "callback" : "[URL that the Login Service should call on the Client Service with the User's Id]"
}
```

The POST should have a Bearer authentication token, signed as per the discovery document spec found here: [https://yourloginserviceurl/.well-known/openid-configuration](https://yourloginserviceurl/.well-known/openid-configuration).  It's currently RS256. 

(This example is using [IdentityModel](https://www.nuget.org/packages/identitymodel/) in C#):

```c#
var client = new HttpClient();
var disco = client.GetDiscoveryDocumentAsync("https://at-aslogin.apprenticeships.education.gov.uk").Result;
if (disco.IsError)
{
    _logger.LogError("Error obtaining discovery document", disco.Error);
}

var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = config.DfeSignIn.ApiClientSecret,
    Scope = "api1"
}).Result;

if (tokenResponse.IsError)
{
    _logger.LogError("Error obtaining token", tokenResponse.Error);
}

using (var httpClient = new HttpClient())
{
    httpClient.SetBearerToken(tokenResponse.AccessToken);

    var inviteJson = JsonConvert.SerializeObject(new
    {
        sourceId = userId.ToString(),
        given_name = givenName,
        family_name = familyName,
        email = email,
        userRedirect = config.DfeSignIn.RedirectUri,
        callback = config.DfeSignIn.CallbackUri
    });
    
    var response = httpClient.PostAsync(config.DfeSignIn.ApiUri,
        new StringContent(inviteJson, Encoding.UTF8, "application/json")
    ).Result;
    
    var content = await response.Content.ReadAsStringAsync();
    
    _logger.LogInformation("Returned from Invitation Service. Status Code: {0}. Message: {0}",
        (int) response.StatusCode, content);
    
    if (!response.IsSuccessStatusCode)
    {
        _logger.LogError("Error from Invitation Service. Status Code: {0}. Message: {0}",
            (int) response.StatusCode, content);
        return new InviteUserResponse() {IsSuccess = false};
    }
    
   return new InviteUserResponse();
}
```

#### Callback

Once the user has successfully signed up, the Login Service will use the `callback` url as specified in the Api call to send the following JSON back to the client application:

```json
{
  "sub": "[The id of the user in Login Service that you'll get in the sub claim on sign in]",
  "sourceid": "[Your local user id]"
}
```
A client app must make an association between the ``sub`` and the ``sourceid`` in the client database so that the users local client details can be acccess after a succesfull login.
