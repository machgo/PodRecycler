using System;
using System.Linq;
using k8s;

namespace PodRecycler
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = KubernetesClientConfiguration.InClusterConfig();
            var client = new Kubernetes(config);
            var cNs = config.Namespace;
            if (config.Namespace == null)
                cNs = "default";

            var pods = (client.ListNamespacedPod(cNs))
                .Items.Where(x => x.Metadata.Labels.ContainsKey("podrecycler"));

            foreach (var n in pods)
            {
                if (n.Metadata.Labels["podrecycler"] == "yes")
                {
                    Console.WriteLine($"deleting {n.Metadata.Name}");
                    client.DeleteNamespacedPod(n.Metadata.Name, cNs);
                }
            }
        }
    }
}
