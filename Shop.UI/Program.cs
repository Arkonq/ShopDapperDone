using Microsoft.Extensions.Configuration;
using QIWI.json;
using QIWI_API;
using Shop.DataAccess;
using Shop.Domain;
using Shop.Domain.Repository;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/*
		1. Регистрация и вход (смс-код / email код) - сделать до 11.10 (Email есть на метаните)
		2. История покупок 
		3. Категории и товары (картинка в файловой системе) 
		4. Покупка (корзина), оплата и доставка (PayPal/Qiwi/etc)
		5. Комментарии и рейтинги
		6. Поиск (пагинация - постраничность)

		Кто сделает 2 версии (Подключенный и EF) получит автомат на экзамене
*/

namespace Shop.UI
{
	class Program
	{
		static BCryptHasher bCryptHasher = new BCryptHasher();
		static IConfigurationBuilder builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", false, true);

		static IConfigurationRoot configurationRoot = builder.Build();
		static string connectionString = configurationRoot.GetConnectionString("HomeConnectionString");
		//static string connectionString = configurationRoot.GetConnectionString("ITStepConnectionString");
		static User user = null;

		static UserRepository userRepository = new UserRepository(connectionString);
		static CategoryRepository categoryRepository = new CategoryRepository(connectionString);
		static ItemRepository itemRepository = new ItemRepository(connectionString);
		static ReviewRepository reviewRepository = new ReviewRepository(connectionString);
		static PurchaseRepository purchaseRepository = new PurchaseRepository(connectionString);

		static void Main(string[] args)
		{
			Entry();
		}

		static void Entry()
		{
			int entryAnswer = -1;
			while (entryAnswer != 0)
			{
				Console.Clear();
				Console.WriteLine("1. Регистрация");
				Console.WriteLine("2. Вход");
				Console.WriteLine("0. Выход");
				Console.Write("Выберите действие: ");
				if (Int32.TryParse(Console.ReadLine(), out entryAnswer) == false || entryAnswer < 0)
				{
					entryAnswer = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
					WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
					continue;
				}
				switch (entryAnswer)
				{
					case 1:
						Registration();
						WriteMessage("Ваш аккаунт зарегестрирован!");
						break;
					case 2:
						if (!Authorization())
						{
							WriteMessage("Превышено кол-во попыток авторизации. Переход на главную страницу");
							continue;
						}
						WriteMessage("Вы успешно авторизовались!");
						Menu();
						break;
					case 0:
						entryAnswer = 0;
						break;
				}
			}
		}

		static void FillBalance()
		{
			string token;
			int fillSum;
			Console.WriteLine("Введите ваш QIWI token:");
			token = Console.ReadLine();
			Console.WriteLine("Введите сумму для пополнения:");
			fillSum = Int32.Parse(Console.ReadLine());
			//QIWI_API_Class q_raw = new QIWI_API_Class("af7899285396f3a5ee94f5545d210f5d"); // токен от 04
			//QIWI_API_Class q_raw = new QIWI_API_Class("cff2e4ebb1cb99dd86863e868bbf1eae"); // токен от 03
			QIWI_API_Class q_raw = new QIWI_API_Class(token);
			//QiwiCurrentBalanceClass cur_balance = q_raw.CurrentBalance; // Получить информацию о балансе кошелька
			//Console.WriteLine(cur_balance.GetQiwiBalance);
			QiwiTransactionsUniversalDetailsPaymentClass details_universal_transaction = new QiwiTransactionsUniversalDetailsPaymentClass(); // Этот набор реквизитов универсален. Подходит для перевода на QIWI, для пополнения баланса, для перевода на карту банка и другие переводы, которые требуют один реквизит [номер получателя]
			details_universal_transaction.comment = "Производится пополнение счета"; // комментарий
			details_universal_transaction.sum.amount = fillSum; // сумма
			details_universal_transaction.fields = new QiwiFieldsPaymentUniversalDirectClass() { account = "77078510204" }; // такой формат подойдёт для пеервода на QIWI, на карту банка или для пополнения баланса телефона

			QiwiResultSendUniversalPaymentClass SendUniversalPayment = q_raw.SendPayment("99", details_universal_transaction); // отправить платёж. Получатель для пополнения баланса мобильного телефона без семёрки/восмёрки (формат: 9995554422). Для перевода на киви в с семёркой (формат: 79995554422). Либо номер карты и т.п.
			if (SendUniversalPayment == null)
			{
				Console.WriteLine("Платеж отменен");
				return;
			}
			else
			{
				WriteMessage("Платеж успешно совершен");
				user.Balance += fillSum * 1000;
				userRepository.Update(user);
			}
			Console.ReadLine();
		}

		private static string ConsoleReadLineButHashed()
		{
			ConsoleKeyInfo key;
			StringBuilder sb = new StringBuilder();
			while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
			{
				sb.Append(key.KeyChar);
				Console.Write("*");
			}
			return sb.ToString();
		}

