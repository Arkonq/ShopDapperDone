////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Детали перевода
    /// </summary>
    [DataContract]
    public class QiwiTransactionsDetailsPaymentClass
    {
        /// <summary>
        /// Клиентский ID транзакции (максимум 20 цифр). Должен быть уникальным для каждой транзакции и увеличиваться с каждой последующей транзакцией. Для выполнения этих требований рекомендуется задавать равным 1000*(Standard Unix time в секундах).
        /// </summary>
        [DataMember]
        public string id = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();

        /// <summary>
        /// Данные о сумме платежа
        /// </summary>
        [DataMember]
        public QiwiSummClass sum = new QiwiSummClass();

        /// <summary>
        /// Объект, определяющий обработку платежа процессингом QIWI Wallet
        /// </summary>
        [DataMember]
        public readonly QiwiPaymentMethodClass paymentMethod = new QiwiPaymentMethodClass();

        /// <summary>
        /// Комментарий к платежу. Необязательный параметр.
        /// </summary>
        [DataMember]
        public string comment;
    }
}
