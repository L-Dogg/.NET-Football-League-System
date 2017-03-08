using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Presentation.Views;
using Prism.Commands;
using FootballManager.AuthenticationLayer;

namespace FootballManager.Presentation.Infrastructure
{
	public class FootballManagerBootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			return new PageSwitcher();
		}

		protected override void InitializeShell()
		{
			Application.Current.MainWindow.Show();
		}

		protected override void ConfigureModuleCatalog()
		{
			ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
			catalog.AddModule(typeof(FootballManagerModule));
		}

		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			//DomainBootstrapper.Initialize(this.Container);
			this.Container.RegisterTypes(AllClasses.FromAssemblies(typeof(FootballManagerBootstrapper).Assembly),
				WithMappings.FromMatchingInterface,
				WithName.Default);

			var objectTypeInList = new List<Type>() { typeof(object) };
			this.Container.RegisterTypes(AllClasses.FromAssemblies(Assembly.GetExecutingAssembly()),
				type => objectTypeInList, WithName.TypeName);

			this.Container.RegisterType<ILeagueContext, LeagueContext>();
			this.Container.RegisterType<ILeagueService, LeagueService>();
			this.Container.RegisterType<IAuthenticationContext, AuthenticationContext>();
			this.Container.RegisterType<IAuthenticationService, AuthenticationService>();

		}

		public class FootballManagerModule : IModule
		{
			readonly IRegionManager _regionManager;

			public FootballManagerModule(IRegionManager regionManager)
			{
				_regionManager = regionManager;
			}

			public void Initialize()
			{
				_regionManager.RegisterViewWithRegion(UiRegions.MainRegion, typeof(LoginForm));
				GlobalCommands.ShowStylesCommand.RegisterCommand(new DelegateCommand(() =>
				{
					if (GlobalCommands.EnableClick)
					{
						_regionManager.RequestNavigate(UiRegions.MainRegion, nameof(AccentStyleWindow));
					}
				}));
				GlobalCommands.HomeButtonCommand.RegisterCommand(new DelegateCommand(() =>
				{
					if (GlobalCommands.EnableClick)
					{
						_regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
					}
				}));
				var mainregion = _regionManager.Regions[UiRegions.MainRegion];
				GlobalCommands.GoBackCommand.RegisterCommand(new DelegateCommand(() => mainregion.NavigationService.Journal.GoBack()));
			}
		}
	}
}
