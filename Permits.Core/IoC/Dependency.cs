using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Admin.Core.IoC
{
    public static class Dependency
    {
        static public ServiceProvider ServiceProvider { set; get; }
    }
}
