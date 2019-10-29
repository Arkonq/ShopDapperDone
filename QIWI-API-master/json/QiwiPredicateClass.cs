////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiPredicateClass
    {
        [DataMember]
        public string type;
        [DataMember]
        public string pattern;
    }
}