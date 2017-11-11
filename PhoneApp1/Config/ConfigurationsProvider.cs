using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1.Config
{
    public static class ConfigurationsProvider
    {
        public static EnemyConfigurations EnemyConfigurations { get; private set; }

        static ConfigurationsProvider()
        {
            EnemyConfigurations = new EnemyConfigurations();
        }
    }
}
