﻿using Microsoft.Extensions.DependencyInjection;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.Service.LiveFeeds.Adapters;

namespace rNascar23.Service.LiveFeeds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLiveFeed(this IServiceCollection services)
        {
            services
                .AddTransient<ILiveFeedRepository, LiveFeedRepository>();

            return services;
        }
    }
}
