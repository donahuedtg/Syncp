namespace SyncpWallet
{
    using System.Linq;
    using System.Web.Http;
    using System.Reflection;

    using Unity;
    using Unity.WebApi;

    using Services;

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            // Assembly.GetAssembly(typeof(IService))
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                    .Select(t => new
                    {
                        Interface = t.GetInterface($"I{t.Name}"),
                        Implemantation = t
                    })
                    .ToList()
                    .ForEach(s => container.RegisterType(s.Interface, s.Implemantation));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}