using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using ClickTimeProject;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using NUnit.Framework;

namespace ClickTimeProject
{
    // This Class fills in the Clicktime form details and submits it
   
    public class SubmitForm
    {
        public ElementLocator lblTitle = new ElementLocator(Locator.XPath, "//*[contains(text(),'Give us your feedback')]"),
            txtName = new ElementLocator(Locator.Id, "fullName"),
            txtEmail = new ElementLocator(Locator.Id, "email"),
            txtZipcode = new ElementLocator(Locator.Id, "zipcode"),
            txtComments = new ElementLocator(Locator.Id, "comments"),
            btnSubmit = new ElementLocator(Locator.Id, "Submit1"),
            lblMessage = new ElementLocator(Locator.XPath, "//*[contains(text(),'Ragha, your feedback has been submitted. Thanks for contacting us!')]");

        
        public void FormSubmit(String url, String Name, String Email, String Zipcode, String Comments)
        {
            bool bLbl, btxtName, btxtEmail, btxtZipcode, btxtComments, bbtnSubmit, bMsg = false;
            try
            {
                
                ChromeDriver driver = new ChromeDriver(@"C:\Users\rmulbagal\source\repos\Google\Google\WebDrivers\Chrome");

                //Navigating to the URL
                driver.Navigate().GoToUrl(url);

                //Waiting for the page to load
                Thread.Sleep(4000);

                // Verifying the fields exist and entering Name, Email, ZipCode, Comments in the Form
               
                bLbl = driver.FindElement(By.XPath(lblTitle.Value)).Displayed;
                Assert.IsTrue(bLbl);
                btxtName = driver.FindElement(By.Id(txtName.Value)).Displayed;
                Assert.IsTrue(btxtName);
                driver.FindElement(By.Id(txtName.Value)).SendKeys(Name);
                btxtEmail = driver.FindElement(By.Id(txtEmail.Value)).Displayed;
                Assert.IsTrue(btxtEmail);
                driver.FindElement(By.Id(txtEmail.Value)).SendKeys(Email);
                btxtZipcode = driver.FindElement(By.Id(txtZipcode.Value)).Displayed;
                Assert.IsTrue(btxtZipcode);
                driver.FindElement(By.Id(txtZipcode.Value)).SendKeys(Zipcode);
                btxtComments = driver.FindElement(By.Id(txtComments.Value)).Displayed;
                Assert.IsTrue(btxtComments);
                driver.FindElement(By.Id(txtComments.Value)).SendKeys(Comments);

                //Clicking on Submit Button
                bbtnSubmit = driver.FindElement(By.Id(btnSubmit.Value)).Displayed;
                Assert.IsTrue(bbtnSubmit);
                driver.FindElement(By.Id(btnSubmit.Value)).Click();

                //Verifying the Message
                bMsg = driver.FindElement(By.XPath(lblMessage.Value)).Displayed;
                Assert.IsTrue(bMsg);

                //Closing the Browser
                driver.Quit();
            }
            // Exception handling
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
      
    }
}
