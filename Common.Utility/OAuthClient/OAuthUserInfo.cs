﻿namespace Common.Utility.OAuthClient
{
    public class OAuthUserInfo
    {
        /// <summary>
        /// 性别 true 男
        /// </summary>
        public bool? Gender { get; set; }

        /// <summary>
        /// 可将此ID进行存储便于用户下次登录时辨识其身份
        /// </summary>
        public string Id { get; set; }//唯一标识

        public string ImgUrl { get; set; }
        public string Name { get; set; }

        //昵称
        //头像
    }
}