using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    ///     RSA���ܽ��ܼ�RSAǩ������֤
    /// </summary>
    public class RSACryption
    {
        #region Public Methods

        //��ȡHash������
        public bool GetHash(string m_strSource, ref byte[] HashData)
        {
            //���ַ�����ȡ��Hash����
            byte[] Buffer;
            var MD5 = HashAlgorithm.Create("MD5");
            Buffer = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);

            return true;
        }

        //��ȡHash������
        public bool GetHash(string m_strSource, ref string strHashData)
        {
            //���ַ�����ȡ��Hash����
            byte[] Buffer;
            byte[] HashData;
            var MD5 = HashAlgorithm.Create("MD5");
            Buffer = Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);

            strHashData = Convert.ToBase64String(HashData);
            return true;
        }

        //��ȡHash������
        public bool GetHash(FileStream objFile, ref byte[] HashData)
        {
            //���ļ���ȡ��Hash����
            var MD5 = HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();

            return true;
        }

        //��ȡHash������
        public bool GetHash(FileStream objFile, ref string strHashData)
        {
            //���ļ���ȡ��Hash����
            byte[] HashData;
            var MD5 = HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();

            strHashData = Convert.ToBase64String(HashData);

            return true;
        }

        //RSA�Ľ��ܺ���  string
        public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = new UnicodeEncoding().GetString(DypherTextBArray);
            return Result;
        }

        //RSA�Ľ��ܺ���  byte
        public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            byte[] DypherTextBArray;
            string Result;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            DypherTextBArray = rsa.Decrypt(DecryptString, false);
            Result = new UnicodeEncoding().GetString(DypherTextBArray);
            return Result;
        }

        //RSA�ļ��ܺ���  string
        public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            PlainTextBArray = new UnicodeEncoding().GetBytes(m_strEncryptString);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }

        //##############################################################################
        //RSA ��ʽ����
        //˵��KEY������XML����ʽ,���ص����ַ���
        //����һ����Ҫ˵�������ü��ܷ�ʽ�� ���� ���Ƶģ���
        //##############################################################################
        //RSA�ļ��ܺ��� byte[]
        public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            byte[] CypherTextBArray;
            string Result;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            CypherTextBArray = rsa.Encrypt(EncryptString, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;
        }

        /// <summary>
        ///     RSA ����Կ���� ����˽Կ �͹�Կ
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }

        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            var RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                return true;
            return false;
        }

        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
        {
            byte[] HashbyteDeformatter;

            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);

            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            var RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                return true;
            return false;
        }

        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
        {
            byte[] DeformatterData;

            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            var RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            DeformatterData = Convert.FromBase64String(p_strDeformatterData);

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                return true;
            return false;
        }

        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter,
                    string p_strDeformatterData)
        {
            byte[] DeformatterData;
            byte[] HashbyteDeformatter;

            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPublic);
            var RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
            //ָ�����ܵ�ʱ��HASH�㷨ΪMD5
            RSADeformatter.SetHashAlgorithm("MD5");

            DeformatterData = Convert.FromBase64String(p_strDeformatterData);

            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                return true;
            return false;
        }

        //RSAǩ��
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature,
            ref byte[] EncryptedSignatureData)
        {
            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            var RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            return true;
        }

        //RSAǩ��
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature,
            ref string m_strEncryptedSignatureData)
        {
            byte[] EncryptedSignatureData;

            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            var RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

            return true;
        }

        //RSAǩ��
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature,
            ref byte[] EncryptedSignatureData)
        {
            byte[] HashbyteSignature;

            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            var RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            return true;
        }

        //RSAǩ��
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature,
            ref string m_strEncryptedSignatureData)
        {
            byte[] HashbyteSignature;
            byte[] EncryptedSignatureData;

            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            var RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(p_strKeyPrivate);
            var RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
            //����ǩ�����㷨ΪMD5
            RSAFormatter.SetHashAlgorithm("MD5");
            //ִ��ǩ��
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

            return true;
        }

        #endregion Public Methods
    }
}