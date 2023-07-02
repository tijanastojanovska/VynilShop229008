
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VynilShop.Services.Interfaces;

namespace VynilShop.Services
{
    public class ConsumeScopedHostedService 
    {
        private readonly IServiceProvider _service;
        public ConsumeScopedHostedService(IServiceProvider service)
        {
            _service = service;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
