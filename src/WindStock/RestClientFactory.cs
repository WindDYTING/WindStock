using System;
using System.Collections.Generic;
using RestSharp;

namespace WindStock
{
    internal static class RestClientFactory 
    {
        private static readonly IDictionary<Type, RestClient> _clients = new Dictionary<Type, RestClient>();

        public static RestClient Get(Type key)
        {
            if (_clients.TryGetValue(key, out var instance))
            {
                return instance;
            }

            var client = new RestClient();
            _clients.Add(key, client);
            return client;
        }

        public static void Returns(Type key)
        {
            if (_clients.Remove(key, out var client))
            {
                client.Dispose();
            }
        }
    }
}
