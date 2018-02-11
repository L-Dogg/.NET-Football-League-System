# .NET-Football-League-System
.NET elective course, 5th semester (2017), by me and [andrzejbg](https://github.com/andrzejbg)

(Disclaimer: this project was developed using TFS so no commit (checkin) history is available here)

This system consists of 3 apps:
* **Football Manager** - CRUD application, which allows system administrator to manipulate system objects (players, teams, referees, etc.)
  * WPF
  * MS SQL 
  * EntityFramework 6
  * MahApps Metro
  * Prism
  * Unity
  
<p align="center">
 <img src="http://i.imgur.com/WAknXAH.png" alt="Screenshot 1"/>
</p>

* **Referee Application** - distributed application for league referees, using WCF API referee can input match data (result, scorers, etc.). Moreover, commute directions are provided using Bing Maps. 
  * WCF
  
<p align="center">
 <img src="http://i.imgur.com/iV1nlLa.png" alt="Screenshot 2"/>
</p> 
  
* **Fan Application** - web single page appliaction for league fans: users can log in using system account or using existing Facebook account. They can sign up for match notifications, rate matches, browse multiple rankings and stats. Commute directions are provided using Google Maps.
  * ASP.NET WebAPI
  * Autofac
  * AutoMapper
  * FluentValidation
  * Jquery
  * AngularJS 1.6.1
  * Toastr
  * Bootstrap
  
<p align="center">
  <img src="http://i.imgur.com/WVw1nqq.png" alt="Screenshot 3"/>
</p>

All three subsystems were tested using FluentAssertions and Moq. Internet APIs had been hosted on Azure server until free subscription ended.  

Users' credentials were stored securely in database with password hashed and salted.  

**Detailed documentation for each of above subsystems can be found in pdf files (preferably open solution first in Visual Studio, so you can choose Documentation from solution tree).**
