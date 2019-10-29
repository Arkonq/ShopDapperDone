////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiCommissionClass
    {
        [DataMember]
        public QiwiRangeCommissionClass[] ranges;
    }
}