		private static bool Authorization()
		{
			Console.Clear();
			string truePassword = null, trueVerCode;
			string login, password, verCode;
			bool wrong = false;
			int attemptToLogIn = 0;
			Console.WriteLine("Введите Логин: ");
			login = Console.ReadLine();
			if (LoginExist(login))
			{
				truePassword = userRepository.GetHashedPassByLogin(login);
			}
			else
			{
				wrong = true;
			}
			Console.WriteLine("Введите пароль: ");
			password = ConsoleReadLineButHashed();
			Console.WriteLine();
			if (!wrong)
			{
				if (!bCryptHasher.VerifyPassword(truePassword, password))
				{
					wrong = true;
				}
			}
			if (wrong)
			{
				while (wrong && attemptToLogIn < 3)
				{
					Console.WriteLine("Ошибка! Неверный логин или пароль! Попробуйте снова.");
					Console.WriteLine($"Осталось попыток - {3 - attemptToLogIn++}");
					Console.WriteLine("Введите Логин: ");
					login = Console.ReadLine();
					if (LoginExist(login))
					{
						truePassword = userRepository.GetHashedPassByLogin(login);
						wrong = false;
					}
					else
					{
						wrong = true;
					}
					Console.WriteLine("Введите пароль: ");
					password = ConsoleReadLineButHashed();
					Console.WriteLine();
					if (!wrong)
					{
						if (!bCryptHasher.VerifyPassword(truePassword, password))
						{
							wrong = true;
						}
						else
						{
							wrong = false;
						}
					}
				}
			}
			if (wrong)
			{
				return false;
			}
			trueVerCode = userRepository.GetHashedVerCodeByLogin(login);
			Console.WriteLine("Введите секретный код: ");
			verCode = ConsoleReadLineButHashed();
			Console.WriteLine();
			if (!bCryptHasher.VerifyPassword(trueVerCode, verCode))
			{
				wrong = true;
			}
			if (wrong)
			{
				while (wrong && attemptToLogIn < 3)
				{
					Console.WriteLine("Ошибка! Неверный секретный код! Попробуйте снова.");
					Console.WriteLine($"Осталось попыток - {3 - attemptToLogIn++}");
					Console.WriteLine("Введите секретный код: ");
					verCode = ConsoleReadLineButHashed();
					Console.WriteLine();
					if (!bCryptHasher.VerifyPassword(trueVerCode, verCode))
					{
						wrong = true;
					}
					else
					{
						wrong = false;
					}
				}
			}
			if (wrong)
			{
				return false;
			}
			user = userRepository.GetUserByLogin(login);
			return true;
		}

		private static bool LoginExist(string login)
		{
			if (userRepository.HowMuchOfLoginsExist(login) > 0)
			{
				return true;
			}
			return false;
		}

		private static void Menu()
		{
			int menuAnswer = -1;
			while (menuAnswer != 0)
			{
				Console.WriteLine("1. Мой аккаунт");
				Console.WriteLine("2. Поиск товаров");
				Console.WriteLine("3. Все категории и товары");
				Console.WriteLine("0. Выход");
				Console.Write("Выберите действие: ");
				if (Int32.TryParse(Console.ReadLine(), out menuAnswer) == false || menuAnswer < 0)
				{
					menuAnswer = -1;  // При выпадении false в Int32.TryParse параметр out menuAnswer присваивается значние 0, и с новым циклом программа завершает работу
					WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
					continue;
				}
				switch (menuAnswer)
				{
					case 1:
						int infoAnswer = -1;
						while (infoAnswer != 0)
						{
							Console.Clear();
							Console.WriteLine("Информация о вашем аккаунте:");
							Console.WriteLine($"\tУникальный идентификатор: {user.Id}");
							Console.WriteLine($"\tДата регистрации: {user.CreationDate}");
							Console.WriteLine($"\tЛогин: {user.Login}");
							Console.WriteLine($"\tНа счету: {user.Balance}");
							Console.WriteLine($"\tПолное имя: {user.FullName}");
							Console.WriteLine($"\tНомер телефона: {user.PhoneNumber}");
							Console.WriteLine($"\tПочта: {user.Email}");
							Console.WriteLine($"\tАдрес доставки: {user.Address}");
							Console.WriteLine($"\tПароль: {user.Password}");
							Console.WriteLine($"\tСекретный код: {user.VerificationCode}");
							Console.WriteLine($"---------------------------------------");
							Console.WriteLine($"1. История покупок");
							Console.WriteLine($"2. Пополнить баланс");
							Console.WriteLine($"0. Назад");
							if (Int32.TryParse(Console.ReadLine(), out infoAnswer) == false || infoAnswer < 0 || infoAnswer > 2)
							{
								infoAnswer = -1;  // При выпадении false в Int32.TryParse параметр out infoAnswer присваивается значние 0, и с новым циклом программа завершает работу
								WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
								continue;
							}
							switch (infoAnswer)
							{
								case 1:
									Console.Clear();
									var purchases = purchaseRepository.GetAll(user.Id);
									int num = 1;
									Console.WriteLine($"  Товар\t\tЦена(тг.)\t(Дата покупки)");
									Console.WriteLine(purchases.Count);
									foreach (var purchase in purchases)
									{
										Item tempItem = itemRepository.SelectItemById(purchase.ItemId);
										Console.WriteLine($"{num++}){tempItem.Name}\t\t{tempItem.Price}\t{purchase.CreationDate}");
									}
									Console.ReadLine();
									break;
								case 2:
									FillBalance();
									break;
								case 0:
									infoAnswer = 0;
									break;
							}
						}
						Console.Clear();
						break;
					case 2:
						Search();
						break;
					case 3:
						Categories();
						break;
					case 0:
						menuAnswer = 0;
						break;
				}
			}
		}

