////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Аккаунт/баланс
    /// </summary>
    [DataContract]
    public class QiwiAccountClass
    {
        /// <summary>
        /// Псевдоним пользовательского баланса
        /// </summary>
        [DataMember]
        public string alias;

        /// <summary>
        /// Псевдоним банковского баланса
        /// </summary>
        [DataMember]
        public string fsAlias;

        /// <summary>
        /// Название соответствующего счета кошелька
        /// </summary>
        [DataMember]
        public string title;

        /// <summary>
        /// Сведения о счете
        /// </summary>
        [DataMember]
        public QiwiTypeAccountClass type;

        /// <summary>
        /// Логический признак реального баланса в системе QIWI Кошелек (не привязанная карта, не счет мобильного телефона и т.д.)
        /// </summary>
        [DataMember]
        public bool hasBalance;

        /// <summary>
        /// Сведения о балансе данного счета
        /// </summary>
        [DataMember]
        public QiwiSummClass balance;

        /// <summary>
        /// Код валюты баланса (number-3 ISO-4217)
        /// </summary>
        [DataMember]
        public int currency;
    }
}
