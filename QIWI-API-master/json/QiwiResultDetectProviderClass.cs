////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiResultDetectProviderClass
    {
        [DataContract]
        public class QiwiCodeResultDetectProviderMobileOperatorClass
        {
            [DataMember]
            public string value;
            [DataMember]
            public string _name;
        }
        [DataMember]
        public QiwiCodeResultDetectProviderMobileOperatorClass code;
        [DataMember]
        public object data;
        [DataMember]
        public object message;
        [DataMember]
        public object messages;
    }
}
