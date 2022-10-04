using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace EgDAL
{
   public class Util
    {
      
           //string _smtpClient;
           string _subject;
           string _message;
           string _from;
           string _to;
           
           public string Subject
           {
               get { return _subject; }
               set { _subject = value; }
           }

           public string Message
           {
               get { return _message; }
               set { _message = value; }
           }

           public string From
           {
               get { return _from; }
               set { _from = value; }
           }

           public string To
           {
               get { return _to; }
               set { _to = value; }
           }


           public void sendMail()
           {
               
               MailMessage oMessage = new MailMessage();
               oMessage.Subject = _subject;
               oMessage.Body = _message;
               oMessage.IsBodyHtml = true;
               oMessage.From = new MailAddress(_from);
               oMessage.To.Add(new MailAddress(_to));
               oMessage.Priority = MailPriority.High;
               SmtpClient smtp = new SmtpClient();
               smtp.Host = System.Configuration.ConfigurationManager.AppSettings["SMTP"];
               smtp.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["FROMEMAIL"], System.Configuration.ConfigurationManager.AppSettings["FROMPWD"]);
               smtp.EnableSsl = false;
               
               //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
               //smtp.EnableSsl = true;
               //smtp.Credentials = new System.Net.NetworkCredential("your_account@gmail.com", "your_password");
               //SmtpClient client = new SmtpClient(_smtpClient, 25);
               //client.UseDefaultCredentials = true;

               try
               {
                   smtp.Send(oMessage);
               }
               catch { }
           }
       }
    
}
