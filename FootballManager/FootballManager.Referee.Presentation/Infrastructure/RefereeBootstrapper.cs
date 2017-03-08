using FootballManager.Domain.Entity.Contexts.LeagueContext;
using FootballManager.Referee.Presentation.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Referee.Presentation.RefereeService;
using Prism.Commands;

namespace FootballManager.Referee.Presentation.Infrastructure
{
	class RefereeBootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			return new MainWindow();
		}

		protected override void InitializeShell()
		{
			Application.Current.MainWindow.Show();
		}

		protected override void ConfigureModuleCatalog()
		{
			ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
			catalog.AddModule(typeof(RefereeModule));
		}

		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			//DomainBootstrapper.Initialize(this.Container);
			this.Container.RegisterTypes(AllClasses.FromAssemblies(typeof(RefereeBootstrapper).Assembly),
				WithMappings.FromMatchingInterface,
				WithName.Default);

			var objectTypeInList = new List<Type>() { typeof(object) };
			this.Container.RegisterTypes(AllClasses.FromAssemblies(Assembly.GetExecutingAssembly()),
				type => objectTypeInList, WithName.TypeName);
			
			this.Container.RegisterType<IRefereeService, RefereeServiceClient>(new InjectionConstructor());
			this.Container.RegisterType<IGeocodingService, BingGeocodingService>();
		}

		public class RefereeModule : IModule
		{
			readonly IRegionManager _regionManager;

			public RefereeModule(IRegionManager regionManager)
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
				GlobalCommands.GoBackCommand.RegisterCommand(new DelegateCommand(() =>
				{
					mainregion.NavigationService.Journal.GoBack();
					var window = Application.Current.MainWindow as MainWindow;
					if (window.directionFlyout.CloseCommand != null && window.directionFlyout.IsOpen)
						window.directionFlyout.CloseCommand.Execute(null);



				}));
			}
		}
	}
}
