// <copyright file="HomePage.cs" company="Objectivity Bespoke Software Specialists">
// Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>


namespace Ocaramba.Tests.PageObjects.PageObjects.ProAgWorks
{
    using System;
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

    public class HomePage : ProjectPageBase
    {
        //private string
        public static string xpathMainMenuContainer = "//ul[@id='linkMenu']";
        public static string xpathMainMenuToolsSubMenu = "//ul[@id='linkMenu']//span[text()='Tools']/../..";
        public static string xpathMainMenuFcicSubMenu = "//ul[@id='linkMenu']//span[text()='Fcic']/../..";

        // ProAgWorks HomePage ElementLocators
        public ElementLocator ddTools = new ElementLocator(Locator.XPath, "//li[contains(@id,'miTopLevel_Tools')]/a"),
            // tjn added
            // drop downs and lists
            ddAccount = new ElementLocator(Locator.XPath, "//ul[@id='userMenu']//a[@title='Account']"),
            ddTxtActiveUser = new ElementLocator(Locator.XPath, "//a[@id='activeUserNameContainer']"),
            ddSignOut = new ElementLocator(Locator.XPath, "//a[@href='/account/logoff']"),
            ddBtnRiYear = new ElementLocator(Locator.XPath, "//span[@aria-owns='SelectedYear_listbox']//span[@class='k-icon k-i-arrow-60-down']"),
            ddTxtRiYear = new ElementLocator(Locator.XPath, "//input[@id='SelectedYear']"),
            ddBtnSearchOptions = new ElementLocator(Locator.XPath, "//span[@aria-owns='SelectedQueryOID_listbox']//span[@class='k-icon k-i-arrow-60-down']"),
            ddLstSearchOptions = new ElementLocator(Locator.XPath, "//ul[@id='SelectedQueryOID_listbox']"),
            ddTxtSearchOptions = new ElementLocator(Locator.XPath, "//input[@id='SelectedQueryOID']"),

            // Tab elements 

            //Text / Edit fields

            // labels 

            // main menu container
            mnuMainReports = new ElementLocator(Locator.XPath, "//li[contains(@id,'miTopLevel_Reports')]/a"),
            mnuMPCIReporting = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_MPCIReporting')]/a"),
            mnuMPCIAgencyCommReport = new ElementLocator(Locator.LinkText, "MPCI Agency Commission Report"),
            //mnuMainAccounting = new ElementLocator(Locator.XPath, "//span[text()='Accounting']"),
            //mnuActRpts = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_Reports')]/a"),
            //mnuActRptsMpci = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_MPCI')]/a[text()='MPCI']"),
            //mnuActRptsMpciAgcyCommSt = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_MPCIAgencyCommissionStatement')]/a[text()='MPCI Agency Commission Statement']"),

            // --tdn added main menu accounting and its menu items for ExtraSmoke test case PBI 82425
            mnuMainAccounting = new ElementLocator(Locator.XPath, "//li[contains(@id, 'miTopLevel_Accounting')]/a"),
            mnuAcntARProcessing = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_ARProcessing')]/a"),
            mnuAcntARProcessingBilling = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_Billing')]/a[text()='Billing']"),
            mnuAcntARProcessingInterestBatch = new ElementLocator(Locator.XPath, "//li[contains(@id, 'miRoot_InterestBatch')]/a[text()='Interest Batch']"),
            mnuAcntARProcessingLockBox = new ElementLocator(Locator.XPath, "//li[contains(@id, 'miRoot_LockBox')]/a"),
            mnuAcntCommissionSchedules = new ElementLocator(Locator.XPath, "//li[contains(@id, 'miRoot_CommissionSchedules')]/a"),
            mnuAcntFileTrans = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_FileTransmissions')]/a"),
            mnuAcntFileTransDataMart = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_B-template/DataMart')]/a"),
            mnuAcntFileTransNCIS = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_NCIS')]/a"),
            mnuAcntFileTransPrivProdDF = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_PrivateProductDataFiles')]/a"),
            mnuAcntFileTransPrivProdDFPARExp = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_PARFileExport')]/a"),
            mnuAcntFileTransPrivProdDFPARFlexExp = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_ParFlexFileExport')]/a"),
            mnuAcntFileTransPrivProdDFPriceFlexExp = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_PriceFlexFileExport')]/a"),
            mnuAcntFileTransPrivProdDFPriceFlexPrimeExp = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_PriceFlexPrimeFileExport')]/a"),


            // -- main menu Compliance
            mnuMainCompliance = new ElementLocator(Locator.XPath, "//li[contains(@id,'miTopLevel_Compliance')]/a"),

            // --main menu Forms & Docs
            mnuMainFormsAndDocs = new ElementLocator(Locator.XPath, "//span[text()='Forms & Docs']"),
            mnuViewFormsAndDocs = new ElementLocator(Locator.XPath, "//li[contains(@id,'miRoot_ViewFormsandDocuments')]/a"),

            // --main menu tools
            mnuMainTools = new ElementLocator(Locator.XPath, xpathMainMenuContainer + "//span[text()='Tools']" ),
            mnuMainToolsBatchPrint = new ElementLocator(Locator.XPath, xpathMainMenuToolsSubMenu + "//a[text()='Batch Printing']"),
            mnuMainFcic = new ElementLocator(Locator.XPath, xpathMainMenuContainer + "//span[text()='FCIC']"),
            mnuMainFcicTransmissions = new ElementLocator(Locator.XPath, "//a[text()='Transmissions']"),
            mnuMainFcicErrorResolution = new ElementLocator(Locator.XPath, "//a[text()='Error Resolution']"),

