////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Данные о PIN-коде мобильного приложения QIWI Кошелька
    /// </summary>
    [DataContract]
    public class QiwiMobilePinInfoClass
    {
        /// <summary>
        /// Логический признак использования PIN-кода (фактически означает, что мобильное приложение используется)
        /// </summary>
        [DataMember]
        public bool mobilePinUsed;

        /// <summary>
        /// Дата/время последнего изменения PIN-кода мобильного приложения QIWI Кошелька
        /// </summary>
        [DataMember]
        public string lastMobilePinChange;

        /// <summary>
        /// Дата/время следующего (планового) изменения PIN-кода мобильного приложения QIWI Кошелька
        /// </summary>
        [DataMember]
        public string nextMobilePinChange;
    }
}
