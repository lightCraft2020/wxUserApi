using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WxUserApi
{
    public class WxUserHelper
    {


        /// <summary>  
        /// 获取链接返回数据  
        /// </summary>  
        /// <param name="Url">链接</param>  
        /// <param name="type">请求类型</param>  
        /// <returns></returns>  
        public static string GetUrltoString(string Url, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                // Get the response instance.  
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        #region 微信小程序用户数据解密  
        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="inputdata">输入的数据encryptedData</param>  
        /// <param name="AesKey">key</param>  
        /// <param name="AesIV">向量128</param>  
        /// <returns name="result">解密后的字符串</returns>  
        public static string AESDecrypt(string encryptData, string aesKey,string aesIV)
        {
            try
            {
                aesIV = aesIV.Replace(" ", "+");
                aesKey = aesKey.Replace(" ", "+");
                encryptData = encryptData.Replace(" ", "+");
                byte[] encryptedData = Convert.FromBase64String(encryptData);

                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Key = Convert.FromBase64String(aesKey); // Encoding.UTF8.GetBytes(AesKey);  
                rijndaelCipher.IV = Convert.FromBase64String(aesIV);// Encoding.UTF8.GetBytes(AesIV);  
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                string result = Encoding.UTF8.GetString(plainText);

                return result;
            }
            catch (Exception)
            {
                return null;

            }
        }
        #endregion
    }
}