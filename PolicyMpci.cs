
using System.Collections.ObjectModel;

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

    /// <summary>
    /// ProAgWorks Policy MPCI Mapping and Methods
    /// </summary>
    public class PolicyMpci : ProjectPageBase
    {
        //PolicyMpci ElementLocators
        public ElementLocator
            //buttons
            btnCoveragesEdit = new ElementLocator(Locator.Id, "CoverageEditButtonTop"),
            btnGrowerEdit = new ElementLocator(Locator.XPath, "//input[@value='Edit']"),
            btnGrowerCancel = new ElementLocator(Locator.XPath, "(//input[@value='Cancel'])[1]"),
            btnCoveragesCancel = new ElementLocator(Locator.XPath, "(//input[@value='Cancel'])[1]"),
            btnNotesCancel = new ElementLocator(Locator.Id, "NoteWindowClearButton"),
            btnDetailLinesCancel = new ElementLocator(Locator.XPath, "(//input[@value='Cancel'])[1]"),

            //Dropdown lists
        lstPolicyPrintSelCrops = new ElementLocator(Locator.XPath, "//select[@id='selectedCrops']"),
            lstCropPlan = new ElementLocator(Locator.Id, "InsurancePlanCode"),
            lstCoverageLevel = new ElementLocator(Locator.Id, "CoverageLevelSelection"),

            //lstPriceElection = new ElementLocator(Locator.Id, "PriceElection"),
            lstUnitOptionCode = new ElementLocator(Locator.Id, "UnitStructureCode"),
            lstGrowerAgency = new ElementLocator(Locator.Id, "AgencyOID"),
            lstGrowerAgent = new ElementLocator(Locator.Id, "ServicingAgentOid"),
            lstGrowerSSN = new ElementLocator(Locator.XPath, "//ul[@id='TaxIDType_listbox']"),
            elmDdArrowGrowerSSN = new ElementLocator(Locator.XPath, "//*[@aria-owns='TaxIDType_listbox']//span[@class='k-icon k-i-arrow-60-down']"),
            lstGrowerEntityType = new ElementLocator(Locator.XPath, "//ul[@id='EntityType_listbox']"),
            elmDdArrowGrowerEntityType = new ElementLocator(Locator.XPath, "//*[@aria-owns='EntityType_listbox']//span[@class='k-icon k-i-arrow-60-down']"),
            lstCropPractice = new ElementLocator(Locator.Id, "PracticeCode"),
            lstCommCropStatus = new ElementLocator(Locator.Id, "CommodityCroppingTypeCode"),
            lstAcreageType = new ElementLocator(Locator.Id, "AcreageTypeCode"),
            lstPreYrProcCode = new ElementLocator(Locator.Id, "PreviousProcedureCode"),

            // checkboxes

            // labels
            // Labels Detail lines
            lblMaintainDetailLine = new ElementLocator(Locator.XPath, "//td[contains(text(),'Maintain Detail Line')]"),
            lblShareholderInfo = new ElementLocator(Locator.XPath, "//*[contains(@id,'LandIdPanelBarAPHCreate')]//*[contains(text(),'Shareholder Information')]"),
            lblLandIdentifiers = new ElementLocator(Locator.XPath, "//*[contains(text(),'Land Identifiers')]"),
            lblPlantedFields = new ElementLocator(Locator.XPath, "//*[contains(text(),'Planted Fields')]"),
            
            // --labels grower info
            lblPolicyGrowerEligibilityInfo = new ElementLocator(Locator.XPath, "//div[@class='a-banner a-banner-medium']//tr/td[contains(text(),'Eligibility Information')]"),
            lblPolicyGrowerTaxIdType = new ElementLocator(Locator.XPath, "//label[contains(text(),'Tax ID Type')]"),
            lblPolicyGrowerInfo = new ElementLocator(Locator.XPath, "//label[contains(text(),'First Name')]"),

            // labels policy balance
            lblPolicyBalanceHeader = new ElementLocator(Locator.XPath, "//*[contains(text(),'MPCI Policy Balance Summary')]"),
            lblPolicyBalanceGrowerName = new ElementLocator(Locator.XPath, "//div[contains(@id,'PolicyBalancePanelBar')]//label[@for='GrowerName' and contains(text(),'Grower')]"),
            lblPolicyBalanceGrossPremium = new ElementLocator(Locator.XPath, "//*[contains(text(),'Gross Premium')]"),
            lblPolicyBalanceTotalIndemity = new ElementLocator(Locator.XPath, "//*[contains(text(),'Total Indemnity')]"),

            // label policy utilities
            lblPolicyUtilities = new ElementLocator(Locator.XPath, "//div[contains(@id,'MpciPolicyTabStrip') and contains(@style,'display: block')]/div[@class='a-banner a-banner-dark']//td[contains(text(),'Utilities')]"),

            // SI / TI label Estimator
            lblPolicyEstimator = new ElementLocator(Locator.XPath, "//div[contains(@id,'MpciPolicyTabStrip') and contains(@style,'display: block')]/div[@class='a-banner a-banner-dark']//td[contains(text(),'View Estimates')]"),

            // Policy Tab Elements //span[text()='Detail Lines']
            elmTabPolicyCoverages = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Coverages']"),
            elmTabPolicyDetailLines = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Detail Lines']"),
            elmTabPolicyGrower = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Grower']"),
            elmTabPolicyPolicies = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Policies']"),
            elmTabPolicyBalance = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Policy Balance']"),
            elmTabPolicyClaims = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Claims']"),
            elmTabPolicyAttachments = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Attachments']"),
            elmTabPolicyChangeLog = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Change Log']"),
            elmTabPolicyNotes = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Notes']"),
            elmTabPolicyPrint = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Print']"),
            elmTabPolicyUtilities = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Utilities']"),

            // SI / TI specific
            elmTabPolicyEsitmator = new ElementLocator(Locator.XPath, "//li[contains(@aria-controls,'MpciPolicyTabStrip')]//span[text()='Estimator']"),
            elmTabPolicEstimatorTxtNoItems = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridAttachments')]/div/span[contains(text(),'No items to display')]"),

            // Text Edit Fields
            txtMpicPolicyNotes = new ElementLocator(Locator.XPath, "//textarea[@id='Body']"),
            txtBoxNotes = new ElementLocator(Locator.Id, "Body"),

            // Tables and Grids
            tblMpciDetailLineGrid = new ElementLocator(Locator.XPath, "//div[@id='MpciPremiumLineViewModelGrid']//table[@role='treegrid']/tbody"),
            tblHrdsMpciDetailLineGrid = new ElementLocator(Locator.XPath, "//div[@id='MpciPremiumLineViewModelGrid']//table[@role='treegrid']/thead"),
            tblMpciGrowerAddressGrid = new ElementLocator(Locator.XPath, "//div[@id='AddressGrid']//table[@role='grid']/tbody"),
            tblHdrsMpciGrowerAddressGrid = new ElementLocator(Locator.XPath, "//div[@id='AddressGrid']//table[@role='grid']/thead"),
            tblMpciGrowerSbiInfoGrid = new ElementLocator(Locator.XPath, "//div[@id='MpciSBIViewModelGrid']//table[@role='grid']/tbody"),
            tblHdrsMpciGrowerSbiInfoGrid = new ElementLocator(Locator.XPath, "//div[@id='MpciSBIViewModelGrid']//table[@role='grid']/thead"),
            tblMpciPoliciesGrid = new ElementLocator(Locator.XPath, "//div[@id='grid']//table/tbody"),
            tblHdrsMpciPoliciesGrid = new ElementLocator(Locator.XPath, "//div[@id='grid']//table/thead"),
            tblMpciPolicyBalanceTransactionGrid = new ElementLocator(Locator.XPath, "//div[@id='TransactionsGrid']//table/tbody"),
            tblHdrsMpciPolicyBalanceTransactionGrid = new ElementLocator(Locator.XPath, "//div[@id='TransactionsGrid']//table/thead"),
            tblMpciPolicyCliamsViewGrid = new ElementLocator(Locator.XPath, "//div[@id='MpciClaimViewModelGrid']//table/tbody"),
            tblHdrsMpciPolicyCliamsViewGrid = new ElementLocator(Locator.XPath, "//div[@id='MpciClaimViewModelGrid']//table/thead"),
            tblMpciPolicyAttachmentsViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridAttachments')]//table/tbody"),
            tblHdrsMpciPolicyAttachmentsViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridAttachments')]//table/thead"),
            tblMpciPolicyChangeLogViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridTransactionLog')]//table/tbody"),
            tblHdrsMpciPolicyChangeLogViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridTransactionLog')]//table/thead"),
            tblMpciPolicyNotesViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridNotes')]//table/tbody"),
            tblHdrsMpciPolicyNotesViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'gridNotes')]//table/thead"),
            tblMpciPolicyPrintRptViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'reportSelectListGridDiv')]//table/tbody"),
            tblHdrsMpciPolicyPrintRptViewGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'reportSelectListGridDiv')]//table/thead"),
            tblMpciPolicyUtilitiesGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'ConflictReportingInformationGrid')]//table/tbody"),
            tblHdrsMpciPolicyUtilitiesGrid = new ElementLocator(Locator.XPath, "//div[contains(@id,'ConflictReportingInformationGrid')]//table/thead"),
            lnkTblCoveragesCountyR1 = new ElementLocator(Locator.XPath, "//*[@id='MpciCoverageViewModelGrid']/table/tbody/tr[1]/td[2]/a"),
            lnkRegUnitNumber = new ElementLocator(Locator.XPath, "//*[contains(text(),'0001-0002-BU')]"),
            lnkSIUnitNumber = new ElementLocator(Locator.XPath, "//*[contains(text(),'9999-0000-BU')]"),
            lnkTIUnitNumber = new ElementLocator(Locator.XPath, "//*[contains(text(),'0001-0000-BU')]"),
            tblHdrsShareHolderInfoGrid = new ElementLocator(Locator.XPath, "//div[@id='PremiumLineShareholderViewModelGrid']//table[@role='grid']//thead[@role='rowgroup']"),
            tblShareHolderInfoGrid = new ElementLocator(Locator.XPath, "//div[@id='PremiumLineShareholderViewModelGrid']//table[@role='grid']//tbody[@role='rowgroup']"),
            tblHdrsLandIdentifiers = new ElementLocator(Locator.XPath, "//*[@id='LandIdDescriptionGrid2']/table/thead"),
            tblLandIdentifiersGrid = new ElementLocator(Locator.XPath, "//*[contains(@id,'LandIdDescriptionGrid2')]//table[@role='grid']//tbody[@role='rowgroup']"),
            tblHdrsPlantedFields = new ElementLocator(Locator.XPath, "//*[contains(@id,'PlantedCluGrid')]//table[@role='grid']"),

            // SI / TI Specific
            tblMpciPolicyEstimator = new ElementLocator(Locator.XPath, "//div[contains(@id,'EstimateGridViewModel')]//table/tbody"),
            tblHdrsMpciPolicyEstimator = new ElementLocator(Locator.XPath, "//div[contains(@id,'EstimateGridViewModel')]//table/thead"),
            tblMpciPolicyEstimatorManageTemp = new ElementLocator(Locator.XPath, "//div[@id='MpciFullEstimateTemplateGridViewModel']//table/tbody"),
            tblHdrsMpciPolicyEstimatorManageTemp = new ElementLocator(Locator.XPath, "//div[@id='MpciFullEstimateTemplateGridViewModel']//table/thead"),
            tblMpciPolicyEstimatorAttachments = new ElementLocator(Locator.XPath, "//div[@id='MpciEstimatesDiv']//div[contains(@id,'EstimateAttachmentsDiv')]//table[@role='grid']/tbody"),
            // "//div[@id='MpciEstimatesDiv']//div[contains(@id,'EstimateAttachmentsDiv')]//table[@role='grid']/thead
            tblHdrsMpciPolicyEstimatorAttachments = new ElementLocator(Locator.XPath, "//div[@id='MpciEstimatesDiv']//div[contains(@id,'EstimateAttachmentsDiv')]//table[@role='grid']/thead"),

        //*****End locator not used
        zEndLocator = new ElementLocator(Locator.XPath, "//notused[@id='notused']");

        public PolicyMpci(DriverContext driverContext)
            : base(driverContext)
        {
            // Set ElementLocator Logical Names
            this.SetLogNames(this);
        }



        /// <summary>
        /// Click MPCI Policy Tabs, Verify table displayed, headers displayed, row count > 0
        /// </summary>
        /// <param name="tblName">Logical Name of Table being Verified</param>
        /// <param name="locArray">ElementLocator array[0-2] holds elements for steps</param>
        private void VerifyTblHdrsRow(string tblName, ElementLocator[] locArray)
        {
            DriverContext dc = this.DriverContext;
            if (locArray[0] != null)
            {
                RsltLogger.LogStep("Click Tab: " + locArray[0].Value);
                dc.Driver.GetElement(locArray[0], dc).JavaScriptClick();
                Thread.Sleep(500);
            }
            
            ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
            if (dc.Driver.GetElement(locArray[1], dc).WaitTillDisplayed(locArray[1]))
            {
                // pass
                RsltLogger.LogPass(locArray[1].Value + " Is Displayed");
            }
            else
            {
                // fail
                RsltLogger.LogFail(locArray[1].Value + " Is NOT Displayed");
            }

            if (dc.Driver.GetElement(locArray[2], dc).WaitTillDisplayed(locArray[2]))
            {
                // pass
                RsltLogger.LogPass(locArray[2].Value + " Is Displayed");
            }
            else
            {
                // fail
                RsltLogger.LogFail(locArray[2].Value + " Is NOT Displayed");
            }

            string xpathRowsTblFromsDocs = locArray[1].Value + "//tr";
            IList<IWebElement> tblRows = dc.Driver.FindElements(By.XPath(xpathRowsTblFromsDocs));
            if (tblRows.Count <= 0)
            {
                // Expect row count > 0
                string msgVerifyFail = "Verify " + tblName + " Row Count Exp: > 0 Act: " + tblRows.Count.ToString();
                RsltLogger.LogFail(msgVerifyFail);
            }
            else
            {
                string msgVerifyPass = "Verify " + tblName + " Row Count Exp: > 0 Act: " + tblRows.Count.ToString();
                RsltLogger.LogPass(msgVerifyPass);
            }
        }

        /// <summary>
        /// Click MPCI Policy Tab,s Verify table exist and Table Headers displayed
        /// </summary>
        /// <param name="locArray">ElementLocator array[0-2] holds elements for steps</param>
        private void VerifyTblHdrs(ElementLocator[] locArray)
        {
            DriverContext dc = this.DriverContext;
            if (locArray[0] != null)
            {
                RsltLogger.LogStep("Click Tab " + locArray[0].Value);
                dc.Driver.GetElement(locArray[0], dc).JavaScriptClick();
                Thread.Sleep(500);
            }

            ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
            if (dc.Driver.GetElement(locArray[2], dc).WaitTillDisplayed(locArray[2]))
            {
                // pass
                RsltLogger.LogPass(locArray[2].Value + " Is Displayed");
            }
            else
            {
                // fail
                RsltLogger.LogFail(locArray[2].Value + " Is NOT Displayed");
            }

            if (dc.Driver.GetElement(locArray[1], dc) != null)
            {
                //pass
                RsltLogger.LogPass(locArray[1].Value + " Found");
            }
            else
            {
                // fail
                RsltLogger.LogFail(locArray[1].Value + " NOT Found");
            }
        }

        /// <summary>
        /// Verify PAW Navigating MPCI Policy Tabs
        /// </summary>
        #region Old Code Navigate to MPCI Policy tabs for UAT
        // // -- Details Tab rows all environments
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabDetails", "verify MPCI Policy Nav Tab Details check tables found & rows > 0");
        // locStepArray[0] = this.elmTabPolicyDetailLines;
        // locStepArray[1] = this.tblMpciDetailLineGrid;
        // locStepArray[2] = this.tblHrdsMpciDetailLineGrid;
        // this.VerifyTblHdrsRow("tblMpciDetailLineGrid", locStepArray);

        // // -- Grower Tab
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabGrowersLabels", "Click Growers Tab verify details table Address Grid and Grower Sbi");
        // dc.Driver.GetElement(elmTabPolicyGrower, dc).JavaScriptClick();
        // ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

        // // --Verify Grower Eligibility Information
        // if (dc.Driver.GetElement(lblPolicyGrowerEligibilityInfo, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerEligibilityInfo.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerEligibilityInfo.Value + " NOT Displayed");
        // }

        // // label Grower Information Tax Type ID
        // if (dc.Driver.GetElement(lblPolicyGrowerTaxIdType, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerTaxIdType.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerTaxIdType.Value + " NOT Displayed");
        // }

        // // label Grower Information First Name
        // if (dc.Driver.GetElement(lblPolicyGrowerInfo, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerInfo.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerInfo.Value + " NOT Displayed");
        // }

        // // Growers Address Grid table rows > 0
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabGrowersAddressGrid", "verify MPCI Policy Nav Tab Growers check Address Grid found & rows > 0");
        // locStepArray[0] = null;
        // locStepArray[1] = this.tblMpciGrowerAddressGrid;
        // locStepArray[2] = this.tblHdrsMpciGrowerAddressGrid;
        // this.VerifyTblHdrsRow("tblMpciGrowerAddressGrid", locStepArray);

        // // --Verify Grower Sbi grid row
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabGrowersSbiInfoGrid", "verify MPCI Policy Nav Tab Growers check Address Grid found & rows > 0");
        // locStepArray[0] = null;
        // locStepArray[1] = this.tblMpciGrowerSbiInfoGrid;
        // locStepArray[2] = this.tblHdrsMpciGrowerSbiInfoGrid;
        // this.VerifyTblHdrsRow("tblMpciGrowerSbiInfoGrid", locStepArray);

        // // Click Claims Tab and verify claims table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabClaims", "verify MPCI Policy Nav Tab Claims check Claims View Grid found & rows > 0");
        // locStepArray[0] = this.elmTabPolicyClaims;
        // locStepArray[1] = this.tblMpciPolicyCliamsViewGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyCliamsViewGrid;
        // this.VerifyTblHdrsRow("tblMpciPolicyCliamsViewGrid", locStepArray);

        // // Click Attachments Tab and verify Attachments Table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabAttachments", "verify MPCI Policy Nav Tab Attachments check Attachments View Grid found & rows > 0");
        // locStepArray[0] = this.elmTabPolicyAttachments;
        // locStepArray[1] = this.tblMpciPolicyAttachmentsViewGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyAttachmentsViewGrid;
        // this.VerifyTblHdrsRow("tblMpciPolicyAttachmentsViewGrid", locStepArray);

        // // Click Change Log Tab and verify Change Log Table
        // // TI UAT Good
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabChangeLog", "verify MPCI Policy Nav Tab Change Log check Change Log View Grid found & rows > 0");
        // locStepArray[0] = this.elmTabPolicyChangeLog;
        // locStepArray[1] = this.tblMpciPolicyChangeLogViewGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyChangeLogViewGrid;
        // this.VerifyTblHdrsRow("tblMpciPolicyChangeLogViewGrid", locStepArray);

        // // Click Notes Tab and Verify Notes Labels
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabNotesLabels", "Click Notes Tab and Verify Label Notes Exists");
        // dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyNotes.Value);
        // dc.Driver.GetElement(elmTabPolicyNotes, dc).JavaScriptClick();
        // ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        // if (dc.Driver.GetElement(txtMpicPolicyNotes, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(txtMpicPolicyNotes.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(txtMpicPolicyNotes.Value + " NOT Displayed");
        // }

        // // Verify Notes Table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabNotesGrid", "verify MPCI Policy Nav Tab Notes check Notes View Grid found & rows > 0");
        // locStepArray[0] = null;
        // locStepArray[1] = this.tblMpciPolicyNotesViewGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyNotesViewGrid;
        // this.VerifyTblHdrsRow("tblMpciPolicyNotesViewGrid", locStepArray);

        // // Click Print Tab and Verify Elements
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPrintElemntsExist", "Click Print Tab and Verify Elements and Print Report Table");
        // dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyPrint.Value);
        // dc.Driver.GetElement(elmTabPolicyPrint, dc).JavaScriptClick();
        // ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        // if (dc.Driver.GetElement(lstPolicyPrintSelCrops, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " NOT Displayed");
        // }

        // //Verify Print Report Table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPrintGrid", "verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
        // locStepArray[0] = null;
        // locStepArray[1] = this.tblMpciPolicyPrintRptViewGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyPrintRptViewGrid;
        // this.VerifyTblHdrsRow("tblMpciPolicyPrintRptViewGrid", locStepArray);

        // // Click Utilities Tab and verify Elements 
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabUtilitiesElements", "Click Utilities Tab and verify Elements ");
        // dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyUtilities.Value);
        // dc.Driver.GetElement(elmTabPolicyUtilities, dc).JavaScriptClick();
        // ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        // if (dc.Driver.GetElement(lblPolicyUtilities, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " NOT Displayed");
        // }

        // // verify  Utilities table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found & rows > 0");
        // locStepArray[0] = null;
        // locStepArray[1] = this.tblMpciPolicyUtilitiesGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyUtilitiesGrid;
        // this.VerifyTblHdrs( locStepArray); 

        // // Click Coverages Tab and verify Coverages Table
        // // Note below tblPolicyMpciCoverage is passed and is shared table as is Home page mapping
        // HomePage proAgWorksHome = new HomePage(dc);
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabCoverages", "verify MPCI Policy Nav Tab Coverages check Coverages View Grid found & rows > 0");
        // locStepArray[0] = this.elmTabPolicyCoverages;
        // locStepArray[1] = proAgWorksHome.tblPolicyMpciCoverage;
        // locStepArray[2] = proAgWorksHome.tblHdrsPolicyMpciCoverage;
        // this.VerifyTblHdrsRow("tblPolicyMpciCoverage", locStepArray);
        //// dc.Driver.GetElement(tblCoveragesCounty, dc).JavaScriptClick();
        //// dc.Driver.GetElement(btnEdit, dc).JavaScriptClick();
        //// IWebElement element = dc.Driver.GetElement(lstCropPlan, dc);
        //// SelectElement select = new SelectElement(element);
        //// IList<IWebElement> options = select.Options;
        ////if (options.Count > 0)
        //// {
        ////     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Crop Plan Dropdown has data");
        //// }
        //// else
        //// {
        ////     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Crop Plan Dropdown doesn't have any data");
        //// }




        // // Click Policies tab double wait time because policies tab needs to load all policies for grower
        // // Verify Policies Table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");
        // locStepArray[0] = this.elmTabPolicyPolicies;
        // locStepArray[1] = this.tblMpciPoliciesGrid;
        // locStepArray[2] = this.tblHdrsMpciPoliciesGrid;
        // this.VerifyTblHdrsRow("tblMpciPoliciesGrid", locStepArray);

        // // Click Policy Balance Tab and Verify Elements and Policy Balance Table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicyBalanceElements", "Click Policy Balance Tab and Verify Elements");
        // dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyBalance.Value);
        // dc.Driver.GetElement(elmTabPolicyBalance, dc).JavaScriptClick();
        // ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

        // // Label Policy Balance Header
        // if (dc.Driver.GetElement(lblPolicyBalanceHeader, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " NOT Displayed");
        // }

        // // Label Policy Balance Gross Premium 
        // if (dc.Driver.GetElement(lblPolicyBalanceGrossPremium, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " NOT Displayed");
        // }

        // // Label Policy Balance Total Indemity
        // if (dc.Driver.GetElement(lblPolicyBalanceTotalIndemity, dc).WaitTillDisplayed())
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " Displayed");
        // }
        // else
        // {
        //     dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " NOT Displayed");
        // }

        // // Verify Policy Balance Table
        // dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
        // locStepArray[0] = null;
        // locStepArray[1] = this.tblMpciPolicyBalanceTransactionGrid;
        // locStepArray[2] = this.tblHdrsMpciPolicyBalanceTransactionGrid;
        // this.VerifyTblHdrsRow("tblMpciPolicyBalanceTransactionGrid", locStepArray);
        //// Click Print Tab and Verify Elements
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPrintElemntsExist", "Click Print Tab and Verify Elements and Print Report Table");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyPrint.Value);
        //dc.Driver.GetElement(elmTabPolicyPrint, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lstPolicyPrintSelCrops, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " NOT Displayed");
        //}

        ////Verify Print Report Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPrintGrid", "verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyPrintRptViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyPrintRptViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyPrintRptViewGrid", locStepArray);



        //// Click Utilities Tab and verify Elements 
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabUtilitiesElements", "Click Utilities Tab and verify Elements ");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyUtilities.Value);
        //dc.Driver.GetElement(elmTabPolicyUtilities, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lblPolicyUtilities, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " NOT Displayed");
        //}

        //// verify  Utilities table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyUtilitiesGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyUtilitiesGrid;
        //this.VerifyTblHdrs(locStepArray);
        //// Click Policies tab double wait time because policies tab needs to load all policies for grower
        //// Verify Policies Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyPolicies;
        //locStepArray[1] = this.tblMpciPoliciesGrid;
        //locStepArray[2] = this.tblHdrsMpciPoliciesGrid;
        //this.VerifyTblHdrsRow("tblMpciPoliciesGrid", locStepArray);

        //// Click Policy Balance Tab and Verify Elements and Policy Balance Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicyBalanceElements", "Click Policy Balance Tab and Verify Elements");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyBalance.Value);
        //dc.Driver.GetElement(elmTabPolicyBalance, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

        //// Label Policy Balance Header
        //if (dc.Driver.GetElement(lblPolicyBalanceHeader, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " NOT Displayed");
        //}

        //// Label Policy Balance Gross Premium 
        //if (dc.Driver.GetElement(lblPolicyBalanceGrossPremium, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " NOT Displayed");
        //}

        //// Label Policy Balance Total Indemity
        //if (dc.Driver.GetElement(lblPolicyBalanceTotalIndemity, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " NOT Displayed");
        //}

        //// Verify Policy Balance Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyBalanceTransactionGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyBalanceTransactionGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyBalanceTransactionGrid", locStepArray);




        #endregion

        private void VerifyPolicMPCINavTabsTblsForPAW()
        {
            ElementLocator[] locStepArray = new ElementLocator[3];
            DriverContext dc = this.DriverContext;

            // New Detail lines tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyDetailLines, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyDetailLines, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHrdsMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToMPCIPolicyDetailLinesTab", "Navigate to MPCI Policy Detail Lines tab");

            // New Verify Detail Lines Grid and Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHrdsMpciDetailLineGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyDetailLinesHdrsAndRowCntGtZero", "Verify Detail Lines Headers and Row Count greater than 0");

            // Clicking on Unit Number
            dc.ElmLocCmds.AddToListLocElmCmd(lnkRegUnitNumber, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lnkRegUnitNumber, LocatorCmd.JsClick);
            dc.NavCmdsExecute("ClickingonUnitNumber", "Clicking on Unit Number on Detail Lines tab");

            // Verifying the Maintain Detail Line Page
            dc.ElmLocCmds.AddToListLocElmCmd(lblMaintainDetailLine, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPractice, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPractice, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCommCropStatus, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCommCropStatus, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstAcreageType, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstAcreageType, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPreYrProcCode, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPreYrProcCode, LocatorCmd.VerifyListCountGtZero);
            dc.VerifyElemLocCmds("VerifyDropdownMenus", "Verify Dropdown Menus on Maintain Detail Line page");

            // Clicking on ShareHolder Information
            dc.ElmLocCmds.AddToListLocElmCmd(lblShareholderInfo, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblShareholderInfo, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsShareHolderInfoGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickingonShareHolderInfo", "Clicking on ShareHolder Info");

            //Verify ShareHolder Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsShareHolderInfoGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("VerifyShareHolderInfoHdrs", "Verify ShareHolder Information Headers");

            // Clicking & Verify Land Identifiers
            dc.ElmLocCmds.AddToListLocElmCmd(lblLandIdentifiers, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblLandIdentifiers, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsLandIdentifiers, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblLandIdentifiersGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickingonLandIdentifiers", "Clicking on Land Identifiers");

            // Verify Land Identifiers Hdrs and Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsLandIdentifiers, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblLandIdentifiersGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("ClickAndVerifyLandIdentifiersGrid", "Clicking and Verifying Land Identifiers Hdrs and Grid");

            //Click on Planted Fields
            dc.ElmLocCmds.AddToListLocElmCmd(lblPlantedFields, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPlantedFields, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsPlantedFields, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickingonPlantedFields", "Clicking on Planted Fields");

            // Verify Planted Fields Hdrs
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsPlantedFields, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("ClickAndVerifyPlantedFields", "Clicking and Verifying Planted Fields Hdrs");

            // Click on Cancel to go back to Detail Lines Page
            dc.ElmLocCmds.AddToListLocElmCmd(btnDetailLinesCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickCancelToReturnToDetailLinesPage", "Clicking on Cancel to go back to Detail Lines Page");
           

            // New Grower Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyGrower, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyGrower, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerEligibilityInfo, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerTaxIdType, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerInfo, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToMPCIPolicyGrowerTab", "Navigate to MPCI Policy Grower tab");

            // New Verify Grower Tab
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciGrowerAddressGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciGrowerAddressGrid, LocatorCmd.TableRowCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciGrowerSbiInfoGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciGrowerSbiInfoGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyMPCIGrowerAddGridAndMPCIGrowerSBIGrid", "Verify MPCI Address Grid and SBi Grid");

            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerEdit, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerEdit, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToGrowerEditPage", "Navigate to Grower Edit Page");

            // Verify Grower Dropdown menus
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgent, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(elmDdArrowGrowerSSN, LocatorCmd.JsClick); 
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerSSN, LocatorCmd.VerifyOpenListGtZero, "true");
            dc.ElmLocCmds.AddToListLocElmCmd(elmDdArrowGrowerEntityType, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerEntityType, LocatorCmd.VerifyOpenListGtZero, "true");
            dc.VerifyElemLocCmds("VerifyDropdownsOnGrowerEditPage", "Verify Dropdown Menus on Grower Edit Page");

            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerEligibilityInfo, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavBackToGrowerPage", "Navigate back to Grower Page");
                                 
            // New Claims Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyClaims, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyClaims, LocatorCmd.JsClick);
            dc.NavCmdsExecute("NavToClaimsTab", "Navigate to Claims Tab");

            // Verify View MPCI Claims Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyCliamsViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyCliamsViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyMPCIPolicyClaimsViewGrid", "Verify MPCI Policy Claims Grid");

            // New Attachments Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyAttachments, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyAttachments, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyAttachmentsViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToAttachmentsTab", "Navigate to Attachments Tab");

            // Verify New Attachments Tab
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyAttachmentsViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyAttachmentsViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyMPCIPolicyAttachmentsViewGrid", "Verify MPCI Policy Claims Attachments View Grid");

            // New Change Log Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyChangeLog, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyChangeLog, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyChangeLogViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToChangeLogTab", "Navigate to Change Log Tab");

            //Verify New Change Log
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyChangeLogViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyChangeLogViewGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("VerifyMPCIPolicyChangeLogViewGrid", "Verify MPCI Policy Claims Attachments View Grid");

            // New Notes Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyNotes, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyNotes, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(txtMpicPolicyNotes, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToNotesTab", "Navigate to Notes Tab");

            // Verify New Notes table
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyNotesViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyNotesViewGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("VerifyNotesTab", "Verify Headers MPCI Policy Notes View Grid");

            // Adding text in the Notes and clicking Cancel to clear the text
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.SetAttribute, "value|Test");
            dc.NavCmdsExecute("AddingNotes", "Adding Notes in Notes tab");

            // Verify text in Notes is cleared
            // Verify Input Text Test Exists before clicking clear
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.GetAttributeContains, "false|value|Test");
            dc.ElmLocCmds.AddToListLocElmCmd(btnNotesCancel, LocatorCmd.JsClick); // clear Text
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.GetAttributeContains, "false|value|");
            dc.VerifyElemLocCmds("VerifyNotesCleared", "Verify that Notes is cleared on Cancel");

            // New Print Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPrint, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPrint, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPolicyPrintSelCrops, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToPrintTab", "Navigate to Print tab");

            // New Verify Print Tab
            dc.ElmLocCmds.AddToListLocElmCmd(lstPolicyPrintSelCrops, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyPrintRptViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyPrintRptViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyNavTabPrintGrid", "Verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
                       
            // Click on Utilities Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyUtilities, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyUtilities, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyUtilities, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToUtilitiesTab", "Navigate to Utilities tab");

            // Verify Utilities table
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyUtilitiesGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyUtilitiesGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found");
                                                                                                                        
            // New Coverages Tab Code
            HomePage proAgWorksHome = new HomePage(dc);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyCoverages, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyCoverages, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToCoveragesTab", "Navigate to Coverages tab");

            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblPolicyMpciCoverage, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyMPCIPolicyCoverage", "Verify MPCI Policy Coverage");

            // New Navigating to Coverages Edit Page
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTblCoveragesCountyR1, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTblCoveragesCountyR1, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesEdit, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesEdit, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPlan, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToEditPage", "Navigate to Edit Page");

            //New Verifying the Coverages Edit Page
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPlan, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCoverageLevel, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstUnitOptionCode, LocatorCmd.VerifyListCountGtZero);
            dc.VerifyElemLocCmds("VerifyDropdownMenus", "Verify Drop down Menus on Coverages Edit Page ");

            // Navigating back to Coverages tab
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesCancel, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavBackToCoveragesTab", "Navigate back to Coverages tab");

            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("VerifyCoveragesTab", "Verify Coverages table");

            // New Policies tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPolicies, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPolicies, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPoliciesGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToPoliciesTab", "Navigate to Policies tab");

            // New Verify MPCI Policies Grid & Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPoliciesGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPoliciesGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");

            // New Policy Balance
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyBalance, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyBalance, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceHeader, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("NavToPolicyBalanceTab", "Navigate to Policy Balance tab");

            // Click Policies tab double wait time because policies tab needs to load all policies for grower
            // New Verify Policy Balance Table
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceHeader, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceGrossPremium, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceTotalIndemity, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyBalanceTransactionGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyBalanceTransactionGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
            
        }

        /// <summary>
        /// Verify SI-PAW Navigating MPCI Policy Tabs
        /// </summary>
        private void VerifyPolicMPCINavTabsTblsForSI()
        {
            ElementLocator[] locStepArray = new ElementLocator[3];
            DriverContext dc = this.DriverContext;

            #region Old Detail Lines tab Steps
            //// -- Details Tab rows all environments
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabDetails", "verify MPCI Policy Nav Tab Details check tables found & rows > 0");
            //locStepArray[0] = this.elmTabPolicyDetailLines;
            //locStepArray[1] = this.tblMpciDetailLineGrid;
            //locStepArray[2] = this.tblHrdsMpciDetailLineGrid;
            //// Old Code
            //this.VerifyTblHdrsRow("tblMpciDetailLineGrid", locStepArray);
            // -- Grower Tab
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabGrowersLabels", "Click Growers Tab verify details table Address Grid and Grower Sbi");
            //dc.Driver.GetElement(elmTabPolicyGrower, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

            //// --Verify Grower Eligibility Information
            //if (dc.Driver.GetElement(lblPolicyGrowerEligibilityInfo, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerEligibilityInfo.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerEligibilityInfo.Value + " NOT Displayed");
            //}

            //// label Grower Information Tax Type ID
            //if (dc.Driver.GetElement(lblPolicyGrowerTaxIdType, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerTaxIdType.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerTaxIdType.Value + " NOT Displayed");
            //}

            //// label Grower Information First Name
            //if (dc.Driver.GetElement(lblPolicyGrowerInfo, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerInfo.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerInfo.Value + " NOT Displayed");
            //}

            //// Growers Address Grid table rows > 0
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabGrowersAddressGrid", "verify MPCI Policy Nav Tab Growers check Address Grid found & rows > 0");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciGrowerAddressGrid;
            //locStepArray[2] = this.tblHdrsMpciGrowerAddressGrid;
            //this.VerifyTblHdrsRow("tblMpciGrowerAddressGrid", locStepArray);


            //// --Verify Grower Sbi grid row
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabGrowersSbiInfoGrid", "verify MPCI Policy Nav Tab Growers check Address Grid found");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciGrowerSbiInfoGrid;
            //locStepArray[2] = this.tblHdrsMpciGrowerSbiInfoGrid;
            //this.VerifyTblHdrs(locStepArray);

            //// Click Claims Tab and verify claims table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabClaims", "verify MPCI Policy Nav Tab Claims check Claims View Grid found & rows > 0");
            //locStepArray[0] = this.elmTabPolicyClaims;
            //locStepArray[1] = this.tblMpciPolicyCliamsViewGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyCliamsViewGrid;
            //this.VerifyTblHdrsRow("tblMpciPolicyCliamsViewGrid", locStepArray);

            //// Click Attachments Tab and verify Attachments Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabAttachments", "verify MPCI Policy Nav Tab Attachments check Attachments View Grid found & rows > 0");
            //locStepArray[0] = this.elmTabPolicyAttachments;
            //locStepArray[1] = this.tblMpciPolicyAttachmentsViewGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyAttachmentsViewGrid;
            //this.VerifyTblHdrsRow("tblMpciPolicyAttachmentsViewGrid", locStepArray);

            //// Click Change Log Tab and verify Change Log Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabChangeLog", "verify MPCI Policy Nav Tab Change Log check Change Log View Grid found & rows > 0");
            //locStepArray[0] = this.elmTabPolicyChangeLog;
            //locStepArray[1] = this.tblMpciPolicyChangeLogViewGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyChangeLogViewGrid;
            //this.VerifyTblHdrsRow("tblMpciPolicyChangeLogViewGrid", locStepArray);

            //// Click Notes Tab and Verify Notes Labels
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabNotesLabels", "Click Notes Tab and Verify Label Notes Exists");
            //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyNotes.Value);
            //dc.Driver.GetElement(elmTabPolicyNotes, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
            //if (dc.Driver.GetElement(txtMpicPolicyNotes, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(txtMpicPolicyNotes.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(txtMpicPolicyNotes.Value + " NOT Displayed");
            //}

            //// Verify Notes Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabNotesGrid", "verify MPCI Policy Nav Tab Notes check Notes View Grid found & rows > 0");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyNotesViewGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyNotesViewGrid;
            //this.VerifyTblHdrsRow("tblMpciPolicyNotesViewGrid", locStepArray);

            // Click Coverages Tab and verify Coverages Table
            // Note below tblPolicyMpciCoverage is passed and is shared table as is Home page mapping
            //HomePage proAgWorksHome = new HomePage(dc);
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabCoverages", "verify MPCI Policy Nav Tab Coverages check Coverages View Grid found & rows > 0");
            //locStepArray[0] = this.elmTabPolicyCoverages;
            //locStepArray[1] = proAgWorksHome.tblPolicyMpciCoverage;
            //locStepArray[2] = proAgWorksHome.tblHdrsPolicyMpciCoverage;
            //this.VerifyTblHdrsRow("tblPolicyMpciCoverage", locStepArray);

            //// Verify Edit Mode here
            //dc.Driver.GetElement(tblCoveragesCounty, dc).JavaScriptClick();
            //dc.Driver.GetElement(btnCoveragesEdit, dc).JavaScriptClick();
            //IWebElement cropplan = dc.Driver.GetElement(lstCropPlan, dc);
            //SelectElement selectCp = new SelectElement(cropplan);
            //IList<IWebElement> optionsCp = selectCp.Options;
            //if (optionsCp.Count > 0)
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Crop Plan Dropdown has data");

            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Crop Plan Dropdown doesn't have any data");

            //}
            //IWebElement coveragelevel = dc.Driver.GetElement(lstCoverageLevel, dc);
            //SelectElement selectCl = new SelectElement(coveragelevel);
            //IList<IWebElement> optionsCl = selectCl.Options;
            //if (optionsCl.Count > 0)
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Coverage Level Dropdown has data");

            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Coverage Level Dropdown doesn't have any data");

            //}
            ////IWebElement priceelection = dc.Driver.GetElement(lstPriceElection, dc);
            ////SelectElement selectPe = new SelectElement(priceelection);
            ////IList<IWebElement> optionsPe = selectPe.Options;
            ////if (optionsPe.Count > 0)
            ////{
            ////    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Price Election Dropdown has data");
            ////}
            ////else
            ////{
            ////    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Price Election Dropdown doesn't have any data");

            ////}
            //IWebElement unitoptioncode = dc.Driver.GetElement(lstUnitOptionCode, dc);
            //SelectElement selectUoc = new SelectElement(unitoptioncode);
            //IList<IWebElement> optionsUoc = selectUoc.Options;
            //if (optionsUoc.Count > 0)
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Unit Option code Dropdown has data");

            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Unit Option code Dropdown doesn't have any data");

            //}
            //dc.Driver.GetElement(btnCancel, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

            //// Click Print Tab and Verify Elements
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPrintElemntsExist", "Click Print Tab and Verify Elements and Print Report Table");
            //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyPrint.Value);
            //dc.Driver.GetElement(elmTabPolicyPrint, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
            //if (dc.Driver.GetElement(lstPolicyPrintSelCrops, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " NOT Displayed");
            //}

            ////Verify Print Report Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPrintGrid", "verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyPrintRptViewGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyPrintRptViewGrid;
            //this.VerifyTblHdrsRow("tblMpciPolicyPrintRptViewGrid", locStepArray);
            //// Click Utilities Tab and verify Elements 
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabUtilitiesElements", "Click Utilities Tab and verify Elements ");
            //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyUtilities.Value);
            //dc.Driver.GetElement(elmTabPolicyUtilities, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
            //if (dc.Driver.GetElement(lblPolicyUtilities, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " NOT Displayed");
            //}

            //// verify  Utilities table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyUtilitiesGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyUtilitiesGrid;
            //this.VerifyTblHdrs(locStepArray); 
            //// SI and TI Only Tab Estimator Labels
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorLabels", "Click Estimator Tab and verify Label");
            //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyEsitmator.Value);
            //dc.Driver.GetElement(elmTabPolicyEsitmator, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
            //if (dc.Driver.GetElement(lblPolicyEstimator, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyEstimator.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyEstimator.Value + " NOT Displayed");
            //}

            //// SI and TI Only Tab Estimator Tables
            ////tblMpciPolicyEstimator
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorGrid", "verify MPCI Policy Nav Tab Estimator check Estimator View Grid found ");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyEstimator;
            //locStepArray[2] = this.tblHdrsMpciPolicyEstimator;
            //this.VerifyTblHdrs(locStepArray); 

            ////tblMpciPolicyEstimatorManageTemp
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorMangeTemp", "verify MPCI Policy Nav Tab Estimator check Estimator Manage Template View Grid found");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyEstimatorManageTemp;
            //locStepArray[2] = this.tblHdrsMpciPolicyEstimatorManageTemp;
            //this.VerifyTblHdrs(locStepArray); 

            ////tblMpciPolicyEstimatorAttachments
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorAttachments", "verify MPCI Policy Nav Tab Estimator check Estimator Attachments View Grid found");
            // locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyEstimatorAttachments;
            //locStepArray[2] = this.tblHdrsMpciPolicyEstimatorAttachments;
            //this.VerifyTblHdrs(locStepArray);
            //// Verify Policies Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");
            //locStepArray[0] = this.elmTabPolicyPolicies;
            //locStepArray[1] = this.tblMpciPoliciesGrid;
            //locStepArray[2] = this.tblHdrsMpciPoliciesGrid;
            //this.VerifyTblHdrsRow("tblMpciPoliciesGrid", locStepArray);

            //// Click Policy Balance Tab and Verify Elements and Policy Balance Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPolicyBalanceElements", "Click Policy Balance Tab and Verify Elements");
            //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyBalance.Value);
            //dc.Driver.GetElement(elmTabPolicyBalance, dc).JavaScriptClick();
            //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

            //// Label Policy Balance Header
            //if (dc.Driver.GetElement(lblPolicyBalanceHeader, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " NOT Displayed");
            //}

            //// Label Policy Balance Gross Premium 
            //if (dc.Driver.GetElement(lblPolicyBalanceGrossPremium, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " NOT Displayed");
            //}

            //// Label Policy Balance Total Indemity
            //if (dc.Driver.GetElement(lblPolicyBalanceTotalIndemity, dc).WaitTillDisplayed())
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " Displayed");
            //}
            //else
            //{
            //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " NOT Displayed");
            //}

            //// Verify Policy Balance Table
            //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
            //locStepArray[0] = null;
            //locStepArray[1] = this.tblMpciPolicyBalanceTransactionGrid;
            //locStepArray[2] = this.tblHdrsMpciPolicyBalanceTransactionGrid;
            //this.VerifyTblHdrsRow("tblMpciPolicyBalanceTransactionGrid", locStepArray);

            #endregion Old Detail Lines tab Steps

            // New MPCI Policy Detail Lines Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyDetailLines, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyDetailLines, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHrdsMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToMPCIPolicyDetailLinesTab", "Navigate to MPCI Policy Detail Lines tab");

            // New Verify MPCI Detail lines Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHrdsMpciDetailLineGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyDetailLinesHdrsAndRowCntGtZero", "Verify Detail Lines Headers and Row Count greater than 0");

            // Clicking on Unit Number
            dc.ElmLocCmds.AddToListLocElmCmd(lnkSIUnitNumber, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lnkSIUnitNumber, LocatorCmd.JsClick);
            dc.NavCmdsExecute("ClickingonUnitNumber", "Clicking on Unit Number on Detail Lines tab");

            // Verifying the Maintain Detail Line Page
            dc.ElmLocCmds.AddToListLocElmCmd(lblMaintainDetailLine, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPractice, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPractice, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCommCropStatus, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCommCropStatus, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstAcreageType, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstAcreageType, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPreYrProcCode, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPreYrProcCode, LocatorCmd.VerifyListCountGtZero);
            dc.VerifyElemLocCmds("VerifyDropdownMenus", "Verify Dropdown Menus on Maintain Detail Line page");

            // Clicking on ShareHolder Information
            dc.ElmLocCmds.AddToListLocElmCmd(lblShareholderInfo, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblShareholderInfo, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsShareHolderInfoGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickingonShareHolderInfo", "Clicking on ShareHolder Info");

            //Verify ShareHolder Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsShareHolderInfoGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("VerifyShareHolderInfoHdrs", "Verify ShareHolder Information Headers");

            // Clicking & Verify Land Identifiers
            dc.ElmLocCmds.AddToListLocElmCmd(lblLandIdentifiers, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblLandIdentifiers, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsLandIdentifiers, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblLandIdentifiersGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickingonLandIdentifiers", "Clicking on Land Identifiers");

            // Verify Land Identifiers Hdrs and Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsLandIdentifiers, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblLandIdentifiersGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("ClickAndVerifyLandIdentifiersGrid", "Clicking and Verifying Land Identifiers Hdrs and Grid");

            //Click on Planted Fields
            dc.ElmLocCmds.AddToListLocElmCmd(lblPlantedFields, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPlantedFields, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsPlantedFields, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickingonPlantedFields", "Clicking on Planted Fields");

            // Verify Planted Fields Hdrs
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsPlantedFields, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("ClickAndVerifyPlantedFields", "Clicking and Verifying Planted Fields Hdrs");

            // Click on Cancel to go back to Detail Lines Page
            dc.ElmLocCmds.AddToListLocElmCmd(btnDetailLinesCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("ClickCancelToReturnToDetailLinesPage", "Clicking on Cancel to go back to Detail Lines Page");


            // New Grower Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyGrower, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyGrower, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerEligibilityInfo, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerTaxIdType, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerInfo, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToMPCIPolicyGrowerTab", "Navigate to MPCI Policy Grower tab");

            // New Verify Grower Tab
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciGrowerAddressGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciGrowerAddressGrid, LocatorCmd.TableRowCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciGrowerSbiInfoGrid, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciGrowerSbiInfoGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyMPCIGrowerAddGridAndMPCIGrowerSBIGrid", "Verify MPCI Address Grid and SBi Grid");

            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerEdit, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerEdit, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToGrowerEditPage", "Navigate to Edit Page");

            // Verify Grower Dropdown menus
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgent, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(elmDdArrowGrowerSSN, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerSSN, LocatorCmd.VerifyOpenListGtZero, "true");
            dc.ElmLocCmds.AddToListLocElmCmd(elmDdArrowGrowerEntityType, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerEntityType, LocatorCmd.VerifyOpenListGtZero, "true");
            dc.VerifyElemLocCmds("SI-VerifyDropdownsOnGrowerEditPage", "Verify Dropdown Menus on Grower Edit Page");

            // Clicking Cancel on Edit
            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerEligibilityInfo, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavBackToGrowerPage", "Navigate back to Grower Page");

            
            // New Claims Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyClaims, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyClaims, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyCliamsViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToClaimsTab", "Navigate to Claims Tab");

            // Verify View MPCI Claims Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyCliamsViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyCliamsViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyMPCIPolicyClaimsViewGrid", "Verify MPCI Policy Claims Grid");

            // New Attachments Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyAttachments, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyAttachments, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyAttachmentsViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToAttachmentsTab", "Navigate to Attachments Tab");

            // Verify New Attachments Tab
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyAttachmentsViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyAttachmentsViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyMPCIPolicyAttachmentsViewGrid", "Verify MPCI Policy Claims Attachments View Grid");

            // New Change Log Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyChangeLog, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyChangeLog, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyChangeLogViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToChangeLogTab", "Navigate to Change Log Tab");

            //Verify New Change Log
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyChangeLogViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyChangeLogViewGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("SI-VerifyMPCIPolicyChangeLogViewGrid", "Verify MPCI Policy Claims Attachments View Grid");

            // New Notes Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyNotes, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyNotes, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(txtMpicPolicyNotes, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToNotesTab", "Navigate to Notes Tab");

            // Verify New Notes table
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyNotesViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyNotesViewGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("SI-VerifyNotesTab", "Verify Headers MPCI Policy Notes View Grid");

            // Adding text in the Notes and clicking Cancel to clear the text
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.SetAttribute, "value|Test");
            dc.NavCmdsExecute("SI-AddingNotes", "Adding Notes in Notes tab");

            // Verify text in Notes is cleared
            // Verify Input String Test Exists before clicking clear
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.GetAttributeContains, "false|value|Test");
            dc.ElmLocCmds.AddToListLocElmCmd(btnNotesCancel, LocatorCmd.JsClick); // clear Text
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.GetAttributeContains, "false|value|");
            dc.VerifyElemLocCmds("SI-VerifyNotesCleared", "Verify that Notes is cleared on Cancel");



            // New Print Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPrint, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPrint, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPolicyPrintSelCrops, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyPrintRptViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToPrintTab", "Navigate to Print tab");

            // New Verify Print Tab
            dc.ElmLocCmds.AddToListLocElmCmd(lstPolicyPrintSelCrops, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyPrintRptViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyPrintRptViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabPrintGrid", "Verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
                       
            // Click on Utilities Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyUtilities, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyUtilities, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyUtilities, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToUtilitiesTab", "Navigate to Utilities tab");

            // Verify Utilities table
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyUtilitiesGrid, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyUtilitiesGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found");

            // New SI and TI Only Tab Estimator Labels
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyEsitmator, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyEsitmator, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyEstimator, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-VerifyNavTabEstimatorLabels", "Click Estimator Tab and verify Label");

            // New SI-TI Verify Estimator Grid & Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyEstimator, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyEstimator, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabEstimatorGrid", "verify MPCI Policy Nav Tab Estimator check Estimator View Grid found ");

            // Verify tblMpciPolicyEstimatorManageTemp
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyEstimatorManageTemp, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyEstimatorManageTemp, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabEstimatorMangeTemp", "verify MPCI Policy Nav Tab Estimator check Estimator Manage Template View Grid found");

            //tblMpciPolicyEstimatorAttachments
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyEstimatorAttachments, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyEstimatorAttachments, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabEstimatorAttachments", "verify MPCI Policy Nav Tab Estimator Attachments");


            // New Coverages Tab Code
            HomePage proAgWorksHome = new HomePage(dc);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyCoverages, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyCoverages, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToCoveragesTab", "Navigate to Coverages tab");

            // New Verify MPCI Policy Coverage
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblPolicyMpciCoverage, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyMPCIPolicyCoverage", "Verify MPCI Policy Coverage");

            // New Navigating to Coverages Edit Page
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTblCoveragesCountyR1, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTblCoveragesCountyR1, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesEdit, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesEdit, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPlan, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToCoveragesEditPage", "Navigate to Coverages Edit Page");

            //New Verifying the Coverages Edit Page
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPlan, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCoverageLevel, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstUnitOptionCode, LocatorCmd.VerifyListCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyCoveragesDropdownMenus", "Verify Drop down Menus on Coverages Edit Page ");

            // Navigating back to Coverages tab
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesCancel, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavBackToCoveragesTab", "Navigate back to Coverages tab");

            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("SI-VerifyCoveragesTab", "Verify Coverages table");


            // New Policies tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPolicies, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPolicies, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPoliciesGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToPoliciesTab", "Navigate to Policies tab");

            // New Verify MPCI Policies Grid & Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPoliciesGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPoliciesGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");

            // New Policy Balance
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyBalance, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyBalance, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceHeader, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyBalanceTransactionGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("SI-NavToPolicyBalanceTab", "Navigate to Policy Balance tab");

            // Click Policies tab double wait time because policies tab needs to load all policies for grower
            // New Verify Policy Balance Table
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceHeader, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceGrossPremium, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceTotalIndemity, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyBalanceTransactionGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyBalanceTransactionGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("SI-VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
            
        }

        /// <summary>
        /// Verify TI-PAW Navigating MPCI Policy Tabs
        /// </summary>
        ///   //// -- Details Tab rows all environments
        #region Old code for NavPolicyMPCI for TI
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabDetails", "verify MPCI Policy Nav Tab Details check tables found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyDetailLines;
        //locStepArray[1] = this.tblMpciDetailLineGrid;
        //locStepArray[2] = this.tblHrdsMpciDetailLineGrid;
        //this.VerifyTblHdrsRow("tblMpciDetailLineGrid", locStepArray);

        //// -- Grower Tab
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabGrowersLabels", "Click Growers Tab verify details table Address Grid and Grower Sbi");
        //dc.Driver.GetElement(elmTabPolicyGrower, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

        //// --Verify Grower Eligibility Information
        //if (dc.Driver.GetElement(lblPolicyGrowerEligibilityInfo, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerEligibilityInfo.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerEligibilityInfo.Value + " NOT Displayed");
        //}

        //// label Grower Information Tax Type ID
        //if (dc.Driver.GetElement(lblPolicyGrowerTaxIdType, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerTaxIdType.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerTaxIdType.Value + " NOT Displayed");
        //}

        //// label Grower Information First Name
        //if (dc.Driver.GetElement(lblPolicyGrowerInfo, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerInfo.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyGrowerInfo.Value + " NOT Displayed");
        //}

        //// Growers Address Grid table rows > 0
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabGrowersAddressGrid", "verify MPCI Policy Nav Tab Growers check Address Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciGrowerAddressGrid;
        //locStepArray[2] = this.tblHdrsMpciGrowerAddressGrid;
        //this.VerifyTblHdrsRow("tblMpciGrowerAddressGrid", locStepArray);

        //// --Verify Grower Sbi grid row
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabGrowersSbiInfoGrid", "verify MPCI Policy Nav Tab Growers check Address Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciGrowerSbiInfoGrid;
        //locStepArray[2] = this.tblHdrsMpciGrowerSbiInfoGrid;
        //this.VerifyTblHdrsRow("tblMpciGrowerSbiInfoGrid", locStepArray);

        //// Click Claims Tab and verify claims table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabClaims", "verify MPCI Policy Nav Tab Claims check Claims View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyClaims;
        //locStepArray[1] = this.tblMpciPolicyCliamsViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyCliamsViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyCliamsViewGrid", locStepArray);

        //// Click Attachments Tab and verify Attachments Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabAttachments", "verify MPCI Policy Nav Tab Attachments check Attachments View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyAttachments;
        //locStepArray[1] = this.tblMpciPolicyAttachmentsViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyAttachmentsViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyAttachmentsViewGrid", locStepArray);

        //// Click Change Log Tab and verify Change Log Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabChangeLog", "verify MPCI Policy Nav Tab Change Log check Change Log View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyChangeLog;
        //locStepArray[1] = this.tblMpciPolicyChangeLogViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyChangeLogViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyChangeLogViewGrid", locStepArray);

        //// Click Notes Tab and Verify Notes Labels
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabNotesLabels", "Click Notes Tab and Verify Label Notes Exists");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyNotes.Value);
        //dc.Driver.GetElement(elmTabPolicyNotes, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(txtMpicPolicyNotes, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(txtMpicPolicyNotes.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(txtMpicPolicyNotes.Value + " NOT Displayed");
        //}

        //// Verify Notes Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabNotesGrid", "verify MPCI Policy Nav Tab Notes check Notes View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyNotesViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyNotesViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyNotesViewGrid", locStepArray);

        //// Click Print Tab and Verify Elements
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabPrintElemntsExist", "Click Print Tab and Verify Elements and Print Report Table");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyPrint.Value);
        //dc.Driver.GetElement(elmTabPolicyPrint, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lstPolicyPrintSelCrops, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " NOT Displayed");
        //}

        ////Verify Print Report Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabPrintGrid", "verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyPrintRptViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyPrintRptViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyPrintRptViewGrid", locStepArray);

        //// Click Utilities Tab and verify Elements 
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabUtilitiesElements", "Click Utilities Tab and verify Elements ");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyUtilities.Value);
        //dc.Driver.GetElement(elmTabPolicyUtilities, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lblPolicyUtilities, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " NOT Displayed");
        //}

        //// verify  Utilities table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyUtilitiesGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyUtilitiesGrid;
        //this.VerifyTblHdrs(locStepArray); 

        //// SI and TI Only Tab Estimator Labels
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabEstimatorLabels", "Click Estimator Tab and verify Label");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyEsitmator.Value);
        //dc.Driver.GetElement(elmTabPolicyEsitmator, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lblPolicyEstimator, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyEstimator.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyEstimator.Value + " NOT Displayed");
        //}

        //// SI and TI Only Tab Estimator Tables
        ////tblMpciPolicyEstimator
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabEstimatorGrid", "verify MPCI Policy Nav Tab Estimator check Estimator View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyEstimator;
        //locStepArray[2] = this.tblHdrsMpciPolicyEstimator;
        //this.VerifyTblHdrs(locStepArray); 

        ////tblMpciPolicyEstimatorManageTemp
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabEstimatorMangeTemp", "verify MPCI Policy Nav Tab Estimator check Estimator Manage Template View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyEstimatorManageTemp;
        //locStepArray[2] = this.tblHdrsMpciPolicyEstimatorManageTemp;
        //this.VerifyTblHdrs(locStepArray); 

        ////tblMpciPolicyEstimatorAttachments
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabEstimatorAttachments", "verify MPCI Policy Nav Tab Estimator check Estimator Attachments View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyEstimatorAttachments;
        //locStepArray[2] = this.tblHdrsMpciPolicyEstimatorAttachments;
        //this.VerifyTblHdrs(locStepArray); 

        //// Click Coverages Tab and verify Coverages Table
        //// Note below tblPolicyMpciCoverage is passed and is shared table as is Home page mapping
        //HomePage proAgWorksHome = new HomePage(dc);
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabCoverages", "verify MPCI Policy Nav Tab Coverages check Coverages View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyCoverages;
        //locStepArray[1] = proAgWorksHome.tblPolicyMpciCoverage;
        //locStepArray[2] = proAgWorksHome.tblHdrsPolicyMpciCoverage;
        //this.VerifyTblHdrsRow("tblPolicyMpciCoverage", locStepArray);

        //// Click Policies tab double wait time because policies tab needs to load all policies for grower
        //// Verify Policies Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyPolicies;
        //locStepArray[1] = this.tblMpciPoliciesGrid;
        //locStepArray[2] = this.tblHdrsMpciPoliciesGrid;
        //this.VerifyTblHdrsRow("tblMpciPoliciesGrid", locStepArray);

        //// Click Policy Balance Tab and Verify Elements and Policy Balance Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabPolicyBalanceElements", "Click Policy Balance Tab and Verify Elements");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyBalance.Value);
        //dc.Driver.GetElement(elmTabPolicyBalance, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

        //// Label Policy Balance Header
        //if (dc.Driver.GetElement(lblPolicyBalanceHeader, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " NOT Displayed");
        //}

        //// Label Policy Balance Gross Premium 
        //if (dc.Driver.GetElement(lblPolicyBalanceGrossPremium, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " NOT Displayed");
        //}

        //// Label Policy Balance Total Indemity
        //if (dc.Driver.GetElement(lblPolicyBalanceTotalIndemity, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " NOT Displayed");
        //}

        //// Verify Policy Balance Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("TI-VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyBalanceTransactionGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyBalanceTransactionGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyBalanceTransactionGrid", locStepArray);
        //// Click Print Tab and Verify Elements
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPrintElemntsExist", "Click Print Tab and Verify Elements and Print Report Table");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyPrint.Value);
        //dc.Driver.GetElement(elmTabPolicyPrint, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lstPolicyPrintSelCrops, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lstPolicyPrintSelCrops.Value + " NOT Displayed");
        //}

        ////Verify Print Report Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPrintGrid", "verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyPrintRptViewGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyPrintRptViewGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyPrintRptViewGrid", locStepArray);

        //// Click Utilities Tab and verify Elements 
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabUtilitiesElements", "Click Utilities Tab and verify Elements ");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyUtilities.Value);
        //dc.Driver.GetElement(elmTabPolicyUtilities, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lblPolicyUtilities, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyUtilities.Value + " NOT Displayed");
        //}

        //// verify  Utilities table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyUtilitiesGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyUtilitiesGrid;
        //this.VerifyTblHdrs(locStepArray);

        //// SI and TI Only Tab Estimator Labels
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorLabels", "Click Estimator Tab and verify Label");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyEsitmator.Value);
        //dc.Driver.GetElement(elmTabPolicyEsitmator, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //if (dc.Driver.GetElement(lblPolicyEstimator, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyEstimator.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyEstimator.Value + " NOT Displayed");
        //}

        //// SI and TI Only Tab Estimator Tables
        ////tblMpciPolicyEstimator
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorGrid", "verify MPCI Policy Nav Tab Estimator check Estimator View Grid found ");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyEstimator;
        //locStepArray[2] = this.tblHdrsMpciPolicyEstimator;
        //this.VerifyTblHdrs(locStepArray);

        ////tblMpciPolicyEstimatorManageTemp
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorMangeTemp", "verify MPCI Policy Nav Tab Estimator check Estimator Manage Template View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyEstimatorManageTemp;
        //locStepArray[2] = this.tblHdrsMpciPolicyEstimatorManageTemp;
        //this.VerifyTblHdrs(locStepArray);

        ////tblMpciPolicyEstimatorAttachments
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabEstimatorAttachments", "verify MPCI Policy Nav Tab Estimator check Estimator Attachments View Grid found");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyEstimatorAttachments;
        //locStepArray[2] = this.tblHdrsMpciPolicyEstimatorAttachments;
        //this.VerifyTblHdrs(locStepArray);
        //// Click Policies tab double wait time because policies tab needs to load all policies for grower
        //// Verify Policies Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");
        //locStepArray[0] = this.elmTabPolicyPolicies;
        //locStepArray[1] = this.tblMpciPoliciesGrid;
        //locStepArray[2] = this.tblHdrsMpciPoliciesGrid;
        //this.VerifyTblHdrsRow("tblMpciPoliciesGrid", locStepArray);

        //// Click Policy Balance Tab and Verify Elements and Policy Balance Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPolicyBalanceElements", "Click Policy Balance Tab and Verify Elements");
        //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + elmTabPolicyBalance.Value);
        //dc.Driver.GetElement(elmTabPolicyBalance, dc).JavaScriptClick();
        //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

        //// Label Policy Balance Header
        //if (dc.Driver.GetElement(lblPolicyBalanceHeader, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceHeader.Value + " NOT Displayed");
        //}

        //// Label Policy Balance Gross Premium 
        //if (dc.Driver.GetElement(lblPolicyBalanceGrossPremium, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceGrossPremium.Value + " NOT Displayed");
        //}

        //// Label Policy Balance Total Indemity
        //if (dc.Driver.GetElement(lblPolicyBalanceTotalIndemity, dc).WaitTillDisplayed())
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " Displayed");
        //}
        //else
        //{
        //    dc.ProAgXmlRsltLogger.LogPass(lblPolicyBalanceTotalIndemity.Value + " NOT Displayed");
        //}

        //// Verify Policy Balance Table
        //dc.ProAgXmlRsltLogger.AddTestStepNd("SI-VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
        //locStepArray[0] = null;
        //locStepArray[1] = this.tblMpciPolicyBalanceTransactionGrid;
        //locStepArray[2] = this.tblHdrsMpciPolicyBalanceTransactionGrid;
        //this.VerifyTblHdrsRow("tblMpciPolicyBalanceTransactionGrid", locStepArray);

        #endregion
        private void VerifyPolicMPCINavTabsTblsForTI()
        {
            ElementLocator[] locStepArray = new ElementLocator[3];
            DriverContext dc = this.DriverContext;

            // New MPCI Policy Detail Lines Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyDetailLines, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyDetailLines, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHrdsMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToMPCIPolicyDetailLinesTab", "Navigate to MPCI Policy Detail Lines tab");

            // New Verify MPCI POlicy Detail Lines Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHrdsMpciDetailLineGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyDetailLinesHdrsAndRowCntGtZero", "Verify Detail Lines Headers and Row Count greater than 0");

            // Clicking on Unit Number
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTIUnitNumber, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTIUnitNumber, LocatorCmd.JsClick);
            dc.NavCmdsExecute("TI-ClickingonUnitNumber", "Clicking on Unit Number on Detail Lines tab");

            // Verifying the Maintain Detail Line Page
            dc.ElmLocCmds.AddToListLocElmCmd(lblMaintainDetailLine, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPractice, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPractice, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCommCropStatus, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCommCropStatus, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstAcreageType, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstAcreageType, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPreYrProcCode, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPreYrProcCode, LocatorCmd.VerifyListCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyDropdownMenus", "Verify Dropdown Menus on Maintain Detail Line page");

            // Clicking on ShareHolder Information
            dc.ElmLocCmds.AddToListLocElmCmd(lblShareholderInfo, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblShareholderInfo, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsShareHolderInfoGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-ClickingonShareHolderInfo", "Clicking on ShareHolder Info");

            //Verify ShareHolder Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsShareHolderInfoGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("TI-VerifyShareHolderInfoHdrs", "Verify ShareHolder Information Headers");

            // Clicking & Verify Land Identifiers
            dc.ElmLocCmds.AddToListLocElmCmd(lblLandIdentifiers, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblLandIdentifiers, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsLandIdentifiers, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblLandIdentifiersGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-ClickingonLandIdentifiers", "Clicking on Land Identifiers");

            // Verify Land Identifiers Hdrs and Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsLandIdentifiers, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblLandIdentifiersGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-ClickAndVerifyLandIdentifiersGrid", "Clicking and Verifying Land Identifiers Hdrs and Grid");

            //Click on Planted Fields
            dc.ElmLocCmds.AddToListLocElmCmd(lblPlantedFields, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPlantedFields, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsPlantedFields, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-ClickingonPlantedFields", "Clicking on Planted Fields");

            // Verify Planted Fields Hdrs
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsPlantedFields, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("TI-ClickAndVerifyPlantedFields", "Clicking and Verifying Planted Fields Hdrs");

            // Click on Cancel to go back to Detail Lines Page
            dc.ElmLocCmds.AddToListLocElmCmd(btnDetailLinesCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciDetailLineGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-ClickCancelToReturnToDetailLinesPage", "Clicking on Cancel to go back to Detail Lines Page");

            // New Grower Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyGrower, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyGrower, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerEligibilityInfo, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerTaxIdType, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerInfo, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToMPCIPolicyGrowerTab", "Navigate to MPCI Policy Grower tab");

            // New Verify Grower Tab
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciGrowerAddressGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciGrowerAddressGrid, LocatorCmd.TableRowCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciGrowerSbiInfoGrid, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciGrowerSbiInfoGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyMPCIGrowerAddGridAndMPCIGrowerSBIGrid", "Verify MPCI Address Grid and SBi Grid");

            // Clicking on Grower Edit
            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerEdit, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerEdit, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToGrowerEditPage", "Navigate to Edit Page");

            // Verify Grower Dropdown menus
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgency, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerAgent, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(elmDdArrowGrowerSSN, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerSSN, LocatorCmd.VerifyOpenListGtZero, "true");
            dc.ElmLocCmds.AddToListLocElmCmd(elmDdArrowGrowerEntityType, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstGrowerEntityType, LocatorCmd.VerifyOpenListGtZero, "true");
            dc.VerifyElemLocCmds("TI-VerifyDropdownsOnGrowerEditPage", "Verify Dropdown Menus on Grower Edit Page");

            // Clicking on Cancel on Edit
            dc.ElmLocCmds.AddToListLocElmCmd(btnGrowerCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyGrowerEligibilityInfo, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavBackToGrowerPage", "Navigate back to Grower Page");
                                 
            // New Claims Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyClaims, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyClaims, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyCliamsViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToClaimsTab", "Navigate to Claims Tab");

            // Verify View MPCI Claims Grid
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyCliamsViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyCliamsViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyMPCIPolicyClaimsViewGrid", "Verify MPCI Policy Claims Grid");

            // New Attachments Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyAttachments, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyAttachments, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyAttachmentsViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToAttachmentsTab", "Navigate to Attachments Tab");

            // Verify New Attachments Tab
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyAttachmentsViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyAttachmentsViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyMPCIPolicyAttachmentsViewGrid", "Verify MPCI Policy Claims Attachments View Grid");

            // New Change Log Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyChangeLog, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyChangeLog, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyChangeLogViewGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToChangeLogTab", "Navigate to Change Log Tab");

            //Verify New Change Log
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyChangeLogViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyChangeLogViewGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("TI-VerifyMPCIPolicyChangeLogViewGrid", "Verify MPCI Policy Claims Attachments View Grid");

            // New Notes Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyNotes, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyNotes, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(txtMpicPolicyNotes, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToNotesTab", "Navigate to Notes Tab");

            // Verify New Notes table
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyNotesViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyNotesViewGrid, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("TI-VerifyNotesTab", "Verify Headers MPCI Policy Notes View Grid");

            // Adding text in the Notes and clicking Cancel to clear the text
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.SetAttribute, "value|Test");
            dc.NavCmdsExecute("TI-AddingNotes", "Adding Notes in Notes tab");

            // Verify text in Notes is cleared
            // Verify Input Text Test Exists before clicking clear
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.GetAttributeContains, "false|value|Test");
            dc.ElmLocCmds.AddToListLocElmCmd(btnNotesCancel, LocatorCmd.JsClick); // clear Text
            dc.ElmLocCmds.AddToListLocElmCmd(txtBoxNotes, LocatorCmd.GetAttributeContains, "false|value|");
            dc.VerifyElemLocCmds("TI-VerifyNotesCleared", "Verify that Notes is cleared on Cancel");

            // New Print Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPrint, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPrint, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstPolicyPrintSelCrops, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToPrintTab", "Navigate to Print tab");

            // New Verify Print Tab
            dc.ElmLocCmds.AddToListLocElmCmd(lstPolicyPrintSelCrops, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyPrintRptViewGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyPrintRptViewGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabPrintGrid", "Verify MPCI Policy Nav Tab Print check Print View Grid found & rows > 0");
                       
            // Click on Utilities Tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyUtilities, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyUtilities, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyUtilities, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToUtilitiesTab", "Navigate to Utilities tab");

            // Verify Utilities table
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyUtilitiesGrid, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyUtilitiesGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabUtilitiesGrid", "verify MPCI Policy Nav Tab Utilities check Utilities View Grid found");

            // New SI and TI Only Tab Estimator Labels
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyEsitmator, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyEsitmator, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyEstimator, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-VerifyNavTabEstimatorLabels", "Click Estimator Tab and verify Label");

            // New SI-TI Verify Estimator Grid & Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyEstimator, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyEstimator, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabEstimatorGrid", "verify MPCI Policy Nav Tab Estimator check Estimator View Grid found ");

            // Verify tblMpciPolicyEstimatorManageTemp
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyEstimatorManageTemp, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyEstimatorManageTemp, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabEstimatorMangeTemp", "verify MPCI Policy Nav Tab Estimator check Estimator Manage Template View Grid found");

            //tblMpciPolicyEstimatorAttachments
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyEstimatorAttachments, LocatorCmd.Displayed);
            //dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyEstimatorAttachments, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabEstimatorMangeAttachments", "verify MPCI Policy Nav Tab Estimator check Estimator Manage Attachments View Grid found");
                           
            // New Coverages Tab Code
            HomePage proAgWorksHome = new HomePage(dc);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyCoverages, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyCoverages, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToCoveragesTab", "Navigate to Coverages tab");

            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblPolicyMpciCoverage, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyMPCIPolicyCoverage", "Verify MPCI Policy Coverage");

            // New Navigating to Coverages Edit Page
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTblCoveragesCountyR1, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lnkTblCoveragesCountyR1, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesEdit, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesEdit, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPlan, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToEditPage", "Navigate to Edit Page");

            //New Verifying the Coverages Edit Page
            dc.ElmLocCmds.AddToListLocElmCmd(lstCropPlan, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstCoverageLevel, LocatorCmd.VerifyListCountGtZero);
            dc.ElmLocCmds.AddToListLocElmCmd(lstUnitOptionCode, LocatorCmd.VerifyListCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyDropdownMenus", "Verify Drop down Menus on Coverages Edit Page ");

            // Navigating back to Coverages tab
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesCancel, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(btnCoveragesCancel, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavBackToCoveragesTab", "Navigate back to Coverages tab");

            dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.tblHdrsPolicyMpciCoverage, LocatorCmd.Displayed);
            dc.VerifyElemLocCmds("TI-VerifyCoveragesTab", "Verify Coverages table");

            // New Policies tab
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPolicies, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyPolicies, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPoliciesGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToPoliciesTab", "Navigate to Policies tab");

            // New Verify MPCI Policies Grid & Headers
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPoliciesGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPoliciesGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabPolicies", "verify MPCI Policy Nav Tab Polices check Policies View Grid found & rows > 0");

            // New Policy Balance
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyBalance, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(elmTabPolicyBalance, LocatorCmd.JsClick);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceHeader, LocatorCmd.WaitTilDisplayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyBalanceTransactionGrid, LocatorCmd.WaitTilDisplayed);
            dc.NavCmdsExecute("TI-NavToPolicyBalanceTab", "Navigate to Policy Balance tab");

            // Click Policies tab double wait time because policies tab needs to load all policies for grower
            // New Verify Policy Balance Table
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceGrossPremium, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(lblPolicyBalanceTotalIndemity, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblHdrsMpciPolicyBalanceTransactionGrid, LocatorCmd.Displayed);
            dc.ElmLocCmds.AddToListLocElmCmd(tblMpciPolicyBalanceTransactionGrid, LocatorCmd.TableRowCountGtZero);
            dc.VerifyElemLocCmds("TI-VerifyNavTabPolicyBalanceTable", "verify MPCI Policy Nav Tab Policy Balance check Policy Balance View Grid found & rows > 0");
        

        }

        /// <summary>
        /// Verify MPCI Policy Tab Navigation Main Method
        /// Handles ProAgPAW, SI and TI
        /// </summary>
        public void VerifyPolicMPCINavTabsTblsMain()
        {
            DriverContext dc = this.DriverContext;

            // add reference to home page?
            // this where we can put common navigation steps to get into edit mode and dropdown list count verification.

            PageObjects.ProAgWorks.HomePage PawHomePage = new HomePage(dc);

            switch (dc.CurEnv.ToLower().Trim())
            {
                case "ti-prod":
                case "ti-uat":
                    VerifyPolicMPCINavTabsTblsForTI();
                    break;
                case "si-prod":
                case "si-uat":
                    VerifyPolicMPCINavTabsTblsForSI();

                    break;
                default:
                    VerifyPolicMPCINavTabsTblsForPAW();
                    break;
            }
        }


        #region inital metod for Prolicy Tab Nav and Verfy no longer used
        ///// <summary>
        ///// Local Common Verify Select Policy Tabs Table Grid View exists and Row count > 0
        ///// </summary>
        ///// <param name="varDc">Current DriverContext</param>
        ///// <param name="locTbl">The locator of the table to be verified</param>
        ///// <param name="sTabTblName">Text Tab click and Table / Grid View to checked, used if error logged</param>
        ///// <param name="waitOverride">Override default wait time if needed, if 0.0 sent default wait time used</param>
        //public void VerifyPolicyNavTabTable(DriverContext varDc, ElementLocator locTbl, string sTabTblName)
        //{
        //    if (sTabTblName.ToLower().Contains("tab attachments"))
        //    {
        //        // scroll estimator attachment table into view
        //        varDc.Driver.GetElement(elmTabPolicEstimatorTxtNoItems, varDc).JavaScriptSrollIntoView();
        //    }

        //    if (sTabTblName.ToLower().Contains("grower sbi grid"))
        //    {
        //        // string xpathElmTblGrowerSBIGrid = "//div[@id='MpciSBIViewModelGrid']//div//span[contains(text(),'No items to display')]";
        //        string xpathElmTblGrowerSBIGrid = "//div[@id='MpciSBIViewModelGrid']//table[@role='grid']/thead";
        //        ElementLocator ElmGrowerSBIGirdNoItems = new ElementLocator(Locator.XPath, xpathElmTblGrowerSBIGrid);
        //        varDc.Driver.GetElement(ElmGrowerSBIGirdNoItems, varDc).JavaScriptSrollIntoView();
        //    }

            
        //    if (varDc.Driver.GetElement(locTbl, varDc).WaitTillDisplayed())
        //    {
        //        RsltLogger.LogPass("Table " + sTabTblName + " Found");
        //    }
        //    else
        //    {
        //        RsltLogger.LogFail("Table " + sTabTblName + " NOT Found");
        //    }

        //    // build xpath to table rows
        //    string xpathRowsTblGrid = locTbl.Value + "//tr";
        //    ElementLocator rowsTblGird = new ElementLocator(Locator.XPath, xpathRowsTblGrid);
        //    IList<IWebElement> tblRows = varDc.Driver.GetElements(rowsTblGird, varDc);
        //    //Verify Details Grid row count > 0

        //    if (sTabTblName.ToLower().Contains("utilities") || sTabTblName.ToLower().Contains("estimator") || sTabTblName.ToLower().Contains("grower sbi grid"))
        //    {
        //        switch (varDc.CurEnv.ToLower().Trim())
        //        {
        //            case "si-uat":
        //            case "si-prod":
        //            case "ti-uat":
        //            case "ti-prod":
        //                // SI / TI Utilities row count is 0
        //                if (sTabTblName.ToLower().Contains("tab attachments") || (sTabTblName.ToLower().Contains("grower sbi grid") && varDc.CurEnv.ToLower().Trim().Contains("ti")))
        //                {
        //                    // Estimator Attachments tab has one row with  stating no records
        //                    if (sTabTblName.ToLower().Contains("utilities") 
        //                        || (sTabTblName.ToLower().Contains("grower sbi grid") && varDc.CurEnv.ToLower().Trim().Contains("ti")))
        //                    {
        //                        if (tblRows.Count <= 0)
        //                        {
        //                            // error Detail lines grid row count <= 0 
        //                            string msgRowErr = " Click " + sTabTblName + " row count exp: > 0, act: row count: "
        //                                               + tblRows.Count.ToString();
        //                            RsltLogger.LogFail(msgRowErr);
        //                            //Verify.That(varDc, () => Assert.Fail(msgRowErr));
        //                        }
        //                        else
        //                        {
        //                            string msgPass = "Passed Click " + sTabTblName + " row count > 0, act: row count: " +
        //                                             tblRows.Count.ToString();
        //                            RsltLogger.LogPass(msgPass);

        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (tblRows.Count > 0)
        //                    {
        //                        // error Detail lines grid row count > 0 
        //                        string msgRowErr = " Click " + sTabTblName + " row count exp: 0, act: row count: "
        //                                           + tblRows.Count.ToString();
        //                        RsltLogger.LogFail(msgRowErr);
        //                        //Verify.That(varDc, () => Assert.Fail(msgRowErr));
        //                    }
        //                    else
        //                    {
        //                        string msgPass = "Passed Click " + sTabTblName + " row count = 0, act: row count: " +
        //                                         tblRows.Count.ToString();
        //                        RsltLogger.LogPass(msgPass);

        //                    }
        //                }
                        
        //                break;
        //            default:
        //                // ProAgWorks Utilities row count is > 0
        //                if (sTabTblName.ToLower().Contains("utilities") || sTabTblName.ToLower().Contains("grower sbi grid"))
        //                {
        //                    if (tblRows.Count <= 0)
        //                    {
        //                        // error Detail lines grid row count <= 0 
        //                        string msgRowErr = " Click " + sTabTblName + " row count exp: > 0, act: row count: "
        //                                           + tblRows.Count.ToString();
        //                        RsltLogger.LogFail(msgRowErr);
        //                        //Verify.That(varDc, () => Assert.Fail(msgRowErr));
        //                    }
        //                    else
        //                    {
        //                        string msgPass = "Passed Click " + sTabTblName + " row count > 0, act: row count: " +
        //                                         tblRows.Count.ToString();
        //                        RsltLogger.LogPass(msgPass);

        //                    }
        //                }
        //                break;
        //        }
        //    }
        //    else
        //    {
                
        //        if (tblRows.Count <= 0)
        //        {
        //            // error Detail lines grid row count <= 0 
        //            string msgRowErr = " Click " + sTabTblName + " row count exp: > 0, act: row count: "
        //                               + tblRows.Count.ToString();
        //            RsltLogger.LogFail(msgRowErr);
        //            //Verify.That(varDc, () => Assert.Fail(msgRowErr));
        //        }
        //        else
        //        {
        //            string msgPass = "Passed Click " + sTabTblName + " row count > 0, act: row count: " +
        //                             tblRows.Count.ToString();
        //            RsltLogger.LogPass(msgPass);

        //        }
        //    }
        //}
        #endregion inital metod for Prolicy Tab Nav and Verfy no longer used
    }
}
