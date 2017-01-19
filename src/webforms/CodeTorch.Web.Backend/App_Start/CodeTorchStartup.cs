using Autofac;
using CodeTorch.Abstractions;
using CodeTorch.Configuration.FileStore;
using CodeTorch.Core;
using CodeTorch.Logger.Log4Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CodeTorch.Web.Backend.App_Start.CodeTorchStartup), "PreStart")]


namespace CodeTorch.Web.Backend.App_Start
{
    public static class CodeTorchStartup
    {
        public static async void PreStart()
        {
            log4net.Config.XmlConfigurator.Configure();

            //setup logging framework

            
            IDependencyContainer container = new Ioc.Autofac.AutofacContainer(new ContainerBuilder().Build());

            container.Register<CodeTorch.Abstractions.ILogManager, CodeTorch.Logger.Log4Net.Log4NetLogManager>();
            container.Register<CodeTorch.Abstractions.IConfigurationStore, CodeTorch.Configuration.FileStore.FileConfigurationStore>();

            container.Register<CodeTorch.Backend.DataCommandManager, CodeTorch.Backend.DataCommandManager>();
            container.Register<CodeTorch.Backend.LookupManager, CodeTorch.Backend.LookupManager>();
            container.Register<CodeTorch.Backend.PickerManager, CodeTorch.Backend.PickerManager>();
            container.Register<CodeTorch.Backend.PermissionManager, CodeTorch.Backend.PermissionManager>();

            Resolver.SetResolver(container.GetResolver());

            //get the configuration store
            var store = container.GetResolver().Resolve<CodeTorch.Abstractions.IConfigurationStore>();

            //load frontend configuration
            await CodeTorch.Web.Common.LoadWebConfiguration(store, RouteTable.Routes);


        }
    }
}