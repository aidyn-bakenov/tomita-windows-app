/*   ..........   */

public void insert_to_db()
{
    // Подключаемся к базе данных на сервере            
    SqlConnection conn = new SqlConnection(connStr);
    try
    {
        conn.Open();
        if (textBox_choose_directory.Text != "")
        {
            directory_finereader_files = textBox_choose_directory.Text;

            if ((checkBox_choose_enabled.Checked == true) && ((comboBox_choose_year.SelectedItem.ToString() != "") && (comboBox_choose_Month.SelectedItem.ToString() != "")))
            {
                year = comboBox_choose_year.SelectedItem.ToString();
                Month = comboBox_choose_Month.SelectedItem.ToString();
            }
            else
            { 
                year = "";
                Month = "";
            }
            Simple_function.directory_exists(directory_tomita_result);
            Simple_function.directory_exists(directory_error_log);

            // Глобальные переменные
            global_year = year;
            global_Month = Month;
            global_directory_finereader_files = directory_finereader_files;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directory_finereader_files);
            System.IO.DirectoryInfo[] dir_1 = dir.GetDirectories();
            System.IO.FileInfo[] files_dir_1 = dir.GetFiles();

            // Файлы из указанной директории directory_finereader_files
            for (var i = 0; i < files_dir_1.Length; i++)
            {
                SqlCommand add_files_name = new SqlCommand("INSERT INTO " + tomitaFiles + "(name,id,bool,directory,date_create) VALUES(@name,@id,@bool,@directory,@date_create)", conn);
                SqlParameter files_name = new SqlParameter();
                Simple_function.first_query_parameter(add_files_name, files_name, "@name", SqlDbType.NVarChar, files_dir_1[i].ToString());

                string file_name = files_dir_1[i].ToString();
                string test = file_name.Replace(" ", "").Replace("вх.", "");

                var lastIndex = test.LastIndexOf("от");
                if (lastIndex != -1)
                {
                    test = test.Remove(lastIndex, test.Length - lastIndex);
                    Simple_function.add_query_parameter(add_files_name, files_name, "@id", SqlDbType.NVarChar, test.ToString());

                    SqlCommand search_in_excel = new SqlCommand("SELECT id FROM tomitaExcel WHERE id='" + test.ToString() + "' AND year='" + year + "'", conn);
                    object search_res = search_in_excel.ExecuteScalar();
                    byte bool_value;
                    if (search_res != null)
                    {
                        bool_value = 1;
                    }
                    else
                    {
                        bool_value = 0;
                    }
                    Simple_function.add_query_parameter(add_files_name, files_name, "@bool", SqlDbType.Bit, bool_value);
                }

                Simple_function.add_query_parameter(add_files_name, files_name, "@directory", SqlDbType.NVarChar, directory_finereader_files);

                string date_create = file_name.Replace(" ", "").Replace("вх.", "").Replace(test.ToString(), "").Replace("от", "").Replace(".txt", "");
                Simple_function.add_query_parameter(add_files_name, files_name, "@date_create", SqlDbType.NVarChar, date_create);

                add_files_name.ExecuteNonQuery();

                // Удаление всех символов после десятого 01.01.1970 (10 символов)
                // Заполнение поля timestamp - время появления записи в БД
                SqlCommand timestamp = new SqlCommand("UPDATE " + tomitaFiles + " SET date_create=substring(date_create,1,10), timestamp=GETDATE() where name='" + files_dir_1[i].ToString() + "'" + "AND directory='" + directory_finereader_files + "'", conn);
                timestamp.ExecuteNonQuery();

                System.Threading.Thread.Sleep(75);

                MessageBox.Show("Файлы успешно загружены");
            }

            // Внутренние папки директории
            for (var i = 0; i < dir_1.Length; i++)
            {
                string directory_finereader_files_1 = directory_finereader_files + dir_1[i].ToString() + "/";
                System.IO.DirectoryInfo dirs_1 = new System.IO.DirectoryInfo(directory_finereader_files_1);
                System.IO.FileInfo[] files_dirs_2 = dirs_1.GetFiles();

                // Файлы внутренних папок
                for (var j = 0; j < files_dirs_2.Length; j++)
                {
                    SqlCommand add_files_name = new SqlCommand("INSERT INTO " + tomitaFiles + "(name,id,bool,directory,date_create) VALUES (@name,@id,@bool,@directory,@date_create)", conn);
                    SqlParameter files_name = new SqlParameter();
                    Simple_function.first_query_parameter(add_files_name, files_name, "@name", SqlDbType.NVarChar, files_dirs_2[j].ToString());

                    string file_name = files_dirs_2[j].ToString();
                    string test = file_name.Replace(" ", "").Replace("вх.", "");

                    var lastIndex = test.LastIndexOf("от");
                    if (lastIndex != -1)
                    {
                        test = test.Remove(lastIndex, test.Length - lastIndex);
                        Simple_function.add_query_parameter(add_files_name, files_name, "@id", SqlDbType.NVarChar, test.ToString());

                        SqlCommand search_in_excel = new SqlCommand("SELECT id FROM tomitaExcel WHERE id='" + test.ToString() + "' AND year='" + year + "'", conn);
                        object search_res = search_in_excel.ExecuteScalar();
                        byte bool_value;
                        if (search_res != null)
                        {
                            bool_value = 1;
                        }
                        else
                        {
                            bool_value = 0;
                        }
                        Simple_function.add_query_parameter(add_files_name, files_name, "@bool", SqlDbType.Bit, bool_value);
                    }

                    Simple_function.add_query_parameter(add_files_name, files_name, "@directory", SqlDbType.NVarChar, directory_finereader_files_1);

                    string date_create = file_name.Replace(" ", "").Replace("вх.", "").Replace(test.ToString(), "").Replace("от", "").Replace(".txt", "");
                    Simple_function.add_query_parameter(add_files_name, files_name, "@date_create", SqlDbType.NVarChar, date_create);

                    add_files_name.ExecuteNonQuery();

                    // Удаление всех символов после десятого 01.01.1970 (10 символов)
                    // Заполнение поля timestamp - время появления записи в БД
                    SqlCommand timestamp = new SqlCommand("UPDATE " + tomitaFiles + " SET date_create=substring(date_create,1,10), timestamp=GETDATE() where name='" + files_dirs_2[j].ToString() + "'" + "AND directory='" + directory_finereader_files_1 + "'", conn);
                    timestamp.ExecuteNonQuery();

                    System.Threading.Thread.Sleep(75);
                }
            }
        }
        else
        {
            MessageBox.Show("Заполните все поля!");
        }
    }
    catch (Exception exc)
    {
        Simple_function.write_error(directory_error_log, exc.ToString() + Environment.NewLine);
    }
}


