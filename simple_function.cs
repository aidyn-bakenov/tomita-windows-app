using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace TomitaWindowsApp
{
    static class Simple_function
    {
        public static void directory_exists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void first_query_parameter(SqlCommand SqlCommand, SqlParameter SqlParameter, string ParameterName, SqlDbType SqlDbType, object Value)
        {
            SqlParameter.ParameterName = ParameterName;
            SqlParameter.SqlDbType = SqlDbType;
            SqlParameter.Value = Value;
            SqlCommand.Parameters.Add(SqlParameter);
        }
        public static void add_query_parameter(SqlCommand SqlCommand, SqlParameter SqlParameter, string ParameterName, SqlDbType SqlDbType, object Value)
        {
            SqlParameter = new SqlParameter();
            SqlParameter.ParameterName = ParameterName;
            SqlParameter.SqlDbType = SqlDbType;
            SqlParameter.Value = Value;
            SqlCommand.Parameters.Add(SqlParameter);
        }
        public static void write_error(string directory_error_log, string exc)
        {
            if (!File.Exists(directory_error_log + "/error.txt"))
            {
                // Создаем файл для записи
                string createText = "Error list:" + Environment.NewLine;
                File.WriteAllText(directory_error_log + "/error.txt", createText, Encoding.UTF8);
            }

            // Если файл создан, добавим имя файла
            File.AppendAllText(directory_error_log + "/error.txt", exc + " \n", Encoding.UTF8);
        }
    }
}
