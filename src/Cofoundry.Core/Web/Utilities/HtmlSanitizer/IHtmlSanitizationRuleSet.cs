﻿using AngleSharp.Css.Dom;

namespace Cofoundry.Core.Web;

/// <summary>
/// A set of configuration rules for html sanitization used
/// by the IHtmlSanitizer.
/// </summary>
public interface IHtmlSanitizationRuleSet
{
    /// <summary>
    /// Collection of css "@" rules to permit e.g. "@media" or "@font-face".
    /// Disallowing @namespace while allowing other types of at-rules can lead 
    /// to errors. Property declarations in @font-face and @viewport are not 
    /// sanitized <see cref="https://github.com/mganss/HtmlSanitizer#css-at-rules-allowed-by-default"/>.
    /// </summary>
    ISet<CssRuleType> PermittedAtRules { get; }

    /// <summary>
    /// Collection of html tag permit to allow e.g. "title" and "alt".
    /// </summary>
    ISet<string> PermittedAttributes { get; }

    /// <summary>
    /// Collection of css classes to allow. An empty set indicates that all
    /// classes should be allowed.
    /// </summary>
    ISet<string> PermittedCssClasses { get; }

    /// <summary>
    /// Collection of style properties to permit e.g. "font" and "margin".
    /// </summary>
    ISet<string> PermittedCssProperties { get; }

    /// <summary>
    /// Collection of http schemas to permit e.g. "http", "https" and "mailto".
    /// </summary>
    ISet<string> PermittedSchemes { get; }

    /// <summary>
    /// Collection html tags to permit e.g. "a" and "div".
    /// </summary>
    ISet<string> PermittedTags { get; }

    /// <summary>
    /// Collection html tags that are permitted to have uri properties e.g. "src", "href".
    /// </summary>
    ISet<string> PermittedUriAttributes { get; }
}
