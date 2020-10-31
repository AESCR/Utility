using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Service;
using COSXML.Model.Tag;
using COSXML.Transfer;
using COSXML.Utils;
using System;
using System.Collections.Generic;

namespace Common.Utility.Tencent.OSS
{
    public class CosXmlServer2
    {
        #region Private Fields

        private readonly string _bucket = String.Empty;

        private readonly CosXmlConfig _config = null;

        /// <summary>
        /// 提供各种 COS API 服务接口。
        /// </summary>
        private readonly CosXmlServer _cosXml = null;

        /// <summary>
        /// 提供设置密钥信息接口。
        /// </summary>
        private readonly QCloudCredentialProvider _qcloud = null;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// 实例化 COS API 服务接口。
        /// </summary>
        /// <param name="config"> 提供配置 SDK 接口。 </param>
        /// <param name="cosCredentialProvider"> 提供设置密钥信息接口。 </param>
        public CosXmlServer2(CosXmlConfig config, QCloudCredentialProvider cosCredentialProvider, string bucket)
        {
            _qcloud = cosCredentialProvider;
            _config = config;
            _cosXml = new CosXmlServer(config, cosCredentialProvider);
            this._bucket = bucket;
        }

        /// <summary>
        ///  永久密钥 实例化 COS API 服务接口。
        /// </summary>
        /// <param name="appid">设置腾讯云账户的账户标识 APPID</param>
        /// <param name="region">设置一个默认的存储桶地域</param>
        /// <param name="secretId">设置腾讯云账户的账户标识 APPID</param>
        /// <param name="secretKey">设置一个默认的存储桶地域</param>
        ///<param name="https">设置默认 https 请求</param>
        ///<param name="showLogs"> 显示日志</param>
        public CosXmlServer2(string appid, string region, string secretId, string secretKey, string bucket, bool https = true, bool showLogs = true)
        {
            _config = new CosXmlConfig.Builder()
                //.SetConnectionTimeoutMs(60000) //设置连接超时时间，单位 毫秒 ，默认 45000ms
                //.SetReadWriteTimeoutMs(40000) //设置读写超时时间，单位 毫秒 ，默认 45000ms
                .IsHttps(https) //设置默认 https 请求
                .SetAppid(appid) //设置腾讯云账户的账户标识 APPID
                .SetRegion(region) //设置一个默认的存储桶地域
                .SetDebugLog(showLogs) //显示日志
                .Build(); //创建 CosXmlConfig 对象
            long durationSecond = 600; //secretKey 有效时长,单位为 秒
            _qcloud = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            _cosXml = new COSXML.CosXmlServer(_config, _qcloud);
            this._bucket = bucket;
        }

        /// <summary>
        /// 临时密钥 实例化 COS API 服务接口。
        /// </summary>
        /// <param name="appid"> 设置腾讯云账户的账户标识 APPID </param>
        /// <param name="region"> 设置一个默认的存储桶地域 </param>
        /// <param name="tmpSecretId"> </param>
        /// <param name="tmpSecretKey"> </param>
        /// <param name="tmpToken"> </param>
        /// <param name="tmpExpireTime"> </param>
        /// <param name="https"> </param>
        /// <param name="showLogs"> </param>
        public CosXmlServer2(string appid, string region, string tmpSecretId, string tmpSecretKey, string tmpToken, long tmpExpireTime, string bucket, bool https = true, bool showLogs = true)
        {
            _config = new CosXmlConfig.Builder()
                .SetConnectionTimeoutMs(60000) //设置连接超时时间，单位 毫秒 ，默认 45000ms
                .SetReadWriteTimeoutMs(40000) //设置读写超时时间，单位 毫秒 ，默认 45000ms
                .IsHttps(https) //设置默认 https 请求
                .SetAppid(appid) //设置腾讯云账户的账户标识 APPID
                .SetRegion(region) //设置一个默认的存储桶地域
                .SetDebugLog(showLogs) //显示日志
                .Build(); //创建 CosXmlConfig 对象
            _qcloud = new DefaultSessionQCloudCredentialProvider(tmpSecretId, tmpSecretKey, tmpExpireTime, tmpToken);
            _cosXml = new COSXML.CosXmlServer(_config, _qcloud);
            this._bucket = bucket;
        }

