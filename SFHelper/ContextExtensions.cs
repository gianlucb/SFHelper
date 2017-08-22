using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFHelper
{
    /// <summary>
    /// Add some useful methods to read the service configuration
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Get the settings configuration from the service manifest
        /// </summary>
        /// <param name="context">current service context</param>
        /// <param name="packageName">as defined in the ServiceManifest.xml </param>
        /// <param name="sectionName">as defined in the Settings.xml</param>
        /// <returns>the section that can be used to read the parameters</returns>
        public static ConfigurationSection Config(this ServiceContext context, string sectionName, string packageName = "Config")
        {
            try
            {
                var configurationPackage = context.CodePackageActivationContext.GetConfigurationPackageObject(packageName);
                return configurationPackage.Settings.Sections[sectionName];
            }
            catch (Exception ex)
            {
                throw new SFHelperException($"failed to get the section {sectionName} from package {packageName}. Make sure is defined in the service configuration", ex);
            }
   
        }


    }
}
