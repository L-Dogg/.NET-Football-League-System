using System;
using System.Data.Entity;

namespace FootballManager.Domain.Entity.Contexts.LeagueContext
{
	public class LeagueContextInitializer : DropCreateDatabaseIfModelChanges<LeagueContext>
	{
		protected override void Seed(LeagueContext context)
		{
			context.Seasons.Add(new Models.Season() { Name = "Sezon 2016/17" });

			this.GenerateCoaches(context);
			this.GenerateStadiums(context);
			this.GenerateTeams(context);
			this.GenerateFootballers(context);
			this.GenerateRefs(context);
			this.GenerateMatches(context);
			this.GenerateGoals(context);
			this.GenerateTable(context);

			base.Seed(context);
		}

		private void GenerateCoaches(LeagueContext context)
		{

			context.Coaches.Add(new Models.Coach() { FirstName = "Piotr", Surname = "Nowak", PictureUrl = "nowak.png" });
			context.Coaches.Add(new Models.Coach() { FirstName = "Michał", Surname = "Probierz", PictureUrl = "probierz.png" });
			context.Coaches.Add(new Models.Coach() { FirstName = "Piotr", Surname = "Stokowiec", PictureUrl = "stokowiec.png" });
			context.Coaches.Add(new Models.Coach() { FirstName = "Czesław", Surname = "Michniewicz", PictureUrl = "michniewicz.png"});
		}

		private void GenerateStadiums(LeagueContext context)
		{
			var adres = new Models.Address() { Street = "Pokoleń Lechii Gdańsk", Number = "1", Zipcode = "80-560", City = "Gdańsk", };
			context.Addresses.Add(adres);
			context.Stadiums.Add(new Models.Stadium() { Name = "Stadion Energa Gdańsk", Capacity = 43165, Address = adres, PictureUrl = "gdansk.jpg"});
			adres = new Models.Address() { Street = "Słoneczna", Number = "1", Zipcode = "15-281", City = "Białystok" };
			context.Addresses.Add(adres);
			context.Stadiums.Add(new Models.Stadium() { Name = "Stadion Miejski", Capacity = 22432, Address = adres, PictureUrl = "bialystok.jpg" });
			adres = new Models.Address() { Street = "Marii Skłodowskiej-Curie", Number = "98", Zipcode = "56-301", City = "Lubin" };
			context.Addresses.Add(adres);
			context.Stadiums.Add(new Models.Stadium() { Name = "Stadion Zagłebia", Capacity = 16100, Address = adres, PictureUrl = "lubin.jpg" });
			adres = new Models.Address() { Street = "Nieciecza", Number = "199", Zipcode = "33-240", City = "Żabno" };
			context.Addresses.Add(adres);
			context.Stadiums.Add(new Models.Stadium() { Name = "Stadion Nieciecza KS", Capacity = 4595, Address = adres, PictureUrl = "nieciecza.jpg" });
		}

		private void GenerateTeams(LeagueContext context)
		{
			var adres = new Models.Address() { Street = "Pokoleń Lechii Gdańsk", Number = "1", Zipcode = "80-560", City = "Gdańsk" };
			context.Addresses.Add(adres);
			context.Teams.Add(new Models.Team()
			{
				Name = "Lechia Gdańsk", Budget = 10000, CoachId = 1, StadiumId = 1, Salaries = 11, Founded = new DateTime(1945, 1, 1), Address = adres,
				LogoUrl = "lechia-gdansk.png"
			});
			adres = new Models.Address() { Street = "Legionowa", Number = "28", Zipcode = "15-281", City = "Białystok" };
			context.Addresses.Add(adres);
			context.Teams.Add(new Models.Team()
			{
				Name = "Jagiellonia Białystok", Budget = 10000, CoachId = 2, StadiumId = 2, Salaries = 11, Founded = new DateTime(1932, 1, 27), Address = adres,
				LogoUrl = "jagiellonia-bialystok.png"
			});
			adres = new Models.Address() { Street = "Marii Skłodowskiej-Curie", Number = "98", Zipcode = "56-301", City = "Lubin" };
			context.Addresses.Add(adres);
			context.Teams.Add(new Models.Team()
			{
				Name = "Zagłębie Lubin", Budget = 10000, CoachId = 3, StadiumId = 3, Salaries = 11, Founded = new DateTime(1945, 9, 1), Address = adres,
				LogoUrl = "zaglebie-lubin.png"
			});
			adres = new Models.Address() { Street = "Nieciecza", Number = "199", Zipcode = "33-240", City = "Żabno" };
			context.Addresses.Add(adres);
			context.Teams.Add(new Models.Team()
			{
				Name = "Termalica Bruk-Bet Nieciecza", Budget = 10000, CoachId = 4, StadiumId = 4, Salaries = 11, Founded = new DateTime(1922, 1, 1), Address = adres,
				LogoUrl = "termalica.png"
			});
			context.SaveChanges();
		}

