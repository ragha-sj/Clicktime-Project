

namespace Ocaramba.Tests.ProAgWorks.AllEnvTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
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

    
    [Parallelizable(ParallelScope.Fixtures)]
    // Get test fixtures from AutoFixtures.xml
    [TestFixtureSource(typeof(ProAgGetFixtures), "FixtureArgsRun")]
    public class ProAgWorksSmokeTests: ProjectTestBase
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        //private DriverContext dc;

        // HomePage and LogOnPage need for Most Projects Remove if not needed
        public LogOnPage logon;
        // Make sure to reference correct HomePage
        public HomePage proAgWorksHome;
        public PolicyMpci proAgWorksPolicyMpci;
        public Transmissions proAgWorksTrans;
        public MpciAgencyCommissionReport proAgWorksAgencyCommRpt;
        public BatchPrinting proAgWorksBatchPrinting;
        public FormsAndDocs proAgFormsDocs;
        public Compliance proAgCompliance;
        public ErrorResolution proAgErrorResolution;

        // Test Entry and makes call to ProjectTestBase to init environment
        public ProAgWorksSmokeTests(object fixture)
            : base(fixture)
        {
            // this test has been configured so it can be run against production
            DriverContext.TestIsProdReady = true;

            if (string.IsNullOrEmpty(DriverContext.CurEnv))
            {
                DriverContext.CurEnv = BaseConfiguration.getProAgGetConfigXmlEnv();
            }

            // New Defaults for all environments
            this.ProAgDbNameForDbUserId = ProAgSqlHelper.ProAgDbNames.SkipGetDbUserId;
            switch (DriverContext.CurEnv.ToLower().Trim())
            {
                case "si-prod":
                case "si-uat":
                    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksCreds.xlsx", "SiTestData", 0);
                    break;
                case "ti-prod":
                case "ti-uat":
                    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksCreds.xlsx", "TiTestData", 0);
                    break;
                default:
                    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksCreds.xlsx", "ProdTestData", 0);
                    break;
             
            }
            DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksCreds.xlsx", "ProAgWorks", 0);


            // Set ProAgDbName for Getting DbUserId from a Database or Skip
            
            switch (DriverContext.CurEnv.ToLower().Trim())
            {
                case "prod":
                case "ti-prod":
                case "si-prod":
                    // prod get prod test user
                    // Use correct parameters based on how you setup Creds.xlsx file above
                    this.SetLoginInfo("ProAgWorksCreds_ProAgWorks", "GroupAlias", "ProdProAgWorksUser");
                    break;
                default:
                    // Non prod get test user
                    // Use correct parameters based on how you setup Creds.xlsx file above
                    this.SetLoginInfo("ProAgWorksCreds_ProAgWorks", "GroupAlias", "CommonProAgWorks");

                    // load Claims Query
                    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksQueries.xlsx", "WorksClaims", 0);

                    // load Mpci policy queries
                    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksQueries.xlsx", "WorksPolicyMpci", 0);
                    break;
            }
            //if (DriverContext.CurEnv.ToLower().Trim() == BaseConfiguration.ProAgEnvList["EnvProd"].ToLower())//DriverContext.EnvProd)
            //{
               
            //    // prod get prod test user
            //    // Use correct parameters based on how you setup Creds.xlsx file above
            //    this.SetLoginInfo("ProAgWorksCreds_ProAgWorks", "GroupAlias", "ProdProAgWorksUser");

            //}
            //else
            //{
            //    // Non prod get test user
            //    // Use correct parameters based on how you setup Creds.xlsx file above
            //    this.SetLoginInfo("ProAgWorksCreds_ProAgWorks", "GroupAlias", "CommonProAgWorks");

            //    // load Claims Query
            //    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksQueries.xlsx", "WorksClaims", 0);

            //    // load Mpci policy queries
            //    DriverContext.ProAgExcelReaderData.ExcelLoadDataWorksheet("\\Data", "ProAgWorksQueries.xlsx", "WorksPolicyMpci", 0);
            //}
        }

        #region local helper methods
        /// <summary>
        /// Get test data from loaded excel worksheet. Worksheet to get data from must be loaded prior
        /// The TestName, DataKey and DataValue columns hold the values to be used
        /// Query is based on TestName which should match test method name which is TestTitle property on DriverContext
        /// Match on TestName and DataKey returns data value.
        /// </summary>
        /// <param name="dataKey">The DataKey for the current TestName to Return</param>
        /// <returns>DataValue matching TestName and DataKey as string</returns>
        private string getProdExcelDataValue(string dataKey)
        {
            DriverContext dc = this.DriverContext;
            string sRslt = null;
            string sTestDataTblName = string.Empty;
            switch(DriverContext.CurEnv.ToLower().Trim())
            {
                case "si-prod":
                case "si-uat":
                    sTestDataTblName = "ProAgWorksCreds_SiTestData";
                    break;
                case "ti-prod":
                case "ti-uat":
                    sTestDataTblName = "ProAgWorksCreds_TiTestData";
                    break;
                default:
                    sTestDataTblName = "ProAgWorksCreds_ProdTestData";
                    break;
            }
            string dataSelect = "TestName = '" + dc.TestTitle + "' AND DataKey = ";
            sRslt = dc.ProAgExcelReaderData.DataWorksheets.Tables[sTestDataTblName]
                .Select(dataSelect + "'" + dataKey+ "'")[0]["DataValue"].ToString();
            return sRslt;
        }


        /// <summary>
        /// Get Table RMS Transmission Column Index Based Column Name
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="iRow"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        private int getTblRmsTransIdColValue(DriverContext dc, int iRow, int iCol)
        {
            int iRslt = 0;
            string sRow = iRow.ToString();
            string sCol = iCol.ToString();
            string xPathIDCell = proAgWorksTrans.tblRmaTransmissions.Value + "/tr[" + sRow + "]/td[" + sCol + "]/a";
            ElementLocator locIDCell = new ElementLocator(Locator.XPath, xPathIDCell);
            iRslt = Convert.ToInt32(dc.Driver.GetElement(locIDCell, dc).GetAttribute("innerText"));
            
            return iRslt;
        }
        
        #endregion local helper methods

        [SetUp]
        public void TestInit()
        {
            // Need to create / Initialize Page objects here after webdriver is created 
            // HomePage and LogOn Page will be needed by most programs
            logon = new LogOnPage(this.DriverContext);
            
            // Make sure to reference correct HomePage
            proAgWorksHome = new PageObjects.PageObjects.ProAgWorks.HomePage(this.DriverContext);

            // Make Reference to PolicyMpci
            proAgWorksPolicyMpci = new PolicyMpci(this.DriverContext);

            // Reference Transmission page objects
            proAgWorksTrans = new PageObjects.PageObjects.ProAgWorks.Transmissions(this.DriverContext);

            // Reference Accounting Report Agency Commission Report
            proAgWorksAgencyCommRpt = new MpciAgencyCommissionReport(this.DriverContext);

            // Reference Tools Batch Printing
            proAgWorksBatchPrinting = new BatchPrinting(this.DriverContext);

            // Reference Forms & Docs
            proAgFormsDocs = new FormsAndDocs(this.DriverContext);

            // Reference Compliance
            proAgCompliance = new Compliance(this.DriverContext);

            // reference ErrorResolution
            proAgErrorResolution = new ErrorResolution(this.DriverContext);

        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
	    [Description("Login and Logout of ProAgWorks as Employee Admin")]
        [Property("TFS-TC-ID", "66930")]
        public void ALoginLogout()
        {
            DriverContext dc = this.DriverContext;

            try
            {
                // Navigate to home page for login
                dc.NavToStartUrl();
                
                // LogOn from common LogOn page user must belong to application being logged into
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);

                // wait for mapping policy search page to load
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyLogon", "Verify ProAgWorks home page search edit field exists");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);
                if (dc.Driver.GetElement(proAgWorksHome.txtSearch, dc).Displayed)
                {
                    dc.ProAgXmlRsltLogger.LogPass(" Found search edit field with desc: " + proAgWorksHome.txtSearch.Value);
                }
                else
                {
                    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find search edit field with desc: " + proAgWorksHome.txtSearch.Value);
                }

                //logout 
                dc.Driver.GetElement(proAgWorksHome.ddAccount,dc).WaitTillDisplayed();
                
                proAgWorksHome.LogOffProAgWorks();
            }
            catch (Exception exUnHandled)
            {
               //Log Exception information;
                this.TestBaseCaughtException(dc, exUnHandled);
            }
            
        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Search Smoke Tests, Verify Grower Name, Policy Number, Agency Name, Personal Name, Claim Number")]
        [Property("TFS-TC-ID", "66930")]
        [Property("TFS-TC-ID", "65735")]
        public void SearchSmokeTests()
        {
            DriverContext dc = this.DriverContext;
            try
            {
                string curYear = ProAgDateTime.GetYearString(0);
                string policyYr = string.Empty;
                //string policyRsltTbl = "tblExistingPolicies";
                string claimYr = string.Empty;
                //string claimRsltTbl = "tblExistingClaims";
                string prevYear = ProAgDateTime.GetYearString(-1);
                string policyNum = "5453"; //5634      1042952
                string agencyCode = "11291-00";
                string agencyName = "aa";
                string claimNum = "MP100131"; //19000020 20000616 20000139 MP100131
                string personnelName = "aa";
                string growerName = "aa";

                // get prod data from load excel data file
                dc.ProAgXmlRsltLogger.AddTestStepNd("GetExcelTestData", "Get Test Data From ProAgWorksCreds_ProdTestData");
                curYear = getProdExcelDataValue("curYear");
                growerName = getProdExcelDataValue("growerName");
                policyYr = getProdExcelDataValue("policyYr");
                policyNum = getProdExcelDataValue("policyNum");
                agencyName = getProdExcelDataValue("agencyName");
                agencyCode = getProdExcelDataValue("agencyCode");
                personnelName = getProdExcelDataValue("personnelName");
                claimYr = getProdExcelDataValue("claimYr");
                claimNum = getProdExcelDataValue("claimNum");

                #region orig query non prod, prod read from excel
                // Get Test Data Based on Env
                //if (dc.CurEnv == dc.EnvProd)
                //{
                //    // get prod data from load excel data file
                //    curYear = getProdExcelDataValue("curYear");
                //    growerName = getProdExcelDataValue("growerName");
                //    policyYr = getProdExcelDataValue("policyYr");
                //    policyNum = getProdExcelDataValue("policyNum");
                //    agencyName = getProdExcelDataValue("agencyName");
                //    agencyCode = getProdExcelDataValue("agencyCode");
                //    personnelName = getProdExcelDataValue("personnelName");
                //    claimYr = getProdExcelDataValue("claimYr");
                //    claimNum = getProdExcelDataValue("claimNum");
                //}
                //else
                //{
                //    // get data from datable loaded from query to AgWorksSubscriber
                //    // execute policy query
                //    proAgWorksHome.WorksBasicQueryPolicy(dc, "GetMpciPoliciesForSmoke", policyRsltTbl);
                //    policyYr = dc.ProAgSqlHelper.QueryResults.Tables[policyRsltTbl].Rows[0]["ReinsuranceYear"].ToString();
                //    policyNum = dc.ProAgSqlHelper.QueryResults.Tables[policyRsltTbl].Rows[0]["policynumber"].ToString();

                //    // execute claim query  
                //    proAgWorksHome.WorksBasicQueryClaims(dc, "ClaimsOpenOrClosedSmoke", claimRsltTbl);
                //    claimYr = dc.ProAgSqlHelper.QueryResults.Tables[claimRsltTbl].Rows[0]["RiYr"].ToString();
                //    claimNum = dc.ProAgSqlHelper.QueryResults.Tables[claimRsltTbl].Rows[0]["ClaimNumber"].ToString();

                //    growerName = getProdExcelDataValue("growerName");
                //    agencyName = getProdExcelDataValue("agencyName");
                //    agencyCode = getProdExcelDataValue("agencyCode");
                //    personnelName = getProdExcelDataValue("personnelName");
                //}
                #endregion orig query non prod, prod read from excel

                // Navigate to home page for login
                dc.NavToStartUrl();

                // LogOn from common LogOn page user must belong to application being logged into
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);


                // wait for mapping policy search page to load
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);
                dc.Driver.GetElement(proAgWorksHome.txtSearch, dc).WaitTillDisplayed();

                // Verify Search Options List > 0 and Items to select for search test exist
                // test search option claims exists
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddBtnSearchOptions, LocatorCmd.SelectClickDropDownArrow);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListGtZero, "false");
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListItemTextExists, "Grower Name|false");
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListItemTextExists, "Policy Number|false");
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListItemTextExists, "Agency Name|false");
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListItemTextExists, "Agency Code|false");
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListItemTextExists, "Personnel Name|false");
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.ddLstSearchOptions, LocatorCmd.VerifyOpenListItemTextExists, "Claim Number|true");

                dc.VerifyElemLocCmds("VerifySearchOptionList", "Test Verify Search Option List");

                // VerifyGrowerNameSearch
                dc.ProAgXmlRsltLogger.AddTestStepNd("SearchGrowerName" , " Search Text " + growerName);
                proAgWorksHome.VerifySearch(curYear, "Grower Name", growerName, proAgWorksHome.tblGrowerSearch);

                // VerifyPolicyNumSearch
                dc.ProAgXmlRsltLogger.AddTestStepNd("SearchPolicyNum", " Search Text " + policyNum);
                proAgWorksHome.VerifySearch(policyYr, "Policy Number", policyNum, proAgWorksHome.tabMpciCoverages);

                // VerifyAgencyNameSearch
                dc.ProAgXmlRsltLogger.AddTestStepNd("SearchAgencyName", " Search Text " + agencyName);
                proAgWorksHome.VerifySearch(curYear, "Agency Name", agencyName, proAgWorksHome.tblAgencySearch);

                // VerifyAgencyCodeSearch
                dc.ProAgXmlRsltLogger.AddTestStepNd("SearchAgencyCode", " Search Text " + agencyCode);
                proAgWorksHome.VerifySearch(curYear, "Agency Code", agencyCode, proAgWorksHome.tblAgencySearch);

                // VerifyPersonnelNameSearch
                dc.ProAgXmlRsltLogger.AddTestStepNd("SearchPersonnelName", " Search Text " + personnelName);
                proAgWorksHome.VerifySearch(curYear, "Personnel Name", personnelName, proAgWorksHome.tblPersonnelNameSearch);

                // VerifyClaimCodeSearch 
                dc.ProAgXmlRsltLogger.AddTestStepNd("SearchClaimNum", " Search Text " + claimNum);
                proAgWorksHome.VerifySearch(claimYr, "Claim Number", claimNum, proAgWorksHome.tblMpciClaimTracking);
            }
            catch (Exception exUnHandled)
            {
                
                this.TestBaseCaughtException(dc, exUnHandled);
            }
            
        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Batch Print, Verify Print Agency Report using agency with minimal policy holders to reduce time")]
        [Property("TFS-TC-ID", "65965")]
        public void BatchPrintTest()
        {
            DriverContext dc = this.DriverContext;
            try
            {
                // declare vars need for test
                string batchPrintRiYr = string.Empty;
                string batchPrintProcessingSeason = string.Empty;
                string batchPrintAgencyCode = string.Empty;
                string batchPrintReportDesc = "POMAutoTest";
                string batchPrintFormToPrint = "Acreage Report";
                string batchPrintExpResult = "Complete";

                // get data check Current Env to determine how to get data
                dc.ProAgXmlRsltLogger.AddTestStepNd("GetBatchPrintData", "Get data needed to perform batch print test");
                batchPrintRiYr = getProdExcelDataValue("batchPrintRiYr");
                batchPrintProcessingSeason = getProdExcelDataValue("batchPrintProcessingSeason");
                batchPrintAgencyCode = getProdExcelDataValue("batchPrintAgencyCode");
                batchPrintReportDesc = getProdExcelDataValue("batchPrintReportDesc");
                batchPrintFormToPrint = getProdExcelDataValue("batchPrintFormToPrint");
                batchPrintExpResult = getProdExcelDataValue("batchPrintExpResult");

                //start the tests steps
                // Navigate to home page for login
                dc.NavToStartUrl();

                // LogOn from common LogOn page user must belong to application being logged into
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);


                // wait for mapping policy search page to load
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyLogon", "Verify ProAgWorks home page search edit field exists");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);
                if (dc.Driver.GetElement(proAgWorksHome.txtSearch,dc).Displayed)
                {
                    dc.ProAgXmlRsltLogger.LogPass(" Found search edit field with desc: " + proAgWorksHome.txtSearch.Value);
                }
                else
                {
                    dc.ProAgXmlRsltLogger.LogFail(" Did NOT Find search edit field with desc: " + proAgWorksHome.txtSearch.Value);
                }

                // Navigate to Batch Print Page
                dc.ProAgXmlRsltLogger.AddTestStepNd("NavToBatchPrintPage", "Select ProAgWorks menus to navigate to Batch Print Page");
                dc.Driver.GetElement(proAgWorksHome.mnuMainTools,dc).WaitTillDisplayed();
                ElementLocator[] locToolsBatchPrint = new ElementLocator[2];
                locToolsBatchPrint[0] = proAgWorksHome.mnuMainTools;
                locToolsBatchPrint[1] = proAgWorksHome.mnuMainToolsBatchPrint;
                proAgWorksHome.SelectMenuMainSubMenu(locToolsBatchPrint, proAgWorksBatchPrinting.ddLstBatchPrntProductType);

                // select RiYr 
                dc.ProAgXmlRsltLogger.AddTestStepNd("CreateBatchPrintJob", "Set batch print selection and enter batch print desc");
                dc.ProAgXmlRsltLogger.LogStep("Select Reinsurance Year: " + batchPrintRiYr);
                proAgWorksBatchPrinting.BatchPrintSelectRiYear(batchPrintRiYr);

                // select processing season
                dc.ProAgXmlRsltLogger.LogStep("Select Processing Season: " + batchPrintProcessingSeason);
                proAgWorksBatchPrinting.BatchPrintSelectProcessingSeason(batchPrintProcessingSeason);

                // select Agency
                dc.ProAgXmlRsltLogger.LogStep("Search for and Select Agency with Code: " + batchPrintAgencyCode);
                proAgWorksBatchPrinting.BatchPrintSelectAgency(batchPrintAgencyCode);

                // select batch print report name
                dc.ProAgXmlRsltLogger.LogStep("Select Report Name: " + batchPrintFormToPrint);
                proAgWorksBatchPrinting.BatchPrintSelectReportName(batchPrintFormToPrint);

                // enter batch print description
                string finalDesc = proAgWorksBatchPrinting.BatchPrintSetRptDesc(batchPrintReportDesc);
                dc.ProAgXmlRsltLogger.LogStep("Set Report Desc: " + finalDesc);

                // Verify Generate Report
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyBatchPrint", "Verify print job created, completes and description can be found");
                proAgWorksBatchPrinting.VerifyBatchPrintGenRpt(finalDesc, batchPrintExpResult);

            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }

        }

        
        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Navigate MPCI Policy Tabs, Search for valid MPCI Policy, navigate to Detail Lines, Grower, Policies, Policy Balance, Etc.")]
        [Property("TFS-TC-ID", "65735")]
        [Property("TFS-TC-ID", "66930")]
        public void NavPolicyMPCITest()
        {
            DriverContext dc = this.DriverContext;
            try
            {
                string policyYr = string.Empty;
                string policyNum = string.Empty;

                // Navigate to home page for login
                dc.NavToStartUrl();

                // LogOn from common LogOn page user must belong to application being logged into
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);


                // wait for mapping policy search page to load
                dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);
                dc.Driver.GetElement(proAgWorksHome.txtSearch, dc).WaitTillDisplayed();

                // get data for MPCINavTest from ProData Worksheet
                dc.ProAgXmlRsltLogger.AddTestStepNd("GetPolicyYearAndNum", "Get policy yr and policy number to use with test");
                policyYr = getProdExcelDataValue("policyYr");
                policyNum = getProdExcelDataValue("policyNum");

                // Search policy and verify Verify Search successful
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifySearchPolicyNum", "Verify Policy Number Search Success");
                proAgWorksHome.VerifySearch(policyYr, "Policy Number", policyNum, proAgWorksHome.tabMpciCoverages);

                proAgWorksPolicyMpci.VerifyPolicMPCINavTabsTblsMain();

                #region old tab nav and verify
                //// Navigate all policy tabs
                //// Click Details Tab verify details table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabDetails", "Click Details Tab verify details table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyDetailLines.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyDetailLines, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksPolicyMpci.tblMpciDetailLineGrid, dc);
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciDetailLineGrid,
                //             "Details Tab Detail Lines Grid");

                //// Click Grower tab and verify elements and tables
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabGrowers", "Click Growers Tab verify details table Address Grid and Grower Sbi");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyGrower.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyGrower, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //// --Verify Grower Elements
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyGrowerEligibilityInfo, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyGrowerEligibilityInfo.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyGrowerEligibilityInfo.Value + " NOT Displayed");
                //}

                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyGrowerTaxIdType, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyGrowerTaxIdType.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyGrowerTaxIdType.Value + " NOT Displayed");
                //}

                //// si - does not exist, but business name does, ti -
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyGrowerInfo, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyGrowerInfo.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyGrowerInfo.Value + " NOT Displayed");
                //}

                //// --Verify Grower Address table
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciGrowerAddressGrid,
                //             "Grower Tab Grower Address Grid View");
                //// --Verify Grower Sbi grid row 
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciGrowerSbiInfoGrid,
                //             "Grower Tab Grower Sbi Grid View");

                //// Click Claims Tab and verify claims table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabClaims", "Click Claims Tab and verify claims table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyClaims.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyClaims, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyCliamsViewGrid,
                //             "Claims Tab Claims Grid View");

                //// Click Attachments Tab and verify Attachments Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabAttachments", "Click Attachments Tab and verify Attachments Table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyAttachments.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyAttachments, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyAttachmentsViewGrid,
                //             "Attachments Tab Attachment Grid View");

                //// Click Change Log Tab and verify Change Log Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabChangeLog", "Click Change Log Tab and verify Change Log Table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyChangeLog.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyChangeLog, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyChangeLogViewGrid,
                //             "Change Log Tab Change Grid View");

                //// Click Notes Tab and Verify Notes Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabNotes", "Click Notes Tab and Verify Notes Table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyNotes.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyNotes, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.txtMpicPolicyNotes, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.txtMpicPolicyNotes.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.txtMpicPolicyNotes.Value + " NOT Displayed");
                //}
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyNotesViewGrid,
                //             "Notes Tab Notes View Grid");

                //// Click Print Tab and Verify Elements and Print Report Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPrint", "Click Print Tab and Verify Elements and Print Report Table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyPrint.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyPrint, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lstPolicyPrintSelCrops, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lstPolicyPrintSelCrops.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lstPolicyPrintSelCrops.Value + " NOT Displayed");
                //}
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyPrintRptViewGrid,
                //             "Print Tab Print Report Grid");

                //// Click Utilities Tab and verify Elements and a Utilities table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabUtilities", "Click Utilities Tab and verify Elements and a Utilities table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyUtilities.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyUtilities, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyUtilities, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyUtilities.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyUtilities.Value + " NOT Displayed");
                //}
                
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyUtilitiesGrid,
                //             "Utilities Tab Utilities Grid");

                //// si - has estimator tab, ti - has estimator and map(not sure what it is used for open blank popup window) 
                //switch (dc.CurEnv.ToLower().Trim())
                //{
                //    case "si-uat":
                //    case "si-prod":
                //    case "ti-uat":
                //    case "ti-prod":
                //        dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabEstimator", "Click Estimator Tab and verify Elements and a Estimator table");
                //        dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyEsitmator.Value);
                //        dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyEsitmator, dc).JavaScriptClick();
                //        ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //        if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyEstimator, dc).WaitTillDisplayed())
                //        {
                //            dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyEstimator.Value + " Displayed");
                //        }
                //        else
                //        {
                //            dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyEstimator.Value + " NOT Displayed");
                //        }

                //        proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyEstimator,
                //            "Estimator Tab Estimator Grid");

                //        proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyEstimatorManageTemp,
                //            "Estimator Tab Manage Attachments Grid");

                //        proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyEstimatorAttachments,
                //            "Estimator Tab Attachments Grid");

                //        break;
                //    default:
                //        // do nothing
                //        break;
                //}

                //// Click Coverages Tab and verify Coverages Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabCoverages", "Click Coverages Tab and verify Coverages Table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyCoverages.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyCoverages, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //// Note below tblPolicyMpciCoverage is passed and is shared table as is Home page mapping
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksHome.tblPolicyMpciCoverage,
                //             "Coverages Tab Table MPCI Coverages Grid");

                //// Click Policies tab double wait time because policies tab needs to load all policies for grower
                //// Verify Policies Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicies", "Click Verify Policies Tab Table");
                //double dPoliciesWait = BaseConfiguration.LongTimeout * 2.0;
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyPolicies.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyPolicies, dc).JavaScriptClick();
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.tblMpciPoliciesGrid, dc).WaitTillDisplayed(dPoliciesWait))
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.tblMpciPoliciesGrid.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.tblMpciPoliciesGrid.Value + " NOT Displayed");
                //}
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dPoliciesWait, dc);
                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPoliciesGrid,
                //             "Policies Tab Policies Grid View");

                //// Click Policy Balance Tab and Verify Elements and Policy Balance Table
                //dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyNavTabPolicyBalance", "Click Policy Balance Tab and Verify Elements and Policy Balance Table");
                //dc.ProAgXmlRsltLogger.LogStep("Click Tab: " + proAgWorksPolicyMpci.elmTabPolicyBalance.Value);
                //dc.Driver.GetElement(proAgWorksPolicyMpci.elmTabPolicyBalance, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);
                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyBalanceHeader, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyBalanceHeader.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyBalanceHeader.Value + " NOT Displayed");
                //}

                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyBalanceGrossPremium, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyBalanceGrossPremium.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyBalanceGrossPremium.Value + " NOT Displayed");
                //}

                //if (dc.Driver.GetElement(proAgWorksPolicyMpci.lblPolicyBalanceTotalIndemity, dc).WaitTillDisplayed())
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyBalanceTotalIndemity.Value + " Displayed");
                //}
                //else
                //{
                //    dc.ProAgXmlRsltLogger.LogPass(proAgWorksPolicyMpci.lblPolicyBalanceTotalIndemity.Value + " NOT Displayed");
                //}

                //proAgWorksPolicyMpci.VerifyPolicyNavTabTable(dc, proAgWorksPolicyMpci.tblMpciPolicyBalanceTransactionGrid,
                //            "Policy Balance Tab Policy Balance Transaction Grid");

                #endregion old tab nav and verify
            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }

           
        }
      

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Open FCIC Transmission Page, Click Add button, Set RI Year, Trans Type, Desc, Agency Code , Verify Trans Status Created")]
        [Property("TFS-TC-ID", "68750")]
        public void RMATransmissionAddTest()
        {
            DriverContext dc = this.DriverContext;
            try
            {
                //start
                // set default test data
                string addRmaTransRiYr = string.Empty;
                string addRmaTransType = string.Empty;
                string addRmaTransDesc = string.Empty;
                string addRmaTransAgencyCode = "POMAutoTestRmsTrans";
                string addRmtTransExpStatus = "Created|Added";
                //string addRmtTransExpStatusNoLink = "Added";

                // get data needed for test
                dc.ProAgXmlRsltLogger.AddTestStepNd("LoadTestData", "Load data needed to execute test");
                addRmaTransRiYr = getProdExcelDataValue("addRmaTransRiYr");
                addRmaTransType = getProdExcelDataValue("addRmaTransType");
                addRmaTransDesc = getProdExcelDataValue("addRmaTransDesc");
                addRmaTransAgencyCode = getProdExcelDataValue("addRmaTransAgencyCode");
                addRmtTransExpStatus = getProdExcelDataValue("addRmtTransExpStatus");

                // Navigate to home page for login
                dc.NavToStartUrl();

                // LogOn from common LogOn page user must belong to application being logged into
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);

                #region old wait for poragworks main page to load tjn 9/10/2020
                // wait for mapping policy search page to load
                //dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);
                #endregion old wait for poragworks main page to load tjn 9/10/2020

                //new wait for ProAgWorks Main Page to Load
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.txtSearch, LocatorCmd.WaitTilDisplayed);
                dc.NavCmdsExecute("WaitLogonComplete", "Wait ProAgWorks Logon to complete");

                #region old nav steps tjn 9/10/2020
                // navigate to add transmission
                //dc.ProAgXmlRsltLogger.AddTestStepNd("NavToAddRmaTransmission", "navigate to add transmission page");
                //dc.Driver.GetElement(proAgWorksHome.mnuMainFcic, dc).WaitTillDisplayed();
                //ElementLocator[] locFcicTrans = new ElementLocator[2];
                //locFcicTrans[0] = proAgWorksHome.mnuMainFcic;
                //locFcicTrans[1] = proAgWorksHome.mnuMainFcicTransmissions;
                //proAgWorksHome.SelectMenuMainSubMenu(locFcicTrans, proAgWorksTrans.tblColRmaTransmission);
                #endregion old nav steps tjn 9/10/2020

                // new navigate to add transmission
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainFcic, LocatorCmd.WaitTilDisplayed);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainFcic, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksHome.mnuMainFcicTransmissions, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.tblColRmaTransmission, LocatorCmd.WaitTilDisplayed);
                dc.NavCmdsExecute("NavToAddRmaTransmission", "navigate to add transmission page");

                // get column indexes for ID and Status
                int colIdxId = proAgWorksTrans.GetTblColIdxFromName(dc, "ID");
                int colIdxStatus = proAgWorksTrans.GetTblColIdxFromName(dc, "Status");

                // get ID row 1 prior to add RMA Transmission
                int startId = getTblRmsTransIdColValue(dc, 1, colIdxId);
                dc.ProAgXmlRsltLogger.LogStep("Before Add RMA Get Row 1 Column ID as StartID: " + startId.ToString());

                #region old steps create transmission tjn 9/10/2020
                // open add transmission 
                //dc.ProAgXmlRsltLogger.AddTestStepNd("CreateAddRmaTransmission", "open add transmission page and populate field to create Transmission ");
                //dc.ProAgXmlRsltLogger.LogStep("Click Add Transmission");
                //dc.Driver.GetElement(proAgWorksTrans.btnTransAdd, dc).JavaScriptClick();
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);


                // populate add transmission fields
                // -select Ri Yr
                //dc.ProAgXmlRsltLogger.LogStep("Select RMA Reinsurance Year: " + addRmaTransRiYr);
                //dc.Driver.GetElement(proAgWorksTrans.ddBtnTransAddRiYear, dc).JavaScriptClick();
                //string xpathRiYrItem = proAgWorksTrans.ddLstTransAddMainRiYear.Value + "//li[text()='" + addRmaTransRiYr.Trim() + "']";
                //ElementLocator locRiYrItem = new ElementLocator(Locator.XPath, xpathRiYrItem);
                //dc.Driver.GetElement(locRiYrItem, dc).WaitTillDisplayed();
                //dc.Driver.GetElement(locRiYrItem, dc).JavaScriptClick();

                // -Select TransmissionType
                //dc.ProAgXmlRsltLogger.LogStep("Select RMA Transmission Type: " + addRmaTransType);
                //dc.Driver.GetElement(proAgWorksTrans.ddBtnTransAddMainTransType, dc).JavaScriptClick();
                //string xpathTransTypeItem = proAgWorksTrans.ddLstTransAddMainTransType.Value + "//li[text()='" + addRmaTransType + "']";
                //ElementLocator locTransTypeItem = new ElementLocator(Locator.XPath, xpathTransTypeItem);
                //dc.Driver.GetElement(locTransTypeItem, dc).WaitTillDisplayed();
                //dc.Driver.GetElement(locTransTypeItem, dc).JavaScriptClick();

                // -Set Transmission Description
                //string transDescDtStamp = ProAgDateTime.GetMonthDayYearHrMinSecMs();
                //string brwsType = dc.Driver.GetDriverBrwsType();
                //addRmaTransDesc = addRmaTransDesc + transDescDtStamp + brwsType;
                //dc.Driver.GetElement(proAgWorksTrans.txtTransDescription, dc).SetAttribute("value", addRmaTransDesc);
                //dc.ProAgXmlRsltLogger.LogStep("Set RMA Transmission Desc: " + addRmaTransDesc);
                //dc.Driver.GetElement(proAgWorksTrans.txtTransAgencyCodes, dc).SetAttribute("value", addRmaTransAgencyCode);
                //dc.ProAgXmlRsltLogger.LogStep("Set RMA Tranmission Agency Code: " + addRmaTransAgencyCode);


                // -get date time prior to saving transmission
                //dc.ProAgXmlRsltLogger.LogStep("Click btn Save Transmission");
                //dc.Driver.GetElement(proAgWorksTrans.btnTransSave, dc).JavaScriptClick();

                // wait for Transmission table to exist
                //ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksTrans.tblColRmaTransmission, dc);
                #endregion old steps create transmission

                // new steps create
                // -build description 
                string transDescDtStamp = ProAgDateTime.GetMonthDayYearHrMinSecMs();
                string brwsType = dc.Driver.GetDriverBrwsType();
                addRmaTransDesc = addRmaTransDesc + transDescDtStamp + brwsType;
                //build create steps
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.btnTransAdd, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddBtnTransAddRiYear, LocatorCmd.WaitTilDisplayed);
                //dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddBtnTransAddRiYear, LocatorCmd.SelectClickDropDownArrow);
                //dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddLstTransAddMainRiYear, LocatorCmd.SelectOpenListItemText, addRmaTransRiYr);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddLstTransAddMainRiYear, LocatorCmd.SelectKendoListText, addRmaTransRiYr);
                //dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddBtnTransAddMainTransType, LocatorCmd.JsClick);
                //dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddLstTransAddMainTransType, LocatorCmd.SelectOpenListItemText, addRmaTransType);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.ddLstTransAddMainTransType, LocatorCmd.SelectKendoListText, addRmaTransType);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.txtTransDescription, LocatorCmd.SetAttribute, "value|" + addRmaTransDesc);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.txtTransAgencyCodes, LocatorCmd.SetAttribute, "value|" + addRmaTransAgencyCode);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.btnTransSave, LocatorCmd.JsClick);
                dc.ElmLocCmds.AddToListLocElmCmd(proAgWorksTrans.tblColRmaTransmission, LocatorCmd.WaitTilDisplayed);
                // perform steps
                dc.NavCmdsExecute("CreateAddRmaTransmission", "open add transmission page and populate field to create Transmission ");


                // refresh browser
                dc.Driver.Navigate().Refresh();

                // wait for Transmission table
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksTrans.tblColRmaTransmission, dc);

                // wait for row 1 col ID value to be different from start id
                proAgWorksTrans.WaitTblTransIDColRow1ValToChange(dc, startId.ToString());

                // get current ID Row 1 after add
                int endId = getTblRmsTransIdColValue(dc, 1, colIdxId);
                dc.ProAgXmlRsltLogger.LogStep("After Add RMA Get Row 1 Column ID as EndID: " + endId.ToString());

                //wait until status created for end id to start id
                dc.ProAgXmlRsltLogger.AddTestStepNd("WaitCreateComplete", "wait for add transmission to complete");
                proAgWorksTrans.WaitForAddRmaCreated(dc, addRmtTransExpStatus, startId, endId);

                // search end index to start index until description found
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyAddRmaTransmission", "Verify Add Transmission completes and description found");
                proAgWorksTrans.VerifyAddRmsTransCreate(dc, addRmaTransDesc, startId, endId);

            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }
            
        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Create Accounting MPCI Agency Commission Statement, Verify report created via name, start date time and end date time")]
        [Property("TFS-TC-ID", "65901")]
        [Property("TFS-TC-ID", "65965")]
        [Property("TFS-TC-ID", "73416")]
        public void MPCIAcctCommStTest()
        {
            //Driver Context Reference For Test
            DriverContext dc = this.DriverContext;

            try
            {
                // set variable defaults
                string statementYr = string.Empty;
                string agencySelect = string.Empty;
                string agentSelect = string.Empty;
                string useChkNonCommTrans = string.Empty;
                string reportStatus = "Complete";
                string reportName = string.Empty;

                // get test data from excel ProdTestData worksheet
                dc.ProAgXmlRsltLogger.AddTestStepNd("LoadTestData", "Load Test Data for Creating MPCI Commission statement");
                statementYr = getProdExcelDataValue("statementYr");
                agencySelect = getProdExcelDataValue("agencySelect");
                agentSelect = getProdExcelDataValue("agentSelect");
                useChkNonCommTrans = getProdExcelDataValue("useChkNonCommTrans");
                reportStatus = getProdExcelDataValue("reportStatus");
                reportName = getProdExcelDataValue("reportName");
                reportName = reportName.Trim() + " " + statementYr;

                // Navigate to home page for login proAgWorksAgencyCommRpt * View Agents link "//a[text()='View Agents']" new tab
                dc.NavToStartUrl();

                // LogOn from common LogOn page user must belong to application being logged into *Agent Table "//div[@id='AgentkendoGrid']//table/tbody"
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);

                // wait for Works policy search page to load Agent Table Comp Test 'Yes' click link name col
                dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);

                // build menu selections to navigate to Accounting \ Reports \ MPCI Reports \ MPCI Agency Commission Statement
                dc.ProAgXmlRsltLogger.AddTestStepNd("NavToCreateCommSt", "navigate to Accounting \\ Reports \\ MPCI Reports \\ MPCI ");
                dc.Driver.GetElement(proAgWorksHome.mnuMainReports,dc).WaitTillDisplayed();
                ElementLocator[] locAcctCommRpt = new ElementLocator[3];
                locAcctCommRpt[0] = proAgWorksHome.mnuMainReports;
                locAcctCommRpt[1] = proAgWorksHome.mnuMPCIReporting;
                locAcctCommRpt[2] = proAgWorksHome.mnuMPCIAgencyCommReport;
                //locAcctCommRpt[3] = proAgWorksHome.mnuActRptsMpciAgcyCommSt;

                // wait for page MPCI Agency Commission Statement to display
                proAgWorksHome.SelectMenuMainSubMenu(locAcctCommRpt, proAgWorksAgencyCommRpt.lstAllAgencies);
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, dc);

                // Create MPCI Agency Commission Statement
                dc.ProAgXmlRsltLogger.AddTestStepNd("CreateCommSt", "Create MPCI Agent Commission statement");
                proAgWorksAgencyCommRpt.CreateMpciAgencyCommRpt(dc,statementYr,agencySelect,agentSelect, useChkNonCommTrans);
                
                // Wait for tblPrintQueue to eixst and Row Count > 0
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.tblPrintQueue, dc);
                proAgWorksHome.TblWaitRowCountGt(dc, proAgWorksHome.tblPrintQueue, 0);

                // get Status, Batch and Report Description columns after click print and tblPrintQueue exists
                int iStatus = proAgWorksHome.TblGetColIdxFromName(dc, proAgWorksHome.tblPrintQueue, "//th", "Status");
                string sColStatus = iStatus.ToString();

                int iBatch = proAgWorksHome.TblGetColIdxFromName(dc, proAgWorksHome.tblPrintQueue, "//th", "Batch");
                string sColBatch = iBatch.ToString();

                int iRptDesc = proAgWorksHome.TblGetColIdxFromName(dc, proAgWorksHome.tblPrintQueue, "//th", "Report Description");
                string sColRptDesc = iRptDesc.ToString();

                // Wait for Report Status and Report Batch Number
                string sStartBatchVal = string.Empty;
                string sStartStatusVal = proAgWorksHome.TblGetCellValue(dc, proAgWorksHome.tblPrintQueue, "1",
                    sColStatus, "innerText");

                // Wait for for tblPrintQueue status to complete and return Column Batch cell value
                sStartBatchVal = proAgWorksAgencyCommRpt.WaitForMpciAgencyCommRpt(this, dc, proAgWorksHome.tblPrintQueue, 
                                                                 sColStatus,sColBatch);

                // Find row index with Batch cell value 
                int iCurBatchRow = proAgWorksHome.GetRowIdxFromRowVal(dc, proAgWorksHome.tblPrintQueue, "Batch", sStartBatchVal,
                    "innerText");
                
                // Get Actual report status from row with Batch value
                string sActStatus = proAgWorksHome.TblGetCellValue(dc, proAgWorksHome.tblPrintQueue,
                    iCurBatchRow.ToString(), sColStatus, "innerText");

                // Find row index again row index with Batch cell value, in case change due to other process
                iCurBatchRow = proAgWorksHome.GetRowIdxFromRowVal(dc, proAgWorksHome.tblPrintQueue,
                                                        "Batch", sStartBatchVal, "innerText");
                
                // get actual report desc for row with Batch value
                string actRptDesc = proAgWorksHome.TblGetCellValue(dc, proAgWorksHome.tblPrintQueue,
                    iCurBatchRow.ToString(), sColRptDesc, "innerText");


                // Verify Status Complete, Report Desc name from Row index With Batch value
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyCommStCreated", "Verify MPCI Commission statement created and completed");
                proAgWorksAgencyCommRpt.VerifyMpciAgencyCommRtp(dc, sActStatus, reportStatus, 
                                                                actRptDesc, reportName, sStartBatchVal);
                // -- Test End

            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }
        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Click main menu Forms & Docs, Forms and Docs tabs verify tables load, verify some tables row count > 0")]
        [Property("TFS-TC-ID", "65965")]
        [Property("TFS-TC-ID", "73416")]
        
        public void NavFormsAndDocsTest()
        {
            //Driver Context Reference For Test
            DriverContext dc = this.DriverContext;

            try
            {
                
                // start Nav Tests, test does not use test data
                dc.NavToStartUrl();

                // logon 
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);

                // wait for Works policy search page to load 
                dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);

                // click main menu forms & docs / view forms and documents
                // build menu selections to navigate to menu forms & docs / view forms and documents
                dc.ProAgXmlRsltLogger.AddTestStepNd("NavToFormsAndDocs", "Navigate to Forms and Documents");
                dc.Driver.GetElement(proAgWorksHome.mnuMainFormsAndDocs, dc).WaitTillDisplayed();
                ElementLocator[] locFormsAndDocs = new ElementLocator[2];
                locFormsAndDocs[0] = proAgWorksHome.mnuMainFormsAndDocs;
                locFormsAndDocs[1] = proAgWorksHome.mnuViewFormsAndDocs;
                // click menus and wait from Forms And Docs MPCI Table to load
                proAgWorksHome.SelectMenuMainSubMenu(locFormsAndDocs, proAgFormsDocs.tblFormsDocsMpci);

                // Navigate All Forms and Doc Tabs, Verify Tables, Headers, Row Counts
                proAgFormsDocs.VerifyFormAndDocsNavTabsTblsMain();
                
            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }
        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Click main menu Compliance, Verify Page Loads")]
        [Property("TFS-TC-ID", "73416")]
        [Property("TFS-TC-ID", "65965")]
        public void NavComplianceTest()
        {
            // Make Reference to current DriverContext
            DriverContext dc = this.DriverContext;
            try
            {
                // start Nav Tests, test does not use test data
                dc.NavToStartUrl();

                // logon 
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);

                // wait for Works policy search page to load 
                dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);

                //click main Compliance menu
                dc.ProAgXmlRsltLogger.AddTestStepNd("NavToCompliance", "Navigate to compliance page");
                dc.Driver.GetElement(proAgWorksHome.mnuMainCompliance, dc).WaitTillDisplayed();
                dc.ProAgXmlRsltLogger.LogStep("Click Main Menu: Compliance");
                dc.Driver.GetElement(proAgWorksHome.mnuMainCompliance,dc).JavaScriptClick();

                //Wait for Compliance page to load
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgCompliance.ddlRiYear, dc);

                //Verify Page Loaded
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyComplianceLoaded", "Verify compliance page loads with no error");
                proAgCompliance.VerifyCompliancePageLoaded(dc);

            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }
        }

        [Test]
        [ProAgRetry(ProAgRetryConfig.DefaultRetryCount)]
        [Category("1 - High")]
        [Description("Click main menu FCIC/ErrorResolution, Verify Page Loads")]
        [Property("TFS-TC-ID", "65965")]
        [Property("TFS-TC-ID", "73416")]
        public void NavFcicErrorResolutionTest()
        {
            DriverContext dc = this.DriverContext;

            try
            {
                // start Nav Tests, test does not use test data
                dc.NavToStartUrl();

                // logon 
                logon.LogOn(this.UserName, this.Password, this.SecretAnswer);

                // wait for Works policy search page to load 
                dc.ProAgXmlRsltLogger.AddTestStepNd("WaitLogonComplete", "Wait ProAgWorks Logon to complete");
                ProAgWebDriverExtensions.WaitPageLoadElem(dc.Driver, proAgWorksHome.txtSearch, dc);

                // nav to error resolution
                dc.ProAgXmlRsltLogger.AddTestStepNd("NavToErrorRes", "navigate to error resolution page");
                dc.Driver.GetElement(proAgWorksHome.mnuMainFcic,dc).WaitTillDisplayed();
                ElementLocator[] locFCICErrorResolution = new ElementLocator[2];
                locFCICErrorResolution[0] = proAgWorksHome.mnuMainFcic;
                locFCICErrorResolution[1] = proAgWorksHome.mnuMainFcicErrorResolution;
                // click menus and wait from Forms And Docs MPCI Table to load
                proAgWorksHome.SelectMenuMainSubMenu(locFCICErrorResolution, proAgErrorResolution.tblFCICReconPolicy);

                // verify Error Resolution Page Loads
                dc.ProAgXmlRsltLogger.AddTestStepNd("VerifyErrorResLoaded", "verify Error Resolution Page Loads");
                proAgErrorResolution.VerifyErrorResolutionPageLoaded(dc);

                // verify Error Resolution Nav Tabs check tables
                proAgErrorResolution.VerifyErrorResolutionNavTabsTbls();
                
            }
            catch (Exception exUnHandled)
            {
                this.TestBaseCaughtException(dc, exUnHandled);
            }
        }
    }
}

