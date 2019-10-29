////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Данные о пароле к сайту qiwi.com
    /// </summary>
    [DataContract]
    public class QiwiPassInfoClass
    {
        /// <summary>
        /// Логический признак использования пароля (фактически означает, что пользователь заходит на сайт)
        /// </summary>
        [DataMember]
        public bool passwordUsed;

        /// <summary>
        /// Дата/время последнего изменения пароля сайта qiwi.com
        /// </summary>
        [DataMember]
        public string lastPassChange;

        /// <summary>
        /// Дата/время следующего (планового) изменения пароля сайта qiwi.com
        /// </summary>
        [DataMember]
        public string nextPassChange;
    }
}
