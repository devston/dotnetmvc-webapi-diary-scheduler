# ASP.Net Core MVC & Web API Diary Scheduler
An ASP.Net Core 5 MVC & Web API diary scheduling app based on the domain-driven design concept. This makes use of Asp.Net Core 5 MVC, Asp.Net Core Web API, WebPack and fullcalendar.

# Getting Started:
## Prerequisites
- [Visual Studio 2019](https://visualstudio.microsoft.com/) with ASP.Net/ web component installed.
- [Node js](https://nodejs.org/en/)

## Running the solution
- Run `update-database` in the **nuget package manager** console against the `DiaryScheduler.Data` project to create the initial datastore.
- Start `DiaryScheduler.Presentation.Web` & `DiaryScheduler.Api`
- Navigate to the scheduler area

## TODO:
- Update projects to .net 6
- Add Identity Server 4 for authorisation & authentication
- Remove jQuery
- Add google calendar integration
- Add Office 365 integration

## Credits

- [fullcalendar](https://fullcalendar.io/)
- [WebPack](https://webpack.js.org/)
- [jQuery](https://jquery.com/)
- [Autofac](https://autofac.org/)
