﻿using Autofac;
using Autofac.Integration.WebApi;
using StudentsProject.Context;
using StudentsProject.Interfaces;
using StudentsProject.Models;
using StudentsProject.Repositories;
using StudentsProject.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace StudentsProject.App_Start
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            builder.RegisterType<StudentsContext>()
                   .As<DbContext>()
                   .InstancePerRequest();

            builder.RegisterType<DbFactory>()
                 .As<IDbFactory>()
                 .InstancePerRequest();

            builder.RegisterGeneric(typeof(EntityCrudService<>))
                   .As(typeof(IEntityCrudService<>))
                   .InstancePerRequest();

            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}