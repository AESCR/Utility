using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utility
{
    /// <summary>
    ///     MySecurity(安全类) 的摘要说明。
    /// </summary>
    public class MySecurity
    {
        #region Private Fields

        private readonly string key; //默认密钥
        private byte[] sIV;

        private byte[] sKey;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     初始化安全类
        /// </summary>
        public MySecurity()
        {
            ///默认密码
            key = "0123456789";
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text)) return "";

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if (text.Length % 4 != 0) return "包含不正确的BASE64编码";
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase)) return "包含不正确的BASE64编码";
                var Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                var page = text.Length / 4;
                var outMessage = new ArrayList(page * 3);
                var message = text.ToCharArray();
                for (var i = 0; i < page; i++)
                {
                    var instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    var outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    else
                        outstr[2] = 0;
                    if (instr[3] != 64)
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    else
                        outstr[2] = 0;
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }

                var outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text)) return "";

            try
            {
                char[] Base64Code =
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                    'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '+', '/', '='
                };
                byte empty = 0;
                var byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                var messageLen = byteMessage.Count;
                var page = messageLen / 3;
                var use = 0;
                if ((use = messageLen % 3) > 0)
                {
                    for (var i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }

                outmessage = new StringBuilder(page * 4);
                for (var i = 0; i < page; i++)
                {
                    var instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    var outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = instr[2] & 0x3f;
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }

                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     128位MD5算法加密字符串
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        public static string MD5(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text)) return "";
            //返回MD5值的字符串表示
            return MD5(Encoding.Unicode.GetBytes(text));
        }

        /// <summary>
        ///     128位MD5算法加密Byte数组
        /// </summary>
        /// <param name="data">要加密的Byte数组</param>
        public static string MD5(byte[] data)
        {
            //如果Byte数组为空，则返回
            if (data.Length == 0) return "";

            try
            {
                //创建MD5密码服务提供程序
                var md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                var result = md5.ComputeHash(data);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return Convert.ToBase64String(result);
            }
            catch
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "";
            }
        }

        public static string Md5Sum(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text)) return "";
            //返回MD5值的字符串表示
            return Md5Sum(Encoding.UTF8.GetBytes(text));
        }

        public static string Md5Sum(byte[] bs)
        {
            // 创建md5 对象
            MD5 md5;
            md5 = System.Security.Cryptography.MD5.Create();

            // 生成16位的二进制校验码
            var hashBytes = md5.ComputeHash(bs);

            // 转为32位字符串
            var hashString = "";
            for (var i = 0; i < hashBytes.Length; i++) hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

            return hashString.PadLeft(32, '0');
        }

        /// <summary>
        ///     解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        public static string SDecryptString(string inputStr, string keyStr)
        {
            var ws = new MySecurity();
            return ws.DecryptString(inputStr, keyStr);
        }

        /// <summary>
        ///     解密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        public static string SDecryptString(string inputStr)
        {
            var ws = new MySecurity();
            return ws.DecryptString(inputStr, "");
        }

        /// <summary>
        ///     加密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>输出加密后字符串</returns>
        public static string SEncryptString(string inputStr)
        {
            var ws = new MySecurity();
            return ws.EncryptString(inputStr, "");
        }

        /// <summary>
        ///     加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        public static string SEncryptString(string inputStr, string keyStr)
        {
            var ws = new MySecurity();
            return ws.EncryptString(inputStr, keyStr);
        }

        /// <summary>
        ///     解密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">解密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>
        public bool DecryptFile(string filePath, string savePath, string keyStr)
        {
            var des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            var fs = File.OpenRead(filePath);
            var inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            var keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            var hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (var i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (var i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (var b in ms.ToArray()) fs.WriteByte(b);
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }

        /// <summary>
        ///     解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        public string DecryptString(string inputStr, string keyStr)
        {
            var des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            var inputByteArray = new byte[inputStr.Length / 2];
            for (var x = 0; x < inputStr.Length / 2; x++)
            {
                var i = Convert.ToInt32(inputStr.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            var keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            var hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (var i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (var i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        ///     加密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">加密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>
        public bool EncryptFile(string filePath, string savePath, string keyStr)
        {
            var des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            var fs = File.OpenRead(filePath);
            var inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            var keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            var hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (var i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (var i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (var b in ms.ToArray()) fs.WriteByte(b);
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }

        /// <summary>
        ///     加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        public string EncryptString(string inputStr, string keyStr)
        {
            var des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            var inputByteArray = Encoding.Default.GetBytes(inputStr);
            var keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            var hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (var i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (var i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray()) ret.AppendFormat("{0:X2}", b);
            cs.Close();
            ms.Close();
            return ret.ToString();
        }

        #endregion Public Methods
    }
}