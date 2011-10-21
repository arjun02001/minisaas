using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace MiniSAAS.Back.Classes
{
    public class Utility
    {
        public static bool ValidateEmail(string emailid)
        {
            string email_regex = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
                                    @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?	[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
                                    @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]? [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
                                    @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
            try
            {
                if (!String.IsNullOrEmpty(emailid))
                {
                    if (Regex.IsMatch(emailid, email_regex))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendEmail(string to, string subject, string body)
        {
            try
            {
                if (!ValidateEmail(to))
                {
                    return false;
                }
                MailMessage message = new MailMessage();
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.From = new MailAddress("replytoarjunmukherji@gmail.com");
                using(SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new NetworkCredential("replytoarjunmukherji@gmail.com", "arjunmukherji");
                    client.EnableSsl = true;
                    client.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}