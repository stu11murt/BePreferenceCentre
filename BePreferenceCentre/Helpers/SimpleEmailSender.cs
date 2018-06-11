using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace BePreferenceCentre.Helpers
{
    public static class SimpleEmailSender
    {
        // Replace sender@example.com with your "From" address. 
        // This address must be verified with Amazon SES.
        const String FROM = "stuart@deepbluedevelopment.co.uk";
        const String FROMNAME = "BEforBeauty";

        // Replace recipient@example.com with a "To" address. If your account 
        // is still in the sandbox, this address must be verified.
        const String TO = "stu11murt@gmail.com";

        // Replace smtp_username with your Amazon SES SMTP user name.
        const String SMTP_USERNAME = "AKIAJUMR34DMKUQWDISQ";

        // Replace smtp_password with your Amazon SES SMTP user name.
        const String SMTP_PASSWORD = "AnNUEgsJrec8C/hRQYm57eZCSMoFqPT2XqH2WeTWeWEg";

        // (Optional) the name of a configuration set to use for this message.
        // If you comment out this line, you also need to remove or comment out
        // the "X-SES-CONFIGURATION-SET" header below.
        //const String CONFIGSET = "ConfigSet";

        // If you're using Amazon SES in a region other than US West (Oregon), 
        // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
        // endpoint in the appropriate Region.
        const String HOST = "email-smtp.us-east-1.amazonaws.com";

        // The port you will connect to on the Amazon SES SMTP endpoint. We
        // are choosing port 587 because we will use STARTTLS to encrypt
        // the connection.
        const int PORT = 587;

        // The subject line of the email
        const String SUBJECT =
            "BE for Beauty test email sent from BE Preference centre";

        // The body of the email
        const String BODY =
            "<h1>BE Preference Centre Test</h1>" +
            "<p>This email was sent through the " +
            "<a href='https://aws.amazon.com/ses'>Amazon SES</a> SMTP interface " +
            "using the .NET System.Net.Mail library.</p>";



        public static void SendSimpleEmail(string mailTo)
        {

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(mailTo));
            message.Subject = SUBJECT;
            message.Body = BODY;
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            // Create and configure a new SmtpClient
            SmtpClient client =
                new SmtpClient(HOST, PORT);
            // Pass SMTP credentials
            client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            // Enable SSL encryption
            client.EnableSsl = true;

            // Send the email. 
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }
}