		static void ProcessCollections()
		{
			List<string> cityNames = new List<string>
			{
				"Almaty", "Ankara", "Boriswill", "Nur-Sultan", "Yalta"
			};

			List<string> processedCityNames = new List<string>(); // для поиска товаров от пользователя
			foreach (string name in cityNames)
			{
				if (name.Contains("-"))
				{
					processedCityNames.Add(name);
				}
			}

			var result = from name
									 in cityNames
									 where name.Contains("-")
									 select name;

			var shortResult = cityNames.Where(name => name.Contains("-"));
			var shortResult2 = cityNames.Select(name => name.Contains("-"));
		}

		private static void Test()
		{
			Category category = new Category
			{
				Name = "Бытовая техника",
				//ImagePath = "C:/data",
			};


			Item item = new Item
			{
				Name = "Пылесос",
				//ImagePath = "C:/data/items",
				//Price = 25999,
				//Description = "Обычный пылесос",
				CategoryId = category.Id
			};

			User user = new User
			{
				Login = "Vanya",
				FullName = "Иван Иванов",
				PhoneNumber = "123456",
				Email = "qwer@qwr.qwr",
				Address = "Twesd, 12",
				Password = "password",
				VerificationCode = "1234"
			};

			User userForComment1 = new User
			{
				Login = "Alex"
			};
			User userForComment2 = new User
			{
				Login = "Sam"
			};

			for (int i = 0; i < 10; i++)
			{

				Review comment1 = new Review
				{
					UserId = userForComment1.Id,
					ItemId = item.Id,
					Rate = 5,
					Value = "Довольно мощный простой пылесос.Легкий, компактный, можно спрятать в небольшую нишу. Нравится, что пыль собирается в отдельную емкость, которую можно вытряхнуть и тут же помыть контейнер. Щетка не забивается шерстью и волосами. При работе пылесос чуть электризуется и корпус в части,где прикрепляется шланг, чуть покрывается тонким слоем пыли. Просто протираю тряпкой после использования. Не очень мобильная щетка, приходится прилагать легкое усилие, чтобы поворачивать её на полу как нужно, - ну так физкультура нам полезна. Но нужно учитывать и такой момент: психотравма от осознания того, сколько же пыли и грязи собирается в жалких 33 квадраных метрах даже через два дня после последней уборки. В пылесосах с мешками этого не видно. Готовьте свои и котовьи неррррвы."
				};

				Review comment2 = new Review
				{
					UserId = userForComment2.Id,
					ItemId = item.Id,
					Rate = 5,
					Value = "Испытал сразу перед НГ. Мощно и хорошо пылесосит ламинат и ковры. Щетка прилипает к полу от мощности. Засосал битые стела, мишуру, пыль и грязь - все собралось в циклоне, а мешок остался практически чистым. Не знаю есть ли там защита от перегрева, но пол часа на полной мощности проработал. Модель называется SC20M255AWB. Модель 251 - без циклона, лучше не покупать."
				};


				Review comment = new Review
				{
					UserId = Guid.Parse("c368194e-3f8c-4753-b39a-cceedf7b59ec"),
					ItemId = item.Id,
					Value = "Долго выбирал между Лж, Бошем и Самсунгом. Посмотрел 'Контрольную закупку' про пылесосы с мешком, и выбрал этот, с регулятором( модель SC5610 дешевле, но без регулятора мощности) Покупал за 1800, хотел именно классик, который понимает одноразовые бумажные мешки, и эти мешки есть в продаже по адекватной цене.До этого был занусси без мешка(циклон) и моющий кёрхер.Циклон оказался непрактичным, малоёмким 1, 2л, дорогим по замене фильтра.Моющий, конечно, хорош, и свежесть, и влажность повышает, но с ним много мороки: уборка занимает минут 20, а мойка и чистка самого моющего пылесоса и всех ёмкостей, мойка ванной после мойки моющего пылесоса на полчаса, иначе пахнет плесенью"
				};

				Purchase purchase = new Purchase
				{
					UserId = Guid.Parse("c368194e-3f8c-4753-b39a-cceedf7b59ec"),
					ItemId = Guid.Parse("FD70A938-A363-4EE1-A9F7-986B0BA419FD")
				};

				reviewRepository.Add(comment1);
				reviewRepository.Add(comment2);
			}
		}


