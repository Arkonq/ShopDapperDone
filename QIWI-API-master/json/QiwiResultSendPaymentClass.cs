////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiResultSendPaymentClass
    {
        [DataMember]
        public string id;
        [DataMember]
        public string terms;
        //[DataMember]
        //public QiwiFieldsPaymentUniversalDirectClass fields;
        [DataMember]
        public QiwiSummClass sum;
        [DataMember]
        public QiwiTransactionCodeClass transaction;
        [DataMember]
        public string comment;
        [DataMember]
        public string source;
    }
}
