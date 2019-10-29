////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Прочие пользовательские данные
    /// </summary>
    [DataContract]
    public class QiwiUserInfoClass
    {        
        /// <summary>
        /// Код валюты баланса кошелька по умолчанию (number-3 ISO-4217)
        /// </summary>
        [DataMember]
        public long defaultPayCurrency;

        [DataMember]
        public string defaultPayAccountAlias;

        /// <summary>
        /// Название мобильного оператора номера пользователя
        /// </summary>
        [DataMember(Name = "operator")]
        public string mob_operator;

        /// <summary>
        /// E-mail пользователя
        /// </summary>
        [DataMember]
        public string email;

        /// <summary>
        /// Служебная информация
        /// </summary>
        [DataMember]
        public string promoEnabled;
        
        /// <summary>
        /// Служебная информация
        /// </summary>
        [DataMember]
        public long defaultPaySource;

        /// <summary>
        /// Служебная информация
        /// </summary>
        [DataMember]
        public string language;

        /// <summary>
        /// Номер первой транзакции пользователя после регистрации
        /// </summary>
        [DataMember]
        public long firstTxnId;

        /// <summary>
        /// Служебная информация
        /// </summary>
        [DataMember]
        public string phoneHash;

        [DataMember]
        public QiwiIntegrationHashesClass integrationHashes;
    }
}
