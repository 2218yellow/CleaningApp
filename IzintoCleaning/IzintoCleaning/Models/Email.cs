using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace IzintoCleaning.Models
{
    public class Email
    {
        //overload with what you want to show in the email.
        public void SendConfirmation(string email, string Name, string ServiceNmae, DateTime date, string employee)
        {
            try
            {
                var myMessage = new SendGridMessage
                {
                    From = new EmailAddress("ndumisomajola2@gmail.com", "Izinto Cleaning")
                };

                myMessage.AddTo(email);
                string subject = "Do not Reply!";
                string body = (
                    "Dear " + Name + "<br/>"
                    + "<br/>"
                    + "Please find below your details of your Request with Izinto Cleaning Services: "
                    + "<br/>"
                    + "<br/>" + "Service name   :" + ServiceNmae
                     + "<br/>" + "On this date  :" + date

                    + "<br/>" + "Done by  :" + employee +
             
                    "<br/>" +
                    "<br/>" +
                    "<br/>" +

                    "Sincerely Yours, " +
                    "<br/>" +
                    "Izinto Cleaning Management");

                myMessage.Subject = subject;
                myMessage.HtmlContent = body;

                var transportWeb = new SendGrid.SendGridClient("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");

                transportWeb.SendEmailAsync(myMessage);
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }

        }


    }
}