# Copilot Instructions

The generated code should assume the latest version of the used libraries is present.
The generated code should use the latest features of C#, TypeScript, .NET, and Angular.

Do not comment the code you generate that describes obvious things it is doing.

## C\# and .NET

The libraries used for database access is Entity Framework Core.

Tests should be written for XUnit for .NET apps.
Use AutoFixture in .NET tests to create classes under test.
Use Fluent Assertions in .NET tests to assert statements.
Use NSubstitute in .NET tests to setup mocks that are received from AutoFixture.

Make sure to use async code over sync code.
Make sure to use cancellation tokens in async code. Never use `Task.Wait` or `Task.Result`.
Make sure to use an Async suffix for asynchronous methods names.
Make sure to use file scoped namespaces.

## TypeScript

The libraries used are Angular for the frontend and ASP.NET Core for the backend.
Tests should be written for Jest for the front end web apps.

Make sure to use async for promises.
Make sure to unsubscribe to subscriptions when using observables.

## Bicep

The generated code for infrastructure as code should be using Bicep for Azure.
The Azure resources are using the Microsoft Cloud Adoption Framework naming conventions and abbreviation recommendations.
The naming convention is `{{resourceAbbreviation}}-{{application}}-{{environmentAbbreviation}}-{{locationAbbreviation}}-{{resourceName}}`. An example would be `app-osp-prod-westeu-api`.
The application is osp for open space planner.
Azure resources that don't support dashes should not use them in their name.
