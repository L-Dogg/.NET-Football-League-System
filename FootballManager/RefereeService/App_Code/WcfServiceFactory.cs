using FootballManager.AuthenticationLayer;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using Microsoft.Practices.Unity;
using Unity.Wcf;

public class WcfServiceFactory : UnityServiceHostFactory
{
    protected override void ConfigureContainer(IUnityContainer container)
    {
		container.RegisterType<RefereeServiceLibrary.IRefereeService, RefereeServiceLibrary.RefereeService>();
		container.RegisterType<ILeagueContext, LeagueContext>(new HierarchicalLifetimeManager());
		container.RegisterType<IAuthenticationContext, AuthenticationContext>(new HierarchicalLifetimeManager());
		container.RegisterType<IAuthenticationService, AuthenticationService>(new HierarchicalLifetimeManager());
		container.RegisterType<IRefereeLeagueService, LeagueService>(new HierarchicalLifetimeManager());
    }
}    