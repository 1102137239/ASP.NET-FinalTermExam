using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_FinalTermExam.Models
{
    public class EMPSearchArg
    {
        public int City { get; set; }
        public String Country { get; set; }
        public int EMPId { get; set; }
        public String EMPName { get; set; }
        public String Gender { get; set; }
        public int Title { get; set; }
    }
}