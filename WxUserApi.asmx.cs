using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Services;
using WxUserApi;
using WxUserApiV2.Model;

namespace WxUserApiV2
{
    /// <summary>
    /// WxUserApi 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class WxUserApi : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetWxUserInfo(string code, string iv, string encryptedData)
        {
            //服务器端存储，需要提取到配置中  to do ...
            string Appid = "微信appid，如wx0e3183bc76125487";
            string Secret = "微信appid的密码，如285bcab5c222ff1241dc4ef8a1c";
            string grant_type = "authorization_code";

            //向微信服务端 使用登录凭证 code 获取 session_key 和 openid   
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid="
                + Appid + "&secret=" + Secret + "&js_code=" + code + "&grant_type=" + grant_type;
            string type = "utf-8";

            string openIdSessionKey = WxUserHelper.GetUrltoString(url, type);//获取微信服务器返回字符串  

            //将字符串转换为json格式   session_key openid
            JObject json = (JObject)JsonConvert.DeserializeObject(openIdSessionKey);

            SessionResultInfo sessionResultInfo = new SessionResultInfo();
            try
            {
                //微信服务器验证成功  
                sessionResultInfo.openid = json["openid"].ToString();
                sessionResultInfo.session_key = json["session_key"].ToString();
            }
            catch (Exception)
            {
                //微信服务器验证失败  
                sessionResultInfo.errcode = json["errcode"].ToString();
                sessionResultInfo.errmsg = json["errmsg"].ToString();
            }

            if (!string.IsNullOrEmpty(sessionResultInfo.openid))
            {
                //用户数据解密  
                var userString = WxUserHelper.AESDecrypt(encryptedData, sessionResultInfo.session_key, iv);
                //存储用户数据  
                JObject _usrInfo = (JObject)JsonConvert.DeserializeObject(userString);

                WxUserInfo userInfo = new WxUserInfo();
                userInfo.openId = _usrInfo["openId"].ToString();

                try //部分验证返回值中没有unionId  
                {
                    userInfo.unionId = _usrInfo["unionId"].ToString();
                }
                catch (Exception)
                {
                    userInfo.unionId = "unionId";
                }

                userInfo.nickName = _usrInfo["nickName"].ToString();
                userInfo.gender = _usrInfo["gender"].ToString();
                userInfo.city = _usrInfo["city"].ToString();
                userInfo.province = _usrInfo["province"].ToString();
                userInfo.country = _usrInfo["country"].ToString();
                userInfo.avatarUrl = _usrInfo["avatarUrl"].ToString();

                object watermark = _usrInfo["watermark"].ToString();
                object appid = _usrInfo["watermark"]["appid"].ToString();
                object timestamp = _usrInfo["watermark"]["timestamp"].ToString();

                return JsonConvert.SerializeObject(userInfo);
            }
            else
            {
                return "";
            }
        }
    }
}
