using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickTimeProject
{
   public class ElementLocator
    {
        public ElementLocator(Locator kind, string value)
        {
            this.Kind = kind;
            this.Value = value;
        }
        /// <summary>
        /// Gets or sets the kind of element locator.
        /// </summary>
        /// <value>
        /// Kind of element locator (Id, Xpath, ...).
        /// </value>
        public Locator Kind { get; set; }

        /// <summary>
        /// Gets or sets the element locator value.
        /// </summary>
        /// <value>
        /// The the element locator value.
        /// </value>
        public string Value { get; set; }

       
    }
}
