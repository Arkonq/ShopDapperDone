////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QIWI
{
    /// <summary>
    /// Класс базового/вспомогательного/универсального функционала: чтение и запись данных объекта, сериализация/де-сериализация и т.д.
    /// </summary>
    [Serializable]
    public class basic
    {
        /// <summary>
        /// Путь до папки-типа данных.
        /// Путь хранения текущего типа данных: [Папка исполняемого файла]/[пользовательская папка для хранения данных]/[имя типа-класса]
        /// </summary>
        protected string owner_dir { get { return Path.Combine(Environment.CurrentDirectory, "data_base_fs", GetType().Name); } }

        /// <summary>
        /// Получает полный путь до папки-объекта: [owner_dir]/[идентефикатор текущего объекта]
        /// </summary>
        protected string obj_dir { get { return Path.Combine(owner_dir, ID); } }

        /// <summary>
        /// Идентефикатор текущего объекта
        /// </summary>
        protected string ID = "";

        /// <summary>
        /// Сохранить сериализацию {строка полученная методом GetAsString()} текущего объекта.
        /// Сохраняются только помеченные для сериализации поля
        /// </summary>
        protected void SaveAsIs()
        {
            UpdateData("dump.json", MultiTool.glob_tools.SerialiseJSON(this));
        }

        /// <summary>
        /// Проверка существования объекта-папки
        /// </summary>
        protected bool Exist { get { return Directory.Exists(obj_dir); } }

        /// <summary>
        /// Получить текущий объект в виде байтов из json строки {GetJSON}
        /// </summary>
        protected byte[] GetBytes { get { return Encoding.UTF8.GetBytes(MultiTool.glob_tools.SerialiseJSON(this)); } }
        
        /// <summary>
        /// Извлекает все идентификаторы из папки-типа
        /// </summary>
        /// <returns>Список всех существующих идентефикаторов объектов из папки-типа</returns>
        protected List<string> AllKeys()
        {
            if (!Directory.Exists(owner_dir))
                Directory.CreateDirectory(owner_dir);
            List<string> ret = new List<string>(Directory.GetDirectories(owner_dir));
            for (int i = 0; i < ret.Count; i++)
                ret[i] = Path.GetFileName(ret[i]);
            return ret;
        }

        /// <summary>
        /// Записать значение данных в файл объекта
        /// </summary>
        /// <param name="data_name">имя данных</param>
        /// <param name="data_value">значение данных</param>
        protected void UpdateData(string data_name, string data_value)
        {
            data_value = data_value.Trim();
            string file_patch = Path.Combine(obj_dir, data_name);
            //
            if (!Directory.Exists(Path.GetDirectoryName(file_patch)))
                Directory.CreateDirectory(Path.GetDirectoryName(file_patch));
            //
            if (string.IsNullOrEmpty(data_value) && File.Exists(file_patch))
                File.Delete(file_patch);
            else if (data_value != null)
                File.WriteAllText(file_patch, data_value);
        }

        /// <summary>
        /// Прочитать данные из файла объекта
        /// </summary>
        /// <param name="data_name">имя данных</param>
        /// <returnsзначение данных></returns>
        protected string ReadData(string data_name)
        {
            string file_patch = Path.Combine(obj_dir, data_name);
            string returned_val = "";
            if (File.Exists(file_patch))
                returned_val = File.ReadAllText(file_patch).Trim();
            //
            return returned_val;
        }
    }
}
