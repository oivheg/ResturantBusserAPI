﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ResturantBusserAPI.Startup))]

namespace ResturantBusserAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}