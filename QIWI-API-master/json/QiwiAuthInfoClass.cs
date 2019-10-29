////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Текущие настройки авторизации пользователя
    /// </summary>
    [DataContract]
    public class QiwiAuthInfoClass
    {
        /// <summary>
        /// Данные о пароле к сайту qiwi.com
        /// </summary>
        [DataMember]
        public QiwiPassInfoClass passInfo;
        
        /// <summary>
        /// Дата/время последней сессии в QIWI Кошельке
        /// </summary>
        [DataMember]
        public string lastLoginDate;

        /// <summary>
        /// Номер кошелька пользователя
        /// </summary>
        [DataMember]
        public long personId;

        /// <summary>
        /// Дата/время регистрации QIWI Кошелька пользователя (через сайт/мобильное приложение, либо другим способом)
        /// </summary>
        [DataMember]
        public string registrationDate;

        /// <summary>
        /// E-mail, привязанный к кошельку.Если отсутствует, то null
        /// </summary>
        [DataMember]
        public string boundEmail;

        /// <summary>
        /// Данные о PIN-коде мобильного приложения QIWI Кошелька
        /// </summary>
        [DataMember]
        public QiwiMobilePinInfoClass mobilePinInfo;

        /// <summary>
        /// Данные о PIN-коде к приложению QIWI Кошелька на QIWI терминалах
        /// </summary>
        [DataMember]
        public QiwiPinInfoClass pinInfo;

        /// <summary>
        /// IP-адрес последней пользовательской сессии
        /// </summary>
        [DataMember]
        public string ip;
    }
}
