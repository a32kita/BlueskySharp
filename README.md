# BlueskySharp
BlueskySharp is a client library for utilizing the Bluesky API from .NET applications.

It is recommended to install the package from NuGet when using it in a .NET application;  
[NuGet -  BlueskySharp](https://www.nuget.org/packages/BlueskySharp/)

## 1. Get started
This section introduces how to log in to CryPlanet and make actual posts.

### 1 - 1. Log in to CryPlanet using an application password
Log in to the [bsky.app](https://bsky.app/) and issue an "App Password" from the account security menu. Logging in from the application is done using the handle name and application password.

```csharp
// using BlueskySharp;

var bskySv = await BlueskyService.LoginWithLoginInfoAsync(
    BlueskyServiceInfo.BskySocial, // <= It means connect to "bsky.social"
    new() { Handle = "ACCOUNT_HANDLE", Password = "ACCOUNT_APP_PASSWORD" });
```

### 1 - 2. New Post
Make a new post from the logged-in account.

```csharp
var postTime = DateTimeOffset.Now;
var postParam = new BlueskySharp.Endpoints.Repo.CreateRecordParam()
{
    Repo = "ACCOUNT_HANDLE",
    Collection = "app.bsky.feed.post",
    Record = new()
    {
        Text = "hello, world !!",
        CreatedAt = postTime,
    }
};

var postResult = await bskySv.Repo.CreateRecordAsync(postParam);
```

### 1 - 3. List of Past Posts
Retrieve the past posts of the logged-in account.

```csharp
var listRecordsResult = await service.Repo.ListRecordsAsync(new()
    {
        Repo = "ACCOUNT_HANDLE",
        Collection = "app.bsky.feed.post",
        Limit = 10,
    });

foreach (var record in listRecordsResult.Records)
{
    var recordValue = record.Value;
    Console.WriteLine("{0}: {1}", recordValue.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss"), recordValue.Text.Replace("\n", "\\n"));
}
```

## 2. License
Available under the MIT License.

## 3. Notes
* This library is currently under development. There may be breaking changes to namespaces and other elements during the development process.
* When running test code, you will be asked for account authentication information, and actual posts will be made during the test scenarios. Please be careful with the account you connect to.
