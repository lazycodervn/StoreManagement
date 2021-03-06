﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GoogleDriveUploader;
using Ninject;
using StoreManagement.Admin.Models;
using StoreManagement.Admin.ScheduledTasks;
using StoreManagement.Service.DbContext;
using WebMatrix.WebData;

namespace StoreManagement.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {


      //  [Inject]
//        public IUploadHelper UploadHelper { set; get; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();


           
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);



           // var UploadHelper = (IUploadHelper)DependencyResolver.Current.GetService(typeof(IUploadHelper));
           // UploadHelper.ConnectToGoogleDriveServiceAsyn();
        }
        //string custom filled with the value of "varyByCustom" in your web.config
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if ("User".Equals(custom, StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    return context.User.Identity.Name;
                }
                else
                {
                    return base.GetVaryByCustomString(context, custom);
                }
            }
            return base.GetVaryByCustomString(context, custom);
        }
      
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;


        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<StoreContext>(null);



                try
                {
                    //using (var context = new UsersContext())
                    //{
                    //    if (!context.Database.Exists())
                    //    {
                    //        // Create the SimpleMembership database without Entity Framework migration schema
                    //   //     ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                    //    }
                    //}

                    //     WebSecurity.InitializeDatabaseConnection(AppConstants.ConnectionStringName, "System.Data.SqlClient", "UserProfile", "UserId", "UserName", false);

                    WebSecurity.InitializeDatabaseConnection("Stores", "UserProfile", "UserId", "UserName", autoCreateTables: false);

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}