        #endregion Public Constructors

        #region Public Delegates

        /// <summary>
        /// 下载更新进度回调
        /// </summary>
        /// <param name="completed"> </param>
        /// <param name="total"> </param>
        public delegate void DownLoadProgress(long completed, long total);

        public delegate void PutFail(CosClientException clientEx, CosServerException serverEx);

        /// <summary>
        /// 上传更新进度回调
        /// </summary>
        /// <param name="completed"> </param>
        /// <param name="total"> </param>
        public delegate void PutProgress(long completed, long total);

        public delegate void PutSuccess(CosResult CosResult);

        #endregion Public Delegates

        #region Public Methods

        /// <summary>
        /// 创建存储桶
        /// </summary>
        /// <param name="bucket"> 存储桶名称 格式：BucketName-APPID </param>
        /// <param name="signStartTimeSecond"> 签名有效期起始时间（Unix 时间戳），例如1557902800 </param>
        /// <param name="durationSecond"> 签名有效期时长（单位为秒），例如签名有效时期为1分钟：60 </param>
        /// <returns> </returns>
        public CosResult CreateBucket(long signStartTimeSecond, long durationSecond)
        {
            try
            {
                PutBucketRequest request = new PutBucketRequest(_bucket);

                //设置签名有效时长
                request.SetSign(signStartTimeSecond, durationSecond);
                //执行请求
                PutBucketResult result = _cosXml.PutBucket(request);
                //请求成功
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建存储桶
        /// </summary>
        /// <param name="bucket"> 存储桶名称 格式：BucketName-APPID </param>
        /// <param name="durationSecond"> 设置签名有效时长 </param>
        /// <returns> </returns>
        public CosResult CreateBucket(long durationSecond)
        {
            try
            {
                PutBucketRequest request = new PutBucketRequest(_bucket);

                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), durationSecond);
                //执行请求
                PutBucketResult result = _cosXml.PutBucket(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 删除存储桶
        /// </summary>
        /// <param name="bucket"> 存储桶名 </param>
        /// <returns> </returns>
        public CosResult DeleteBucket(string bucket)
        {
            try
            {
                DeleteBucketRequest request = new DeleteBucketRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                DeleteBucketResult result = _cosXml.DeleteBucket(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 删除存储桶
        /// </summary>
        /// <param name="bucket"> 存储桶名 </param>
        /// <param name="signStartTimeSecond"> 签名有效期起始时间（Unix 时间戳），例如1557902800 </param>
        /// <param name="durationSecond"> 签名有效期时长（单位为秒），例如签名有效时期为1分钟：60 </param>
        /// <returns> </returns>
        public CosResult DeleteBucket(long signStartTimeSecond, long durationSecond)
        {
            try
            {
                DeleteBucketRequest request = new DeleteBucketRequest(_bucket);
                //设置签名有效时长
                request.SetSign(signStartTimeSecond, durationSecond);
                DeleteBucketResult result = _cosXml.DeleteBucket(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public CosResult DeleteObject(string key)
        {
            try
            {
                //string bucket = "examplebucket-1250000000"; //存储桶，格式：BucketName-APPID
                //string key = "exampleobject"; //对象在存储桶中的位置，即称对象键.
                DeleteObjectRequest request = new DeleteObjectRequest(_bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                DeleteObjectResult result = _cosXml.DeleteObject(request);
                //请求成功
                return result;
            }
            catch
            {
                return null;
            }
        }

        public CosResult DownloadObject(string key, DownLoadProgress downLoadProgress)
        {
            try
            {
                GetObjectBytesRequest request = new GetObjectBytesRequest(_bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    downLoadProgress?.Invoke(completed, total);
                });
                //执行请求
                GetObjectBytesResult result = _cosXml.GetObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 下载对象
        /// </summary>
        /// <param name="bucket"> 存储桶名 </param>
        /// <param name="key"> Key </param>
        /// <param name="localDir"> 下载路径 </param>
        /// <param name="localFileName"> 文件名称 </param>
        /// <param name="downLoadProgress"> 下载进度回调 </param>
        /// <returns> </returns>
        public CosResult DownLoadObject(string key, string localDir, string localFileName, DownLoadProgress downLoadProgress)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest(_bucket, key, localDir, localFileName);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    downLoadProgress?.Invoke(completed, total);
                    /**/ /*Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));*/
                });
                //执行请求
                GetObjectResult result = _cosXml.GetObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <returns> </returns>
        public string GenerateSign(string method, string key = "", Dictionary<string, string> queryParameters = null, Dictionary<string, string> headers = null, long signDurationSecond = 60)
        {
            return _cosXml.GenerateSign(method, key, queryParameters, headers, signDurationSecond);
        }

        /// <summary>
        /// 查询对象列表
        /// </summary>
        /// <param name="bucket"> 桶名 </param>
        /// <param name="prefix"> 获取 /xxx 下的对象 </param>
        /// <returns> </returns>
        public CosResult GetBucket(string prefix = "")
        {
            try
            {
                //string bucket = "examplebucket-1250000000"; //格式：BucketName-APPID
                GetBucketRequest request = new GetBucketRequest(_bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                if (prefix != "")
                {
                    request.SetPrefix(prefix);
                }

                //执行请求
                GetBucketResult result = _cosXml.GetBucket(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 查询存储桶的访问控制列表。
        /// </summary>
        /// <returns> </returns>
        public CosResult GetBucketAcl(string bucket)
        {
            try
            {
                // string bucket = "examplebucket-1250000000"; //格式：BucketName-APPID
                GetBucketACLRequest request = new GetBucketACLRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                GetBucketACLResult result = _cosXml.GetBucketACL(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取请求预签名 URL
        /// </summary>
        /// <param name="appid"> 腾讯云账号 APPID </param>
        /// <param name="bucket"> 存储桶 </param>
        /// <param name="region"> 存储桶所在地域 </param>
        /// <param name="method"> HTTP 请求方法 </param>
        /// <param name="isHttps"> true：HTTPS 请求，false：HTTP 请求 </param>
        /// <param name="key"> HTTP 请求路径，即对象键 </param>
        /// <param name="headers"> 签名是否校验 header </param>
        /// <param name="queryParameters"> 签名是否校验请求 url 中查询参数 </param>
        /// <param name="signDurationSecond"> 签名有效期时长（单位为秒），例如签名有效时期为1分钟：60 </param>
        /// <returns> </returns>
        public string GetGenerateSignUrL(string method, string appid, string region, string key, bool isHttps = true, Dictionary<string, string> headers = null, Dictionary<string, string> queryParameters = null, long signDurationSecond = 60)
        {
            PreSignatureStruct preSignatureStruct = new PreSignatureStruct
            {
                appid = appid,
                bucket = _bucket,
                region = region,
                httpMethod = method,
                isHttps = isHttps,
                key = key,
                headers = headers,
                queryParameters = queryParameters,
                signDurationSecond = signDurationSecond,
            };
            return _cosXml.GenerateSignURL(preSignatureStruct);
        }

        /// <summary>
        /// 获取请求预签名 URL
        /// </summary>
        /// <param name="preSignatureStruct"> </param>
        /// <returns> </returns>
        public string GetGenerateSignUrL(PreSignatureStruct preSignatureStruct)
        {
            return _cosXml.GenerateSignURL(preSignatureStruct);
        }

        /// <summary>
        /// 获取请求预签名 URL
        /// </summary>
        /// <param name="bucket"> 存储桶 </param>
        /// <param name="region"> 存储桶所在地域 </param>
        /// <param name="key"> 对象键（Object 的名称），对象在存储桶中的唯一标识，如果请求操作是对文件的，则为文件名，且为必须参数。如果操作是对于存储桶，则为空 </param>
        /// <param name="headers"> 签名是否校验 header </param>
        /// <param name="queryParameters"> 签名是否校验请求 url 中查询参数 </param>
        /// <param name="signDurationSecond"> 签名有效期时长（单位为秒），例如签名有效时期为1分钟：60 </param>
        /// <returns> </returns>
        public string GetGenerateSignUrL(string httpMethod, string region, string key = "", Dictionary<string, string> headers = null, Dictionary<string, string> queryParameters = null, long signDurationSecond = 60)
        {
            if (_config != null)
            {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = _config.Appid;
                preSignatureStruct.isHttps = _config.IsHttps;
                preSignatureStruct.region = region;
                preSignatureStruct.bucket = _bucket;
                preSignatureStruct.httpMethod = httpMethod;
                preSignatureStruct.key = key;
                preSignatureStruct.signDurationSecond = signDurationSecond;
                preSignatureStruct.queryParameters = queryParameters;
                preSignatureStruct.headers = headers;
                return _cosXml.GenerateSignURL(preSignatureStruct);
            }
            return "请配置appid";
        }

        /// <summary>
        /// 获取对象 URL
        /// </summary>
        /// <param name="cosRequest"> 上传结果 </param>
        /// <returns> </returns>
        public string GetUrl(CosRequest cosRequest)
        {
            return _cosXml.GetAccessURL(cosRequest);
        }

        /// <summary>
        /// 确认该存储桶是否存在 是否有权限访问
        /// </summary>
        /// <returns>
        ///存储桶存在且有读取权限，返回 HTTP 状态码为200。
        ///无存储桶读取权限，返回 HTTP 状态码为403。
        ///存储桶不存在，返回 HTTP 状态码为404。
        /// </returns>
        public CosResult HaveBucket(string bucket)
        {
            try
            {
                HeadBucketRequest request = new HeadBucketRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                HeadBucketResult result = _cosXml.HeadBucket(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="bucket"> </param>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public CosResult HaveObject(string key)
        {
            try
            {
                HeadObjectRequest request = new HeadObjectRequest(_bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                HeadObjectResult result = _cosXml.HeadObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 表单上传对象
        /// </summary>
        /// <param name="bucket"> 存储桶 </param>
        /// <param name="key"> 对象键 </param>
        /// <param name="srcPath"> 本地文件 </param>
        /// <param name="putObjectProgress"> 回调上传进度 </param>
        /// <returns> </returns>
        public CosResult PostObject(string key, string srcPath, PutProgress putObjectProgress)
        {
            try
            {
                PostObjectRequest request = new PostObjectRequest(_bucket, key, srcPath);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    putObjectProgress?.Invoke(completed, total);
                });
                //执行请求
                PostObjectResult result = _cosXml.PostObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 上传大文件
        /// </summary>
        /// <param name="bucket"> 存储桶 </param>
        /// <param name="key"> 对象键 </param>
        /// <param name="srcPath"> 本地路径 </param>
        /// <param name="putBigObjectProgress"> 上传进度回调 </param>
        /// <param name="putSuccess"> 上传成功 </param>
        /// <param name="putFail"> 上传失败 </param>
        public void PutBigObject(string key, string srcPath, PutProgress putBigObjectProgress, PutSuccess putSuccess, PutFail putFail)
        {
            TransferManager transferManager = new TransferManager(_cosXml, new TransferConfig());
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(_bucket, null, key);
            uploadTask.SetSrcPath(srcPath);
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                putBigObjectProgress?.Invoke(completed, total);
            };
            uploadTask.successCallback = delegate (CosResult CosResult)
            {
                putSuccess?.Invoke(CosResult);
            };
            uploadTask.failCallback = delegate (CosClientException clientEx, CosServerException serverEx)
            {
                putFail?.Invoke(clientEx, serverEx);
            };
            transferManager.Upload(uploadTask);
        }

        /// <summary>
        /// 设置存储桶ACL
        /// </summary>
        /// <param name="bucket"> 存储桶 </param>
        /// <param name="cosAcl"> 读写权限 </param>
        /// <param name="ownerUin"> 根账号 </param>
        /// <param name="subUin"> 子账号 </param>
        /// <returns> </returns>
        public CosResult PutBucketAcl(CosACL cosAcl, string ownerUin, string subUin)
        {
            try
            {
                //string bucket = "examplebucket-1250000000"; //格式：BucketName-APPID
                PutBucketACLRequest request = new PutBucketACLRequest(_bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置私有读写权限
                request.SetCosACL(cosAcl);
                //授予账号读权限
                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
                readAccount.AddGrantAccount(ownerUin, subUin);
                request.SetXCosGrantRead(readAccount);
                //执行请求
                PutBucketACLResult result = _cosXml.PutBucketACL(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 上传一个对象到存储桶
        /// </summary>
        /// <param name="bucket"> 存储桶名 </param>
        /// <param name="key"> 对象在存储桶中的位置 即称对象键 </param>
        /// <param name="data"> 文件字节 </param>
        /// <param name="putObjectProgress"> 滚动条进度 </param>
        /// <returns> </returns>
        public CosResult PutByteObject(string key, byte[] data, PutProgress putObjectProgress)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_bucket, key, data);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    putObjectProgress?.Invoke(completed, total);
                });
                //执行请求
                PutObjectResult result = _cosXml.PutObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public string PutByteObject(string key, byte[] data, PutProgress putObjectProgress, bool signature = false)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_bucket, key, data);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    putObjectProgress?.Invoke(completed, total);
                });
                //执行请求
                PutObjectResult result = _cosXml.PutObject(request);
                if (signature) //是否是预签名url
                {
                    return GetGenerateSignUrL("Get", _config.Appid, _config.Region, key, true, null, null);
                }
                else
                {
                    return GetUrl(request);
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="bucket"> </param>
        /// <param name="key"> </param>
        /// <param name="data"> </param>
        /// <returns> </returns>
        public CosResult PutByteObject(string key, byte[] data)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_bucket, key, data);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                PutObjectResult result = _cosXml.PutObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 上传大文件成功回调
        /// </summary>
        /// <param name="CosResult "> 响应结果 </param>
        /// <summary>
        /// 上传大文件失败错误
        /// </summary>
        /// <param name="clientEx"> </param>
        /// <param name="serverEx"> </param>
        /// <summary>
        /// 上传一个对象至存储桶
        /// </summary>
        /// <param name="key"> 对象在存储桶中的位置 即称对象键 </param>
        /// <param name="srcPath"> 本地文件绝对路径 </param>
        /// <param name="putObjectProgress"> 滚动条进度 </param>
        /// <returns> </returns>
        public CosResult PutObject(string key, string srcPath, PutProgress putObjectProgress)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_bucket, key, srcPath);
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    putObjectProgress?.Invoke(completed, total);
                });

                //执行请求
                PutObjectResult result = _cosXml.PutObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 上传一个对象至存储桶
        /// </summary>
        /// <param name="bucket"> 存储桶名 </param>
        /// <param name="key"> 对象在存储桶中的位置 即称对象键 </param>
        /// <param name="srcPath"> 本地文件绝对路径 </param>
        /// <param name="signStartTimeSecond"> 签名有效期起始时间（Unix 时间戳），例如1557902800 </param>
        /// <param name="durationSecond"> 签名有效期时长（单位为秒），例如签名有效时期为1分钟：60 </param>
        /// <param name="putObjectProgress"> 滚动条进度 </param>
        /// <returns> </returns>
        public CosResult PutObject(string key, string srcPath, long signStartTimeSecond, long durationSecond, PutProgress putObjectProgress)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(_bucket, key, srcPath);
                request.SetSign(signStartTimeSecond, durationSecond);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    putObjectProgress?.Invoke(completed, total);
                });
                //执行请求
                PutObjectResult result = _cosXml.PutObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 预签名 URL上传对象
        /// </summary>
        /// <param name="requestSignUrl"> 上传预签名 URL </param>
        /// <param name="srcPath"> 本地文件 </param>
        /// <param name="putObjectProgress"> 回调进度 </param>
        public CosResult PutObject2(string requestSignUrl, string srcPath, PutProgress putObjectProgress)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(null, null, srcPath);
                //设置上传请求预签名 UR L
                request.RequestURLWithSign = requestSignUrl;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    putObjectProgress?.Invoke(completed, total);
                });
                //执行请求
                PutObjectResult result = _cosXml.PutObject(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有存储桶
        /// </summary>
        /// <returns> </returns>
        public CosResult ShowBucketList()
        {
            try
            {
                GetServiceRequest request = new GetServiceRequest();
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                GetServiceResult result = _cosXml.GetService(request);
                return result;
            }
            catch
            {
                return null;
            }
        }

        #endregion Public Methods
    }
}