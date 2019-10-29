////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Информация о кошельке пользователя
    /// </summary>
    [DataContract]
    public class QiwiContractInfoClass
    {
        /// <summary>
        /// Логический признак блокировки кошелька
        /// </summary>
        [DataMember]
        public bool blocked;

        /// <summary>
        /// Номер кошелька пользователя
        /// </summary>
        [DataMember]
        public long contractId;

        /// <summary>
        /// Дата/время создания QIWI Кошелька пользователя (через сайт/мобильное приложение, либо при первом пополнении, либо другим способом)
        /// </summary>
        [DataMember]
        public string creationDate;

        /// <summary>
        /// Служебная информация
        /// </summary>
        [DataMember]
        public QiwiFeatureClass[] features;

        /// <summary>
        /// Данные об идентификации пользователя.
        /// </summary>
        [DataMember]
        public QiwiIdentificationInfoClass[] identificationInfo;
    }
}
