////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using QIWI.json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace QIWI_API
{
    public class QIWI_API_Class : Curl.ClassCurl
    {
        /// <summary>
        /// QIWI токен доступа https://qiwi.com/api
        /// </summary>
        private readonly string my_auth_bearer_token;
        
        private const string date_time_format = "yyyy-MM-ddTHH:mm:sszzz";

        public readonly QiwiPersonProfileClass my_profile;

        /// <summary>
        /// Инициализация API и загрузка профиля кошелька
        /// </summary>
        /// <param name="bearer_token">QIWI токен доступа https://qiwi.com/api </param>
        public QIWI_API_Class(string bearer_token)
        {
            my_auth_bearer_token = bearer_token.Trim();
            if (my_auth_bearer_token.IndexOf("Bearer") < 0)
                my_auth_bearer_token = "Bearer " + my_auth_bearer_token;
            my_profile = PersonProfile();
            Console.Out.WriteLine("Профиль: " + (my_profile == null ? " -> не удалось загрузить. " + ReasonPhraseHTTP : my_profile.authInfo.personId.ToString()));

        }

        /// <summary>
        /// Добавляем заголовок с токеном
        /// </summary>
        protected override string SendRequest(string adress_direct, HttpMethod Method, Dictionary<string, string> headers = null, string post_data = null, bool insideAllowAutoRedirect = true, string save_result_to_file = null)
        {
            if (headers == null)
                headers = new Dictionary<string, string>();
            headers.Add("Authorization", my_auth_bearer_token);
            return base.SendRequest(adress_direct, Method, headers, post_data, insideAllowAutoRedirect, save_result_to_file);
        }

        /// <summary>
        /// Профиль пользователя
        /// URL https://edge.qiwi.com/person-profile/v1/profile/current?parameter=value
        /// Запрос → GET
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer***
        ///   
        /// Параметры передаются в строке запроса: 
        /// </summary>
        /// <param name="authInfoEnabled">Логический признак выгрузки настроек авторизации пользователя. По умолчанию true</param>
        /// <param name="contractInfoEnabled">Логический признак выгрузки данных о кошельке пользователя. По умолчанию true</param>
        /// <param name="userInfoEnabled">Логический признак выгрузки прочих пользовательских данных. По умолчанию true</param>
        /// <returns>Успешный JSON-ответ содержит данные о профиле пользователя:
        /// Параметр Тип     Описание
        /// authInfo Object Текущие настройки авторизации пользователя.Объект может отсутствовать, в зависимости от признака authInfoEnabled в запросе.
        /// authInfo.personId Number  Номер кошелька пользователя
        /// authInfo.registrationDate   String Дата/время регистрации QIWI Кошелька пользователя (через сайт/мобильное приложение, либо другим способом)
        /// authInfo.boundEmail String  E-mail, привязанный к кошельку.Если отсутствует, то null
        /// authInfo.ip String  IP-адрес последней пользовательской сессии
        /// authInfo.lastLoginDate String  Дата/время последней сессии в QIWI Кошельке
        /// authInfo.mobilePinInfo Object  Данные о PIN-коде мобильного приложения QIWI Кошелька
        /// mobilePinInfo.mobilePinUsed     Boolean Логический признак использования PIN-кода (фактически означает, что мобильное приложение используется)
        /// mobilePinInfo.lastMobilePinChange String  Дата/время последнего изменения PIN-кода мобильного приложения QIWI Кошелька
        /// mobilePinInfo.nextMobilePinChange   String Дата/время следующего (планового) изменения PIN-кода мобильного приложения QIWI Кошелька
        /// authInfo.passInfo Object  Данные о пароле к сайту qiwi.com
        /// passInfo.passwordUsed Boolean     Логический признак использования пароля (фактически означает, что пользователь заходит на сайт)
        /// passInfo.lastPassChange String  Дата/время последнего изменения пароля сайта qiwi.com
        /// passInfo.nextPassChange String  Дата/время следующего (планового) изменения пароля сайта qiwi.com
        /// authInfo.pinInfo Object  Данные о PIN-коде к приложению QIWI Кошелька на QIWI терминалах
        /// pinInfo.pinUsed Boolean     Логический признак использования PIN-кода (фактически означает, что пользователь заходил в приложение)
        /// contractInfo Object  Информация о кошельке пользователя.Объект может отсутствовать, в зависимости от признака contractInfoEnabled в запросе.
        /// contractInfo.blocked Boolean     Логический признак блокировки кошелька
        /// contractInfo.contractId Number  Номер кошелька пользователя
        /// contractInfo.creationDate String  Дата/время создания QIWI Кошелька пользователя (через сайт/мобильное приложение, либо при первом пополнении, либо другим способом)
        /// contractInfo.features Array[Object]   Служебная информация
        /// contractInfo.identificationInfo Array[Object]   Данные об идентификации пользователя.
        /// identificationInfo[].bankAlias String  Акроним системы, в которой пользователь получил идентификацию:
        /// QIWI - QIWI Кошелек.
        /// identificationInfo[].identificationLevel String  Текущий уровень идентификации кошелька. Возможные значения:
        /// ANONYMOUS - без идентификации;
        /// SIMPLE, VERIFIED - упрощенная идентификация;
        /// FULL - полная идентификация.
        /// userInfo Object Прочие пользовательские данные. Объект может отсутствовать, в зависимости от признака userInfoEnabled в запросе.
        /// userInfo.defaultPayCurrency Number  Код валюты баланса кошелька по умолчанию (number-3 ISO-4217)
        /// userInfo.defaultPaySource Number  Служебная информация
        /// userInfo.email String  E-mail пользователя
        /// userInfo.firstTxnId Number  Номер первой транзакции пользователя после регистрации
        /// userInfo.language String  Служебная информация
        /// userInfo.operator   String 	Название мобильного оператора номера пользователя
        /// userInfo.phoneHash String  Служебная информация
        /// userInfo.promoEnabled String  Служебная информация</returns>
        public QiwiPersonProfileClass PersonProfile(bool authInfoEnabled = true, bool contractInfoEnabled = true, bool userInfoEnabled = true)
        {
            Console.Out.WriteLine("Запрос персонального профиля...");
            SendRequest("https://edge.qiwi.com/person-profile/v1/profile/current?authInfoEnabled=" + authInfoEnabled + "&contractInfoEnabled=" + contractInfoEnabled + "&userInfoEnabled=" + userInfoEnabled, HttpMethod.Get);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка получения профиля. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiPersonProfileClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiPersonProfileClass), HttpResponseResult);
        }

        /// <summary>
        /// История платежей. Запрос выгружает историю платежей и пополнений вашего кошелька.
        /// URL https://edge.qiwi.com/payment-history/v1/persons/{wallet}/payments?parameter=value
        /// Запрос → GET
        /// В pathname GET-запроса используется параметр: wallet - номер кошелька, для которого получен токен доступа(с международным префиксом, но без +)
        /// 
        /// Максимальная частота запросов истории платежей - не более 100 запросов в минуту для одного и того же номера кошелька. При превышении доступ к API блокируется на 5 минут.
        /// Максимальный допустимый интервал между startDate и endDate - 90 календарных дней.
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer*** 
        /// </summary>
        /// <param name="wallet">Номер кошелька, для которого получен токен доступа (с международным префиксом, но без +)</param>
        /// <param name="rows">Число платежей в ответе, для разбивки отчета на части. Целое число от 1 до 50. Обязательный параметр</param>
        /// <param name="operation">Тип операций в отчете, для отбора. Допустимые значения: ALL - все операции, IN - только пополнения, OUT - только платежи, QIWI_CARD - только платежи по картам QIWI (QVC, QVP). По умолчанию ALL</param>
        /// <param name="sources">Источники платежа, для отбора. Каждый источник задается как отдельный параметр и нумеруется элементом массива, начиная с нуля (sources[0], sources[1] и т.д.). Допустимые значения: QW_RUB - рублевый счет кошелька, QW_USD - счет кошелька в долларах, QW_EUR - счет кошелька в евро, CARD - привязанные и непривязанные к кошельку банковские карты, MK - счет мобильного оператора. Если не указаны, учитываются все источники</param>
        /// <param name="startDate">Начальная дата поиска платежей. Дату можно указать в любой временной зоне TZD (формат ГГГГ-ММ-ДД'T'чч:мм:ссTZD), однако она должна совпадать с временной зоной в параметре endDate. Обозначение временной зоны TZD: +чч:мм или -чч:мм (временной сдвиг от GMT). Используется только вместе с endDate. По умолчанию, равна суточному сдвигу от текущей даты по московскому времени (Максимальный допустимый интервал между startDate и endDate - 90 календарных дней)</param>
        /// <param name="endDate">Конечная дата поиска платежей. Дату можно указать в любой временной зоне TZD (формат ГГГГ-ММ-ДД'T'чч:мм:ссTZD), однако она должна совпадать с временной зоной в параметре startDate. Обозначение временной зоны TZD: +чч:мм или -чч:мм (временной сдвиг от GMT). Используется только вместе с startDate. По умолчанию, равна текущим дате/времени по московскому времени (Максимальный допустимый интервал между startDate и endDate - 90 календарных дней.)</param>
        /// <param name="nextTxnDate">Дата транзакции для отсчета от предыдущего списка (равна параметру nextTxnDate в предыдущем списке). Используется только вместе с nextTxnId</param>
        /// <param name="nextTxnId">Дата транзакции для отсчета от предыдущего списка (равна параметру nextTxnDate в предыдущем списке). Используется только вместе с nextTxnId</param>
        /// <returns>
        /// Пример 1. Последние 10 платежей с рублевого баланса и с привязанной карты
        /// GET /payment-history/v1/persons/79112223344/payments?rows=10&operation=OUT&sources[0]=QW_RUB&sources[1]=CARD HTTP/1.1
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Content-type: application/json
        /// Host: edge.qiwi.com
        /// # # # # #
        /// Пример 2. Платежи за 10.05.2017
        /// GET /payment-history/v1/persons/79112223344/payments?rows=50&startDate=2017-05-10T00%3A00%3A00%2B03%3A00&endDate=2017-05-10T23%3A59%3A59%2B03%3A00 HTTP/1.1
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Content-type: application/json
        /// Host: edge.qiwi.com
        /// # # # # #
        /// Пример 3. Продолжение списка платежей
        /// (в предыдущем запросе истории возвращены параметры nextTxnId=9103121 и nextTxnDate=2017-05-11T12:35:23+03:00)
        /// GET /payment-history/v1/persons/79112223344/payments?rows=50&nextTxnId=9103121&nextTxnDate=2017-05-11T12%3A35%3A23%2B03%3A00 HTTP/1.1
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Content-type: application/json
        /// Host: edge.qiwi.com
        /// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~
        /// HTTP/1.1 200 OK
        /// Content-Type: application/json
        /// 
        /// {
        /// 	"data": [{
        /// 		"txnId": 9309,
        /// 		"personId": 79112223344,
        /// 		"date": "2017-01-21T11:41:07+03:00",
        /// 		"errorCode": 0,
        /// 		"error": null,
        /// 		"status": "SUCCESS",
        /// 		"type": "OUT",
        /// 		"statusText": "Успешно",
        /// 		"trmTxnId": "1489826461807",
        /// 		"account": "0003***",
        /// 		"sum": {
        /// 			"amount": 70,
        /// 			"currency": "RUB"
        /// 		},
        /// 		"commission": {
        /// 			"amount": 0,
        /// 			"currency": "RUB"
        /// 		},
        /// 		"total": {
        /// 			"amount": 70,
        /// 			"currency": "RUB"
        /// 		},
        /// 		"provider": {
        /// 			...
        /// 		},
        /// 		"source": {
        /// 			
        /// 		},
        /// 		"comment": null,
        /// 		"currencyRate": 1,
        /// 		"extras": null,
        /// 		"chequeReady": true,
        /// 		"bankDocumentAvailable": false,
        /// 		"bankDocumentReady": false,
        /// 		"repeatPaymentEnabled": false,
        /// 		"favoritePaymentEnabled": true,
        /// 		"regularPaymentEnabled": true
        /// 	}],
        /// 	"nextTxnId": 9001,
        /// 	"nextTxnDate": "2017-01-31T15:24:10+03:00"
        /// }
        /// </returns>
        public QiwiPaymentsDataClass PaymentHistory(int rows = 50, string operation = "ALL", string sources = null, DateTime? startDate = null, DateTime? endDate = null, DateTime? nextTxnDate = null, long? nextTxnId = null)
        {
            Console.Out.WriteLine("Запрос истории платежей [" + rows.ToString() + " rows]" + ((startDate == null || endDate == null || ((DateTime)startDate) >= ((DateTime)endDate)) ? " за последние сутки " : " c " + ((DateTime)startDate).ToString("yyyy-MM-ddTHH:mm:sszzz") + " по " + ((DateTime)endDate).ToString("yyyy-MM-ddTHH:mm:sszzz")) + "  ...");
            string requwst_string = "rows=" + rows + "&operation=" + operation;
            if (!string.IsNullOrEmpty(sources))
                requwst_string += "&sources=" + HttpUtility.UrlEncode(sources);
            if (startDate != null && endDate != null)
                requwst_string += "&startDate=" + HttpUtility.UrlEncode(((DateTime)startDate).ToString(date_time_format)) + "&endDate=" + HttpUtility.UrlEncode(((DateTime)endDate).ToString(date_time_format));
            if (nextTxnDate != null && nextTxnId != null)
                requwst_string += "&nextTxnDate=" + HttpUtility.UrlEncode(((DateTime)nextTxnDate).ToString(date_time_format)) + "&nextTxnId=" + nextTxnId.ToString();
            SendRequest("https://edge.qiwi.com/payment-history/v1/persons/" + my_profile.contractInfo.contractId + "/payments?" + requwst_string, HttpMethod.Get);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка получения истории платежей. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiPaymentsDataClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiPaymentsDataClass), HttpResponseResult);
        }

        /// <summary>
        /// Статистика платежей. Данный запрос используется для получения сводной статистики по суммам платежей за заданный период.
        /// URL https://edge.qiwi.com/payment-history/v1/persons/{wallet}/payments/total?parameter=value
        /// Запрос → GET
        /// В pathname GET-запроса используется параметр: wallet - номер кошелька, для которого получен токен доступа(с международным префиксом, но без +)
        /// 
        /// Максимальный период для выгрузки статистики - 90 календарных дней.
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer***
        /// </summary>
        /// <param name="startDate">Начальная дата периода статистики. Дату можно указать в любой временной зоне TZD (формат ГГГГ-ММ-ДД'T'чч:мм:ссTZD), однако она должна совпадать с временной зоной в параметре endDate. Обозначение временной зоны TZD: +чч:мм или -чч:мм (временной сдвиг от GMT). Обязательный параметр</param>
        /// <param name="endDate">Конечная дата периода статистики. Дату можно указать в любой временной зоне TZD (формат ГГГГ-ММ-ДД'T'чч:мм:ссTZD), однако она должна совпадать с временной зоной в параметре startDate. Обозначение временной зоны TZD: +чч:мм или -чч:мм (временной сдвиг от GMT). Обязательный параметр</param>
        /// <param name="operation">Тип операций, учитываемых при подсчете статистики. Допустимые значения: ALL - все операции, IN - только пополнения, OUT - только платежи, QIWI_CARD - только платежи по картам QIWI (QVC, QVP). По умолчанию ALL</param>
        /// <param name="sources">Источники платежа, учитываемые при подсчете статистики. Каждый источник задается как отдельный параметр и нумеруется элементом массива, начиная с нуля (sources[0], sources[1] и т.д.). Допустимые значения: QW_RUB - рублевый счет кошелька, QW_USD - счет кошелька в долларах, QW_EUR - счет кошелька в евро, CARD - привязанные и непривязанные к кошельку банковские карты, MK - счет мобильного оператора. Если не указаны, учитываются все источники</param>
        /// <returns>
        /// Пример:
        /// GET /payment-history/v1/persons/79112223344/payments/total?startDate=2017-03-01T00%3A00%3A00%2B03%3A00&endDate=2017-03-31T11%3A44%3A15%2B03%3A00 HTTP/1.1
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Content-type: application/json
        /// Host: edge.qiwi.com
        /// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 
        /// HTTP/1.1 200 OK
        /// Content-Type: application/json
        /// {
        /// 	"incomingTotal": [{
        /// 		"amount": 3500,
        /// 		"currency": "RUB"
        /// 	}],
        /// 	"outgoingTotal": [{
        /// 		"amount": 3497.5,
        /// 		"currency": "RUB"
        /// 	}]
        /// }
        /// </returns>
        public QiwiTotalPaymentsClass PaymentsTotal(DateTime startDate, DateTime endDate, string operation = "ALL", string sources = null)
        {
            Console.Out.WriteLine("Запрос оборотной финансовой статистики c " + startDate.ToString(date_time_format) + " по " + endDate.ToString(date_time_format));
            string requwst_string = "startDate=" + HttpUtility.UrlEncode(startDate.ToString(date_time_format)) + "&endDate=" + HttpUtility.UrlEncode(endDate.ToString(date_time_format)) + "&operation=" + operation;
            if (!string.IsNullOrEmpty(sources))
                requwst_string += "&sources=" + HttpUtility.UrlEncode(sources);
            SendRequest("https://edge.qiwi.com/payment-history/v1/persons/" + my_profile.contractInfo.contractId + "/payments/total?" + requwst_string, HttpMethod.Get);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка получения оборотной финансовой статистики. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiTotalPaymentsClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiTotalPaymentsClass), HttpResponseResult);
        }

        /// <summary>
        /// Информация о транзакции. Данный запрос используется для получения информации по определенной транзакции из вашей истории платежей.
        /// URL https://edge.qiwi.com/payment-history/v1/transactions/{transactionId}?type=value
        /// Запрос → GET
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer***
        /// </summary>
        /// <param name="transactionId">Номер транзакции из истории платежей (параметр data[].txnId в ответе)</param>
        /// <param name="type">Тип транзакции из истории платежей (параметр data[].type в ответе)</param>
        /// <returns>
        /// GET /payment-history/v1/transactions/9112223344?type=IN HTTP/1.1
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Content-type: application/json
        /// Host: edge.qiwi.com
        /// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 
        /// HTTP/1.1 200 OK
        /// Content-Type: application/json
        /// {
        /// 	"txnId": 11233344692,
        /// 	"personId": 79161122331,
        /// 	"date": "2017-08-30T14:38:09+03:00",
        /// 	"errorCode": 0,
        /// 	"error": null,
        /// 	"status": "WAITING",
        /// 	"type": "OUT",
        /// 	"statusText": "Запрос обрабатывается",
        /// 	"trmTxnId": "11233344691",
        /// 	"account": "15040930424823121081",
        /// 	"sum": {
        /// 		"amount": 1,
        /// 		"currency": 643
        /// 	},
        /// 	"commission": {
        /// 		"amount": 0,
        /// 		"currency": 643
        /// 	},
        /// 	"total": {
        /// 		"amount": 1,
        /// 		"currency": 643
        /// 	},
        /// 	"provider": {
        /// 		"id": 1,
        /// 		"shortName": "MTS",
        /// 		"longName": "MTS",
        /// 		"logoUrl": null,
        /// 		"description": null,
        /// 		"keys": null,
        /// 		"siteUrl": null,
        /// 		"extras": []
        /// 	},
        /// 	"source": {
        /// 		"id": 7,
        /// 		"shortName": "QIWI Wallet",
        /// 		"longName": "QIWI Wallet",
        /// 		"logoUrl": null,
        /// 		"description": null,
        /// 		"keys": "мобильный кошелек, кошелек, перевести деньги, личный кабинет, отправить деньги, перевод между пользователями",
        /// 		"siteUrl": null,
        /// 		"extras": []
        /// 	},
        /// 	"comment": null,
        /// 	"currencyRate": 1,
        /// 	"extras": [],
        /// 	"chequeReady": false,
        /// 	"bankDocumentAvailable": false,
        /// 	"bankDocumentReady": false,
        /// 	"repeatPaymentEnabled": false,
        /// 	"favoritePaymentEnabled": false,
        /// 	"regularPaymentEnabled": false
        /// }
        /// </returns>
        public QiwiPaymentClass TransactionIdInfo(long transactionId, string type)
        {
            Console.Out.WriteLine("Запрос информации о транзакции");
            SendRequest("https://edge.qiwi.com/payment-history/v1/transactions/"+ transactionId + "?type="+ type, HttpMethod.Get);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка получения информации о транзакции. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiPaymentClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiPaymentClass), HttpResponseResult);
        }

        /// <summary>
        /// Баланс QIWI Кошелька
        /// URL https://edge.qiwi.com/funding-sources/v1/accounts/current
        /// Запрос → GET
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer***
        /// </summary>
        /// <returns>
        /// GET /funding-sources/v1/accounts/current HTTP/1.1
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Content-type: application/json
        /// Host: edge.qiwi.com
        /// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 
        /// HTTP/1.1 200 OK
        /// Content-Type: application/json
        /// {
        /// 	"accounts": [{
        /// 		"alias": "qw_wallet_rub",
        /// 		"fsAlias": "qb_wallet",
        /// 		"title": "WALLET",
        /// 		"type": {
        /// 			"id": "WALLET",
        /// 			"title": "Visa QIWI Wallet"
        /// 		},
        /// 		"hasBalance": true,
        /// 		"balance": {
        /// 			"amount": 453.00,
        /// 			"currency": 643
        /// 		},
        /// 		"currency": 643
        /// 	},
        /// 	{
        /// 		"alias": "mc_megafon_rub",
        /// 		"fsAlias": "qb_mc_megafon",
        /// 		"title": "MC",
        /// 		"type": {
        /// 			"id": "MC",
        /// 			"title": "Mobile Wallet"
        /// 		},
        /// 		"hasBalance": true,
        /// 		"balance": null,
        /// 		"currency": 643
        /// 	}]
        /// }
        /// </returns>
        public QiwiCurrentBalanceClass CurrentBalance
        {
            get
            {
                Console.Out.WriteLine("Запрос баланса");
                SendRequest("https://edge.qiwi.com/funding-sources/v1/accounts/current", HttpMethod.Get);
                if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
                {
                    Console.Out.WriteLine("ERR: Ошибка получения баланса. [" + ReasonPhraseHTTP + "]");
                    return null;
                }
                return (QiwiCurrentBalanceClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiCurrentBalanceClass), HttpResponseResult);
            }
        }

        /// <summary>
        /// Комиссионные тарифы по провайдеру
        /// URL https://edge.qiwi.com/sinap/providers/{id}/form
        /// Запрос → GET
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///
        /// В pathname GET-запроса используется параметр:
        /// id - идентификатор провайдера.Возможные значения:
        /// 99 - Перевод на QIWI Wallet
        /// 1963 - Перевод на карту Visa (карты российских банков)
        /// 21013 - Перевод на карту MasterCard(карты российских банков)
        /// Для карт, выпущенных банками стран Азербайджан, Армения, Белоруссия, Грузия, Казахстан, Киргизия, Молдавия, Таджикистан, Туркменистан, Украина, Узбекистан:
        ///     1960 – Перевод на карту Visa
        ///      21012 – Перевод на карту MasterCard
        /// 31652 - национальная платежная система МИР
        /// 466 - Тинькофф Банк
        /// 464 - Альфа-Банк
        /// 821 - Промсвязьбанк
        /// 815 - Русский Стандарт
        /// Идентификаторы операторов мобильной связи (https://developer.qiwi.com/ru/qiwi-wallet-personal/index.html#mnp)
        /// Идентификаторы других провайдеров (https://developer.qiwi.com/ru/qiwi-wallet-personal/index.html#charity)
        /// 1717 - платеж по банковским реквизитам (https://developer.qiwi.com/ru/qiwi-wallet-personal/index.html#freepay)
        /// </summary>
        /// <param name="provider_id">ID провайдера</param>
        /// <returns>
        /// GET /sinap/providers/99/form HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// Host: edge.qiwi.com
        /// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 
        /// HTTP/1.1 200 OK
        /// Content-Type: application/json
        /// {
        /// 	"content": {
        /// 		"terms": {
        /// 			"commission": {
        /// 				"ranges": [{
        /// 					"bound": 0,
        /// 					"fixed": 50.0,
        /// 					"rate": 0.02
        /// 				}]
        /// 			}
        /// 		}
        /// 	}
        /// }
        /// </returns>
        public QiwiCommissionByProviderClass CommissionByProvider(string provider_id)
        {
            Console.Out.WriteLine("Камиссионные тарифы по провайдеру ["+ provider_id + "]");
            base.SendRequest("https://edge.qiwi.com/sinap/providers/"+ provider_id + "/form", HttpMethod.Get);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка получения тарифов. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiCommissionByProviderClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiCommissionByProviderClass), HttpResponseResult);
        }

        /// <summary>
        /// Рассчитать полную комиссию за платеж (с учетом всех тарифов) по заданному набору платежных реквизитов
        /// URL https://edge.qiwi.com/sinap/providers/{id}/onlineCommission
        /// Запрос → POST
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer ***
        /// 
        /// В pathname POST-запроса используется параметр:
        /// id - идентификатор провайдера.Возможные значения:
        /// 99 - Перевод на QIWI Wallet
        /// 1963 - Перевод на карту Visa (карты российских банков)
        /// 21013 - Перевод на карту MasterCard(карты российских банков)
        /// Для карт, выпущенных банками стран Азербайджан, Армения, Белоруссия, Грузия, Казахстан, Киргизия, Молдавия, Таджикистан, Туркменистан, Украина, Узбекистан:
        ///     1960 – Перевод на карту Visa
        ///      21012 – Перевод на карту MasterCard
        /// 31652 - национальная платежная система МИР
        /// 466 - Тинькофф Банк
        /// 464 - Альфа-Банк
        /// 821 - Промсвязьбанк
        /// 815 - Русский Стандарт
        /// Идентификаторы операторов мобильной связи (https://developer.qiwi.com/ru/qiwi-wallet-personal/index.html#mnp)
        /// Идентификаторы других провайдеров (https://developer.qiwi.com/ru/qiwi-wallet-personal/index.html#charity)
        /// 1717 - платеж по банковским реквизитам (https://developer.qiwi.com/ru/qiwi-wallet-personal/index.html#freepay)
        /// </summary>
        /// <param name="provider_id">ID провайдера</param>
        /// <param name="account">Номер телефона (с международным префиксом) или номер карты/счета получателя, в зависимости от провайдера</param>
        /// <param name="amount">Сумма перевода</param>
        /// <returns></returns>
        public QiwiResponseCalculationOnlineCommissionClass CalculationOnlineCommission(string provider_id, string account, int amount)
        {
            ContentType = "application/json";
            Console.Out.WriteLine("Расчёт комиссии по реквизитам [provider = " + provider_id + "] [получатель = " + account + "]");
            QiwiQueryCalculationOnlineCommissionClass json_calc = new QiwiQueryCalculationOnlineCommissionClass();
            json_calc.account = account;
            json_calc.purchaseTotals.total.amount = amount;
            string json_data = MultiTool.glob_tools.SerialiseJSON(json_calc);
            SendRequest("https://edge.qiwi.com/sinap/providers/" + provider_id + "/onlineCommission", HttpMethod.Post, null, json_data);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка расчёта коммиссии. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiResponseCalculationOnlineCommissionClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiResponseCalculationOnlineCommissionClass), HttpResponseResult);
        }

        /// <summary>
        /// Отправка платежа
        /// </summary>
        /// <param name="url">Адрес API</param>
        /// <param name="json_post">Реквизиты строй JSON</param>
        /// <returns>true если платёж прошёл удачно, false в противном случае</returns>
        private bool SendPayment(string url, string json_post)
        {
            ContentType = "application/json";
            SendRequest(url, HttpMethod.Post, null, json_post);
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка отправки денег. [" + ReasonPhraseHTTP + "]");
                if (StatusCodeHTTP == System.Net.HttpStatusCode.BadRequest)
                {
                    QiwiErrorSendPaymentClass err = (QiwiErrorSendPaymentClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiErrorSendPaymentClass), HttpResponseResult);
                    Console.Out.WriteLine("~ " + err.FullDescription);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Отправить платёж банку-партнёру по номеру счёта/договора и т.д.
        /// </summary>
        /// <param name="provider_id">Номер провайдера оплаты</param>
        /// <param name="details_bank_partner_transaction">Реквизиты для передачи банку-партнёру</param>
        /// <returns>Результат операции перевода банку-партнёру  либо null в случае ошибки</returns>
        public QiwiResultSendToBankParnerPaymentClass SendPayment(string provider_id, QiwiTransactionsPartnerBankDetailsPaymentClass details_bank_partner_transaction)
        {
            Console.Out.WriteLine("Отправка денег [provider=" + provider_id + "] [сумма=" + details_bank_partner_transaction.sum.amount.ToString() + "] [получатель=" + details_bank_partner_transaction.fields.account + "] [exp_date = " + details_bank_partner_transaction.fields.exp_date + "] [account_type = " + details_bank_partner_transaction.fields.account_type + "] [комментарий=" + details_bank_partner_transaction.comment + "]");
            string json_data = MultiTool.glob_tools.SerialiseJSON(details_bank_partner_transaction);
            if (SendPayment("https://edge.qiwi.com/sinap/api/v2/terms/" + provider_id + "/payments", json_data))
                return (QiwiResultSendToBankParnerPaymentClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiResultSendToBankParnerPaymentClass), HttpResponseResult);
            else
                return null;
        }

        /// <summary>
        /// Отправить платёж по стандартным правилам. Унивесальный/стандартный платёж используется когда провайдеру оплаты требуется всего один единственный реквизит получателя (номер карты\телефона).
        /// </summary>
        /// <param name="provider_id">Номер провайдера оплаты</param>
        /// <param name="details_transaction">>Реквизиты для передачи провайдеру оплаты</param>
        /// <returns>Результат операции перевода либо null в случае ошибки</returns>
        public QiwiResultSendUniversalPaymentClass SendPayment(string provider_id, QiwiTransactionsUniversalDetailsPaymentClass details_transaction)
        {
            Console.Out.WriteLine("Отправка денег [provider=" + provider_id + "] [сумма=" + details_transaction.sum.amount.ToString() + "] [получатель=" + details_transaction.fields.account + "] [комментарий=" + details_transaction.comment + "]");
            string json_data = MultiTool.glob_tools.SerialiseJSON(details_transaction);
            if (SendPayment("https://edge.qiwi.com/sinap/api/v2/terms/" + provider_id + "/payments", json_data))
                return (QiwiResultSendUniversalPaymentClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiResultSendUniversalPaymentClass), HttpResponseResult);
            else
                return null;
        }

        /// <summary>
        /// Отправить деньги
        /// URL https://edge.qiwi.com/sinap/api/v2/terms/{provider_id}/payments
        /// Запрос → POST
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/json
        ///   Authorization: Bearer***
        /// ##################################
        /// Вывод в банк. Коды провайдера:
        ///     466 - Тинькофф Банк
        ///     464 - Альфа-Банк
        ///     821 - Промсвязьбанк
        ///     815 - Русский Стандарт
        /// ##################################
        /// Перевод на карту. Коды провайдера:
        /// 1963 - Перевод на карту Visa(карты российских банков)
        /// 21013 - Перевод на карту MasterCard(карты российских банков)
        /// Для карт, выпущенных банками стран Азербайджан, Армения, Белоруссия, Грузия, Казахстан, Киргизия, Молдавия, Таджикистан, Туркменистан, Украина, Узбекистан:
        ///    1960 – Перевод на карту Visa
        ///    21012 – Перевод на карту MasterCard
        /// 31652 - национальная платежная система МИР
        /// </summary>
        /// <param name="provider_id">Идентэфикатор провайдера (Перевод на киви - 99)</param>
        /// <param name="TransactionDetails">Детали транзакции (получатель, сумма, комментарий и другие реквизиты).</param>
        /// <returns>
        /// POST /sinap/api/v2/terms/{provider_id}/payments HTTP/1.1
        /// Content-Type: application/json
        /// Accept: application/json
        /// Authorization: Bearer YUu2qw048gtdsvlk3iu
        /// Host: edge.qiwi.com
        /// {
        /// 	"id": "11111111111111",
        /// 	"sum": {
        /// 		"amount": 100.50,
        /// 		"currency": "643"
        /// 	},
        /// 	"paymentMethod": {
        /// 		"type": "Account",
        /// 		"accountId": "643"
        /// 	},
        /// 	"comment": "test",
        /// 	"fields": {
        /// 		"account": "+79121112233"
        /// 	}
        /// }
        /// ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ 
        /// HTTP/1.1 200 OK
        /// Content-Type: application/json
        /// {
        /// 	"id": "150217833198900",
        /// 	"terms": "99",
        /// 	"fields": {
        /// 		"account": "79121238345"
        /// 	},
        /// 	"sum": {
        /// 		"amount": 100,
        /// 		"currency": "643"
        /// 	},
        /// 	"transaction": {
        /// 		"id": "11155897070",
        /// 		"state": {
        /// 			"code": "Accepted"
        /// 		}
        /// 	},
        /// 	"source": "account_643",
        /// 	"comment": "test"
        /// }
        /// </returns>
        //public QiwiResultSendPaymentClass SendPayment(string provider_id, long sum_amount, string account, string comment = "")
        //{
            
        //    Console.Out.WriteLine("Отправка денег [provider=" + provider_id + "] [сумма=" + sum_amount.ToString() + "] [получатель=" + account + "] [комментарий=" + comment + "]");
        //    QiwiTransactionsUniversalDetailsPaymentClass TransactionDetails = new QiwiTransactionsUniversalDetailsPaymentClass();
        //    TransactionDetails.comment = comment;
        //    TransactionDetails.fields.account = account;
        //    TransactionDetails.sum.amount = sum_amount;
        //    string json_data = g.GetJSON(TransactionDetails);
        //    if (SendPayment("https://edge.qiwi.com/sinap/api/v2/terms/" + provider_id + "/payments", json_data))
        //        return (QiwiResultSendUniversalPaymentClass)g.ReadObject(typeof(QiwiResultSendUniversalPaymentClass), HttpResponseResult);
        //    else
        //        return null;
        //}

        /// <summary>
        /// Определение оператора мобильного номера. В ответе возвращается идентификатор оператора для запроса пополнения телефона.
        /// URL https://qiwi.com/mobile/detect.action
        /// Запрос → POST
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/x-www-form-urlencoded
        /// </summary>
        /// <param name="phone">Мобильный номер в международном формате +79991112233</param>
        /// <returns>
        /// Ответ с HTTP Status 200 и параметром code.value = 0 является признаком успешной проверки. Идентификатор оператора находится в параметре message.
        /// Ответ с HTTP Status 200 и параметром code.value = 2 означает, что невозможно определить оператора.
        /// </returns>
        public QiwiResultDetectProviderClass DetectMobileOperatorProvider(string phone)
        {
            ContentType = "application/x-www-form-urlencoded";
            Console.Out.WriteLine("Определение кода провайдера мобильного оператора по номеру телефона [" + phone + "]");
            base.SendRequest("https://qiwi.com/mobile/detect.action", HttpMethod.Post, null, "phone="+ HttpUtility.UrlEncode(phone));
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка определения кода провайдера мобильного ператора. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiResultDetectProviderClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiResultDetectProviderClass), HttpResponseResult);
        }

        /// <summary>
        /// Определение платежной системы карты. В ответе возвращается идентификатор платежной системы для запроса перевода на карту.
        /// URL https://qiwi.com/card/detect.action
        /// Запрос → POST
        /// 
        /// HEADERS
        ///   Accept: application/json
        ///   Content-type: application/x-www-form-urlencoded
        /// </summary>
        /// <param name="cardNumber">Немаскированный номер карты</param>
        /// <returns>
        /// Ответ с HTTP Status 200 и параметром code.value = 0 является признаком успешной проверки. Идентификатор платежной системы находится в параметре message.
        /// Ответ с HTTP Status 200 и параметром code.value = 2 означает, что в номере карты ошибка.
        /// </returns>
        public QiwiResultDetectProviderClass DetectCachCardProvider(string cardNumber)
        {
            ContentType = "application/x-www-form-urlencoded";
            Console.Out.WriteLine("Определение кода провайдера [Visa/MasterCard/МИР] по номеру карты [" + cardNumber + "]");
            base.SendRequest("https://qiwi.com/card/detect.action", HttpMethod.Post, null, "cardNumber=" + HttpUtility.UrlEncode(cardNumber));
            if (StatusCodeHTTP != System.Net.HttpStatusCode.OK)
            {
                Console.Out.WriteLine("ERR: Ошибка определения кода провайдера [Visa/MasterCard/МИР]. [" + ReasonPhraseHTTP + "]");
                return null;
            }
            return (QiwiResultDetectProviderClass)MultiTool.glob_tools.DeSerialiseJSON(typeof(QiwiResultDetectProviderClass), HttpResponseResult);
        }
    }
}