		private void GenerateFootballers(LeagueContext context)
		{
			context.Footballers.Add(new Models.Footballer() { FirstName = "Mateusz", Surname = "Bąk", BirthDate = new DateTime(1983, 2, 24), TeamId = 1, Salary = 1, PictureUrl = "gdansk (1).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Adam", Surname = "Chrzanowski", BirthDate = new DateTime(1999, 3, 31), TeamId = 1, Salary = 1, PictureUrl = "gdansk (2).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Rafał", Surname = "Janicki", BirthDate = new DateTime(1992, 7, 5), TeamId = 1, Salary = 1, PictureUrl = "gdansk (3).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Paweł", Surname = "Stolarski", BirthDate = new DateTime(1996, 1, 28), TeamId = 1, Salary = 1, PictureUrl = "gdansk (4).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Jakub", Surname = "Wawrzyniak", BirthDate = new DateTime(1983, 7, 7), TeamId = 1, Salary = 1, PictureUrl = "gdansk (5).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Flávio", Surname = "Paixão", BirthDate = new DateTime(1984, 9, 19), TeamId = 1, Salary = 1, PictureUrl = "gdansk (6).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Martin", Surname = "Kobylański", BirthDate = new DateTime(1994, 3, 8), TeamId = 1, Salary = 1, PictureUrl = "gdansk (7).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Miloš", Surname = "Krasić", BirthDate = new DateTime(1984, 11, 1), TeamId = 1, Salary = 1, PictureUrl = "gdansk (8).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Sebastian", Surname = "Mila", BirthDate = new DateTime(1982, 7, 10), TeamId = 1, Salary = 1, PictureUrl = "gdansk (9).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Grzegorz", Surname = "Kuświk", BirthDate = new DateTime(1987, 5, 23), TeamId = 1, Salary = 1, PictureUrl = "gdansk (10).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Marco", Surname = "Paixão", BirthDate = new DateTime(1984, 9, 19), TeamId = 1, Salary = 1, PictureUrl = "gdansk (11).png" });

			context.Footballers.Add(new Models.Footballer() { FirstName = "Hubert", Surname = "Gostomski", BirthDate = new DateTime(1998, 2, 25), TeamId = 2, Salary = 1, PictureUrl = "jaga (1).png"});
			context.Footballers.Add(new Models.Footballer() { FirstName = "Łukasz", Surname = "Burliga", BirthDate = new DateTime(1988, 5, 10), TeamId = 2, Salary = 1, PictureUrl = "jaga (2).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Paweł", Surname = "Olszewski", BirthDate = new DateTime(1999, 6, 7), TeamId = 2, Salary = 1, PictureUrl = "jaga (3).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Piotr", Surname = "Tomasik", BirthDate = new DateTime(1987, 10, 31), TeamId = 2, Salary = 1, PictureUrl = "jaga (4).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Marek", Surname = "Wasiluk", BirthDate = new DateTime(1987, 6, 3), TeamId = 2, Salary = 1, PictureUrl = "jaga (5).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Damian", Surname = "Grabowski", BirthDate = new DateTime(1996, 3, 1), TeamId = 2, Salary = 1, PictureUrl = "jaga (6).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Rafał", Surname = "Grzyb", BirthDate = new DateTime(1983, 1, 16), TeamId = 2, Salary = 1, PictureUrl = "jaga (7).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Przemysław", Surname = "Mystkowski", BirthDate = new DateTime(1998, 4, 25), TeamId = 2, Salary = 1, PictureUrl = "jaga (8).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Damian", Surname = "Szymański", BirthDate = new DateTime(1995, 6, 16), TeamId = 2, Salary = 1, PictureUrl = "jaga (9).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Maciej", Surname = "Górski", BirthDate = new DateTime(1990, 3, 1), TeamId = 2, Salary = 1, PictureUrl = "jaga (10).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Patryk", Surname = "Klimała", BirthDate = new DateTime(1998, 8, 5), TeamId = 2, Salary = 1, PictureUrl = "jaga (11).png" });


			context.Footballers.Add(new Models.Footballer() { FirstName = "Konrad", Surname = "Forenc", BirthDate = new DateTime(1992, 7, 17), TeamId = 3, Salary = 1, PictureUrl = "lubin (1).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Daniel", Surname = "Dziwniel", BirthDate = new DateTime(1992, 8, 19), TeamId = 3, Salary = 1, PictureUrl = "lubin (2).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Jaromir", Surname = "Jach", BirthDate = new DateTime(1994, 2, 17), TeamId = 3, Salary = 1, PictureUrl = "lubin (3).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Sebastian", Surname = "Madera", BirthDate = new DateTime(1985, 5, 30), TeamId = 3, Salary = 1, PictureUrl = "lubin (4).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Filip", Surname = "Jagiełło", BirthDate = new DateTime(1997, 8, 6), TeamId = 3, Salary = 1, PictureUrl = "lubin (5).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Łukasz", Surname = "Janoszka", BirthDate = new DateTime(1987, 3, 18), TeamId = 3, Salary = 1, PictureUrl = "lubin (6).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Aleksander", Surname = "Woźniak", BirthDate = new DateTime(1990, 7, 1), TeamId = 3, Salary = 1, PictureUrl = "lubin (7).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Paweł", Surname = "Żyra", BirthDate = new DateTime(1998, 4, 7), TeamId = 3, Salary = 1, PictureUrl = "lubin (8).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Damian", Surname = "Zbozień", BirthDate = new DateTime(1989, 4, 25), TeamId = 3, Salary = 1, PictureUrl = "lubin (9).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Adam", Surname = "Buska", BirthDate = new DateTime(1996, 7, 12), TeamId = 3, Salary = 1, PictureUrl = "lubin (10).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Michal", Surname = "Papadopulos", BirthDate = new DateTime(1985, 4, 14), TeamId = 3, Salary = 1, PictureUrl = "lubin (11).png" });


			context.Footballers.Add(new Models.Footballer() { FirstName = "Krzysztof", Surname = "Baran", BirthDate = new DateTime(1990, 2, 12), TeamId = 4, Salary = 1, PictureUrl = "termalica (2).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Patryk", Surname = "Fryc", BirthDate = new DateTime(1993, 2, 24), TeamId = 4, Salary = 1, PictureUrl = "termalica (3).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Kornel", Surname = "Osyra", BirthDate = new DateTime(1993, 2, 7), TeamId = 4, Salary = 1, PictureUrl = "termalica (4).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Przemysław", Surname = "Szajek", BirthDate = new DateTime(1996, 4, 22), TeamId = 4, Salary = 1, PictureUrl = "termalica (5).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Sebastian", Surname = "Ziajka", BirthDate = new DateTime(1982, 12, 15), TeamId = 4, Salary = 1, PictureUrl = "termalica (6).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Mateusz", Surname = "Kupczak", BirthDate = new DateTime(1992, 2, 20), TeamId = 4, Salary = 1, PictureUrl = "termalica (7).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Krzysztof", Surname = "Miroszka", BirthDate = new DateTime(1998, 10, 31), TeamId = 4, Salary = 1, PictureUrl = "termalica (8).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Winicjusz", Surname = "Wanicki", BirthDate = new DateTime(1999, 1, 5), TeamId = 4, Salary = 1, PictureUrl = "termalica (9).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Bartłomiej", Surname = "Babiarz", BirthDate = new DateTime(1989, 2, 3), TeamId = 4, Salary = 1, PictureUrl = "termalica (10).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Wojciech", Surname = "Kędziora", BirthDate = new DateTime(1980, 12, 20), TeamId = 4, Salary = 1, PictureUrl = "termalica (11).png" });
			context.Footballers.Add(new Models.Footballer() { FirstName = "Jakub", Surname = "Wróbel", BirthDate = new DateTime(1993, 7, 30), TeamId = 4, Salary = 1, PictureUrl = "termalica (1).png" });
		}

