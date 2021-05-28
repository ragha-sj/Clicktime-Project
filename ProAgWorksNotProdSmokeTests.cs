
namespace Ocaramba.Tests.ProAgWorks.NonProdTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Policy;
    using System.Threading;
    using System.Threading.Tasks;
    using global::NUnit.Framework;
    using NUnit.Framework.Internal;
    using NUnit.Framework.Internal.Commands;
    using NLog;
    using NLog.LayoutRenderers;
    using NPOI.SS.Formula.Functions;
    using Ocaramba;
    using Ocaramba.Types;
    using Ocaramba.Extensions;
    using Ocaramba.Helpers;
    // Make Reference for Common LogOn Page
    using Ocaramba.Tests.PageObjects.PageObjects.LogOn;
    // Make Reference to Project Under Test Page Objects
    using Ocaramba.Tests.PageObjects.PageObjects.ProAgWorks;
    using OpenQA.Selenium;
    using System.Runtime.InteropServices;

    [Parallelizable(ParallelScope.Fixtures)]
    // Get test fixtures from AutoFixtures.xml
    [TestFixtureSource(typeof(ProAgGetFixtures), "FixtureArgsRun")]

    class ProAgWorksNotProdSmokeTests : ProjectTestBase
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        //private DriverContext dc;

        // HomePage and LogOnPage need for Most Projects Remove if not needed
        public LogOnPage logon;
        // Make sure to reference correct HomePage
        public HomePage proAgWorksHome;
        // proAgWorks Accounting (menu item) mapping objects and methods
        public Billing pawBilling;
        public InterestBatch pawInterestBatch;
        public Lockbox pawLockBox;
        public DataMart pawDataMart;
        public NCIS pawNCIS;
        public PPDFPARExport pawPPDFPARExport;
        public PPDFPARFlexExport pawPPDFPARFlexExport;
        public PPDFPriceFlexExport pawPPDFPriceFlexExport;
        public PPDFPriceFlexPrimeExport pawPPDFPriceFlexPrimeExport;
        public MpciAgencyCommissionReport pawMpciAgencyCommissionReport;
        public CommissionSchedules pawCommissionSchedules;

        // Test Entry and makes call to ProjectTestBase to init environment
        public ProAgWorksNotProdSmokeTests(object fixture)
            : base(fixture)
        {
            if (string.IsNullOrEmpty(DriverContext.CurEnv))
            {
                DriverContext.CurEnv = BaseConfiguration.getProAgGetConfigXmlEnv();
            }

            // This loads the standard data for determining user credentials information
            DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksCreds.xlsx", "ProAgWorks", 0);


            // Set ProAgDbName for Getting DbUserId from a Database or Skip

            // tjn added Avoids trying to query DB for user id - we need DB name for SI and TI
            // Also SI and TI Data menus and data may not allow navigation
            this.ProAgDbNameForDbUserId = ProAgSqlHelper.ProAgDbNames.SkipGetDbUserId;

            // Non prod get test user
            // Use correct parameters based on how you setup Creds.xlsx file above
            this.SetLoginInfo("ProAgWorksCreds_ProAgWorks", "GroupAlias", "CommonProAgWorks");

            // This is where we would pullin our SQL statements if needed like - load Claims Query
            //DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksQueries.xlsx", "WorksClaims", 0);

        }

        #region local helper methods
        /// <summary>
        /// Get test data from loaded excel worksheet. Worksheet to get data from must have been loaded previously
        /// The TestName, DataKey and DataValue columns hold the values to be used
        /// Query is based on TestName which should match test method name which is TestTitle property on DriverContext
        /// Match on TestName and DataKey returns data value.
        /// </summary>
        /// <param name="dataKey">The DataKey for the current TestName to Return</param>
        /// <returns>DataValue matching TestName and DataKey as string</returns>
        /// At this time we do not have a helper method defined

        /// <summary>
        /// Get Table RMS Transmission Column Index Based Column Name
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="iRow"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        #endregion local helper methods

        [SetUp]
        public void TestInit()
        {
            // Need to create / Initialize Page objects here after webdriver is created 
            // HomePage and LogOn Page will be needed by most programs
            logon = new LogOnPage(this.DriverContext);

            // Make sure to reference correct HomePage
            proAgWorksHome = new PageObjects.PageObjects.ProAgWorks.HomePage(this.DriverContext);

            // Make Reference to "Accounting | AR Processing | Billings" page object
            pawBilling = new PageObjects.PageObjects.ProAgWorks.Billing(this.DriverContext);

            // Make Reference to "Accounting | AR Processing | InterestBatch" page object
            pawInterestBatch = new PageObjects.PageObjects.ProAgWorks.InterestBatch(this.DriverContext);

            // Make Reference to "Accounting | AR Processing | LockBox" page object
            pawLockBox = new PageObjects.PageObjects.ProAgWorks.Lockbox(this.DriverContext);

            // Make Reference to "Accounting | Commission Schedules
            pawCommissionSchedules = new PageObjects.PageObjects.ProAgWorks.CommissionSchedules(this.DriverContext);

            // Make Reference to "Accounting | Reports | MPCI | MPCI Agency Commission Statement" page object
            pawMpciAgencyCommissionReport = new PageObjects.PageObjects.ProAgWorks.MpciAgencyCommissionReport(this.DriverContext);

            // Make Reference to "Accounting | File Transmissions | B-template/DataMart" page objects
            pawDataMart = new PageObjects.PageObjects.ProAgWorks.DataMart(this.DriverContext);

            // Make Reference to "Accounting | File Transmissions | NCIS page object
            pawNCIS = new PageObjects.PageObjects.ProAgWorks.NCIS(this.DriverContext);

            // Make Reference to "Accounting | File Transmissions | Private Product Data Files | (PAR File Export or Par Flex File Export or Price Flex File Export or Price Flex Prime File Export)" page objects
            pawPPDFPARExport = new PageObjects.PageObjects.ProAgWorks.PPDFPARExport(this.DriverContext);
            pawPPDFPARFlexExport = new PageObjects.PageObjects.ProAgWorks.PPDFPARFlexExport(this.DriverContext);
            pawPPDFPriceFlexExport = new PageObjects.PageObjects.ProAgWorks.PPDFPriceFlexExport(this.DriverContext);
            pawPPDFPriceFlexPrimeExport = new PageObjects.PageObjects.ProAgWorks.PPDFPriceFlexPrimeExport(this.DriverContext);

            // Make Reference to "Accounting | File Transmissions | NCIS page object
            pawCommissionSchedules = new PageObjects.PageObjects.ProAgWorks.CommissionSchedules(this.DriverContext);

        }


        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Accounting Navigation")]
        [Property("TFS-PBI", "78466")]

        public void NavAccountingTest()
        {
            DriverContext dc = this.DriverContext;
            try
            {

                // Navigate to home page for login
                dc.NavToStartUrl();

                // LogOn from common LogOn page user must belong to application being logged into
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);


                // wait for Home page menu bar to display -- this test step setup can be removed
                
                //tjn added
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.NavCmdsExecute("WaitLogonComplete", "Wait ProAgWorks Logon to complete");

                // tjn added verify
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.Displayed,
                    "Find the Main Menu Accounting");
                dc.VerifyElemLocCmds("VerifyAccoutingMenu", "Does the Accouting Menu Exist");

                //tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");

                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.mnuMainAccounting, dc);
                ////The two GetElements for this test most likely can be taken out --- Not needed
                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //if (dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Main Menu Accounting with desc: " + proAgWorksHome.mnuMainAccounting.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Main Menu Accounting with desc: " + proAgWorksHome.mnuMainAccounting.Value);
                //}
                #endregion commentout
                //Start Reporting Navigation Test here

                // 1. The first navigation test condition - navigate to Accounting | AR Processing | Billing

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessing, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessingBilling, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessingBilling, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToBillingPage", "Select ProAgWorks menus to navigate to Billing Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawBilling.bnrBilling, LocatorCmd.Displayed,
                    "Find the Navigated Accounting AR Processing Billing page bnrBilling");
                dc.VerifyElemLocCmds("VerifyCreateNewBatchButton", "Verify the Create New Batch button exists on the page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToBillingPage", "Select ProAgWorks menus to navigate to Billing Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();

                //ElementLocator[] locAccountingMnu3 = new ElementLocator[3];
                //locAccountingMnu3[0] = proAgWorksHome.mnuMainAccounting;
                //locAccountingMnu3[1] = proAgWorksHome.mnuAcntARProcessing;
                //locAccountingMnu3[2] = proAgWorksHome.mnuAcntARProcessingBilling;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu3, pawBilling.bnrBilling);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawBilling.bnrBilling, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting AR Processing Billing page " + pawBilling.bnrBilling.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Billing page: " + pawInterestBatch.bnrInterestBatch.Value);
                //}

                // tjn commented out 
                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyCreateNewBatchButton", "Verify the Create New Batch button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawInterestBatch.btnCreateNewBatch, dc);
                #endregion commentout


                // 2. The second navigation test condition - navigate to Accounting | AR Processing | Interest Batch

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessing, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessingInterestBatch, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawInterestBatch.bnrInterestBatch, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToInteretBatchPage", "Select ProAgWorks menus to navigate to Interest Batch Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawInterestBatch.bnrInterestBatch, LocatorCmd.Displayed,
                    "Find the Navigated Accounting AR Processing Interest Batch page bnrInterestBatch");
                dc.VerifyElemLocCmds("VerifyInteretBatchPage", "Verify Elements on Interet Batch Page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToInteretBatchPage", "Select ProAgWorks menus to navigate to Interest Batch Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu3[2] = proAgWorksHome.mnuAcntARProcessingInterestBatch;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu3, pawInterestBatch.bnrInterestBatch);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawInterestBatch.bnrInterestBatch, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting AR Processing Interest Batch page " + pawInterestBatch.bnrInterestBatch.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Interest Batch page: " + pawInterestBatch.bnrInterestBatch.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyCreateNewBatchButton", "Verify the Create New Batch button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawInterestBatch.btnCreateNewBatch, dc);

                #endregion commentout

                // 3. The third navigation test condition - navigate to Accounting | AR Processing | LockBox

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessing, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntARProcessingLockBox, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawLockBox.bnrLockBox, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToLockboxPage", "Select ProAgWorks menus to navigate to LockBox Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawLockBox.bnrLockBox, LocatorCmd.Displayed,
                    "Find the Navigated Accounting AR Processing LockBox page bnrLockBox");
                dc.VerifyElemLocCmds("VerifyLockboxPage", "Verify Elements on Lock Box Page page");

                //tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToLockboxPage", "Select ProAgWorks menus to navigate to LockBox Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu3[2] = proAgWorksHome.mnuAcntARProcessingLockBox;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu3, pawLockBox.bnrLockBox);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawLockBox.bnrLockBox, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting AR Processing LockBox page " + pawLockBox.bnrLockBox.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected LockBox page: " + pawLockBox.bnrLockBox.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyImportOnLineLockBoxButton", "Verify the Import On Line LockBox button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawLockBox.btnImportOnlineLockBox, dc);

                #endregion commentout


                // 4. The fourth navigation test condition - navigate to Accounting | AR Processing | Commission Schedules

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntCommissionSchedules, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawCommissionSchedules.bnrCommissionSchedule, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToCommissionScheduleshPage", "Select ProAgWorks menus to navigate to Commission Schedules Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawCommissionSchedules.bnrCommissionSchedule, LocatorCmd.Displayed,
                    "Find the Navigated Accounting Commission Schedules page bnrCommissionSchedule");
                dc.VerifyElemLocCmds("VerifyCommissionScheduleshPage", "Verify Elements on the Commission Schedulesh Page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToCommissionScheduleshPage", "Select ProAgWorks menus to navigate to Commission Schedules Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //ElementLocator[] locAccountingMnu2 = new ElementLocator[2];
                //locAccountingMnu2[0] = proAgWorksHome.mnuMainAccounting;
                //locAccountingMnu2[1] = proAgWorksHome.mnuAcntCommissionSchedules;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu2, pawCommissionSchedules.bnrCommissionSchedule);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawCommissionSchedules.bnrCommissionSchedule, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting Commission Schedules page " + pawCommissionSchedules.bnrCommissionSchedule.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting Commission Schedules page: " + pawCommissionSchedules.bnrCommissionSchedule.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyAddNewScheduleButton", "Verify the Add New Schedule button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawCommissionSchedules.btnAddNew, dc);
                #endregion commentout


                // 5. The fifth navigation test condition - navigate to Accounting | Transmission | B-template/DataMart

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTrans, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransDataMart, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawDataMart.bnrDataMart, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToDataMartPage", "Select ProAgWorks menus to navigate to B-template/DataMart Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawDataMart.bnrDataMart, LocatorCmd.Displayed,
                    "Find the Navigated Accounting Data Mart page bnrDataMart");
                dc.VerifyElemLocCmds("VerifyDataMartPage", "Verify Elements on the Data Mart Page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToDataMartPage", "Select ProAgWorks menus to navigate to B-template/DataMart Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu3[1] = proAgWorksHome.mnuAcntFileTrans;
                //locAccountingMnu3[2] = proAgWorksHome.mnuAcntFileTransDataMart;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu3, pawDataMart.bnrDataMart);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawDataMart.bnrDataMart, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting Data Mart page " + pawDataMart.bnrDataMart.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting Data Mart page: " + pawDataMart.bnrDataMart.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyDataMartAddButton", "Verify the Data Mart Add button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawDataMart.btnAdd, dc);
                #endregion commentout

                // 6. The sixth navigation test condition - navigate to Accounting | Transmission | NCIS

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTrans, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransNCIS, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawNCIS.bnrNCIS, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToNCISPage", "Select ProAgWorks menus to navigate to NCIS Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawNCIS.bnrNCIS, LocatorCmd.Displayed,
                    "Find the Navigated Accounting NCIS page bnrNCIS");
                dc.VerifyElemLocCmds("VerifyNCISPage", "Verify Elements on NCIS Page");

                // tjn commented
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToNCISPage", "Select ProAgWorks menus to navigate to NCIS Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu3[2] = proAgWorksHome.mnuAcntFileTransNCIS;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu3, pawNCIS.bnrNCIS);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawNCIS.bnrNCIS, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting NCIS page " + pawNCIS.bnrNCIS.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting NCIS page: " + pawNCIS.bnrNCIS.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNCISAddButton", "Verify the NCIS Add button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawNCIS.btnAdd, dc);
                #endregion commentout

                // 7. The seventh navigation test condition - navigate to Accounting | Transmission | Private Product Data Files | PAR File Export

                // add by tjn
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTrans, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDF, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDFPARExp, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPARExport.bnrPARExport, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToPARFileExportPage", "Select ProAgWorks menus to navigate to PAR File Export Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPARExport.bnrPARExport, LocatorCmd.Displayed,
                    "Find the Navigated Accounting PAR File Export page bnrPARExport");
                dc.VerifyElemLocCmds("VerifyPARFileExportPage", "Verify Elements on PAR File Export Page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToPARFileExportPage", "Select ProAgWorks menus to navigate to PAR File Export Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //ElementLocator[] locAccountingMnu4 = new ElementLocator[4];
                //locAccountingMnu4[0] = proAgWorksHome.mnuMainAccounting;
                //locAccountingMnu4[1] = proAgWorksHome.mnuAcntFileTrans;
                //locAccountingMnu4[2] = proAgWorksHome.mnuAcntFileTransPrivProdDF;
                //locAccountingMnu4[3] = proAgWorksHome.mnuAcntFileTransPrivProdDFPARExp;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu4, pawPPDFPARExport.bnrPARExport);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawPPDFPARExport.bnrPARExport, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting PAR File Export page " + pawPPDFPARExport.bnrPARExport.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting PAR Export page: " + pawPPDFPARExport.bnrPARExport.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyPARExportButton", "Verify the PAR Export button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawPPDFPARExport.btnExport, dc);
                #endregion commentout


                // 8. The eigth navigation test condition - navigate to Accounting | Transmission | Private Product Data Files | PAR Flex File Export

                // tjn added
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTrans, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDF, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDFPARFlexExp, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPARFlexExport.bnrPARFlexExport, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToPARFlexFileExportPage", "Select ProAgWorks menus to navigate to PAR Flex File Export Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPARFlexExport.bnrPARFlexExport, LocatorCmd.Displayed,
                    "Find the Navigated Accounting PAR Flex File Export page bnrPARFlexExport");
                dc.VerifyElemLocCmds("VerifyPARFlexFileExportPage", "Verify Elements on the PAR Flex File Export Page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToPARFlexFileExportPage", "Select ProAgWorks menus to navigate to PAR Flex File Export Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu4[3] = proAgWorksHome.mnuAcntFileTransPrivProdDFPARFlexExp;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu4, pawPPDFPARFlexExport.bnrPARFlexExport);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawPPDFPARFlexExport.bnrPARFlexExport, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting PAR Flex File Export page " + pawPPDFPARFlexExport.bnrPARFlexExport.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting PAR Flex File Export page: " + pawPPDFPARFlexExport.bnrPARFlexExport.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyPARFlexExportButton", "Verify the PAR Flex Export button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawPPDFPARFlexExport.btnExport, dc);
                #endregion commentout

                // 9. The nineth navigation test condition - navigate to Accounting | Transmission | Private Product Data Files | Price Flex File Export

                // tjn added
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTrans, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDF, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDFPriceFlexExp, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPriceFlexExport.bnrPriceFlexExport, LocatorCmd.WaitExist);
                dc.NavCmdsExecute("NavToPriceFlexFileExportPage", "Select ProAgWorks menus to navigate to Price Flex File Export Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPriceFlexExport.bnrPriceFlexExport, LocatorCmd.Displayed,
                    "Find the Navigated Accounting Price Flex File Export page bnrPriceFlexExport");
                dc.VerifyElemLocCmds("VerifyPriceFlexFileExportPage", "Verify Elements on the Price Flex File Export Page");

                // tjn commented out
                #region commentout
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToPriceFlexFileExportPage", "Select ProAgWorks menus to navigate to Price Flex File Export Page");

                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu4[3] = proAgWorksHome.mnuAcntFileTransPrivProdDFPriceFlexExp;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu4, pawPPDFPriceFlexExport.bnrPriceFlexExport);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawPPDFPriceFlexExport.bnrPriceFlexExport, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting Price Flex File Export page " + pawPPDFPriceFlexExport.bnrPriceFlexExport.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting Price Flex File Export page: " + pawPPDFPriceFlexExport.bnrPriceFlexExport.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyPriceFlexExportButton", "Verify the Price Flex Export button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawPPDFPriceFlexExport.btnExport, dc);
                #endregion commentout

                // 10. The tenth navigation test condition - navigate to Accounting | Transmission | Private Product Data Files | Price Flex Prime File Export
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToPriceFlexFileExportPage", "Select ProAgWorks menus to navigate to Price Flex File Export Page");

                // tjn added
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainAccounting, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTrans, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDF, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuAcntFileTransPrivProdDFPriceFlexPrimeExp, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPriceFlexPrimeExport.bnrPriceFlexPrimeExport, LocatorCmd.WaitTilDisplayed);
                dc.NavCmdsExecute("NavToPriceFlexPrimeFileExportPage", "Select ProAgWorks menus to navigate to Price Flex PrimeFile Export Page");

                // tjn added verify
                // todo We can add in other validation steps to verify other elements exist, but for now not needed
                dc.ElmLocCmds.AddToListLocElmCmd(pawPPDFPriceFlexPrimeExport.bnrPriceFlexPrimeExport, LocatorCmd.Displayed,
                    "Find the Navigated Accounting Price Flex Prime File Export page bnrPriceFlexPrimeExport");
                dc.VerifyElemLocCmds("VerifyPriceFlexPrimeFileExportPage", "Verify Elements on Price Flex Prime File Export Page");

                // tjn commented out
                #region commentout
                //dc.Driver.GetElement(proAgWorksHome.mnuMainAccounting, dc).WaitTillDisplayed();
                //locAccountingMnu4[3] = proAgWorksHome.mnuAcntFileTransPrivProdDFPriceFlexPrimeExp;
                //proAgWorksHome.SelectMenuMainSubMenu(locAccountingMnu4, pawPPDFPriceFlexPrimeExport.bnrPriceFlexPrimeExport);
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, dc);
                //if (dc.Driver.GetElement(pawPPDFPriceFlexPrimeExport.bnrPriceFlexPrimeExport, dc).Displayed)
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(" Found the Navigated Accounting Price Flex Prime File Export page " + pawPPDFPriceFlexPrimeExport.bnrPriceFlexPrimeExport.Value);
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find Expected Accounting Price Flex File Export page: " + pawPPDFPriceFlexPrimeExport.bnrPriceFlexPrimeExport.Value);
                //}

                // We can add in other validation steps to verify other elements exist, but for now not needed
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyPriceFlexExportButton", "Verify the Price Flex Prime Export button exists on the page");
                //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, pawPPDFPriceFlexPrimeExport.btnExport, dc);
                #endregion commentout


            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }

        }
    }
}