		static void Registration()
		{
			Console.Clear();
			string login, fullName, phoneNum, email, address, password, verCode;
			bool exist = false;
			Console.WriteLine("Введите Логин: ");
			login = Console.ReadLine();
			int loginsCnt = userRepository.HowMuchOfLoginsExist(login);
			if (loginsCnt != 0)
			{
				exist = true;
			}
			if (exist)
			{
				while (exist)
				{
					WriteMessage("Ошибка! Введеный вами логин уже существует! Попробуйте ввести другой.");
					Console.WriteLine("Введите Логин: ");
					login = Console.ReadLine();
					loginsCnt = userRepository.HowMuchOfLoginsExist(login);
					if (loginsCnt != 0)
					{
						exist = true;
					}
					else
					{
						exist = false;
					}
				}
			}

			Console.WriteLine("Введите ФИО: ");
			fullName = Console.ReadLine();
			Console.WriteLine("Введите почту: ");
			email = Console.ReadLine();
			Console.WriteLine("Введите номер телефона: ");
			phoneNum = Console.ReadLine();
			Console.WriteLine("Введите адрес: ");
			address = Console.ReadLine();
			Console.WriteLine("Введите пароль: ");
			password = ConsoleReadLineButHashed();
			Console.WriteLine();
			password = bCryptHasher.EncryptPassword(password);
			Console.WriteLine("Введите секретный код (****): ");
			verCode = ConsoleReadLineButHashed();
			verCode = bCryptHasher.EncryptPassword(verCode);


			User user = new User
			{
				Login = login,
				FullName = fullName,
				PhoneNumber = phoneNum,
				Email = email,
				Address = address,
				Password = password,
				VerificationCode = verCode
			};

			userRepository.Add(user);
		}

