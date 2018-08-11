﻿using System;
using CasaDoCodigo.API.Areas.Identity.Data;
using CasaDoCodigo.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CasaDoCodigo.API.Areas.Identity.IdentityHostingStartup))]
namespace CasaDoCodigo.API.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CasaDoCodigoAPIContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CasaDoCodigoAPIContextConnection")));

                services.AddDefaultIdentity<CasaDoCodigoAPIUser>()
                    .AddEntityFrameworkStores<CasaDoCodigoAPIContext>();
            });
        }
    }
}