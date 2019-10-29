////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    ///  Список платежей из истории кошелька
    /// </summary>
    [DataContract]
    public class QiwiPaymentsDataClass
    {
        /// <summary>
        /// Массив платежей
        /// </summary>
        [DataMember]
        public QiwiPaymentClass[] data;

        /// <summary>
        /// ID следующей транзакции в полном списке
        /// </summary>
        [DataMember]
        public string nextTxnId;

        /// <summary>
        /// Дата/время следующей транзакции в полном списке, время московское (в формате ГГГГ-ММ-ДД'T'чч:мм:сс+03:00)
        /// </summary>
        [DataMember]
        public string nextTxnDate;
    }
}
