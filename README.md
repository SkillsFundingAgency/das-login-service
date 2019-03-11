# ![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png) Digital Apprenticeships Service

##  Login Service

[comment]: # (![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/1496/badge)

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