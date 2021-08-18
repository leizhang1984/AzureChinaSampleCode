using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;

namespace ListBlobWithSASToken
{
    class Program
    {
        static void Main(string[] args)
        {
            cc();


        }
        static void cc()
        {
            string sasToken = @"https://leizhangstorage.blob.core.chinacloudapi.cn/private?sv=2019-10-10&si=rule1&sr=c&sig=%2FLxgfPPDm0eMV1iY%2FakiXb8vBgOeMJd6maa396J0foE%3D";

            //string sasToken = @"https://leizhangstorage.blob.core.chinacloudapi.cn/private/AzureCostDetails.png?sv=2019-10-10&si=rule1&sr=b&sig=TlTccE7QaM9kjz5ceHTWBLvEYC5uADuFwZRH0oER%2FOE%3D";
            
            CloudBlobContainer blobContainer = new CloudBlobContainer(new Uri(sasToken));

            StorageCredentials accountSAS = new StorageCredentials(sasToken);
            CloudStorageAccount accountWithSAS = new CloudStorageAccount(accountSAS, "leizhangstorage", "core.chinacloudapi.cn", false);
            CloudBlobClient client = accountWithSAS.CreateCloudBlobClient();

            foreach (IListBlobItem item in client.ListBlobs(string.Empty, false))
            {
                Console.WriteLine(item.ToString());
            }

        }
        //
    }



}
