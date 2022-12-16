using Autofac
using System.Reflection;

namespace TimeManager.IdP.services
{
    public class ContainerFactory
    {

        public static IContainer CreateProcessorsContainer()
        {
            var container = new ContainerBuilder();

            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace == "TimeManager.IdP.Processors.UserProcessor" || t.Namespace == "TimeManager.IdP.Processors.TokenProcessor")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return container.Build();
        }

    }
}
