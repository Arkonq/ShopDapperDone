////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Запрос коммиссии за перевод
    /// </summary>
    [DataContract]
    public class QiwiQueryCalculationOnlineCommissionClass
    {
        /// <summary>
        /// Объект с платежными реквизитами
        /// </summary>
        [DataContract]
        public class QiwiPurchaseTotalsClass
        {
            /// <summary>
            /// Объект, содержащий данные о сумме платежа:
            /// </summary>
            [DataMember]
            public QiwiSummClass total = new QiwiSummClass();
        }

        /// <summary>
        /// Номер телефона (с международным префиксом) или номер карты/счета получателя, в зависимости от провайдера
        /// </summary>
        [DataMember]
        public string account;

        /// <summary>
        /// Объект, определяющий обработку платежа процессингом QIWI Wallet
        /// </summary>
        [DataMember]
        public QiwiPaymentMethodClass paymentMethod = new QiwiPaymentMethodClass();

        /// <summary>
        /// Объект с платежными реквизитами
        /// </summary>
        [DataMember]
        public QiwiPurchaseTotalsClass purchaseTotals = new QiwiPurchaseTotalsClass();
    }
}