		static void Categories()
		{
			int page = 1, pageSize = 2, pages = 1, categories = 0, itemsPageSize = 2;
			List<Category> categoriesList = null;

			categories = categoryRepository.AllCategoriesCount();

			pages = categories / pageSize;
			if (categories % pageSize != 0) // Если кол-во страниц выпало как 5/3 то выйдет лишь 1 страница, поэтому добавляем еще одну
			{
				pages++;
			}
			int chooseCategoryOrPageAnswer = -1;
			while (chooseCategoryOrPageAnswer != 0)
			{
				int categoryNum;
				CategoryPage(ref page, pageSize, pages);
				Console.WriteLine("1. Выбрать категорию.");
				Console.WriteLine("2. Выбрать страницу.");
				Console.WriteLine("0. Вернуться в главное меню.");
				if (Int32.TryParse(Console.ReadLine(), out chooseCategoryOrPageAnswer) == false || chooseCategoryOrPageAnswer > 2 || chooseCategoryOrPageAnswer < 0)
				{
					chooseCategoryOrPageAnswer = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
					WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
					continue;
				}
				switch (chooseCategoryOrPageAnswer)
				{
					case 1:
						Console.WriteLine($"Введите номер категории (1 - {pageSize}):");
						if (Int32.TryParse(Console.ReadLine(), out categoryNum) == false || categoryNum == 0)
						{
							WriteMessage("Введенное действие не является корректным. Возврат в выбор товара");
							continue;
						}
						if (categoryNum < 0)
						{
							categoryNum = -categoryNum;
						}
						int categoriesOnPage = categoryRepository.CategoriesOnPageCnt(page, pageSize); // подсчет кол-ва записей на странице

						categoriesOnPage = categoryRepository.CategoriesOnPageCnt(page, pageSize);

						if (categoryNum > pageSize || categoryNum > categoriesOnPage)
						{
							WriteMessage("Данный номер категории отсутсвует на странице.");
							continue;
						}
						Category choosenCategory = categoryRepository.ChooseCategory(page, pageSize, categoryNum);

						int itemsInCategoryCount = itemRepository.ItemsInCategory(choosenCategory.Id);

						if (itemsInCategoryCount == 0)
						{
							WriteMessage("По вашему запросу не найдено ни одного товара.");
							continue;
						}

						int itemsPages = itemsInCategoryCount / pageSize;

						if (itemsInCategoryCount % pageSize != 0) // Если кол-во страниц выпало как 5/3 то выйдет лишь 1 страница, поэтому добавляем еще одну
						{
							itemsPages++;
						}

						int itemsPage = 1, itemNum;
						int chooseItemOrPageAnswer = -1;

						while (chooseItemOrPageAnswer != 0)
						{
							ShopPage(itemsPage, itemsPageSize, itemsPages, choosenCategory);
							Console.WriteLine("1. Выбрать товар.");
							Console.WriteLine("2. Выбрать страницу.");
							Console.WriteLine("0. Вернуться к выбору категории.");
							if (Int32.TryParse(Console.ReadLine(), out chooseItemOrPageAnswer) == false)
							{
								chooseItemOrPageAnswer = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
								WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
								continue;
							}
							int itemsOnPage = 0;
							switch (chooseItemOrPageAnswer)
							{
								case 0:
									Console.Clear();
									continue;
								case 1:
									Console.WriteLine($"Введите товар (1 - {pageSize}):");
									if (Int32.TryParse(Console.ReadLine(), out itemNum) == false || itemNum == 0)
									{
										WriteMessage("Введенное действие не является корректным. Возврат в выбор товара");
										continue;
									}
									if (itemNum < 0)
									{
										itemNum = -itemNum;
									}

									itemsOnPage = itemRepository.ItemsOnPage(itemsPage, pageSize).Count;

									if (itemNum > pageSize || itemNum > itemsOnPage)
									{
										WriteMessage("Данный номер товара отсутсвует на странице.");
										continue;
									}
									int chooseCommentsOrBuyAnswer = -1, commentsPageSize = 2, comments = 1, commentsPage = 1, commentsPages = 1;
									while (chooseCommentsOrBuyAnswer != 0)
									{
										commentsPage = 1; // Есть вероятность что пользователь попадет в false в Int32.TryParse параметр out commentsPage, где commentsPage присваивается -1, что вскоре приведет к ошибке
										Guid resultItemId = ChooseItem(itemNum, itemsPage, pageSize, choosenCategory).Id;
										Console.WriteLine("1. Посмотреть комментарии");
										Console.WriteLine("2. Приобрести товар");
										Console.WriteLine("3. Оставить комментарий");
										Console.WriteLine("0. Назад");
										if (Int32.TryParse(Console.ReadLine(), out chooseCommentsOrBuyAnswer) == false || chooseCommentsOrBuyAnswer < 0)
										{
											chooseCommentsOrBuyAnswer = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
											WriteMessage("Введенное действие не является корректным. Возврат в выбор товара.");
											continue;
										}
										switch (chooseCommentsOrBuyAnswer)
										{
											case 0:
												continue;
											case 1:
												comments = reviewRepository.ReviewsCount(resultItemId);

												if (comments == 0)
												{
													WriteMessage("По вашему запросу не найдено ни одного комментария.");
													continue;
												}
												commentsPages = comments / commentsPageSize;
												if (comments % commentsPageSize != 0) // Если кол-во страниц выпало как 5/3 то выйдет лишь 1 страница, поэтому добавляем еще одну
												{
													commentsPages++;
												}
												CommentPage(commentsPages, commentsPageSize, commentsPage, resultItemId);
												while (commentsPage != 0)
												{
													Console.WriteLine($"Введите страницу (1 - {commentsPages}; 0 - Назад):");
													if (Int32.TryParse(Console.ReadLine(), out commentsPage) == false)
													{
														commentsPage = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
														WriteMessage("Введенная страница не является числом.");
														continue;
													}
													if (commentsPage == 0)
													{
														continue;
													}
													CommentPage(commentsPages, commentsPageSize, commentsPage, resultItemId);
												}
												break;
											case 2:
												int price;
												int itemsOnStock;

												itemsOnStock = itemRepository.GetQuantity(resultItemId);
												price = itemRepository.GetPrice(resultItemId);

												if (itemsOnStock == 0)
												{
													WriteMessage("Товар не доступен к приобретению, так как закончился на складе");
													break;
												}
												if (user.Balance >= price)
												{
													user.Balance -= price;
													userRepository.Update(user);
													Purchase purchase = new Purchase
													{
														ItemId = resultItemId,
														UserId = user.Id
													};

													purchaseRepository.Add(purchase);

													Minus1ToQuantity(resultItemId);
													Console.WriteLine("Платеж успешно совершен.");
													Console.ReadLine();
												}
												else
												{
													WriteMessage("На счету недостаточно средств");
												}
												break;
											case 3:
												int purchases = purchaseRepository.BoughtItemsByUser(user, resultItemId);

												if (purchases == 0)
												{
													WriteMessage("Вы никогда не приобретали этот товар. Вы не можете оставить комментарий");
													break;
												}
												int reviewRate = 0;
												Console.WriteLine("Введите оценку продукту (1-5)");
												if (Int32.TryParse(Console.ReadLine(), out reviewRate) == false || (reviewRate <= 0 || reviewRate > 5))
												{
													bool wrong = true;
													while (wrong)
													{
														WriteMessage("Введена некорректная оценка. Введите новую оценку");
														Console.WriteLine("Введите оценку продукту(1-5)");
														if (Int32.TryParse(Console.ReadLine(), out reviewRate) == true && (reviewRate > 0 && reviewRate <= 5))
														{
															wrong = false;
														}
													}
												}
												Console.WriteLine("Введите ваш комментарий");
												string reviewText;
												reviewText = Console.ReadLine();
												Review userReview = new Review
												{
													ItemId = resultItemId,
													Rate = reviewRate,
													UserId = user.Id,
													Value = reviewText
												};
												reviewRepository.Add(userReview);
												Console.WriteLine("Комментарий успешно добавлен");
												Console.ReadLine();
												break;
										}
									}
									break;
								case 2:
									Console.WriteLine($"Введите страницу (1 - {pages}):");
									if (Int32.TryParse(Console.ReadLine(), out itemsPage) == false || itemsPage <= 0 || itemsPage > pages)
									{
										itemsPage = 1;
										WriteMessage("Введен некорректный номер страницы.");
										continue;
									}
									ShopPage(itemsPage, pageSize, pages, choosenCategory);
									break;
							}
						}
						break;
					case 2:

						Console.WriteLine($"Введите страницу (1 - {pages}):");
						if (Int32.TryParse(Console.ReadLine(), out page) == false || page <= 0)
						{
							page = 1;
							WriteMessage("Введен некорректный номер страницы.");
							continue;
						}
						CategoryPage(ref page, pageSize, pages);
						break;
					case 0:
						Console.Clear();
						chooseCategoryOrPageAnswer = 0;
						continue;
				}
			}
		}

