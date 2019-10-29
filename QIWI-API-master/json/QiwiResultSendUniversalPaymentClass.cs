////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiResultSendUniversalPaymentClass: QiwiResultSendPaymentClass
    {
        [DataMember]
        public QiwiFieldsPaymentUniversalDirectClass fields;
    }
}