		private void GenerateRefs(LeagueContext context)
		{
			context.Referees.Add(new Models.Referee() { FirstName = "Szymon", Surname = "Marciniak", RefereeUserId = 2, PictureUrl = "marciniak.png"});
			context.Referees.Add(new Models.Referee() { FirstName = "Pierluigi", Surname = "Colina", RefereeUserId = 3, PictureUrl = "colina.png"});
			context.SaveChanges();
		}

		private void GenerateMatches(LeagueContext context)
		{
			context.Matches.Add(new Models.Match() { HomeTeamId = 1, AwayTeamId = 2, Date = DateTime.Today, RefereeId = 1, SeasonId = 1, Round = 1, StadiumId = 1, HomeGoals = 2, AwayGoals = 1, Attendance = 1253});
			context.Matches.Add(new Models.Match() { HomeTeamId = 1, AwayTeamId = 3, Date = DateTime.Today.AddDays(7), RefereeId = 2, SeasonId = 1, Round = 2, StadiumId = 1, HomeGoals = 0, AwayGoals = 2, Attendance = 1313 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 1, AwayTeamId = 4, Date = DateTime.Today.AddDays(14), RefereeId = 1, SeasonId = 1, Round = 3, StadiumId = 1, HomeGoals = 4, AwayGoals = 0, Attendance = 341 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 2, AwayTeamId = 3, Date = DateTime.Today.AddDays(7), RefereeId = 2, SeasonId = 1, Round = 2, StadiumId = 2, HomeGoals = 1, AwayGoals = 0, Attendance = 300 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 2, AwayTeamId = 4, Date = DateTime.Today.AddDays(14), RefereeId = 1, SeasonId = 1, Round = 3, StadiumId = 2, HomeGoals = 1, AwayGoals = 1, Attendance = 5232 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 2, AwayTeamId = 1, Date = DateTime.Today.AddDays(42), RefereeId = 1, SeasonId = 1, Round = 6, StadiumId = 2, HomeGoals = 0, AwayGoals = 0, Attendance = 300 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 3, AwayTeamId = 4, Date = DateTime.Today, RefereeId = 2, SeasonId = 1, Round = 1, StadiumId = 3, HomeGoals = 0, AwayGoals = 0, Attendance = 1250 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 3, AwayTeamId = 1, Date = DateTime.Today.AddDays(28), RefereeId = 1, SeasonId = 1, Round = 4, StadiumId = 3, HomeGoals = 2, AwayGoals = 0, Attendance = 300 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 3, AwayTeamId = 2, Date = DateTime.Today.AddDays(35), RefereeId = 2, SeasonId = 1, Round = 5, StadiumId = 3, HomeGoals = 0, AwayGoals = 1, Attendance = 313 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 4, AwayTeamId = 1, Date = DateTime.Today.AddDays(35), RefereeId = 1, SeasonId = 1, Round = 5, StadiumId = 4, HomeGoals = 3, AwayGoals = 0, Attendance = 3050 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 4, AwayTeamId = 2, Date = DateTime.Today.AddDays(28), RefereeId = 2, SeasonId = 1, Round = 4, StadiumId = 4, HomeGoals = 2, AwayGoals = 0, Attendance = 5123 });
			context.Matches.Add(new Models.Match() { HomeTeamId = 4, AwayTeamId = 3, Date = DateTime.Today.AddDays(42), RefereeId = 1, SeasonId = 1, Round = 6, StadiumId = 4, HomeGoals = 0, AwayGoals = 0, Attendance = 1341 });

			context.SaveChanges();
		}

