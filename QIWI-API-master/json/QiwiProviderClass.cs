////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Данные о провайдере
    /// </summary>
    [DataContract]
    public class QiwiProviderClass
    {
        /// <summary>
        /// ID провайдера в QIWI Wallet
        /// </summary>
        [DataMember]
        public long id;

        /// <summary>
        /// Краткое наименование провайдера
        /// </summary>
        [DataMember]
        public string shortName;

        /// <summary>
        /// Развернутое наименование провайдера
        /// </summary>
        [DataMember]
        public string longName;

        /// <summary>
        /// Ссылка на логотип провайдера
        /// </summary>
        [DataMember]
        public string logoUrl;

        /// <summary>
        /// Описание провайдера (HTML)
        /// </summary>
        [DataMember]
        public string description;

        /// <summary>
        /// Список ключевых слов
        /// </summary>
        [DataMember]
        public string keys;

        /// <summary>
        /// Сайт провайдера
        /// </summary>
        [DataMember]
        public string siteUrl;

        [DataMember]
        public QiwiProviderExtrasClass[] extras;
    }
}
