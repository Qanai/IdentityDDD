﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityDDD.Web.Controllers
{
    public enum ManageMessageId
    {
        AddPhoneSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        Error
    }
}