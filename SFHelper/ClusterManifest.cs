using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Management.ServiceModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SFHelper
{

    /// <summary>
    /// Wrapper and helper class to access useful information stored in the cluster manifest. 
    /// As reading the manifest is an expensive operation, this is a singleton and the result is cached
    /// </summary>
    public sealed class ClusterManifest
    {
        /// <summary>
        /// Hard-typed cluster manifest. Useful to access the properties not exposes via this class
        /// </summary>
        public ClusterManifestType Manifest { get; private set; }

        // _lock is required for thread safety and having this object assumes that creating
        // the singleton object is more expensive than creating a System.Object object and that
        // creating the singleton object may not be necessary at all. Otherwise, it is more
        // efficient and easier to just create the singleton object in a class constructor
        private static readonly Object _lock = new Object();
        private static ClusterManifest _value = null;

        private ClusterManifest() {

            ReadManifest().Wait();
        }

        private async Task ReadManifest()
        {

            try
            {
                using (var cl = new FabricClient())
                {
                    var manifestStr = await cl.ClusterManager.GetClusterManifestAsync().ConfigureAwait(false);
                    var serializer = new XmlSerializer(typeof(ClusterManifestType));

                    using (var reader = new StringReader(manifestStr))
                    {
                        Manifest = (ClusterManifestType)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SFHelperException("failed to deserialize the manifest", ex);
            }
        }

        /// <summary>
        /// Singleton instance (cached)
        /// </summary>
        public static ClusterManifest Instance
        {
            get
            {         // If the Singleton was already created, just return it (this is fast)
                if (_value != null) return _value;
                Monitor.Enter(_lock); // Not created, let 1 thread create it
                if (_value == null)
                {
                    // Still not created, create it
                    ClusterManifest temp = new ClusterManifest();
                    Volatile.Write(ref _value, temp);
                }
                Monitor.Exit(_lock);
                // Return a reference to the one Singleton object
                return _value;
            }
        }

        /// <summary>
        /// Node types as defined in the cluster config
        /// </summary>
        public List<string> NodeTypes
        {
            get
            {
                try
                {
                    return Manifest.NodeTypes.Select(x => x.Name).ToList<string>();
                }
                catch (Exception ex)
                {
                    throw new SFHelperException("failed to read node types", ex);
                }
            }
        }

        /// <summary>
        /// Current node type where the code is executed
        /// </summary>
        public string CurrentNodeType {

            get {
                try
                {
                    return FabricRuntime.GetNodeContext().NodeType;
                }
                catch (FabricConnectionDeniedException)
                {
                    return "Unknown";
                }
                catch (Exception ex)
                {
                    throw new SFHelperException("failed to get the current Node type", ex);
                }
            }
        }

    }
}
