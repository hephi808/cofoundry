﻿using Cofoundry.Core.Mail;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cofoundry.Domain.MailTemplates.DefaultMailTemplates
{
    /// <summary>
    /// Template for the email sent to a new user when their account has
    /// been created with a temporary password. This default version of the 
    /// template is used for all user areas except the Cofoundry admin 
    /// user area.
    /// </summary>
    public class DefaultNewUserWithTemporaryPasswordMailTemplate : DefaultMailTemplateBase
    {
        public DefaultNewUserWithTemporaryPasswordMailTemplate()
        {
            ViewFile = DefaultMailTemplatePath.TemplateView(nameof(DefaultNewUserWithTemporaryPasswordMailTemplate));
            SubjectFormat = "{0}: Your account has been created";
        }

        /// <summary>
        /// The username of the new user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The temporary password that the user can use to log in to 
        /// the site.
        /// </summary>
        public IHtmlContent TemporaryPassword { get; set; }

        /// <summary>
        /// The path that the user can use to log in.
        /// </summary>
        public string LoginPath { get; set; }
    }
}
