using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxUserApiV2.Model
{
    public class SessionResultInfo
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }

    }
}