////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiSourceClass
    {
        [DataMember]
        public long id;
        [DataMember]
        public string shortName;
        [DataMember]
        public string longName;
        [DataMember]
        public string logoUrl;
        [DataMember]
        public string description;
        [DataMember]
        public string keys;
        [DataMember]
        public string siteUrl;
        [DataMember]
        public object[] extras;
    }
}
