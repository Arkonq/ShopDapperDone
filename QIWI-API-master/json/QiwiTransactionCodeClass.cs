////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiTransactionCodeClass
    {
        [DataContract]
        public class QiwiStateCode
        {
            [DataMember]
            public string code;
        }
        [DataMember]
        public string id;
        [DataMember]
        public QiwiStateCode state;
    }
}
