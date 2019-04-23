# ![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png) Digital Apprenticeships Service

##  Login Service

[comment]: # (![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/1496/badge)

An [OpenID Connect](https://openid.net/connect/) implementation built using Identity Server 4 on aspnet core 2.2

### Developer Setup

#### Requirements

- Install [.NET Core 2.2](https://www.microsoft.com/net/download)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on v5.3)
- Install [Azure Storage Explorer](http://storageexplorer.com/)

#### Setup

- Clone this repository

##### Database

- Create a database
- Either use Visual Studio's `Publish Database` tool to publish the .Database project to your database or run the `.sql` scripts manually.

##### Config

- Get the das-login-service configuration json file from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-login-service/SFA.DAS.LoginService.json)
- Create a Configuration table in your (Development) local Azure Storage account.
- Add a row to the Configuration table with fields: PartitionKey: LOCAL, RowKey: SFA.DAS.LoginService_1.0, Data: {The contents of the local config json file}.
- Alter the SqlConnectionString value in the json to point to your database.

##### Setup
- `dotnet run` the app
- POST an empty request to `https://localhost:5001/DevSetup`.  This will populate your database.


##### Client setup

```json
{
  "MetadataAddress": "https://localhost:5001/.well-known/openid-configuration",
  "ClientId": "mvc",
  "ApiClientSecret": "",
  "ApiUri": "https://localhost:5001/Invitations/2350df68-e325-4ccc-9027-e1051e48d4a7",   <- This GUID needs to be the Id of the record in LoginService.Clients table
  "RedirectUri": "https://localhost:6016/Users/SignIn",
  "CallbackUri": "https://localhost:6016/Account/Callback",
  "SignOutRedirectUri": "https://localhost:6016/Users/SignedOut"
}
  ```
  
##  Login Service

### Invitations

To invite users, your client app needs to POST the following JSON to the `ApiUrl` specified above.  

```json
{
  "sourceId": "[the id of your local user]",
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

##  Sample Applications

There are 3 sample applications.

1. Sample 1 is an invitation from client application (.Net Core) and callback with created user details (TO BE COMPLETED)

2. Samples 2 & 3 are create an account (.Net Core) and (.Net Framework)

For sample application 2 & 3 there is a Setup Script which inserts the required client details into a Login Service database which
are required to run the sample applications; this assumes that the database to which the sample app will connect has been created
and populated with standard test data during publishing and that the confirmation of the Login Service has been completed to run
locally or in a test environment.

##  Employer Users Service

###  Migration to Login Service

The users which have already been created in the Employer Users service can be migrated to the Login Service whilst retaining their
current password see [Employer Users](docs/EmployerUsers.md "Migration")