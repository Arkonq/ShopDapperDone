////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiParamsContentElementClass
    {
        [DataMember]
        public string field;
        [DataMember]
        public bool triggerReload;
    }
}