////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    [DataContract]
    public class QiwiPersonProfileClass
    {
        /// <summary>
        /// Информация о кошельке пользователя.Объект может отсутствовать
        /// </summary>
        [DataMember]
        public QiwiContractInfoClass contractInfo;
        /// <summary>
        /// Текущие настройки авторизации пользователя.Объект может отсутствовать
        /// </summary>
        [DataMember]
        public QiwiAuthInfoClass authInfo;
        /// <summary>
        /// Прочие пользовательские данные. Объект может отсутствовать.
        /// </summary>
        [DataMember]
        public QiwiUserInfoClass userInfo;
    }
}
