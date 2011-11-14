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
using System.Collections.Generic;
using MiniSAAS.Classes;
using MiniSAAS.DataServiceReference;
using System.Diagnostics;
using System.Reflection;
using MiniSAAS.ChildWindows;

namespace MiniSAAS.Classes
{
    public class MasterObject
    {
        public string Val0 { get; set; }
        public string Val1 { get; set; }
        public string Val2 { get; set; }
        public string Val3 { get; set; }
        public string Val4 { get; set; }
        public string Val5 { get; set; }
        public string Val6 { get; set; }
        public string Val7 { get; set; }
        public string Val8 { get; set; }
        public string Val9 { get; set; }
        public string Val10 { get; set; }
        public string Val11 { get; set; }
        public string Val12 { get; set; }
        public string Val13 { get; set; }
        public string Val14 { get; set; }
        public string Val15 { get; set; }
        public string Val16 { get; set; }
        public string Val17 { get; set; }
        public string Val18 { get; set; }
        public string Val19 { get; set; }
        public string Val20 { get; set; }
        public string Val21 { get; set; }
        public string Val22 { get; set; }
        public string Val23 { get; set; }
        public string Val24 { get; set; }
        public string Val25 { get; set; }
        public string Val26 { get; set; }
        public string Val27 { get; set; }
        public string Val28 { get; set; }
        public string Val29 { get; set; }

        public static List<MasterObject> GetMasterObject(DataDescription dd)
        {
            List<MasterObject> masterobject = new List<MasterObject>();
            MasterObject mo = null;
            try
            {
                foreach (List<string> row in dd.Data)
                {
                    mo = new MasterObject();
                    PropertyInfo[] propertyinfo = mo.GetType().GetProperties();
                    int propertycount = 0;
                    foreach (var s in row)
                    {
                        propertyinfo[propertycount++].SetValue(mo, s, null);
                    }
                    masterobject.Add(mo);
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
            return masterobject;
        }
    }
}
