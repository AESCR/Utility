using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Common.Utility.Extensions
{
    /// <summary>
    /// HttpRequest  扩展
    /// </summary>
    public static class HttpRequestExtension
    {
        private const string NullIpAddress = "::1";
        /// <summary>
        /// 判断是否是本地源访问
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsLocal(this Microsoft.AspNetCore.Http.HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            var address=connection.RemoteIpAddress;
            if (address == null || address.ToString() == NullIpAddress) return true;
            //We have a remote address set up
            var localAddress= connection.LocalIpAddress;
            return (localAddress != null && localAddress.ToString() != NullIpAddress)
                //Is local is same as remote, then we are local
                ? address.Equals(connection.LocalIpAddress)
                //else we are remote if the remote IP address is not a loopback address
                : IPAddress.IsLoopback(address);

        }
    }
}
