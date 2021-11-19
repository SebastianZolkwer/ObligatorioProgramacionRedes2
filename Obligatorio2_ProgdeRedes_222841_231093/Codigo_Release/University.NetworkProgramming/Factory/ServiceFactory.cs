using BusinessLogic.Logic;
using BusinessLogicInterface.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Factory
{
    public class ServiceFactory
	{
		private readonly IServiceCollection services;

		public ServiceFactory(IServiceCollection services)
		{
			this.services = services;
		}

		public void AddCustomServices()
		{
			services.AddScoped<IClientLogic, ClientLogic>();
			services.AddScoped<IGameLogic, GameLogic>();
		}
	}
}

