using System;
using System.Fabric;
using System.Fabric.Management.ServiceModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace SFHelper
{
  
    public class ReverseProxyResolver
    {

        /// <summary>
        /// Represents the port that a fabric node is configured to use when using a reverse proxy
        /// </summary>
        /// <param name="nodeType">if null or empty will use the current node type. The service proxy configuration is per node type</param>
        public static int GetReverseProxyPort(string nodeType = "")
        {
            try
            {
                //Fetch the setting from the correct node type
                nodeType = String.IsNullOrEmpty(nodeType) ? ClusterManifest.Instance.CurrentNodeType : nodeType;
                var nodeTypeSettings = ClusterManifest.Instance.Manifest.NodeTypes.Single(x => x.Name.Equals(nodeType));
                return int.Parse(nodeTypeSettings.Endpoints.HttpApplicationGatewayEndpoint.Port);
            }
            catch (Exception ex)
            {
                throw new SFHelperException("failed to get reverse proxy port", ex);
            }
        }

        /// <summary>
        /// Represents the protocol that a fabric node is configured to use when using a reverse proxy
        /// </summary>
        /// <param name="nodeType">if null or empty will use the current node type. The service proxy configuration is per node type</param>
        public static string GetReverseProxyProtocol(string nodeType = "")
        {
            try
            {
                //Fetch the setting from the correct node type
                nodeType = String.IsNullOrEmpty(nodeType) ? ClusterManifest.Instance.CurrentNodeType : nodeType;
                var nodeTypeSettings = ClusterManifest.Instance.Manifest.NodeTypes.Single(x => x.Name.Equals(nodeType));
                return nodeTypeSettings.Endpoints.HttpApplicationGatewayEndpoint.Protocol.ToString();
            }
            catch (Exception ex)
            {
                throw new SFHelperException("failed to get reverse proxy protocol", ex);
            }
        }

        /// <summary>
        /// Build the URI that a fabric node is configured to listen to when using a reverse proxy
        /// </summary>
        /// <param name="fqdn">base FQDN where the cluster is published. Like the load balancer VIP or the published Azure address</param>
        /// <param name="nodeType">if null or empty will use the current node type</param>       
        public static Uri GetReverseProxyListeningUrl(string fqdn, string nodeType = "")
        {
            try
            {
                //Fetch the setting from the correct node type
                nodeType = String.IsNullOrEmpty(nodeType) ? ClusterManifest.Instance.CurrentNodeType : nodeType;
                var nodeTypeSettings = ClusterManifest.Instance.Manifest.NodeTypes.Single(x => x.Name.Equals(nodeType));
                int port = int.Parse(nodeTypeSettings.Endpoints.HttpApplicationGatewayEndpoint.Port);
                string protocol = nodeTypeSettings.Endpoints.HttpApplicationGatewayEndpoint.Protocol.ToString();
                return new Uri($"{protocol}://{fqdn}:{port}");
            }
            catch (Exception ex)
            {
                throw new SFHelperException("failed to create the listening address", ex);
            }
        }
    }
}
