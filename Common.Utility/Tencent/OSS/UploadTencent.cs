using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Utility.Tencent.OSS
{
    public class UploadTencent
    {
        private string appid;
        private string bucket;
        private string region;
        private string secretKey;

        public UploadTencent(string appid, string region, string secretId, string secretKey, string bucket, bool https = true, bool showLogs = true)
        {
            CosXmlServer = new CosXmlServer2(appid, region, secretId, secretKey, bucket, https, showLogs);
            this.region = region;
            this.secretKey = secretKey;
            this.bucket = bucket;
            this.appid = appid;
        }

        private CosXmlServer2 CosXmlServer { get; set; }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public bool DeleteFile(string name)
        {
            return CosXmlServer.DeleteObject(name) != null;
        }

        public bool FileExist(string name)
        {
            return CosXmlServer.HaveObject(name) != null;
        }

        public string GetDownLoadUrl(string name, long second = 60)
        {
            return CosXmlServer.GetGenerateSignUrL("get", appid, region, name, signDurationSecond: second);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="data"> 文件字节 </param>
        /// <param name="name"> 上传后的文件名称 包含地址 </param>
        /// <param name="path"> 上传的地址 </param>
        /// <param name="progress"> 进度条 </param>
        public bool UploadFile(byte[] data, string name, string path = "", CosXmlServer2.PutProgress progress = null)
        {
            path = path + "/" + name;
            var uploadSate = CosXmlServer.PutByteObject(path, data, progress);
            return uploadSate != null;
        }

        /// <summary>
        /// 批量上传
        /// </summary>
        /// <param name="dataList"> </param>
        /// <param name="nameList"> </param>
        /// <returns> </returns>

        public bool UploadFiles(List<byte[]> dataList, List<string> nameList, string path = "")
        {
            List<string> uploadList = new List<string>();
            if (dataList.Count != nameList.Count) return false;
            for (int i = 0; i < dataList.Count; i++)
            {
                var bytes = dataList[i];
                var name = nameList[i];
                var temp = path + "/" + name;
                if (UploadFile(bytes, temp, null))
                {
                    uploadList.Add(name);
                }
                else
                {
                    Task.Run(() =>
                    {
                        foreach (var tempName in uploadList)
                        {
                            var temp2 = path + "/" + tempName;
                            DeleteFile(temp2);
                        }
                    });
                    return false;
                }
            }
            return true;
        }
    }
}