		private static void Minus1ToQuantity(Guid resultItemId)
		{
			var item = itemRepository.SelectItemById(resultItemId);
			item.Quantity -= 1;
			itemRepository.Update(item);
		}

		private static Item ChooseItem(int toSkip, int page, int pageSize, Category choosenCategory)
		{
			Console.Clear();
			toSkip--; // Если мы берем 1ый Айтем, то нужно сделать 0 скипов на странице, так как сразу берется(Take) первый же, и так далее

			var item = itemRepository.ChooseItem(page, pageSize, toSkip);

			Guid resultItemId = item.Id;

			var reviewList = reviewRepository.GetAll(resultItemId);
			int reviews = reviewList.Count;

			double avrgRate = 0;

			foreach (var review in reviewList)
			{
				avrgRate += review.Rate;
			}

			avrgRate /= (double)reviewList.Count;
			Console.WriteLine($"\tНаименование: {item.Name}");
			Console.WriteLine($"\tИзображение: {item.ImagePath}");
			Console.WriteLine($"\tУникальный идентификатор: {item.Id}");
			Console.WriteLine($"\tЦена: {item.Price}");
			Console.WriteLine($"\tОписание: {item.Description}");
			Console.WriteLine($"\tНа складе: {item.Quantity}");
			Console.WriteLine($"\tОтзывов - {reviews}");
			Console.WriteLine($"\tОбщий рейтинг - {avrgRate.ToString("0.0")}");

			return item;
		}

		private static int ShopPage(int page, int pageSize, int pages, Category choosenCategory)
		{
			Console.Clear();
			if (page < 0)
			{
				page = -page;
			}
			if (page > pages)
			{
				Console.WriteLine("Введенной страницы не существует.");
				Console.ReadLine();
				Console.Clear();
			}
			var itemsOnPage = itemRepository.ItemsOnPage(page, pageSize);

			Console.WriteLine($"Page {page}/{pages}: ");
			int num = 1;

			foreach (var item in itemsOnPage)
			{
				Console.WriteLine($"\t{num++}) {item.Name}");
			}
			return page;
		}

		public static void CategoryPage(ref int page, int pageSize, int pages)
		{
			Console.Clear();

			if (page < 0)
			{
				page = -page;
			}

			if (page > pages)
			{
				WriteMessage("Введенной страницы не существует.");
				page = 1;
				return;
			}
			var categoriesOnPage = categoryRepository.CategoriesOnPage(page, pageSize);

			Console.WriteLine($"Page {page}/{pages}:");

			int num = 1;

			foreach (var category in categoriesOnPage)
			{
				Console.WriteLine($"\t{num++}) {category.Name}");
			}
		}

