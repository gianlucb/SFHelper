# Service Fabric Helper
Service Fabric helper classes to read the cluster manifest, the reverse proxy settings, the service configuration etc..

##Cluster Manifest

```csharp
    var currentNodeType = ClusterManifest.Instance.CurrentNodeType;
    var nodeTypes = ClusterManifest.Instance.NodeTypes;
```

##Reverse Proxy

```csharp
   var url = ReverseProxyResolver.GetReverseProxyListeningUrl("localhost");
   var port = ReverseProxyResolver.GetReverseProxyPort();
   var protocol = ReverseProxyResolver.GetReverseProxyProtocol();
   var port2 = ReverseProxyResolver.GetReverseProxyPort(nodeTypes.Last());
```

##Service Configuration

```csharp
    var parameter = this.Context.Config("MyConfigSection").Parameters["MyParameter"].Value;
```

               
