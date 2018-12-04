using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxUserApiV2.Model
{
    public class WxUserInfo
    {
        public string openId { get; set; }
        public string nickName { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string avatarUrl { get; set; }
        public string unionId { get; set; }
        public WaterMarkInfo watermark { get; set; }

    }
}