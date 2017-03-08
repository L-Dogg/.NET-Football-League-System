# .NET-Football-League-System
.NET elective course, 5th semester (2017), by me and andrzej_badgirl

This system consists of 3 apps:
* **Football Manager** - CRUD application, which allows system administrator to manipulate system objects (players, teams, referees, etc.)
  * WPF
  * MS SQL 
  * EntityFramework 6
  * MahApps Metro
  * Prism
  * Unity
* **Referee Application** - distributed application for league referees, using WCF API referee can input match data (result, scorers, etc.). Moreover, commute directions are provided using Bing Maps. 
  * WCF
* **Fan Application** - web single page appliaction for league fans: users can log in using system account or using existing Facebook account. They can sign up for match notifications, rate matches, browse multiple rankings and stats. Commute directions are provided using Google Maps.
  * ASP.NET WebAPI
  * Autofac
  * Antlr
  * AutoMapper
  * FluentValidation
  * Jquery
  * AngularJS 1.6.1
  * Toastr
  * Bootstrap

All three subsystems were tested using FluentAssertions and Moq. Internet APIs had been hosted on Azure server until free subscription ended.  

Users' credentials were stored securely in database with password hashed and salted.  

**Detailed documentation for each of above subsystems can be found in pdf files (preferably open solution first in Visual Studio, so you can choose Documentation from solution tree).**