public void tomita_region(string parser_subjects, string config_file_name, string gzt_file_name, string articles, string facts)
{
    if (textBox_choose_directory.Text != "")
    {
        directory_finereader_files = textBox_choose_directory.Text;

        if ((checkBox_choose_enabled.Checked == true) && ((comboBox_choose_year.SelectedItem.ToString() != "") && (comboBox_choose_Month.SelectedItem.ToString() != "")))
        {
            year = comboBox_choose_year.SelectedItem.ToString();
            Month = comboBox_choose_Month.SelectedItem.ToString();

            string folder_input_txt = directory_tomita_result + parser_subjects + "/" + global_year + "/input_txt/" + global_Month + "/";
            string folder_output_txt = directory_tomita_result + parser_subjects + "/" + global_year + "/output_txt/" + global_Month + "/";
            string folder_debug_html = directory_tomita_result + parser_subjects + "/" + global_year + "/debug_html/" + global_Month + "/";
            Simple_function.directory_exists(folder_input_txt);
            Simple_function.directory_exists(folder_output_txt);
            Simple_function.directory_exists(folder_debug_html);

            // Подключаемся к базе данных на сервере
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            // Выбираем только файлы, ID которых записаны в таблице tomitaExcel
            SqlCommand bool_1 = new SqlCommand("SELECT name,directory FROM " + tomitaFiles + " WHERE bool = '1'" + "AND directory='" + directory_finereader_files + "'", conn);
            using (SqlDataReader bool_table = bool_1.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (bool_table.Read())
                {
                    // Копируем исходный файл, чтобы переименовать его, так как tomita не читает русские символы
                    string files_copy_name = bool_table.GetValue(0).ToString().Trim().Replace(" ", "").Replace("вх.", "").Replace("от", "_");
                    try
                    {
                        File.Copy(bool_table.GetValue(1).ToString().Trim() + @"\" + bool_table.GetValue(0).ToString().Trim(), folder_input_txt + files_copy_name, true);
                    }
                    catch (Exception exc)
                    {
                        Simple_function.write_error(directory_error_log, exc.ToString() + Environment.NewLine);
                    }

                    string files_html_name = files_copy_name.Replace(".txt", "");
                    // Перезаписываем файл region_config.proto, отвечающий за конфигурацию tomita парсера
                    Tomita_function.rewrite_config_proto(parser_directory, parser_subjects, folder_input_txt, folder_output_txt, folder_debug_html, config_file_name, gzt_file_name, files_copy_name, files_html_name, articles, facts);

                    // Запускаем tomitaparser.exe в командной строке от имени Администратора
                    Tomita_function.start_cmd_parser(parser_directory, parser_subjects, config_file_name);
                }
            }

            string directory_html = folder_debug_html;

            System.IO.DirectoryInfo html_dir = new System.IO.DirectoryInfo(directory_html);
            System.IO.DirectoryInfo[] html_dir_1 = html_dir.GetDirectories();
            System.IO.FileInfo[] files_html_dir = html_dir.GetFiles();

            conn.Open();

            // Файлы из указанной директории directory_html
            for (var i = 0; i < files_html_dir.Length; i++)
            {
                try
                {
                    // Запишем html-файл в таблицу primary_database чтобы получить регион
                    string[] html_text = File.ReadAllLines(directory_html + files_html_dir[i]);
                    string html_name = files_html_dir[i].ToString();
                    string original_file_name = "вх. " + html_name.Replace("_", " от ").Replace(".html", ".txt");
                    SqlCommand html_to_db = new SqlCommand(@"UPDATE " + tomitaFiles + " SET html=@html WHERE name='" + original_file_name + "'" + "AND directory='" + directory_finereader_files + "'", conn);
                    SqlParameter html = new SqlParameter();
                    Simple_function.first_query_parameter(html_to_db, html, "@html", SqlDbType.NVarChar, html_text);

                    // Процедура MS SQL, которая парсит содержимое поля html и записывает в first_region и all_region
                    string region_request = "declare @name nvarchar(max) = '" + original_file_name + "'" + " \n"

                    + "declare @xml xml=" + " \n"
                    + "(select top 1 replace(html,'<html xmlns:fo=\"http://www.w3.org/1999/XSL/Format\"><HEAD><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"></HEAD></html>','')" + " \n"
                    + "from " + tomitaFiles + " where name= @name)" + " \n"

                    + "declare @t table (id int identity,reg nvarchar(max))" + " \n"
                    + "insert into @t (reg)" + " \n"
                    + "select" + " \n"
                    + "b.value('(./text())[1]','nvarchar(max)') as name" + " \n"

                    + "FROM   @xml.nodes('/body/table[1]/tbody/tr/td/a') as a(b)" + " \n"

                    + "declare @i int = 1,@maxi int = (select COUNT(*) from @t),@reg nvarchar(max)=''" + " \n"
                    + "while @i <= @maxi" + " \n"
                    + "begin" + " \n"
                    + "set @reg = @reg + (select reg+', ' from @t t where t.id=@i)" + " \n"
                    + "set @i=@i+1" + " \n"
                    + "end" + " \n"
                    + "select @reg" + " \n"

                    + "update t set first_region=(select reg from @t where id=1),all_region=@reg" + " \n"
                    + "from " + tomitaFiles + " t where name= @name";

                    SqlCommand html_region = new SqlCommand(region_request, conn);
                    html_region.ExecuteNonQuery();

                    // Приведение содержимого first_region к общему виду с первой заглавной буквой, а остальные в нижнем регистре
                    SqlCommand lower = new SqlCommand("UPDATE " + tomitaFiles + " SET first_region=UPPER(substring(LOWER(first_region),1,1))+SUBSTRING(LOWER(first_region),2,100) where name='" + original_file_name + "'", conn);
                    lower.ExecuteNonQuery();

                }
                catch (Exception exc)
                {
                    Simple_function.write_error(directory_error_log, exc.ToString() + Environment.NewLine);
                }
            }

            conn.Close();
            conn.Dispose();

            System.Threading.Thread.Sleep(1000);

            MessageBox.Show("Процесс завершен");
        }
        else
        {
            year = "";
            Month = "";
        }
        Simple_function.directory_exists(directory_tomita_result);
        Simple_function.directory_exists(directory_error_log);

        // Глобальные переменные
        global_year = year;
        global_Month = Month;
        global_directory_finereader_files = directory_finereader_files;
    }
    else
    {
        MessageBox.Show("Заполните все поля!");
    }

}

/*   ..........   */
