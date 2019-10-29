////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class QiwiFieldsPaymentToBankPartnerClass : QiwiFieldsPaymentUniversalDirectClass
    {
        /// <summary>
        /// При переводе на банковскую карту.
        /// Срок действия карты, в формате ММГГ (например, 0218). Параметр указывается только в случае перевода на карту.
        /// </summary>
        [DataMember]
        public string exp_date;

        /// <summary>
        /// При переводе на банковскую карту.
        /// Тип банковского идентификатора. Допустимые значения:
        /// для Тинькофф Банк - карта 1, договор 3
        /// для Альфа-Банка - карта 1, счет 2
        /// для Промсвязьбанка - карта 7, счет 9
        /// для банка Русский Стандарт - карта 1, счет 2, договор 3
        /// </summary>
        [DataMember]
        public string account_type;
    }
}
