using System;
using System.Collections.Generic;

namespace dz3.Container
{
    public class SimpleContainer
    {
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public void Register<TInterface, TImplementation>(TImplementation instance) where TImplementation : TInterface
        {
            services[typeof(TInterface)] = instance;
        }

        public void Register<T>(T instance)
        {
            services[typeof(T)] = instance;
        }

        public T Resolve<T>()
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Сервис {typeof(T).Name} не зарегистрирован");
        }
    }
}