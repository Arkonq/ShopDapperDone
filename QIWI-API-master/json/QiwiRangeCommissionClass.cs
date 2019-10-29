////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiRangeCommissionClass
    {
        [DataMember]
        public double bound;
        /// <summary>
        /// Процент
        /// </summary>
        [DataMember]
        public double rate;
        /// <summary>
        /// Фиксированая ставка
        /// </summary>
        [DataMember(Name = "fixed")]
        public double _fixed;
    }
}
