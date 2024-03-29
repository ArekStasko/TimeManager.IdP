﻿using Autofac;
using System.Reflection;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data;
using TimeManager.IdP.services.MessageQueuer;

namespace TimeManager.IdP.services
{
    public class ContainerFactory
    {

        public static IContainer CreateProcessorsContainer(DataContext _context, ILogger<AuthController> _authLogger, ILogger<TokenController> _tokenLogger, IMQManager _mqManager)
        {
            var container = new ContainerBuilder();

            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace == "TimeManager.IdP.Processors.UserProcessor")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name))
                .WithParameter(new TypedParameter(typeof(DataContext), _context))
                .WithParameter(new TypedParameter(typeof(ILogger<AuthController>), _authLogger))
                .WithParameter(new TypedParameter(typeof(IMQManager), _mqManager));

            container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace == "TimeManager.IdP.Processors.TokenProcessor")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name))
                .WithParameter(new TypedParameter(typeof(DataContext), _context))
                .WithParameter(new TypedParameter(typeof(ILogger<TokenController>), _tokenLogger));

            return container.Build();
        }

    }
}
