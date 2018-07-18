using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    class MSTransactions
    {
        //[DataMember]
        public int      MSTransactionId { get; set; }
        //[DataMember]
        public char     MSIO            { get; set; }
        //[DataMember]
        public float    MSValue         { get; set; }
        //[DataMember]                      
        public string   MSValute        { get; set; }
        //[DataMember]
        public int      MSAccountId     { get; set; }
        //[DataMember]
        public int      MSCategoryId    { get; set; }
        //[DataMember]
        public string   MSNote          { get; set; }
        //[DataMember]
        public DateTime MSDateTime      { get; set; }
        //[DataMember]
        public bool     MSMulticurrency { get; set; }
        
    }
}
