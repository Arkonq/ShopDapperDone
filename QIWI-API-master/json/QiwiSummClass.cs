////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Данные о сумме
    /// </summary>
    [DataContract]
    public class QiwiSummClass
    {
        /// <summary>
        /// Сумма
        /// </summary>
        [DataMember]
        public double amount;

        /// <summary>
        /// Валюта
        /// </summary>
        [DataMember]
        public readonly string currency = "643";
    }
}
