////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Информация о платеже/транзакции
    /// </summary>
    [DataContract]
    public class QiwiPaymentClass
    {
        /// <summary>
        /// ID транзакции в процессинге QIWI Wallet
        /// </summary>
        [DataMember]
        public long txnId;

        /// <summary>
        /// Номер кошелька
        /// </summary>
        [DataMember]
        public long personId;

        /// <summary>
        /// Дата/время платежа, во временной зоне запроса (см. параметр startDate). Формат даты ГГГГ-ММ-ДД'T'чч:мм:сс+03:00
        /// </summary>
        [DataMember]
        public string date;

        /// <summary>
        /// Код ошибки платежа
        /// 0 - OK
        /// 3 - Техническая ошибка, нельзя отправить запрос провайдеру
        /// 4 - Неверный формат счета/телефона
        /// 5 - Номер не принадлежит оператору
        /// 8 - Прием платежа запрещен по техническим причинам
        /// 131 - Платежи на выбранного провайдера запрещено проводить из данной страны.
        /// 202 - Ошибка в параметрах запроса
        /// 220 - Недостаточно средств
        /// 241 - Сумма платежа меньше минимальной
        /// 242 - Сумма платежа больше максимальной
        /// 319 - Платеж невозможен
        /// 500 - По техническим причинам этот платеж не может быть выполнен.Для совершения платежа обратитесь, пожалуйста, в свой обслуживающий банк
        /// 522 - Неверный номер или срок действия карты получателя
        /// 548 - Истек срок действия карты получателя
        /// 561 - Платеж отвергнут оператором банка получателя
        /// 702 - Платеж не проведен из-за ограничений у получателя.Подробности по телефону: 8-800-707-77-59
        /// 705 - Ежемесячный лимит платежей и переводов для статуса Стандарт - 200 000 р.Для увеличения лимита пройдите идентификацию.
        /// 746 - Превышен лимит по платежам в пользу провайдера
        /// 852 - Превышен лимит по платежам в пользу провайдера
        /// 893 - Срок действия перевода истек
        /// 1050 - Превышен лимит на операции
        /// </summary>
        [DataMember]
        public long errorCode;

        /// <summary>
        /// Описание ошибки
        /// </summary>
        [DataMember]
        public string error;

        /// <summary>
        /// Статус платежа. Возможные значения: WAITING - платеж проводится, SUCCESS - успешный платеж, ERROR - ошибка платежа.
        /// </summary>
        [DataMember]
        public string status;

        /// <summary>
        /// Тип платежа. Возможные значения:
        /// IN - пополнение,
        /// OUT - платеж,
        /// QIWI_CARD - платеж с карт QIWI(QVC, QVP).
        /// </summary>
        [DataMember]
        public string type;

        /// <summary>
        /// Текстовое описание статуса платежа
        /// </summary>
        [DataMember]
        public string statusText;

        /// <summary>
        /// Клиентский ID транзакции
        /// </summary>
        [DataMember]
        public string trmTxnId;

        /// <summary>
        /// Номер счета получателя
        /// </summary>
        [DataMember]
        public string account;

        /// <summary>
        /// Данные о сумме платежа
        /// </summary>
        [DataMember]
        public QiwiSummClass sum;

        /// <summary>
        /// Данные о комиссии платежа
        /// </summary>
        [DataMember]
        public QiwiSummClass commission;

        /// <summary>
        /// Данные об общей сумме платежа
        /// </summary>
        [DataMember]
        public QiwiSummClass total;

        /// <summary>
        /// Данные о провайдере
        /// </summary>
        [DataMember]
        public QiwiProviderClass provider;

        /// <summary>
        /// Специальное поле
        /// </summary>
        [DataMember]
        public bool chequeReady;

        /// <summary>
        /// Комментарий к платежу
        /// </summary>
        [DataMember]
        public string comment;

        /// <summary>
        /// Курс конвертации (если применяется в транзакции)
        /// </summary>
        [DataMember]
        public int currencyRate;

        /// <summary>
        /// Служебная информация
        /// </summary>
        [DataMember]
        public QiwiPaymentExtrasClass[] extras;

        /// <summary>
        /// Источник платежа. Допустимые значения:
        /// QW_RUB - рублевый счет кошелька,
        /// QW_USD - счет кошелька в долларах,
        /// QW_EUR - счет кошелька в евро,
        /// CARD - привязанные и непривязанные к кошельку банковские карты,
        /// MK - счет мобильного оператора
        /// </summary>
        [DataMember]
        public QiwiSourceClass source;

        /// <summary>
        /// Специальное поле
        /// </summary>
        [DataMember]
        public bool bankDocumentAvailable;

        /// <summary>
        /// Специальное поле
        /// </summary>
        [DataMember]
        public bool bankDocumentReady;

        /// <summary>
        /// Специальное поле
        /// </summary>
        [DataMember]
        public bool repeatPaymentEnabled;

        /// <summary>
        /// Специальное поле
        /// </summary>
        [DataMember]
        public bool favoritePaymentEnabled;

        /// <summary>
        /// Специальное поле
        /// </summary>
        [DataMember]
        public bool regularPaymentEnabled;
    }
}
