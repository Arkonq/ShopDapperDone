////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System.Runtime.Serialization;

namespace QIWI.json
{
    /// <summary>
    /// Баланс QIWI кошелька
    /// </summary>
    [DataContract]
    public class QiwiCurrentBalanceClass
    {
        

        /// <summary>
        /// Массив балансов
        /// </summary>
        [DataMember]
        public QiwiAccountClass[] accounts;

        /// <summary>
        /// Получить баланс QIWI
        /// </summary>
        public double GetQiwiBalance
        {
            get
            {
                if (accounts == null || accounts.Length == 0)
                    return 0;
                foreach (QiwiAccountClass account in accounts)
                    if (account.alias.ToLower() == "qw_wallet_rub")
                        return account.balance.amount;
                return 0;
            }
        }
    }
}