		private void GenerateGoals(LeagueContext context)
		{
			context.Goals.Add(new Models.Goal() { MatchId = 1, Type = Enums.GoalType.Normal, FootballerId = 10, TeamID = 1, Time = 50});
			context.Goals.Add(new Models.Goal() { MatchId = 1, Type = Enums.GoalType.Penalty, FootballerId = 11, TeamID = 1, Time = 45 });
			context.Goals.Add(new Models.Goal() { MatchId = 1, Type = Enums.GoalType.Normal, FootballerId = 20, TeamID = 2, Time = 55 });

			context.Goals.Add(new Models.Goal() { MatchId = 2, Type = Enums.GoalType.Normal, FootballerId = 30, TeamID = 3, Time = 5 });
			context.Goals.Add(new Models.Goal() { MatchId = 2, Type = Enums.GoalType.Own, FootballerId = 5, TeamID = 3, Time = 88});

			context.Goals.Add(new Models.Goal() { MatchId = 3, Type = Enums.GoalType.Normal, FootballerId = 8, TeamID = 1, Time = 14});
			context.Goals.Add(new Models.Goal() { MatchId = 3, Type = Enums.GoalType.Normal, FootballerId = 9, TeamID = 1, Time = 19});
			context.Goals.Add(new Models.Goal() { MatchId = 3, Type = Enums.GoalType.Freekick, FootballerId = 10, TeamID = 1, Time = 45 });
			context.Goals.Add(new Models.Goal() { MatchId = 3, Type = Enums.GoalType.Normal, FootballerId = 11, TeamID = 1, Time = 76});

			context.Goals.Add(new Models.Goal() { MatchId = 4, Type = Enums.GoalType.Normal, FootballerId = 18, TeamID = 2, Time = 13 });

			context.Goals.Add(new Models.Goal() { MatchId = 5, Type = Enums.GoalType.Normal, FootballerId = 22, TeamID = 2, Time = 32});
			context.Goals.Add(new Models.Goal() { MatchId = 5, Type = Enums.GoalType.Normal, FootballerId = 40, TeamID = 4, Time = 21 });

			context.Goals.Add(new Models.Goal() { MatchId = 8, Type = Enums.GoalType.Normal, FootballerId = 30, TeamID = 3, Time = 21});
			context.Goals.Add(new Models.Goal() { MatchId = 8, Type = Enums.GoalType.Normal, FootballerId = 31, TeamID = 3, Time = 37 });

			context.Goals.Add(new Models.Goal() { MatchId = 9, Type = Enums.GoalType.Normal, FootballerId = 16, TeamID = 2, Time = 4 });

			context.Goals.Add(new Models.Goal() { MatchId = 10, Type = Enums.GoalType.Normal, FootballerId = 40, TeamID = 4, Time = 63 });
			context.Goals.Add(new Models.Goal() { MatchId = 10, Type = Enums.GoalType.Freekick, FootballerId = 39, TeamID = 4, Time = 76 });
			context.Goals.Add(new Models.Goal() { MatchId = 10, Type = Enums.GoalType.Own, FootballerId = 20, TeamID = 4, Time = 90 });

			context.Goals.Add(new Models.Goal() { MatchId = 11, Type = Enums.GoalType.Normal, FootballerId = 40, TeamID = 4, Time = 25 });
			context.Goals.Add(new Models.Goal() { MatchId = 11, Type = Enums.GoalType.Normal, FootballerId = 30, TeamID = 4, Time = 40 });

			context.SaveChanges();
		}

		private void GenerateTable(LeagueContext context)
		{
			context.Tables.Add(new Models.Table() { TeamId = 1, SeasonId = 1, GoalsScored = 10, GoalsConceded = 5, Points = 10, Position = 2 });
			context.Tables.Add(new Models.Table() { TeamId = 2, SeasonId = 1, GoalsScored = 7, GoalsConceded = 7, Points = 8, Position = 3 });
			context.Tables.Add(new Models.Table() { TeamId = 3, SeasonId = 1, GoalsScored = 5, GoalsConceded = 15, Points = 4, Position = 4 });
			context.Tables.Add(new Models.Table() { TeamId = 4, SeasonId = 1, GoalsScored = 6, GoalsConceded = 1, Points = 12, Position = 1 });
		}
	}
}
