////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Curl
{
    public class ClassCurl
    {
        protected string ContentType = "";
        protected string Accept = "application/json";
        protected HttpStatusCode StatusCodeHTTP;
        protected string ReasonPhraseHTTP;
        protected string HttpResponseResult = "";

        /// <summary>
        /// HTTP заголовок:
        /// Способ кодирования содержимого сущности при передаче.
        /// Пример: gzip, deflate, br
        /// </summary>
        protected string AcceptEncoding = "gzip, deflate, br";

        /// <summary>
        /// HTTP заголовок: Перечень поддерживаемых кодировок для предоставления пользователю.
        /// Пример: Accept-Charset: utf-8, iso-8859-1;q=0.5
        /// </summary>
        protected string AcceptCharset = "utf-8";

        /// <summary>
        /// Отправить HTTP запрос к серверу
        /// </summary>
        /// <param name="adress_direct">Адрес HTTP запроса</param>
        /// <param name="Method">Метод HTTP запроса</param>
        /// <param name="post_data">POST данные для отправки</param>
        /// <param name="headers">Заголовки, необходимые для отправки в запросе</param>
        /// <param name="insideAllowAutoRedirect">Поддержка HTTP перенаправлений</param>
        /// <param name="save_result_to_file">Место/файл в который нужно сохранить результат выполнения запроса (если требуется)</param>
        /// <returns></returns>
        protected virtual string SendRequest(string adress_direct, HttpMethod Method, Dictionary<string, string> headers = null, string post_data = null, bool insideAllowAutoRedirect = true, string save_result_to_file = null)
        {
            StatusCodeHTTP = HttpStatusCode.OK;
            HttpResponseResult = "";
            using (HttpClientHandler handler = new HttpClientHandler() { AllowAutoRedirect = insideAllowAutoRedirect })
            {
                using (HttpClient client = new HttpClient(handler) { BaseAddress = new Uri(adress_direct) })
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                    if (!String.IsNullOrEmpty(AcceptEncoding))
                        client.DefaultRequestHeaders.Add("Accept-Encoding", AcceptEncoding);

                    if (!String.IsNullOrEmpty(AcceptCharset))
                        client.DefaultRequestHeaders.Add("Accept-Charset", AcceptCharset);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));

                    HttpRequestMessage request = new HttpRequestMessage(Method, adress_direct);
                    if (headers != null)
                        foreach (KeyValuePair<string, string> kvp in headers)
                            request.Headers.Add(kvp.Key, kvp.Value);

                    if (post_data != null)
                        request.Content = new StringContent(post_data, Encoding.UTF8, ContentType);

                    try
                    {
                        using (HttpResponseMessage hrm = client.SendAsync(request).Result)
                        {
                            using (HttpContent hc = hrm.Content)
                            {
                                StatusCodeHTTP = hrm.StatusCode;
                                ReasonPhraseHTTP = hrm.ReasonPhrase;
                                HttpResponseResult = hc.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(save_result_to_file))
                                {
                                    File.WriteAllText(save_result_to_file, HttpResponseResult);
                                    return save_result_to_file;
                                }
                                else
                                    return HttpResponseResult;

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
        }
    }
}