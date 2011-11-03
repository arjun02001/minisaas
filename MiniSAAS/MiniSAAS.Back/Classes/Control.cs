using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class Control
    {
        [DataMember]
        public int ControlID { get; set; }
        [DataMember]
        public ControlLocation ControlLocation { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string RedirectURL { get; set; }
        [DataMember]
        public string BackgroundImage { get; set; }
    }
}