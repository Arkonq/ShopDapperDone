////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Данные о PIN-коде к приложению QIWI Кошелька на QIWI терминалах
    /// </summary>
    [DataContract]
    public class QiwiPinInfoClass
    {
        /// <summary>
        /// Логический признак использования PIN-кода (фактически означает, что пользователь заходил в приложение)
        /// </summary>
        [DataMember]
        public bool pinUsed;
    }
}
