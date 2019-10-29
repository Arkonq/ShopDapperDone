////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiProviderExtrasClass
    {
        [DataMember]
        public string key;
        [DataMember]
        public string value;
    }
}
