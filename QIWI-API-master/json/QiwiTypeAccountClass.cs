////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Сведения о счете
    /// </summary>
    [DataContract]
    public class QiwiTypeAccountClass
    {
        /// <summary>
        /// ID - Описания счета
        /// </summary>
        [DataMember]
        public string id;

        /// <summary>
        /// Заголовок описания счета
        /// </summary>
        [DataMember]
        public string title;
    }
}
