# ASP.Net Core MVC & Web API Diary Scheduler
An ASP.Net Core 6 MVC & Web API diary scheduling app based on the domain-driven design concept. This makes use of ASP.Net Core 6 MVC, ASP.Net Core Web API, WebPack and fullcalendar.

[![.NET](https://github.com/devston/dotnetmvc-webapi-diary-scheduler/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/devston/dotnetmvc-webapi-diary-scheduler/actions/workflows/dotnet.yml)

# Getting Started:
## Prerequisites
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with ASP.Net/ web component and .Net 6 SDK installed.
- [Node js](https://nodejs.org/en/)

## Running the solution
- Run `update-database` in the **nuget package manager** console against the `DiaryScheduler.Data` project to create the initial datastore.
- Start `DiaryScheduler.Presentation.Web` & `DiaryScheduler.Api` ([SwitchStartupProject](https://marketplace.visualstudio.com/items?itemName=vs-publisher-141975.SwitchStartupProjectForVS2022) is set up to do this if available in Visual Studio)
- Navigate to the scheduler area

## TODO:
- Switch the validation layer with FluentValidation
- Add API authorisation
- Add integration tests
- Add an Angular UI example
- Remove jQuery
- Add google calendar integration
- Add Office 365 integration

## Credits

- [fullcalendar](https://fullcalendar.io/)
- [WebPack](https://webpack.js.org/)
- [jQuery](https://jquery.com/)
- [Autofac](https://autofac.org/)
- [Refit](https://github.com/reactiveui/refit)