		static void Search()
		{
			int page = 1, pageSize = 2, pages = 1, items = 0, chooseItemOrPageAnswer, itemNum;
			string search = null;
			List<Item> result = null;
			IQueryable<Item> query;
			while (search != "0")
			{
				page = 1; // Есть вероятность что пользователь попадет в false в Int32.TryParse параметр out page, где page присваивается -1, что вскоре приведет к ошибке
				Console.Clear();
				Console.WriteLine("Введите искомый товар (0 - Назад):");
				search = Console.ReadLine();
				if (search == "0")
				{
					Console.Clear();
					continue;
				}

				items = itemRepository.ItemsBySearch(search);

				if (items == 0)
				{
					WriteMessage("По вашему запросу не найдено ни одного товара.");
					continue;
				}
				pages = items / pageSize;
				if (items % pageSize != 0) // Если кол-во страниц выпало как 5/3 то выйдет лишь 1 страница, поэтому добавляем еще одну
				{
					pages++;
				}

				while (page != 0)
				{
					ShopPage(page, pageSize, pages, search);
					Console.WriteLine("1. Выбрать товар.");
					Console.WriteLine("2. Выбрать страницу.");
					Console.WriteLine("0. Вернуться к поиску товаров.");
					if (Int32.TryParse(Console.ReadLine(), out chooseItemOrPageAnswer) == false)
					{
						chooseItemOrPageAnswer = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
						WriteMessage("Введенное действие не является корректным. Выберите действие из списка!");
						continue;
					}
					switch (chooseItemOrPageAnswer)
					{
						case 0:
							page = 0;
							Console.Clear();
							continue;
						case 1:
							Console.WriteLine($"Введите товар (1 - {pageSize}):");
							if (Int32.TryParse(Console.ReadLine(), out itemNum) == false || itemNum == 0)
							{
								WriteMessage("Введенное действие не является корректным. Возврат в выбор товара");
								continue;
							}
							if (itemNum < 0)
							{
								itemNum = -itemNum;
							}

							int itemsOnPage = itemRepository.ItemsOnPage(page, pageSize, search).Count;

							if (itemNum > pageSize || itemNum > itemsOnPage)
							{
								WriteMessage("Данный номер товара отсутсвует на странице.");
								continue;
							}
							ItemInformation(page, pageSize, itemNum, search);
							break;
						case 2:
							Console.WriteLine($"Введите страницу (1 - {pages}):");
							if (Int32.TryParse(Console.ReadLine(), out page) == false)
							{
								page = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
								WriteMessage("Введенная страница не является числом.");
								continue;
							}
							ShopPage(page, pageSize, pages, search);
							break;
					}
				}
			}
		}

		private static void ItemInformation(int page, int pageSize, int itemNum, string search)
		{
			int chooseCommentsOrBuyAnswer = -1, commentsPageSize = 2, comments = 1, commentsPage = 1, commentsPages = 1;
			while (chooseCommentsOrBuyAnswer != 0)
			{
				commentsPage = 1; // Есть вероятность что пользователь попадет в false в Int32.TryParse параметр out commentsPage, где commentsPage присваивается -1, что вскоре приведет к ошибке
				Guid resultItemId = ChooseItem(itemNum, page, pageSize, search).Id;
				Console.WriteLine("1. Посмотреть комментарии");
				Console.WriteLine("2. Приобрести товар");
				Console.WriteLine("3. Оставить комментарий");
				Console.WriteLine("0. Назад");
				if (Int32.TryParse(Console.ReadLine(), out chooseCommentsOrBuyAnswer) == false || chooseCommentsOrBuyAnswer < 0)
				{
					chooseCommentsOrBuyAnswer = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
					WriteMessage("Введенное действие не является корректным. Возврат в выбор товара.");
					continue;
				}
				switch (chooseCommentsOrBuyAnswer)
				{
					case 0:
						continue;
					case 1:
						comments = reviewRepository.GetAll(resultItemId).Count;
						if (comments == 0)
						{
							WriteMessage("По вашему запросу не найдено ни одного комментария.");
							continue;
						}
						commentsPages = comments / commentsPageSize;
						if (comments % commentsPageSize != 0) // Если кол-во страниц выпало как 5/3 то выйдет лишь 1 страница, поэтому добавляем еще одну
						{
							commentsPages++;
						}
						CommentPage(commentsPages, commentsPageSize, commentsPage, resultItemId);
						while (commentsPage != 0)
						{
							Console.WriteLine($"Введите страницу (1 - {commentsPages}; 0 - Назад):");
							if (Int32.TryParse(Console.ReadLine(), out commentsPage) == false)
							{
								commentsPage = -1;  // При выпадении false в Int32.TryParse параметр out page присваивается значние 0, и с новым циклом программа завершает работу
								WriteMessage("Введенная страница не является числом.");
								continue;
							}
							if (commentsPage == 0)
							{
								continue;
							}
							CommentPage(commentsPages, commentsPageSize, commentsPage, resultItemId);
						}
						break;
					case 2:
						int itemPrice, itemQuantity;

						itemQuantity = itemRepository.SelectItemById(resultItemId).Quantity;
						itemPrice = itemRepository.SelectItemById(resultItemId).Price;

						if (itemQuantity == 0)
						{
							WriteMessage("Товар не доступен к приобретению, так как закончился на складе");
							break;
						}
						if (user.Balance >= itemPrice)
						{
							user.Balance -= itemPrice;
							userRepository.Update(user);
							Purchase purchase = new Purchase
							{
								ItemId = resultItemId,
								UserId = user.Id
							};

							purchaseRepository.Add(purchase);

							Minus1ToQuantity(resultItemId);
							Console.WriteLine("Платеж успешно совершен.");
							Console.ReadLine();
						}
						else
						{
							WriteMessage("На счету недостаточно средств");
						}
						break;
					case 3:
						int purchases = purchaseRepository.BoughtItemsByUser(user, resultItemId);

						if (purchases == 0)
						{
							WriteMessage("Вы никогда не приобретали этот товар. Вы не можете оставить комментарий");
							break;
						}
						int reviewRate = 0;
						Console.WriteLine("Введите оценку продукту(1-5)");
						if (Int32.TryParse(Console.ReadLine(), out reviewRate) == false || (reviewRate <= 0 || reviewRate > 5))
						{
							bool wrong = true;
							while (wrong)
							{
								WriteMessage("Введена некорректная оценка. Введите новую оценку");
								Console.WriteLine("Введите оценку продукту(1-5)");
								if (Int32.TryParse(Console.ReadLine(), out reviewRate) == true && (reviewRate > 0 && reviewRate <= 5))
								{
									wrong = false;
								}
							}
						}
						Console.WriteLine("Введите ваш комментарий");
						string reviewText;
						reviewText = Console.ReadLine();
						Review userReview = new Review
						{
							ItemId = resultItemId,
							Rate = reviewRate,
							UserId = user.Id,
							Value = reviewText
						};
						reviewRepository.Add(userReview);
						Console.WriteLine("Комментарий успешно добавлен");
						Console.ReadLine();
						break;
				}
			}
		}

