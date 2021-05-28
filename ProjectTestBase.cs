// <copyright file="ProjectTestBase.cs" company="Objectivity Bespoke Software Specialists">
// Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <license>
//     The MIT License (MIT)
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// </license>

namespace Ocaramba.Tests.ProAgWorks
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using global::NUnit.Framework;
    using global::NUnit.Framework.Interfaces;
    using NLog;
    using NPOI.OpenXmlFormats.Dml;
    using Ocaramba;
    using Ocaramba.Helpers;
    using Ocaramba.Logger;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    /// The base class for all tests <see href="https://github.com/ObjectivityLtd/Ocaramba/wiki/ProjectTestBase-class">More details on wiki</see>
    /// </summary>
    public class ProjectTestBase : TestBase
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Create instance of Driver Context for tests using this class as base reference
        /// </summary>
        private readonly DriverContext
            driverContext = new DriverContext();

        private string passedFixture, passedFixtureIdx;
        private string user, pw, sa, curUserId, dbUserId;

        /// <summary>
        /// Gets or Sets Database OID set by call to GetDbUserId base on current UserName passed
        /// This user ID may not be linked to UserName Property
        /// Query to get ID SELECT OID , UserID  FROM [AgWorksSubscriber].[dbo].[Users] where userid = '<CurUser>'
        /// </summary>
        public string DbUserId
        {
            get
            {
                return this.dbUserId;
            }
            set
            {
                this.dbUserId = value;
            }
        }

        /// <summary>
        /// Gets or Sets CurUserId linked to property UserName
        /// when user name set CurUserId set
        /// </summary>
        public string CurUserId
        {
            get
            {
                return this.curUserId;
            }
            set
            {
                this.curUserId = value;
            }
        }

        /// <summary>
        /// Gets or Sets UserName set by call to SetLoginInfo
        /// </summary>
        public string UserName
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }

        }

        /// <summary>
        /// Gets or Sets Password set by call to SetLoginInfo
        /// </summary>
        public string Password
        {
            get
            {
                return this.pw;
            }
            set
            {
                this.pw = value;
            }
        }

        /// <summary>
        /// Gets or Sets SecretAnswer set by call to SetLoginInfo
        /// </summary>
        public string SecretAnswer
        {
            get
            {
                return this.sa;
            }
            set
            {
                this.sa = value;
            }
        }

        /// <summary>
        /// Initial method called when project test based referenced
        /// </summary>
        /// <param name="fixture"></param>
        public ProjectTestBase(object fixture)
        {
            // get fixture name and set fixture index
            string sFixture = fixture.ToString();
            sFixture = ProAgGetFixtures.SetFixtureNameIdx(this.DriverContext, sFixture);

            // Set passed fixture name and index for this instance of project test base
            this.passedFixture = this.DriverContext.CurFixture;
            this.passedFixtureIdx = this.DriverContext.CurFixtureIdx;

            //Logger.Info(CultureInfo.CurrentCulture, "Fixture {0}", fixture.ToString());

        }

        
        /// <summary>
        /// Gets or Sets the driver context
        /// </summary>
        protected DriverContext DriverContext
        {
            get
            {
                return this.driverContext;
            }
        }

        /// <summary>
        /// Set Login information based on YourProjectNameCreds.xlsx 
        /// </summary>
        /// <param name="datasetName">The excel login file name underscore sheet name example: MyProAgCreds_Agent</param>
        /// <param name="aliasCol">The alias column name to use when selecting users, typically this is GroupAlias col</param>
        /// <param name="aliasName">The value(s) of the alias column that will select user row(s)</param>
        public void SetLoginInfo(string datasetName, string aliasCol, string aliasName)
        {
            this.SetLoginInfo(this, this.DriverContext, datasetName,
                            aliasCol, aliasName);
        }

        /// <summary>
        /// Before the class is instantiated.
        /// </summary>
        [OneTimeSetUp]
        public void BeforeClass()
        {
            // set the path of the execution assembly DLL, typically bin\debug folder
            this.DriverContext.CurrentDirectory = BaseConfiguration.ProAgAssembylDllPath;
            
        }

        /// <summary>
        /// After the class deposed.
        /// </summary>
        [OneTimeTearDown]
        public void AfterClass()
        {
        }

        /// <summary>
        /// Before the test class execution begins but after test class instantiated.
        /// </summary>
        [SetUp]
        public void BeforeTest()
        {
            //set test project name, need to call from here to get correct project name
            this.DriverContext.TestProjName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // execute common startup steps
            this.TestBasedStartup(this, this.DriverContext);
        }

        /// <summary>
        /// After the test class completes all tests but before it is disposed.
        /// </summary>
        [TearDown]
        public void AfterTest()
        {
            //Get Test Result Status
            this.DriverContext.IsTestFailed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || !this.driverContext.VerifyMessages.Count.Equals(0);

            // Call Common TestBase CleanUp 
            this.TestBaseCleanUp(this, DriverContext);

        }

        /// <summary>
        /// Saves any attachments to test context to be saved locally
        /// </summary>
        /// <param name="filePaths">String array of path file names to be uploaded</param>
        public void SaveAttachmentsToTestContext(string[] filePaths)
        {
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {
                    //this.LogTest.Info("Uploading file [{0}] to test context", filePath);
                    string tcId = TestContext.CurrentContext.Test.ID;
                    DriverContext dc = ProAgXmlLogHelper.GetCurTestDriverContext(tcId);
                    if (dc != null)
                    {
                        dc.ProAgXmlRsltLogger.LogInfo("Uploading file [" + filePath + "] to test context");
                    }
                    TestContext.AddTestAttachment(filePath);
                }
            }
        }
    }
}