            // existing pre tjn updated
            ddAgency = new ElementLocator(Locator.XPath, "//*[@id='FilterOptionsPanel']/table/tbody/tr/td[1]/table/tbody/tr/td/table/tbody/tr[2]/td[2]/button"),
            liAgency3 = new ElementLocator(Locator.XPath, "//*[@id='ui-multiselect-Agency-option-3']"),
            cbProductionReportAcreageReport = new ElementLocator(Locator.XPath, "//*[@id='ProductionAcreageThreeToAPageReportManager50_select']"),
            btnGenerateReport = new ElementLocator(Locator.XPath, "//*[@id='Postbutton']"),
            btnViewAndPrintBatches = new ElementLocator(Locator.XPath, "//*[@id='TabStrip']/ul/li[2]/span[2]"),
            btnRefreshBatchList = new ElementLocator(Locator.XPath, "//*[@id='GridPrintQueue']/div[5]/a[5]"),
            cbPrintQueueCheckAll = new ElementLocator(Locator.XPath, "//*[@id='GridPrintQueueCheckAll']"),
            btnDeleteSelected = new ElementLocator(Locator.CssSelector, ".a-banner-title > #Postbutton"),
            txtSearch = new ElementLocator(Locator.Id, "SearchValue"),
            btnSearch = new ElementLocator(Locator.Id, "ClassicSearchButton"),
            lnkPolicyInfo = new ElementLocator(Locator.XPath, "//*[@id=\"agworks-grid-grower-search\"]/table/tbody/tr[4]/td[2]/a"),
            lnkPolicies = new ElementLocator(Locator.XPath, "//*[@id=\"DetailsTabStrip\"]/ul/li[2]/span[2]"),
            lnkPolicyNum = new ElementLocator(Locator.XPath, "//*[@id=\"grid\"]/table/tbody/tr[1]/td[3]/a"),
            tabDetailLines = new ElementLocator(Locator.XPath, "//*[@id=\"MpciPolicyTabStrip\"]/ul/li[2]/span"),
            btnFastEditAr = new ElementLocator(Locator.Id, "ButtonFastEditAR"),
            btnSaveAndContinue = new ElementLocator(Locator.Id, "ButtonSaveAndContinueTop"),
            txtInsuredSignedAll = new ElementLocator(Locator.Id, "insuredSignAll"),
            btnSignatureMaintenanceApplyToAllButton = new ElementLocator(Locator.Id, "SignatureMaintenance_ApplyToAll_Button"),
            btnSignatureMaintenanceSaveButton = new ElementLocator(Locator.Id, "SignatureMaintenance_Save_Button"),
            
            // policy tab strip items
            tabMpciCoverages = new ElementLocator(Locator.XPath, "//div[@id='MpciPolicyTabStrip-1']"),
            
            // tjn added 
            // Panel Bar items not sure if we need?
            pnlBarGrowerPolicy = new ElementLocator(Locator.XPath, "//ul[@id='GrowerPanelBar']"),
            pnlBarGrowerPolicyNum = new ElementLocator(Locator.XPath, "//ul[@id='GrowerPanelBar']//a[contains(text(),'Policy Number')]"),
            pnlBarGrowerEligibilityInfo = new ElementLocator(Locator.XPath, "//div[@class='aa-panel mpci-a-panel']"),
            
            // tables and grids
            tblMpciClaimTracking = new ElementLocator(Locator.XPath, "//div[@id='MpciClaimTrackingGrid']//table/tbody"),
            tblPersonnelNameSearch = new ElementLocator(Locator.XPath, "//div[@id='IndividualLicensingResultGrid']//table/tbody"),
            tblAgencySearch = new ElementLocator(Locator.XPath, "//div[@id='AgencyGrid']//table/tbody"),
            tblGrowerSearch = new ElementLocator(Locator.XPath, "//div[@id='agworks-grid-grower-search']//table/tbody"),
            tblPrintQueue = new ElementLocator(Locator.XPath, "//div[@id='GridPrintQueue']//table/tbody"),

            // -- shared table element
            tblPolicyMpciCoverage = new ElementLocator(Locator.XPath, "//*[@id='MpciCoverageViewModelGrid']/table/tbody"),
            tblHdrsPolicyMpciCoverage = new ElementLocator(Locator.XPath, "//*[@id='MpciCoverageViewModelGrid']/table/thead"),
            //eleCounty = new ElementLocator(Locator.XPath, "//*[@id='MpciCoverageViewModelGrid']/table/tbody/tr[1]/td[2]/a"),

            //*****End locator not used
            zEndLocator = new ElementLocator(Locator.XPath, "//notused[@id='notused']");

        public HomePage(DriverContext driverContext)
            : base(driverContext)
        {
            // Set ElementLocator Logical Names
            this.SetLogNames(this);
        }

        #region new methods

        //public void NavToEdit(DriverContext dc, List<ElementLocator> listLocs)
        //{
        //    for (int i = 0; i < listLocs.Count; i++)
        //    {
        //        dc.ProAgXmlRsltLogger.LogStep("Java Click Elm Xpath: " + listLocs[i].Value);
        //        dc.Driver.GetElement(listLocs[i],dc).JavaScriptClick();
        //        ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //    }

            
        //}

        //public void VerifyListItems(DriverContext dc, List<ElementLocator> listLocs)
        //{
        //    for (int i = 0; i < listLocs.Count - 1; i++)
        //    {
        //        IWebElement elmList = dc.Driver.GetElement(listLocs[i], dc);
        //        SelectElement selElm = new SelectElement(elmList);
        //        IList<IWebElement> lstOpt = selElm.Options;
        //        if (lstOpt.Count > 0)
        //        {
        //            dc.ProAgXmlRsltLogger.LogPass("Elm list count > 0 XPath: " + listLocs[i].Value);
        //        }
        //        else
        //        {
        //            dc.ProAgXmlRsltLogger.LogFail("Elm list count = 0 XPath: " + listLocs[i].Value);
        //        }
        //    }

        //    // cancel button
        //    dc.Driver.GetElement(listLocs[listLocs.Count -1], dc).JavaScriptClick();
        //    ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
        //}

