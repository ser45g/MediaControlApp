using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaControlApp.Infrasturcture
{
    public class DITypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public DITypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public object Resolve(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return _provider.GetService(type);
        }

        public void Dispose()
        {
            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
