﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class Success : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            Session.Clear();
            if (!IsPostBack)
            {
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
        }
    }
}