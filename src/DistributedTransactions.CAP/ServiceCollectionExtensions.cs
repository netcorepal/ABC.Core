﻿using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCorePal.Extensions.DistributedTransactions;
using NetCorePal.Extensions.DistributedTransactions.CAP;

namespace NetCorePal.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册所有EventHandler类型
        /// </summary>
        public static IIntegrationEventServicesBuilder UseCap(this IIntegrationEventServicesBuilder builder,
            params Type[] typefromAssemblies)
        {
            var types = typefromAssemblies.Select(p => p.Assembly).SelectMany(assembly => assembly.GetTypes());
            var handlers = types.Where(t =>
                t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(ICapSubscribe)));

            foreach (var handler in handlers)
            {
                builder.Services.TryAddTransient(handler);
            }
            builder.Services.AddSingleton<IIntegrationEventPublisher, CapIntegrationEventPublisher>();
            return builder;
        }

        public static bool IsGenericSubclassOf(this Type type, Type superType)
        {
            if (type.BaseType != null
                && !type.BaseType.Equals(typeof(object))
                && type.BaseType.IsGenericType)
            {
                if (type.BaseType.GetGenericTypeDefinition().Equals(superType))
                {
                    return true;
                }

                return type.BaseType.IsGenericSubclassOf(superType);
            }

            return false;
        }
    }
}