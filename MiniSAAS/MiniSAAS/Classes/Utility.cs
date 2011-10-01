using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace MiniSAAS.Classes
{
    public class Utility
    {
        public static bool ValidateEmail(string emailid)
        {
            string email_regex =    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + 
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
    }
}
