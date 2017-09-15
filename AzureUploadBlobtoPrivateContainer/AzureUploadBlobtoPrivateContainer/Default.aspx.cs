using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;
using System.IO;

namespace AzureUploadBlobtoPrivateContainer
{
    public partial class Default : System.Web.UI.Page
    {
        private string accountname;
        private string accountkey;
        private string containername;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.EnsureContaierExists();
            }
        }


        private void EnsureContaierExists()
        {
            var container = GetContainer();

            // 检查container是否被创建，如果没有，创建container
            container.CreateIfNotExists();

            var permissions = container.GetPermissions();
            //对Storage的访问权限是 允许public blob
            permissions.PublicAccess = BlobContainerPublicAccessType.Off;

            container.SetPermissions(permissions);
        }

        private CloudBlobContainer GetContainer()
        {
            //Get config from Web.Config
            //这个是存储账号名称
            accountname = ConfigurationManager.AppSettings["AccountName"].ToString();
            //这个是存储账号密码
            accountkey = ConfigurationManager.AppSettings["AccountKey"].ToString();
            //这个是container name
            containername = ConfigurationManager.AppSettings["ContainerName"].ToString();

            string connectionString = GenerateConnectionString();

            //Get Azure Storage Connection String 
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();
            //Get BlobContainer Object
            return client.GetContainerReference(containername);
        }

        private string GenerateConnectionString()
        {
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append(@"BlobEndpoint=https://");
            sbuilder.Append(accountname);
            sbuilder.Append(".blob.core.chinacloudapi.cn/");

            sbuilder.Append(@";QueueEndpoint=https://");
            sbuilder.Append(accountname);
            sbuilder.Append(".queue.core.chinacloudapi.cn/");

            sbuilder.Append(@";TableEndpoint=https://");
            sbuilder.Append(accountname);
            sbuilder.Append(".table.core.chinacloudapi.cn/");

            sbuilder.Append(";AccountName=");
            sbuilder.Append(accountname);

            sbuilder.Append(";AccountKey=");
            sbuilder.Append(accountkey);

            return sbuilder.ToString();
        }


        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            //上传代码
            if (FileUpload1.HasFile)
            {
                //输出图片文件的信息
                Lblstatus.Text = "Inserted [" + FileUpload1.FileName + "] - Content Type [" + FileUpload1.PostedFile.ContentType + "] - Length [" + FileUpload1.PostedFile.ContentLength + "]";
                this.SaveFile(FileUpload1.FileName, FileUpload1.PostedFile.ContentType, FileUpload1.FileBytes);
            }
            else
            {
                Lblstatus.Text = "NO File";
            }
        }

        private void SaveFile(string fileName, string contentType, byte[] data)
        {
            //获得BlobContainer对象并把文件上传到这个Container
            var blob = this.GetContainer().GetBlockBlobReference(fileName);
            blob.Properties.ContentType = contentType;

            using (var ms = new MemoryStream(data, false))
            {
                blob.UploadFromStream(ms);
            }

            //获得SAS URL
            GetSAS(blob);
        }
        private void GetSAS(CloudBlockBlob blob)
        {
            //这里设置过期时间
            string seconds = ConfigurationManager.AppSettings["expireseconds"].ToString();
            float result = 20;

            var sas = blob.GetSharedAccessSignature(
                new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddSeconds(float.TryParse(seconds, out result) ? result : 0f)
                });
            var secureURl = blob.Uri.AbsoluteUri + sas;

            txbUrl.Text = secureURl.ToString();
        }
    }
}