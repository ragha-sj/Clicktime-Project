

namespace Ocaramba.Tests.PageObjects.PageObjects.ProAgWorks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Eventing.Reader;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using Microsoft.Win32.SafeHandles;
    using NLog;
    using NUnit.Framework;
    using Ocaramba.Extensions;
    using Ocaramba.Types;
    using Ocaramba.Helpers;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;
    public class Compliance: ProjectPageBase
    {
        //Compliance ElementLocators
        public ElementLocator
            //buttons
            btnRiYear = new ElementLocator(Locator.XPath, "//span[@aria-owns='ReinsuranceYear_listbox']//span[@class='k-select']"),
            btnGenerateReport = new ElementLocator(Locator.XPath, "//input[@id='reportBtn']"),

            //Dropdown lists
            ddlComplianceReviewType = new ElementLocator(Locator.XPath, "//select[@id='SelectedCustomSearch']"),
            ddlRiYear = new ElementLocator(Locator.XPath, "//select[@id='ReinsuranceYear']"),

            // checkboxes
            
            // Text Edit Fields
            txtPolicyNumber = new ElementLocator(Locator.XPath, "//input[@id='PolicyNumber']"),
            txtReviewNumber = new ElementLocator(Locator.XPath, "//input[@id='ReviewNumber']"),
            txtGrowerName = new ElementLocator(Locator.XPath, "//input[@id='GrowerName']"),

            // Tables Forms and Documents
            tblComplianceReviews = new ElementLocator(Locator.XPath, "//div[@id='ComplianceReviewsGrid']//table/tbody"),
            tblHdrsComplianceReviews = new ElementLocator(Locator.XPath, "//div[@id='ComplianceReviewsGrid']//thead"),
            

            //*****End locator not used
            zEndLocator = new ElementLocator(Locator.XPath, "//notused[@id='notused']");

        public Compliance(DriverContext driverContext)
            : base(driverContext)
        {
            // Set ElementLocator Logical Names
            this.SetLogNames(this);
        }

        /// <summary>
        /// Verify some elements on the compliance page exist and/or are displayed
        /// </summary>
        /// <param name="dc">Current DriverContext</param>
        public void VerifyCompliancePageLoaded(DriverContext dc)
        {
            // ddlComplianceReviewType
            if (dc.Driver.GetElement(ddlComplianceReviewType, dc).WaitTillDisplayed())
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(ddlComplianceReviewType.Value + " Is Displayed");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(ddlComplianceReviewType.Value + " Is NOT Displayed");
            }

            // ddlRiYear
            if (dc.Driver.GetElement(ddlRiYear, dc) != null)
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(ddlRiYear.Value + " Found");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(ddlRiYear.Value + " Id Not Found");
            }

            // btnRiYear
            if (dc.Driver.GetElement(btnRiYear, dc).WaitTillDisplayed())
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(btnRiYear.Value + " Is Displayed");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(btnRiYear.Value + " Is NOT Displayed");
            }

            // txtPolicyNumber
            if (dc.Driver.GetElement(txtPolicyNumber, dc).WaitTillDisplayed())
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(txtPolicyNumber.Value + " Is Displayed");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(txtPolicyNumber.Value + " Is NOT Displayed");
            }

            // txtReviewNumber
            if (dc.Driver.GetElement(txtReviewNumber, dc).WaitTillDisplayed())
            {
                // pass
                dc.ProAgXmlRsltLogger.LogPass(txtReviewNumber.Value + " Is Displayed");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(txtReviewNumber.Value + " Is NOT Displayed");
            }

            // txtGrowerName
            if (dc.Driver.GetElement(txtGrowerName, dc).WaitTillDisplayed())
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(txtGrowerName.Value + " Is Displayed");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(txtGrowerName.Value + " Is NOT Displayed");
            }

            // btnGenerateReport
            if (dc.Driver.GetElement(btnGenerateReport, dc).WaitTillDisplayed())
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(btnGenerateReport.Value + " Is Displayed");
            }
            else
            {
                //fail
                dc.ProAgXmlRsltLogger.LogFail(btnGenerateReport.Value + " Is NOT Displayed");
            }

            // tblHdrsComplianceReviews
            if (dc.Driver.GetElement(tblHdrsComplianceReviews, dc).WaitTillDisplayed())
            

            // tblComplianceReviews
            if (dc.Driver.GetElement(tblComplianceReviews, dc) != null)
            {
                //pass
                dc.ProAgXmlRsltLogger.LogPass(tblComplianceReviews.Value + " Found");
            }
            else
            {
                // fail
                dc.ProAgXmlRsltLogger.LogFail(tblComplianceReviews.Value + " NOT Found");
            }

        }
        
    }
}
