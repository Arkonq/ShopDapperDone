////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiErrorSendPaymentClass
    {
        [DataMember]
        public string code;
        [DataMember]
        public string message;
        public string FullDescription
        {
            get
            {
                return "code:" + code + " / message:" + message;
            }
        }
    }
}
