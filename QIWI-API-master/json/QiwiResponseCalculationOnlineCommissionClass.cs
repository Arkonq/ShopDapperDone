////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Результат расчёта коммиссии по реквизитам
    /// </summary>
    [DataContract]
    public class QiwiResponseCalculationOnlineCommissionClass
    {
        [DataMember]
        public int providerId;
        /// <summary>
        /// Сумма вместа с коммиссией
        /// </summary>
        [DataMember]
        public QiwiSummClass withdrawSum;
        /// <summary>
        /// Сумма без камиссии
        /// </summary>
        [DataMember]
        public QiwiSummClass enrollmentSum;
        /// <summary>
        /// Комиссия QIWI
        /// </summary>
        [DataMember]
        public QiwiSummClass qwCommission;
        [DataMember]
        public QiwiSummClass fundingSourceCommission;
        [DataMember]
        public int withdrawToEnrollmentRate;
    }
}
