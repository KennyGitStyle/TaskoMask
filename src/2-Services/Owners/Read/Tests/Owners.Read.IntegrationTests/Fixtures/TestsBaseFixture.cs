﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskoMask.BuildingBlocks.Test;
using TaskoMask.Services.Owners.Read.Api.Infrastructure.DbContext;
using TaskoMask.Services.Owners.Read.Api.Infrastructure.DI;

namespace TaskoMask.Services.Owners.Read.IntegrationTests.Fixtures
{
    public abstract class TestsBaseFixture : IntegrationTestsBase
    {
        public readonly IMapper Mapper;
        public readonly OwnerReadDbContext DbContext;


        protected TestsBaseFixture(string dbNameSuffix) : base(dbNameSuffix)
        {
            Mapper = GetRequiredService<IMapper>();
            DbContext = GetRequiredService<OwnerReadDbContext>();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void InitialDatabase()
        {
            _serviceProvider.InitialDatabase();
        }



        /// <summary>
        /// 
        /// </summary>
        public override void DropDatabase()
        {
            _serviceProvider.DropDatabase();
        }



        /// <summary>
        /// 
        /// </summary>
        public override IServiceProvider GetServiceProvider(string dbNameSuffix)
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                                //Copy from Identity.Api project during the build event
                                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                                .AddJsonFile("appsettings.Staging.json", optional: true)
                                .AddJsonFile("appsettings.Development.json", optional: true)
                                .AddInMemoryCollection(new[]
                                {
                                   new KeyValuePair<string,string>("ConnectionString:DatabaseName", $"IdentityDB_Test_{dbNameSuffix}")
                                })
                                .Build();

            services.AddSingleton<IConfiguration>(provider => { return configuration; });

            services.AddModules(configuration);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}