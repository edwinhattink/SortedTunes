# SortedTunes

## Migrations

1. Install Entity Framework Core tools by following this tutorial: [Install Entity Framework Core Tools Guide](https://docs.microsoft.com/nl-nl/ef/core/cli/dotnet).
2. Add _Data/Migrations_ folder to the _Infrastructure_ project.

4. Run the following command to create an initial migration:

```
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/Web --output-dir Data/Migrations
```

5. Now you're ready to create your database schema from the migration. This can be done via the following command:

```
dotnet ef database update --project src/Infrastructure --startup-project src/Web
```

6. Formulate a deployment strategy for production environments: [Apply migrations](https://docs.microsoft.com/nl-nl/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli).
7. Changes made to the domain or application must be synced to the database. Use the following command to create new migrations:

```
dotnet ef migrations add <AddTitle> --project src/Infrastructure --startup-project src/Web --output-dir Data/Migrations
```

## Running acceptance tests on a local machine

To run the acceptance tests (in Web.AcceptanceTests), we need to get Microsoft.Playwright running. For this, you need to follow these steps:

1. Open a PowerShell window in the Web.AcceptanceTests folder.
   (for example `C:\Projects\SortedTunes\tests\Web.AcceptanceTests`)
2. Run the following command in the folder (or build the project in Visual Studio)

```
dotnet build
```

3. Run the following command after the build is succesful (9.0 is our current dotnet version)

```
pwsh bin/Debug/net9.0/playwright.ps1 install
```

If `pwsh` is not available, you have to [install PowerShell](https://learn.microsoft.com/nl-nl/powershell/scripting/install/installing-powershell-on-windows) with winget 4. Now you can open the seperate solution `Web.AcceptanceTests` and run the tests. Note: you have to be running the other solution aswell on your machine. The URL the acceptance tests use is defined in `appsettings.json`.`

### Codegen for Playwright

Run the following command in the acceptance tests folder to start up codegen windows of Playwright. Make sure you've run the tests once, so you have the jsons for userstates!

```
pwsh bin/Debug/net9.0/playwright.ps1 codegen https://localhost:50000/ --load-storage=bin/Debug/net9.0/userstate-for-Admin.json
```

## Code formatter (React/TypeScript)

The usage of `Prettier` is mandatory for the React.ts project. ("src/Web/ClientAppCostModels") The package is already added as a development dependency to `package.json`. This leaves you with two options to format the project. The general advise is to take the first option.

1. Install the [Prettier Extension](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode) for Visual Studio. After installing hit `Ctrl + Shift + p`, then search and open `Preferences: Open User Settings (JSON)`. Add the following code snippet inside of the JSON file. Now your code will be formatted automatically when you save a file.

```
"[typescript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
},
"[typescriptreact]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
},
"editor.formatOnSave": true
```

2. Run the following command to format all documents in the project: `npm run pretty`.

## Elasticsearch settings

In appsettings, you have to set your local machines elastic settings.
You can see how to install Elastic on your machine on [this wiki page](https://dev.azure.com/buynamics2/Whats%20The%20Price/_wiki/wikis/Whats-The-Price.wiki/36/Elasticsearch-installatie).

```
"Elasticsearch": {
  "Url": "https://localhost:9200",
  "Fingerprint": "{fingerprint}",
  "Username": "elastic",
  "Password": "{password}"
}
```