        /// <summary>
        /// Query to get existing claims for SearchSmokeTests
        /// </summary>
        /// <param name="dc">Current Driver Context</param>
        /// <param name="QueryAlias">The QueryAlias Name from loaded query excel file</param>
        /// <param name="RsltTblName">The name of the Memory Table to store result data in</param>
        public void WorksBasicQueryClaims(DriverContext dc, string QueryAlias, string RsltTblName)
        {
            // Get query from stored table
            string QuerySourceTblKey = "ProAgWorksQueries_WorksClaims";

            // Get Rows via alias
            Dictionary<string, string>[] dArrQueryInfo = dc.ProAgExcelReaderData.QueryLoadedDataGetDictionary(QuerySourceTblKey,
                "QueryAlias = '" + QueryAlias + "'");

            //Get 1st row
            Dictionary<string, string> QueryInfo = dArrQueryInfo[0];

            //Get Query 
            string sQuery = QueryInfo["Query"];
            string dbName = "AgWorksSubscriber"; // AgWorksSubscriber

            // reset db name in case was changed via override 
            string[] sArrDbName = dc.ProAgSqlHelper.CurDbName.Split('|');
            if (sArrDbName.Length == 1)
            {
                dbName = sArrDbName[0];
            }
            else
            {
                dbName = sArrDbName[1];

            }
            
            //open session to database
            dc.ProAgSqlHelper.OpenDbSession(dc, dbName);
            
            // start query timer
            Stopwatch swQuery = new Stopwatch();
            swQuery.Start();

            // Execute Query
            dc.ProAgSqlHelper.QueryGetRsltTable(dc, dbName, sQuery, RsltTblName);

            //stop query timer log time to execute query
            swQuery.Stop();
            string BrowserType = dc.Driver.GetDriverBrwsType();
            string QueryElapsedTime = swQuery.Elapsed.Seconds.ToString(CultureInfo.CurrentCulture);
            dc.ProAgXmlRsltLogger.LogInfo("Fixture Type: " + BrowserType + " Query Time: " + QueryElapsedTime + " seconds " + " Query Alias: "
                        + QueryAlias + " sheet: WorksClaims");
        }

        /// <summary>
        /// Query to get policy numbers for SearchSmokeTests
        /// </summary>
        /// <param name="dc">current driver context</param>
        /// <param name="QueryAlias">The QueryAlias Name from loaded query excel file</param>
        /// <param name="RsltTblName">The name of the Memory Table to store result data in</param>
        public void WorksBasicQueryPolicy(DriverContext dc, string QueryAlias, string RsltTblName)
        {
            // Get query from stored table
            string QuerySourceTblKey = "ProAgWorksQueries_WorksPolicyMpci";

            // Get Rows via alias
            Dictionary<string, string>[] dArrQueryInfo = dc.ProAgExcelReaderData.QueryLoadedDataGetDictionary(QuerySourceTblKey,
                "QueryAlias = '" + QueryAlias + "'");

            //Get 1st row
            Dictionary<string, string> QueryInfo = dArrQueryInfo[0];

            //Get Query 
            string sQuery = QueryInfo["Query"];
            string dbName = "AgWorksSubscriber"; 

            //open session to database
            dc.ProAgSqlHelper.OpenDbSession(dc, dbName);

            // reset db name in case was changed via override 
            string[] sArrDbName = dc.ProAgSqlHelper.CurDbName.Split('|');
            if (sArrDbName.Length == 1)
            {
                dbName = sArrDbName[0];
            }
            else
            {
                dbName = sArrDbName[1];
            }

            // start query timer
            Stopwatch swQuery = new Stopwatch();
            swQuery.Start();

            // Execute Query
            dc.ProAgSqlHelper.QueryGetRsltTable(dc, dbName, sQuery, RsltTblName);

            //stop query timer log time to execute query
            swQuery.Stop();
            string BrowserType = dc.Driver.GetDriverBrwsType();
            string QueryElapsedTime = swQuery.Elapsed.Seconds.ToString(CultureInfo.CurrentCulture);
            dc.ProAgXmlRsltLogger.LogInfo("Fixture Type: " +  BrowserType + " Query Time: " + QueryElapsedTime + " seconds " + " Query Alias: "
                        + QueryAlias + " sheet: WorksPolicyMpci");

        }

        /// <summary>
        /// Select main menus sub menu's
        /// How to create locator collection, below shows creating with two locators
        /// ElementLocator[] locToolsBatchPrint = new ElementLocator[2];
        /// locToolsBatchPrint[0] = proAgWorksHome.mnuMainTools;
        /// locToolsBatchPrint[1] = proAgWorksHome.mnuMainToolsBatchPrint;
        /// </summary>
        /// <param name="subMenus">Collection of menu locators to select</param>
        /// <param name="waitForElem"></param>
        public void SelectMenuMainSubMenu(ElementLocator[] subMenus, ElementLocator waitForElem)
        {
            DriverContext dc = this.DriverContext;
            for ( int iCurMnu = 0; iCurMnu < subMenus.Length; 
                iCurMnu++)
            {
                
                IWebElement menuElem = this.DriverContext.Driver.GetElement(subMenus[iCurMnu], dc);
                if (iCurMnu == 0)
                {
                    // click main menu
                    RsltLogger.LogStep("Click Main Menu "  + subMenus[iCurMnu].Value);
                    menuElem.JavaScriptClick();
                }
                else
                {
                    RsltLogger.LogStep("Click Sub Menu " + subMenus[iCurMnu].Value);
                    menuElem.JavaScriptClick();

                    #region use with mouseover example
                    // Use with mouseover example
                    //if(iCurMnu == (subMenus.Length - 1))
                    //{
                    //    menuElem.JavaScriptClick();
                    //}
                    //else
                    //{
                    //    menuElem.JavaScriptMouseOver();
                    //}
                    #endregion use with mouseover example

                }

            }
            // wait for processing and element to display
            ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, waitForElem, dc);
        }

        /// <summary>
        /// Main home page Selects Ri Year if current selection does not match year passed
        /// </summary>
        /// <param name="year">Re Year To Select</param>
        public void SelectRiYear(string year)
        {
            DriverContext dc = this.DriverContext;

            string curYr = this.Driver.GetElement(this.ddTxtRiYear, dc).GetAttribute("value");

            if (curYr.Trim() != year.Trim())
            {
                var selectYear = new ElementLocator(Locator.XPath, "//li[contains(text(),'" + year + "')]");
                IWebElement ddbRiYear = this.Driver.GetElement(this.ddBtnRiYear, dc);
                
                // click drop down arrow button to display Ri-year selections
                ddbRiYear.JavaScriptClick();

                // find year element to select
                IWebElement selectElement = this.Driver.GetElement(selectYear, dc);
                
                // Wait until year to select is displayed 
                selectElement.WaitTillDisplayed(3.0);

                // select the specified yr
                selectElement.JavaScriptClick();

                ProAgWebDriverExtensions.WaitPageLoadElem(this.Driver, dc);

                dc.ProAgXmlRsltLogger.LogInfo(" : RY selected " + year);
            }
            else
            {
                dc.ProAgXmlRsltLogger.LogInfo(" : RY " + year + " already selected");
            }
        }

