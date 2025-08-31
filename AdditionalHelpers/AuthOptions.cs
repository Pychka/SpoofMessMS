﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdditionalHelpers
{
    public class AuthOptions
    {
        public const string AUDIENCE = "SpoofClientV000";
        public const string ISSUER= "SpoofEntranceServiceV005";
        private const string KEY = "Spoof_Super_Secret_Key_For_Alpha_Version";
        private const int LIFETIME = 12;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
