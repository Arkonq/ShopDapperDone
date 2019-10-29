////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Реквизиты получателя
    /// </summary>
    [DataContract]
    public class QiwiFieldsPaymentUniversalDirectClass
    {
        /// <summary>
        /// Номер получателя.
        /// При перводе на киви - номер киви в формате 79995552244
        /// При пополнении сотового телефона - номер телефона в формате 9995552244
        /// </summary>
        [DataMember]
        public string account;
    }
}
