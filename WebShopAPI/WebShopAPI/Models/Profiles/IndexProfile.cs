﻿using Microsoft.AspNetCore.Identity;

namespace WebShopAPI.Models.Profiles
{
    public class IndexProfile
    {

        public EditExtraProfileModel profile { get; set; }
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        //public bool TwoFactor { get; set; }

        //public bool BrowserRemembered { get; set; }

        //public string AuthenticatorKey { get; set; }
    }
}
