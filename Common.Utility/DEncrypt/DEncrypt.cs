﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// Encrypt 的摘要说明。
    /// </summary>
    public class DEncrypt
    {
        #region Public Methods

        /// <summary>
        /// 使用缺省密钥字符串解密string
        /// </summary>
        /// <param name="original"> 密文 </param>
        /// <returns> 明文 </returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, "kuiyu.net", Encoding.Default);
        }

        /// <summary>
        /// 使用给定密钥字符串解密string
        /// </summary>
        /// <param name="original"> 密文 </param>
        /// <param name="key"> 密钥 </param>
        /// <returns> 明文 </returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, Encoding.Default);
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted"> 密文 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码方案 </param>
        /// <returns> 明文 </returns>
        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            var buff = Convert.FromBase64String(encrypted);
            var kb = Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        /// <summary>
        /// 使用缺省密钥字符串解密byte[]
        /// </summary>
        /// <param name="encrypted"> 密文 </param>
        /// <param name="key"> 密钥 </param>
        /// <returns> 明文 </returns>
        public static byte[] Decrypt(byte[] encrypted)
        {
            var key = Encoding.Default.GetBytes("MATICSOFT");
            return Decrypt(encrypted, key);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted"> 密文 </param>
        /// <param name="key"> 密钥 </param>
        /// <returns> 明文 </returns>
        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// 使用缺省密钥字符串加密string
        /// </summary>
        /// <param name="original"> 明文 </param>
        /// <returns> 密文 </returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, "kuiyu.net");
        }

        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original"> 原始文字 </param>
        /// <param name="key"> 密钥 </param>
        /// <param name="encoding"> 字符编码方案 </param>
        /// <returns> 密文 </returns>
        public static string Encrypt(string original, string key)
        {
            var buff = Encoding.Default.GetBytes(original);
            var kb = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        /// 使用缺省密钥字符串加密
        /// </summary>
        /// <param name="original"> 原始数据 </param>
        /// <param name="key"> 密钥 </param>
        /// <returns> 密文 </returns>
        public static byte[] Encrypt(byte[] original)
        {
            var key = Encoding.Default.GetBytes("MATICSOFT");
            return Encrypt(original, key);
        }

        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original"> 明文 </param>
        /// <param name="key"> 密钥 </param>
        /// <returns> 密文 </returns>
        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original"> 数据源 </param>
        /// <returns> 摘要 </returns>
        public static byte[] MakeMD5(byte[] original)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            var keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }

        #endregion Public Methods
    }
}