        /// <summary>
        /// Select Search Drop Down Option Item for search to be performed
        /// </summary>
        /// <param name="SearchOptionTxt">The text of the search item to select, Example: Policy Number</param>
        public void SelectSearchOptions(string SearchOptionTxt)
        {
            DriverContext dc = this.DriverContext;

            //Note the value returns the idx of the currently selected item
            string curIdx = this.Driver.GetElement(this.ddTxtSearchOptions, dc).GetAttribute("value");

            //build xpath of item to select and get the element
            string xpathSelOpt = ddLstSearchOptions.Value + "//li[contains(text(), '" + SearchOptionTxt +"')]";
            ElementLocator locLstItemToSelect = new ElementLocator(Locator.XPath, xpathSelOpt);
            IWebElement lstItemToSelect = this.Driver.GetElement(locLstItemToSelect, dc);

            // get the value of data-offset-index for list item to select, this is = the dropdown list and xpath value - 1;
            string toSelIdx = lstItemToSelect.GetAttribute("data-offset-index").ToString();

            // add 1 from data-offset-index to get the list items xpath
            int idxToSel = Convert.ToInt32(toSelIdx) + 1;

            // see if item to select is already selected
            if (curIdx.Trim() != idxToSel.ToString().Trim())
            {
                // get the drop down arrow
                IWebElement ddBtnSearchOpt = this.Driver.GetElement(this.ddBtnSearchOptions, dc);
                
                // click drop down arrow button to display Ri-year selections
                ddBtnSearchOpt.JavaScriptClick();

                // need to get element again in case it is stale
                lstItemToSelect = this.Driver.GetElement(locLstItemToSelect, dc);

                // scroll element into view for click
                lstItemToSelect.JavaScriptSrollIntoView();

                // click the item in the list
                lstItemToSelect.JavaScriptClick();

                // wait for any page processing
                ProAgWebDriverExtensions.WaitPageLoadElem(this.Driver, dc);

                dc.ProAgXmlRsltLogger.LogInfo(" : Search Option selected " + SearchOptionTxt);
            }
            else
            {
                dc.ProAgXmlRsltLogger.LogInfo(" : Search Option: " + SearchOptionTxt + " already selected");
            }
        }

        /// <summary>
        /// Perform and Verify PrpAgWorks Search Results
        /// </summary>
        /// <param name="RiYr">Reinsurance to select for search</param>
        /// <param name="SearchOpt">Search option item text to select</param>
        /// <param name="SearchText">Date to be searched for</param>
        /// <param name="elmLoc">Expect element to find when search completes</param>
        public void VerifySearch(string RiYr, string SearchOpt, string SearchText, ElementLocator elmLoc)
        {
            //Make Refrence to Current Driver Context
            DriverContext dc = this.DriverContext;

            // get long wait time
            double maxWait = BaseConfiguration.LongTimeout;
            
            // Select Ri Year
            RsltLogger.LogStep("Select Reinsurance Year: " + RiYr);
            this.SelectRiYear(RiYr);

            // Select Search Option
            RsltLogger.LogStep("Select Search Option: " + SearchOpt);
            this.SelectSearchOptions(SearchOpt);

            //Enter Search Text, overwrite existing value
            RsltLogger.LogStep("Enter Search Text " + SearchText);
            DriverContext.Driver.GetElement(this.txtSearch, dc).SetAttribute("value", SearchText, true);

            // click search button
            RsltLogger.LogStep("Click btnSearch");
            DriverContext.Driver.GetElement(this.btnSearch, dc).JavaScriptClick();

            // wait for search text element to exist after clicking search button
            ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch, dc);