		private static void CommentPage(int commentsPages, int commentsPageSize, int commentsPage, Guid resultItemId)
		{
			Console.Clear();
			if (commentsPage < 0)
			{
				commentsPage = -commentsPage;
			}
			if (commentsPage > commentsPages)
			{
				Console.WriteLine("Введенной страницы не существует.");
				Console.ReadLine();
				Console.Clear();
				return;
			}
			var reviewsOnPage = reviewRepository.ReviewsOnPage(commentsPage, commentsPageSize, resultItemId);

			Console.WriteLine($"Page {commentsPage}/{commentsPages}:");

			int num = 1;
			foreach (var review in reviewsOnPage)
			{
				var userId = reviewRepository.GetUserId(review.Id);
				string userName = userRepository.GetUserById(userId).Login;
				Console.WriteLine($"{num++})Пользователь {userName} оставил комментарий ({review.Rate}/5):");
				Console.Write("\t");
				Console.WriteLine(DivideComment(review.Value));
			}
		}

		private static string DivideComment(string str)
		{
			String[] sublines = str.Split(' ');
			str = null;
			int length = 80; //длина разбиения
			int j = 0;
			for (int i = 0; i < sublines.Count(); i++)
			{
				if (j + sublines[i].Length < length)
				{
					str = str + sublines[i] + " ";
					j = j + sublines[i].Length;
				}
				else
				{
					j = 0;
					str = str + "\r\n\t";
					i--;
				}
			}
			return str;
		}

		private static void WriteMessage(string message)
		{
			Console.Clear();
			Console.WriteLine(message);
			Console.ReadLine();
			Console.Clear();
		}

		private static Item ChooseItem(int toSkip, int page, int pageSize, string search)
		{
			Console.Clear();
			toSkip--; // Если мы берем 1ый Айтем, то нужно сделать 0 скипов на странице, так как сразу берется(Take) первый же, и так далее

			var item = itemRepository.ChooseItem(page, pageSize, toSkip, search);
					 
			var reviews = reviewRepository.ReviewsCount(item.Id);
			var reviewList = reviewRepository.GetAll(item.Id);

			double avrgRate = 0;
			foreach (var review in reviewList)
			{
				avrgRate += review.Rate;
			}
			avrgRate /= (double)reviewList.Count;

			Console.WriteLine($"\tНаименование: {item.Name}");
			Console.WriteLine($"\tИзображение: {item.ImagePath}");
			Console.WriteLine($"\tУникальный идентификатор: {item.Id}");
			Console.WriteLine($"\tЦена: {item.Price}");
			Console.WriteLine($"\tОписание: {item.Description}");
			Console.WriteLine($"\tНа складе: {item.Quantity}");
			Console.WriteLine($"\tОтзывов - {reviews}");
			Console.WriteLine($"\tОбщий рейтинг - {avrgRate.ToString("0.0")}");

			return item;
		}

		private static void ShopPage(int page, int pageSize, int pages, string search)
		{
			Console.Clear();
			if (page < 0)
			{
				page = -page;
			}
			if (page > pages)
			{
				Console.WriteLine("Введенной страницы не существует.");
				Console.ReadLine();
				Console.Clear();
				return;
			}
			var itemsOnPage = itemRepository.ItemsOnPage(page, pageSize, search);

			Console.WriteLine($"Page {page}/{pages}:");
			int num = 1;
			foreach (var item in itemsOnPage)
			{
				Console.WriteLine($"\t{num++}) {item.Name}");
			}
		}
	}
}