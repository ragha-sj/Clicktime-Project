using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTimeProject
{
    public enum Locator
    {
        /// <summary>
        /// The Id selector
        /// </summary>
        Id,

        /// <summary>
        /// The class name selector
        /// </summary>
        ClassName,

        /// <summary>
        /// The CSS selector
        /// </summary>
        CssSelector,

        /// <summary>
        /// The link text selector
        /// </summary>
        LinkText,

        /// <summary>
        /// The name selector
        /// </summary>
        Name,

        /// <summary>
        /// The partial link text selector
        /// </summary>
        PartialLinkText,

        /// <summary>
        /// The tag name selector
        /// </summary>
        TagName,

        /// <summary>
        /// The XPath selector
        /// </summary>
        XPath
    }
}

