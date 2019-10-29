using QIWI.json;
using System;

namespace QIWI_API
{
	class Program
	{
		static void Main()
		{
			string token;
			int fillSum;
			Console.WriteLine("Введите ваш QIWI token:");	
			token = Console.ReadLine();
			Console.WriteLine("Введите сумму пополнения:");
			fillSum = Int32.Parse(Console.ReadLine());
			//QIWI_API_Class q_raw = new QIWI_API_Class("af7899285396f3a5ee94f5545d210f5d"); // токен от 04
			//QIWI_API_Class q_raw = new QIWI_API_Class("cff2e4ebb1cb99dd86863e868bbf1eae"); // токен от 03
			QIWI_API_Class q_raw = new QIWI_API_Class(token);
			//QiwiPaymentsDataClass payments = q_raw.PaymentHistory(); // Получить историю транзакций
			//QiwiTotalPaymentsClass total_payments = q_raw.PaymentsTotal(DateTime.Now.AddDays(-90), DateTime.Now); // Получить оборотную суммарную статистику за интересующий период
			//QiwiPaymentClass transaction_info = q_raw.TransactionIdInfo(12158385869, "OUT"); // Получить информацию о транзакции
			QiwiCurrentBalanceClass cur_balance = q_raw.CurrentBalance; // Получить информацию о балансе кошелька
			Console.WriteLine(cur_balance.GetQiwiBalance);
			QiwiTransactionsUniversalDetailsPaymentClass details_universal_transaction = new QiwiTransactionsUniversalDetailsPaymentClass(); // Этот набор реквизитов универсален. Подходит для перевода на QIWI, для пополнения баланса, для перевода на карту банка и другие переводы, которые требуют один реквизит [номер получателя]
			details_universal_transaction.comment = "Производится пополнение счета"; // комментарий
			details_universal_transaction.sum.amount = fillSum; // сумма
			details_universal_transaction.fields = new QiwiFieldsPaymentUniversalDirectClass() { account = "77078510203" }; // такой формат подойдёт для пеервода на QIWI, на карту банка или для пополнения баланса телефона

			QiwiResultSendUniversalPaymentClass SendUniversalPayment = q_raw.SendPayment("99", details_universal_transaction); // отправить платёж. Получатель для пополнения баланса мобильного телефона без семёрки/восмёрки (формат: 9995554422). Для перевода на киви в с семёркой (формат: 79995554422). Либо номер карты и т.п.
			if(SendUniversalPayment == null)
			{
				Console.WriteLine("Платеж отменен");
				// continue;
			}
			else
			{
				Console.WriteLine("Платеж успешно совершен");
				// user.Balance += sum * 1000;
			}
			Console.ReadLine();
		}
	}
}