using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using TwentyTwenty.Storage;
using TwentyTwenty.Storage.Azure;

namespace Ubora.Web.Infrastructure.Storage
{
    public class CustomDevelopmentAzureStorageProvider : CustomAzureStorageProvider
    {
        public CustomDevelopmentAzureStorageProvider(AzureProviderOptions options, AzureStorageProvider azureStorageProvider) : base(options, azureStorageProvider)
        {
        }

        public override string GetBlobUrl(string containerName, string blobName)
        {
            var blobUrl = base.GetBlobUrl(containerName, blobName);

            //bad solution and should detect if running in a docker or virtual machine
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isLinux)
            {
                return blobUrl.Replace("http://azurite:10000/devstoreaccount1", "http://localhost:32500/devstoreaccount1"); 
            }

            var ip = GetLocalIPAddress();
            return blobUrl.Replace("http://127.0.0.1:32500/devstoreaccount1", $"http://{ip}:32500/devstoreaccount1");
        }

        // For development because azurite does not support this yet!
        public override string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false, string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            return GetBlobUrl(containerName, blobName);
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
