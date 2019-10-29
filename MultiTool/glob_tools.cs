////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace MultiTool
{
    /// <summary>
    /// Набор универсальных часто используемых интсрументов
    /// </summary>
    public class glob_tools
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Формат даты
        /// </summary>
        public const string DateFormat = "dd-MM-yy";
        /// <summary>
        /// Формат времени
        /// </summary>
        public const string TimeFormat = "HH:mm:ss";
        /// <summary>
        /// Формат полной Дата+Время
        /// </summary>
        public static string DateTimeFormat { get { return DateFormat + " " + TimeFormat; } }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string Guid { get { return System.Guid.NewGuid().ToString().Replace("-", ""); } } // генератор уникальной строки

        /// <summary>
        /// Преобразование строки 16-ричного числа в целое число
        /// </summary>
        /// <param name="hex_string">Данные для преобразования</param>
        /// <returns>Целое число. Если преобразование не получится результат вернётся int.MinValue</returns>
        public static int HexToInt(string hex_string)
        {
            int i_result = int.MinValue;
            int.TryParse(hex_string, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out i_result);
            //
            return i_result;
        }

        /// <summary>
        /// Папка расположения исполняемого файла
        /// </summary>
        public static string ExeDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static double GetDoubleFromString(string conv_data)
        {
            if (string.IsNullOrWhiteSpace(conv_data))
                return 0;

            int count_digt_separators = (conv_data.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length - 1) + (conv_data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length - 1);

            if (count_digt_separators > 1)
                return 0;

            if (count_digt_separators == 1)
                conv_data = "0" + conv_data + "0";

            conv_data = conv_data.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            double ret_val;
            if (!double.TryParse(conv_data, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out ret_val))
                return 0;

            return ret_val;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Сериализует объект в JSON строку
        /// </summary>
        /// <returns>строка в формате JSON</returns>
        public static string SerialiseJSON(object _obj)
        {
            MemoryStream m_stream = new MemoryStream();
            DataContractJsonSerializer ser;
            StreamReader sr;
            string s;
            //
            ser = new DataContractJsonSerializer(_obj.GetType());
            ser.WriteObject(m_stream, _obj);
            m_stream.Position = 0;
            sr = new StreamReader(m_stream);
            s = sr.ReadToEnd();
            return s;
        }

        /// <summary>
        /// Прочитать JSON строку в Объект
        /// </summary>
        /// <param name="t">Тип, в котороый требуется преобразовать JON строку</param>
        /// <param name="json">Строка JSON</param>
        /// <returns></returns>
        public static object DeSerialiseJSON(Type t, string json)
        {
            return new DataContractJsonSerializer(t).ReadObject(StringToStream(json));
        }

        /// <summary>
        /// Проверка корректности e-mail
        /// </summary>
        /// <param name="strIn">строка e-mail для провреки</param>
        /// <returns>true если переданая строка валиный e-mail. false в противном случае</returns>
        public static bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Return true if strIn is in valid e-mail format.
            return System.Text.RegularExpressions.Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Строку в Hash MD5 байты
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] HashMD5(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        //
        /// <summary>
        /// Преобразовать строку в ХЕШ строку
        /// </summary>
        /// <param name="inputString">Строка для преобразования в ХЕШ строку</param>
        /// <returns>ХЕШ строка из переданой строки</returns>
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in HashMD5(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Имя текущего (последнего вызванного) метода
        /// </summary>
        /// <returns>Имя метода</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        /// <summary>
        /// Получить имя поля
        /// </summary>
        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }

        public static MemberInfo[] GetMembers(Type t, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy) => t.GetMembers(flags);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Преобразовать поток в массив байт
        /// </summary>
        public static byte[] StreamToBytes(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        /// <summary>
        /// Преобразовать байты в поток
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Преобразует строку в поток памяти
        /// </summary>
        public static MemoryStream StringToStream(string data)
        {
            MemoryStream my_stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(my_stream);
            writer.Write(data);
            writer.Flush();
            my_stream.Position = 0;
            return my_stream;
        }

        /// <summary>
        /// Преобразовать размер файла в человекочитаемы вид
        /// </summary>
        public static string SizeDataAsString(long SizeFile)
        {
            if (SizeFile < 1024)
                return SizeFile.ToString() + " bytes";
            else if (SizeFile < 1024 * 1024)
                return Math.Round((double)SizeFile / 1024, 2).ToString() + " KB";
            else
                return Math.Round((double)SizeFile / 1024 / 1024, 2).ToString() + " MB";
        }

        /// <summary>
        /// Проверка принадлежности расширения файла к тиу "Изображение/Картинка"
        /// return FileExtension == ".jpg" || FileExtension == ".jpeg" || FileExtension == ".png" || FileExtension == ".gif" || FileExtension == ".bmp";
        /// </summary>
        /// <param name="FileExtension">Расширение файла для проверки. Например: .png или my-image.jpg</param>
        public static bool IsImageFile(string FileExtension)
        {
            if (string.IsNullOrEmpty(FileExtension))
                return false;

            FileExtension = FileExtension.ToLower();

            if (!FileExtension.Contains("."))
                return false;

            if (FileExtension.LastIndexOf(".") != FileExtension.IndexOf("."))
                FileExtension = FileExtension.Substring(FileExtension.LastIndexOf("."));

            return
                FileExtension.EndsWith(".jpg") ||
                FileExtension.EndsWith(".jpeg") ||
                FileExtension.EndsWith(".png") ||
                FileExtension.EndsWith(".gif") ||
                FileExtension.EndsWith(".bmp");
        }
    }
}
