////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Данные об идентификации пользователя.
    /// </summary>
    [DataContract]
    public class QiwiIdentificationInfoClass
    {
        /// <summary>
        /// Акроним системы, в которой пользователь получил идентификацию:
        /// </summary>
        [DataMember]
        public string bankAlias;

        /// <summary>
        /// Текущий уровень идентификации кошелька
        /// </summary>
        [DataMember]
        public string identificationLevel;
    }
}