            //wait for expected specified element to exist after search
            ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, elmLoc, dc);

            // perform verification based on SearchOpt
            switch (SearchOpt.Trim().ToLower(CultureInfo.CurrentCulture))
            {
                case "grower name":
                    string xpathGrowerRows = elmLoc.Value + "/tr";
                    ElementLocator tblGrowerRow = new ElementLocator(Locator.XPath, xpathGrowerRows);
                    IList<IWebElement> tblGrowerRows = this.DriverContext.Driver.GetElements(tblGrowerRow, maxWait, this.DriverContext);
                    if (tblGrowerRows.Count <= 0)
                    {
                        string growerNmMsgFail = "Grower Name Search Text: " + SearchOpt + " table row count exp: > 0 act: " +
                                             tblGrowerRows.Count.ToString();
                        growerNmMsgFail = growerNmMsgFail + " Reinsurance Year Used: " + RiYr;
                        RsltLogger.LogFail(growerNmMsgFail);
                    }
                    else
                    {
                        string growerNmMsgPass = "Grower Name Search Text: " + SearchOpt + " table row count exp: > 0 act: " +
                                                 tblGrowerRows.Count.ToString();
                        growerNmMsgPass = growerNmMsgPass + " Reinsurance Year Used: " + RiYr;
                        RsltLogger.LogPass(growerNmMsgPass);
                    }
                    //ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch);
                    break;
                case "policy number":
                    // Verify MPCI Tab Coverages Shows
                    IWebElement elmTabMpciCoverages = Driver.GetElement(this.tabMpciCoverages, dc);
                    if (elmTabMpciCoverages == null)
                    {
                        // Fail Tab coverages not exist
                        string msgFailTabPpciCov = "Search Policy Number " + SearchText + " Tab MPCI Coverages Not Found";
                        RsltLogger.LogFail(msgFailTabPpciCov);
                    }
                    else
                    {
                        string msgPassTabPpciCov = "Search Policy Number " + SearchText + " Tab MPCI Coverages Found";
                        RsltLogger.LogPass(msgPassTabPpciCov);
                    }
                    
                    // Verify MPCI Table Coverages exits and row count > 0
                    IWebElement emlTblMpciCoverages = Driver.GetElement(this.tblPolicyMpciCoverage, dc);
                    if (elmTabMpciCoverages == null)
                    {
                        // Fail Table MPCI Coverages Not Found
                        string msgFailTblMpciCoverages = "Search Policy Number " + SearchText + " Table MPCI Coverages Not Found";
                        RsltLogger.LogFail(msgFailTblMpciCoverages);
                    }
                    else
                    {
                        string msgPassTblMpciCoverages = "Search Policy Number " + SearchText + " Table MPCI Coverages Found";
                        RsltLogger.LogPass(msgPassTblMpciCoverages);

                        // Verify Table Row Count
                        string xpathMpciCovRows = this.tblPolicyMpciCoverage.Value + "/tr";
                        ElementLocator tblMpciCoveragesRow = new ElementLocator(Locator.XPath, xpathMpciCovRows);
                        IList<IWebElement> rowsMpciCoverages = this.DriverContext.Driver.GetElements(tblMpciCoveragesRow, maxWait, this.DriverContext);
                        if (rowsMpciCoverages.Count <= 0)
                        {
                            // fail Rows Coverages count 0
                            string msgFailTblMpciRowCount = "Search Policy Number " + SearchText + " Table MPCI row count Exp: > 0 Act: " + rowsMpciCoverages.Count.ToString();
                            RsltLogger.LogFail(msgFailTblMpciRowCount);
                        }
                        else
                        {
                            string msgPassTblMpciRowCount = "Search Policy Number " + SearchText + " Table MPCI row count Exp: > 0 Act: " + rowsMpciCoverages.Count.ToString();
                            RsltLogger.LogPass(msgPassTblMpciRowCount);
                        }
                    }
                    ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch, dc);
                    break;
                case "agency name":
                    // Verify Table/Grid Agency Search Results shows and row count greater that 0
                    string xpathTblAgentNameRows = this.tblAgencySearch.Value + "/tr";
                    ElementLocator rowTblAgentNameSearch = new ElementLocator(Locator.XPath, xpathTblAgentNameRows);
                    IList<IWebElement> rowsAgentNameSearch = this.DriverContext.Driver.GetElements(rowTblAgentNameSearch, maxWait, this.DriverContext);
                    if (rowsAgentNameSearch.Count <= 0)
                    {
                        string msgAgentNameSearchFail = "Search Agent Name " + SearchText + " Table Agent Search row count Exp: > 0 Act: " + rowsAgentNameSearch.Count.ToString(); ;
                        RsltLogger.LogFail(msgAgentNameSearchFail);
                    }
                    else
                    {
                        string msgAgentNameSearchPass = "Search Agent Name " + SearchText + " Table Agent Search row count Exp: > 0 Act: " + rowsAgentNameSearch.Count.ToString(); ;
                        RsltLogger.LogPass(msgAgentNameSearchPass);
                    }
                    ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch, dc);
                    break;
                case "agency code":
                    // 713246-00
                    string xpathTblAgentCodeRows = this.tblAgencySearch.Value + "/tr";
                    ElementLocator rowTblAgentCodeSearch = new ElementLocator(Locator.XPath, xpathTblAgentCodeRows);
                    IList<IWebElement> rowsAgentCodeSearch = this.DriverContext.Driver.GetElements(rowTblAgentCodeSearch, maxWait, this.DriverContext);
                    if (rowsAgentCodeSearch.Count <= 0)
                    {
                        string msgAgentCodeSearchFail = "Search Agent Code " + SearchText + " Table Agent Code Search row count Exp: > 0 Act: " + rowsAgentCodeSearch.Count.ToString(); ;
                        RsltLogger.LogFail(msgAgentCodeSearchFail);
                    }
                    else
                    {
                        string msgAgentCodeSearchPass = "Search Agent Code " + SearchText + " Table Agent Code Search row count Exp: > 0 Act: " + rowsAgentCodeSearch.Count.ToString(); ;
                        RsltLogger.LogPass(msgAgentCodeSearchPass);
                    }
                    ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch, dc);
                    break;
                case "personnel name":
                    // aa tblPersonnelNameSearch
                    string xpathTblPersonelNmRows = this.tblPersonnelNameSearch.Value + "/tr";
                    ElementLocator rowTblPersonnelNmSearch = new ElementLocator(Locator.XPath, xpathTblPersonelNmRows);
                    IList<IWebElement> rowsPersonnelNmSearch = this.DriverContext.Driver.GetElements(rowTblPersonnelNmSearch, maxWait, this.DriverContext);
                    if (rowsPersonnelNmSearch.Count <= 0)
                    {
                        string msgAgentCodeSearchFail = "Search Personnel Name " + SearchText + " Table Personnel Search row count Exp: > 0 Act: " + rowsPersonnelNmSearch.Count.ToString(); ;
                        RsltLogger.LogFail(msgAgentCodeSearchFail);
                    }
                    else
                    {
                        string msgAgentCodeSearchPass = "Search Personnel Name " + SearchText + " Table Personnel Search row count Exp: > 0 Act: " + rowsPersonnelNmSearch.Count.ToString(); ;
                        RsltLogger.LogPass(msgAgentCodeSearchPass);
                    }
                    ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch, dc);
                    break;
                case "claim number":
                    // tblMpciClaimTracking
                    string xpathTblMpciClaimTrackingRows = this.tblMpciClaimTracking.Value + "/tr";
                    ElementLocator rowTblMpciClaimTrackingSearch = new ElementLocator(Locator.XPath, xpathTblMpciClaimTrackingRows);
                    IList<IWebElement> rowsMpciClaimTrackingSearch = this.DriverContext.Driver.GetElements(rowTblMpciClaimTrackingSearch, maxWait, this.DriverContext);
                    if (rowsMpciClaimTrackingSearch.Count <= 0)
                    {
                        string msgAgentCodeSearchFail = "Search Claim Number " + SearchText + " Table Claim Search row count Exp: > 0 Act: " + rowsMpciClaimTrackingSearch.Count.ToString(); ;
                        RsltLogger.LogFail(msgAgentCodeSearchFail);
                    }
                    else
                    {
                        string msgAgentCodeSearchPass = "Search Claim Number " + SearchText + " Table Claim Search row count Exp: > 0 Act: " + rowsMpciClaimTrackingSearch.Count.ToString(); ;
                        RsltLogger.LogPass(msgAgentCodeSearchPass);
                    }
                    ProAgWebDriverExtensions.WaitPageLoadElem(this.DriverContext.Driver, this.txtSearch, dc);
                    break;
                default:
                    string defaulMsg = "Search Option: " + SearchOpt + " does not have a verification case";
                    RsltLogger.LogFail(defaulMsg);
                    break;
            }

        }

        /// <summary>
        /// Sign out / Log off of ProAgWorks
        /// </summary>
        /// <returns></returns>
        public void LogOffProAgWorks()
        {
            RsltLogger.AddTestStepNd("LogOffProAgWorks", "Click Account drop down list, then click Sign Out");

            RsltLogger.LogStep("Click Dropdown Account");
            this.Driver.GetElement(this.ddAccount, this.DriverContext).JavaScriptClick();

            RsltLogger.LogStep("Click Dropdown Item SignOut");
            this.Driver.GetElement(this.ddSignOut, this.DriverContext).JavaScriptClick();

            // wait for sign out processing
            ProAgWebDriverExtensions.WaitPageLoadElem(this.Driver, this.ddAccount, true, this.DriverContext);

            //Verify Sign Out worked
            if(ProAgWebDriverExtensions.WaitElementNotDisplayed(this.Driver, this.ddAccount))
            {
                RsltLogger.LogPass("Click Sign Out worked " + this.ddAccount.Value + " NOT found after Sign Out");
            }
            else
            {
                RsltLogger.LogFail("Click Sign Out did not work " + this.ddAccount.Value + " STILL found after Sign Out");
            }
            
        }


        public void TblWaitRowCountGt(DriverContext dc, ElementLocator locTbl, int iRowGtVal)
        {
            string xPathTblRow = locTbl.Value + "//tr";
            ElementLocator locTblRows = new ElementLocator(Locator.XPath, xPathTblRow);
            //int iWaited = 0;
            int iMaxWait = Convert.ToInt32(BaseConfiguration.LongTimeout);

            for (int iCurWait = 0; iCurWait <= iMaxWait; iCurWait++)
            {
                IList<IWebElement> elmRows = dc.Driver.GetElements(locTblRows,dc);
                if (elmRows != null)
                {
                    int iTblRowCount = elmRows.Count;
                    if (iTblRowCount > iRowGtVal)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
                
            }
        }
        public int TblGetColIdxFromName(DriverContext dc, ElementLocator locTbl, string relXpathToColHdrs, string colName)
        {
            int iRsltIdx = 0;

            // Get All columns

            string xPathTblHdrs = string.Empty;//locTbl.Value + relXpathToColHdrs;
            if (locTbl.Value.Contains("//tbody"))
            {
                xPathTblHdrs = locTbl.Value.Replace("//tbody", relXpathToColHdrs);
            }
            else if (locTbl.Value.Contains("/tbody"))
            {
                xPathTblHdrs = locTbl.Value.Replace("/tbody", relXpathToColHdrs);
            }
            else
            {
                xPathTblHdrs = locTbl.Value + relXpathToColHdrs;
            }
            ElementLocator locTblhdrRmaTrans = new ElementLocator(Locator.XPath, xPathTblHdrs);
            IList<IWebElement> headers = dc.Driver.GetElements(locTblhdrRmaTrans, dc);

            if (headers != null)
            {
                if (headers.Count > 0)
                {
                    // loop through headers find header text and return 1 based index
                    for (int curIdx = 0; curIdx < headers.Count; curIdx++)
                    {
                        // build xpath to cur header text
                        string sIdxTh = (curIdx + 1).ToString();
                        string xPathHdrTxt = xPathTblHdrs + "[" + sIdxTh + "]";
                        
                        ElementLocator locColHdrLnk = new ElementLocator(Locator.XPath, xPathHdrTxt);
                        IWebElement elmColHdLnk = dc.Driver.GetElement(locColHdrLnk, dc);
                        if (elmColHdLnk != null)
                        {
                            string curColHdrTxt = elmColHdLnk.GetAttribute("innerText").ToString().Trim();
                            if (curColHdrTxt.ToLower() == colName.Trim().ToLower())
                            {
                                iRsltIdx = curIdx + 1;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // error headers count not > 0
                    string msgHeadersCount0 =
                        "GetTblColIdxFromName find headers for Mapped Table Name: " 
                        + locTbl.ToString() + " element list count is 0, search col name" + colName 
                        + " xpath to headers: " + xPathTblHdrs ;
                    dc.ProAgXmlRsltLogger.LogFatal(msgHeadersCount0);
                }
            }
            else
            {
                // error headers object is null
                string msgHeadersCountNull =
                    "GetTblColIdxFromName find headers Element NULL for Mapped Table Name: "
                    + locTbl.ToString() + " element list count is 0, search col name" + colName
                    + " xpath to headers: " + xPathTblHdrs; 
                dc.ProAgXmlRsltLogger.LogFatal(msgHeadersCountNull);
            }

            return iRsltIdx;
        }

        public string TblGetCellValue(DriverContext dc, ElementLocator locTbl, string sRowNum, string sColNum,
            string sAttributeName)
        {
            return TblGetCellValue(dc, locTbl, sRowNum, sColNum, "", sAttributeName);
        }
        public string TblGetCellValue(DriverContext dc, ElementLocator locTbl, string sRowNum, string sColNum,
            string sAdditionalXpath, string sAttributeName)
        {
            string sCellValue = "ERROR";

            // build xpath to cell
            string xpathRowColForVal = locTbl.Value + "//tr[" + sRowNum + "]//td[" + sColNum + "]";
            if (!string.IsNullOrEmpty(sAdditionalXpath))
            {
                // add additional xpath if needed
                xpathRowColForVal = xpathRowColForVal + sAdditionalXpath;
            }

            try
            {
                // get the cell value
                ElementLocator locCell = new ElementLocator(Locator.XPath, xpathRowColForVal);
                IWebElement elmCel = dc.Driver.GetElement(locCell, dc);
                sCellValue = elmCel.GetAttribute(sAttributeName);
            }
            catch(Exception exGetCellVal)
            {
                // Exception Trying to Get Cell value
                string msgExGetCEllVal = "Exception Get Cell Value, xpathToCell: " + xpathRowColForVal +
                                         " Exception Msg: " + exGetCellVal;
                // use Assert Fail vs Verify.that because exception occured
                Assert.Fail(msgExGetCEllVal);
            }
            

            return sCellValue;
        }

        public void TblWaitCellValueToChange(DriverContext dc, ElementLocator locTbl, string sRowNum, string sColNum,
            string startValue, string sAttribNm)
        {
            TblWaitCellValueToChange(dc, locTbl, sRowNum, sColNum, startValue, sAttribNm, "");
        }
        public void TblWaitCellValueToChange(DriverContext dc, ElementLocator locTbl, string sRowNum, string sColNum, string startValue, 
                                            string sAttribNm, string sAdditionalXpathCell)
        {
            // build xpath to cell
            if (locTbl.Kind != Locator.XPath)
            {
                string msgLocatorKindNeedToBeXpath = "WaitTblCellValueToChange locTbl kind needs to be Locator.XPath to dynamically build cell xPath " +
                    " Re-Map Table ElementLocator or use different Table method ";
                // use Assert.Fail vs Verify.That because need hard fail here
                Assert.Fail(msgLocatorKindNeedToBeXpath);
            }

            // wait for table cell attribute value to change
            try
            {
                string xPathCurCell = locTbl.Value + "//tr[" + sRowNum + "]/td[" + sColNum + "]";
                if (!string.IsNullOrEmpty(sAdditionalXpathCell))
                {
                    xPathCurCell = xPathCurCell + sAdditionalXpathCell;
                }

                ElementLocator locIdCell = new ElementLocator(Locator.XPath, xPathCurCell);
                int iWaited = 0;
                int iMaxWait = Convert.ToInt32(BaseConfiguration.ProAgWorksMaxWaitCreateBatchReport);
                for (int iCurWait = 0; iCurWait < iMaxWait; iCurWait++)
                {
                    IWebElement elmIdCol = dc.Driver.FindElement(locIdCell.ToBy());
                    string curID = elmIdCol.GetAttribute(sAttribNm);
                    if (curID.Trim().ToLower() != startValue.Trim().ToLower())
                    {
                        break;
                    }
                    else
                    {
                        // wait  1 sec and try again
                        iWaited = iWaited + 1;
                        Thread.Sleep(1000);
                    }
                }

                if (iWaited >= iMaxWait)
                {
                    // log Warning WaitForTblCell
                    string msgWarnWaitTblCellValueToChange = "WaitTblCellValueToChange find cell XPath: " +
                                                          xPathCurCell + " MaxWait of: " + iMaxWait.ToString() +
                                                          " Reached";
                    Assert.Warn(msgWarnWaitTblCellValueToChange);
                }
            }
            catch (Exception exWaitTblCellValueToChange)
            {
                string msgExWaitTblRowCellToChange =
                    "WaitTblCellValueToChange unhandled exception Msg: " + exWaitTblCellValueToChange.Message;
                Assert.Fail(msgExWaitTblRowCellToChange);
            }
            

        }

        public int GetRowIdxFromRowVal(DriverContext dc, ElementLocator locTbl, string colHdrTxt, string cellExpVal,
            string attrName)
        {
            return GetRowIdxFromRowVal(dc, locTbl, colHdrTxt, cellExpVal, attrName, "");
        }
        /// <summary>
        /// Get Tbl row index that contains cellExpValue based on colHdrTxt and attrName passed passed
        /// </summary>
        /// <param name="dc">Current DriverContext</param>
        /// <param name="colHdrTxt">Column header text for cell index</param>
        /// <param name="cellExpVal">Expected text that cell value should contain, expected text can't be longer then actual cell text</param>
        /// <param name="attrName">Name of attribute that holds the cell value </param>
        /// <returns></returns>
        public int GetRowIdxFromRowVal(DriverContext dc, ElementLocator locTbl,  string colHdrTxt, string cellExpVal, 
            string attrName, string sAdditionalXpathCEll)
        {
            int iRslt = 0;

            string xPathTblRows = locTbl.Value + "//tr";
            int colIdx = TblGetColIdxFromName(dc,  locTbl, "//th", colHdrTxt);

            if (colIdx > 0)
            {
                // get row elements
                ElementLocator locTblRows = new ElementLocator(Locator.XPath, xPathTblRows);
                IList<IWebElement> tblRows = dc.Driver.GetElements(locTblRows, dc);

                if (tblRows != null)
                {
                    if (tblRows.Count > 0)
                    {
                        // loop through all column rows until cell contains value
                        for (int curRowIdx = 0; curRowIdx < tblRows.Count; curRowIdx++)
                        {
                            // xpath to row column cell
                            string sRow = (curRowIdx + 1).ToString();
                            string sCol = colIdx.ToString();
                            string xPathCurCell = xPathTblRows + "[" + sRow + "]/td[" + sCol + "]";
                            if (!string.IsNullOrEmpty(sAdditionalXpathCEll))
                            {
                                xPathCurCell = xPathCurCell + xPathCurCell;
                            }
                            ElementLocator locCell = new ElementLocator(Locator.XPath, xPathCurCell);
                            IWebElement elmCell = dc.Driver.GetElement(locCell, dc);

                            if (elmCell != null)
                            {
                                string cellValue = elmCell.GetAttribute(attrName).Trim().ToLower();
                                if (cellValue.Contains(cellExpVal.Trim().ToLower()))
                                {
                                    iRslt = curRowIdx + 1;
                                    break;
                                }

                            }

                        }
                    }
                    else
                    {
                        // error list of table rows returned 0
                        string msgTblRows0 = "GetRowIdxFromRowVal Rows XPath: " + xPathTblRows + 
                                             " get list of rows returned 0";
                        // Hard Fail use Assert.Fail because no rows to search
                        Assert.Fail(msgTblRows0);
                    }
                }
                else
                {
                    // error rows list returned null
                    string msgTblRowsNull = "GetRowIdxFromRowVal Rows XPath: " + xPathTblRows +  
                                                " get list of rows returned null";
                    // Hard Fail use Assert.Fail because null means now rows to search
                    Assert.Fail(msgTblRowsNull);
                }
            }
            else
            {
                // error header text returned index of 0
                string msgHeaderTxtIdx0 = "GetRowIdxFromRowVal Col Hdr Txt: " + colHdrTxt + " get header index returned 0";
                // Hard Fail use Assert.Fail because Col Hdr Idx  should be 1 or higher 
                Assert.Fail(msgHeaderTxtIdx0);
            }

            return iRslt;
        }
        #endregion new methods

        #region Pre POM Methods
        //public HomePage OpenHomePage()
        //{
        //    //// var url = "https://uat-proagworks.proag.com/Portal";
        //    //var url = "https://testautomation-proagworks.proag.com";

        //    // Get HomePage URL based in AutoProjConfig.xml settings
        //    var url = DriverContext.ProAgXmlHelper.GetProAgAutoProjUrl;

        //    this.Driver = DriverContext.Driver;
        //    this.Driver.NavigateTo(new Uri(url));

        //    Logger.Info(CultureInfo.CurrentCulture, "Opening page: ", url);
        //    return this;
        //}

        //public HomePage DeleteAllCookies()
        //{
        //    this.Driver.Manage().Cookies.DeleteAllCookies();
        //    return this;
        //}


        //public HomePage ClickViewAndPrintBatches()
        //{
        //    this.Driver.GetElement(this.btnViewAndPrintBatches, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, " Clicked on View and Print Batches ");
        //    return this;
        //}

        //public HomePage AssertBatchPrintComplete()
        //{
        //    for (int i = 0; i < 30; i++)
        //    {
        //        var lblStatusComplete = this.Driver.FindElements(By.XPath("//td[text()='Complete']")).Count;
        //        var lblStatusProcessing = this.Driver.FindElements(By.XPath("//td[text()='Processing']")).Count;
        //        Logger.Info(CultureInfo.CurrentCulture, "Status Complete = " + lblStatusComplete);
        //        Logger.Info(CultureInfo.CurrentCulture, "Status Processing = " + lblStatusProcessing);
        //        Logger.Info(CultureInfo.CurrentCulture, "Looping = " + i);

        //        if (lblStatusProcessing == 0)
        //        {
        //            Logger.Info(CultureInfo.CurrentCulture, "Processing Complete ");

        //            this.OpenBatchReport();

        //            this.Driver.GetElement(this.cbPrintQueueCheckAll, this.DriverContext).Click();
        //            this.Driver.GetElement(this.btnDeleteSelected, this.DriverContext).Click();

        //            IAlert a = this.Driver.SwitchTo().Alert();
        //            a.Accept();
        //            Logger.Info(CultureInfo.CurrentCulture, "Deleted Completed Batches");
        //            break;
        //        }
        //        else
        //        {
        //            // click refresh button, sleep and try again.
        //           this.Driver.GetElement(this.btnRefreshBatchList, this.DriverContext).Click();
        //            Thread.Sleep(5000);
        //        }
        //    }

        //    return this;
        //}

        //public HomePage SelectAgency()
        //{
        //    this.Driver.GetElement(this.ddAgency, this.DriverContext).Click();
        //    this.Driver.GetElement(this.liAgency3, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Click on Batch Printing ");
        //    return this;
        //}



        //public HomePage SelectFormsToPrint()
        //{
        //    this.Driver.GetElement(this.cbProductionReportAcreageReport, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Selected: Production Report / Acreage Report ");
        //    return this;
        //}

        //public HomePage GenerateReport()
        //{
        //    this.Driver.GetElement(this.btnGenerateReport, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked Generate Report ");
        //    return this;
        //}

        //public HomePage OpenBatchReport()
        //{
        //    Actions actions = new Actions(this.Driver);
        //    IWebElement batchReportRequest = this.Driver.FindElement(By.XPath("//*[@id='GridPrintQueue']/div[4]/table/tbody/tr[1]/td[4]/a[contains(text(),'Batch')]"));
        //    actions.ContextClick(batchReportRequest).Perform();

        //    // Click on Edit link on the displayed menu options
        //    // IWebElement menu = this.Driver.FindElement(By.LinkText("Open link in new tab"));
        //    // actions.Click(menu);
        //    return this;
        //}

        //public HomePage SearchPolicy(string searchCriteria)
        //{
        //    this.Driver.GetElement(this.txtSearch, this.DriverContext).Clear();
        //    this.Driver.GetElement(this.txtSearch, this.DriverContext).SendKeys(searchCriteria);
        //    this.Driver.GetElement(this.btnSearch, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Search for this value: " + searchCriteria);

        //    return this;
        //}

        //public HomePage SelectPolicy()
        //{
        //    this.Driver.GetElement(this.lnkPolicyInfo, this.DriverContext).Click();

        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked Policy Info: ");

        //    return this;
        //}

        //public HomePage ClickPolicesLink()
        //{
        //    this.Driver.GetElement(this.lnkPolicies, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked on Polcies Link ");
        //    return this;
        //}

        //public HomePage ClickPolicyNumLink()
        //{
        //    this.Driver.GetElement(this.lnkPolicyNum, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked on Polciy Num Link ");
        //    return this;
        //}

        //public HomePage ClickDetailLinesTab()
        //{
        //    this.Driver.GetElement(this.tabDetailLines, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked Details Tab ");
        //    return this;
        //}

        //public HomePage ClickFastEditAr()
        //{
        //    this.Driver.GetElement(this.btnFastEditAr, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked Fast Edit AR ");
        //    return this;
        //}

        //public HomePage ClickSaveAndContinue()
        //{
        //    this.Driver.GetElement(this.btnSaveAndContinue, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked Save and Continue ");
        //    return this;
        //}

        //public HomePage MaintainSignatures(string date)
        //{
        //    this.Driver.GetElement(this.txtInsuredSignedAll, this.DriverContext).SendKeys(date);
        //    this.Driver.GetElement(this.btnSignatureMaintenanceApplyToAllButton, this.DriverContext).Click();
        //    this.Driver.GetElement(this.btnSignatureMaintenanceSaveButton, this.DriverContext).Click();
        //    Logger.Info(CultureInfo.CurrentCulture, "Clicked Save and Continue ");
        //    return this;
        //}


        #endregion Pre POM Methods
    }
}
