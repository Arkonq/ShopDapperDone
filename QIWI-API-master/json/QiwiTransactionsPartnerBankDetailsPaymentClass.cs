////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Перевод-вывод в адрес банка-партнёра. 
    /// </summary>
    [DataContract]
    public class QiwiTransactionsPartnerBankDetailsPaymentClass: QiwiTransactionsDetailsPaymentClass
    {
        [DataMember]
        public QiwiFieldsPaymentToBankPartnerClass fields = new QiwiFieldsPaymentToBankPartnerClass();
    }
}
