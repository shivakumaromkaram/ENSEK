using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENSEK.Models
{
    public class MeterReading
    {
        public int AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
  
    public class MeterReadingResponce
    {
        public List<int> SucessAccountIDs { get; set; }
        public List<int> FailedAccountIDs { get; set; }
        public List<int> NotMatchedAccountIDs { get; set; }
        public List<MeterReading> MeterReadings { get; set; }

    }
}