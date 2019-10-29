////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Статистика платежей
    /// </summary>
    [DataContract]
    public class QiwiTotalPaymentsClass
    {
        /// <summary>
        /// Получить сумму входящих платежей по коду валюты
        /// </summary>
        /// <param name="currency">Код валюты (рубль -> 643)</param>
        /// <returns></returns>
        public double GetInByCurrency(string currency)
        {
            if (incomingTotal == null || incomingTotal.Length == 0)
                return 0;
            //
            foreach(QiwiSummClass summ in incomingTotal)
                if (summ.currency == currency)
                    return summ.amount;
            //
            return 0;
        }

        /// <summary>
        /// Получить сумму исходящих платежей по коду валюты
        /// </summary>
        /// <param name="currency">Код валюты (рубль -> 643)</param>
        /// <returns></returns>
        public double GetOutByCurrency(string currency)
        {
            if (outgoingTotal == null || outgoingTotal.Length == 0)
                return 0;
            //
            foreach (QiwiSummClass summ in outgoingTotal)
                if (summ.currency == currency)
                    return summ.amount;
            //
            return 0;
        }

        /// <summary>
        /// Данные о входящих платежах (пополнениях), отдельно по каждой валюте
        /// </summary>
        [DataMember]
        public QiwiSummClass[] incomingTotal;

        /// <summary>
        /// Данные об исходящих платежах, отдельно по каждой валюте
        /// </summary>
        [DataMember]
        public QiwiSummClass[] outgoingTotal;
    }
}
