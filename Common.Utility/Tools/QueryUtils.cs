using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace Common.Utility.Tools
{
    /// <summary>
    /// 查询工具
    /// </summary>
    public sealed class QueryUtils
    {
        #region 网络信息查询
        /// <summary>
        /// ip归属地查询
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public string IPAddress(string ipAddress)
        {
            if (ValidUtils.IsIP(ipAddress))
            {
                HttpClient client=new HttpClient();
                try
                {
                    string response = client.GetStringAsync($"http://ip.taobao.com/service/getIpInfo.php?ip={ipAddress}").Result;
                    return response;
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }
               
            }

            return String.Empty;
        }

        #endregion

        #region 主机信息查询

        /// <summary>
        /// 指定类型的端口是否已经被使用了
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="type">端口类型</param>
        /// <returns></returns>
        public bool PortInUse(int port, PortType type)
        {
            bool flag = false;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipendpoints = null;
            if (type == PortType.TCP)
            {
                ipendpoints = properties.GetActiveTcpListeners();
            }
            else
            {
                ipendpoints = properties.GetActiveUdpListeners();
            }
            foreach (var point in ipendpoints)
            {
                if (point.Port == port)
                {
                    flag = true;
                    break;
                }
            }
            ipendpoints = null;
            properties = null;
            return flag;
        }
        
        #endregion
    }
    /// <summary>
    /// 端口类型
    /// </summary>
    public enum PortType
    {
        /// <summary>
        /// TCP类型
        /// </summary>
        TCP,
        /// <summary>
        /// UDP类型
        /// </summary>
        UDP
    }
   
}
