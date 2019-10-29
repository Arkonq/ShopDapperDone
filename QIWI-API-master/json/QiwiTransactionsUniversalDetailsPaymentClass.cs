////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Универсальный перевод по одному реквизиту получателя (номер телефона, счёта, карты)
    /// </summary>
    [DataContract]
    public class QiwiTransactionsUniversalDetailsPaymentClass: QiwiTransactionsDetailsPaymentClass
    {
        /// <summary>
        /// Номер получателя
        /// </summary>
        [DataMember]
        public QiwiFieldsPaymentUniversalDirectClass fields = new QiwiFieldsPaymentUniversalDirectClass();
    }
}
