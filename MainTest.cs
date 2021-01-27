

namespace ClickTimeProject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using ClickTimeProject;
    using NUnit.Framework;

    // Author: Ragha Mulbagal
    // Date:01/26/2021
    // This is the Main Class which invokes the method to submit the Clicktime Form
    public class MainTest
    {
        
        [Test]
        public void CallSubmitForm()
        {
            SubmitForm form = new SubmitForm();
            form.FormSubmit("https://login.clicktime.com/qa/", "Ragha", "ragha.sj@gmail.com", "55446", "Submitting Feedback Form");
            
        }
      
    }


    
}