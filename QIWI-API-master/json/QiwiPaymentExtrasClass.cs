////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiPaymentExtrasClass
    {
        [DataMember]
        public long id;
        [DataMember]
        public string name;
        [DataMember]
        public string value;
        [DataMember]
        public string title;
    }
}
