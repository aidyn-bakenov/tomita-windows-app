using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TomitaWindowsApp
{
    static class Tomita_function
    {
        public static void rewrite_config_proto(string parser_directory, string parser_subjects, string folder_input_txt, string folder_output_txt, string folder_debug_html, string config_file_name, string gzt_file_name, string files_copy_name, string files_html_name, string articles, string facts)
        {
            System.IO.StreamWriter config_proto = new System.IO.StreamWriter(parser_directory + "/" + parser_subjects + "/" + config_file_name, false);
            config_proto.WriteLine("/************************************************************");
            config_proto.WriteLine("            Author: Bakenov A.");
            config_proto.WriteLine("            Date: 12.08.2015");
            config_proto.WriteLine("            Company: CURS");
            config_proto.WriteLine("************************************************************/");
            config_proto.WriteLine("");
            config_proto.WriteLine("encoding 'utf8';");
            config_proto.WriteLine("");
            config_proto.WriteLine("TTextMinerConfig {");
            config_proto.WriteLine("");
            config_proto.WriteLine("    Dictionary = '" + parser_directory + "/" + parser_subjects + "/" + gzt_file_name + "'");
            config_proto.WriteLine("");
            config_proto.WriteLine("    Input = {");
            config_proto.WriteLine("        File = '" + folder_input_txt + files_copy_name + "'");
            config_proto.WriteLine("    }");
            config_proto.WriteLine("");
            config_proto.WriteLine("    Output = {");
            config_proto.WriteLine("        File = '" + folder_output_txt + files_copy_name + "'");
            config_proto.WriteLine("        Format = text");
            config_proto.WriteLine("    }");
            config_proto.WriteLine("");
            config_proto.WriteLine("    Articles = [");
            config_proto.WriteLine("        {");
            config_proto.WriteLine("            Name = '" + articles + "'");
            config_proto.WriteLine("        }");
            config_proto.WriteLine("    ]");
            config_proto.WriteLine("");
            config_proto.WriteLine("    Facts = [");
            config_proto.WriteLine("        {");
            config_proto.WriteLine("            Name = '" + facts + "'");
            config_proto.WriteLine("        }");
            config_proto.WriteLine("    ]");
            config_proto.WriteLine("");
            config_proto.WriteLine("    PrettyOutput = '" + folder_debug_html + files_html_name + ".html'");
            config_proto.WriteLine("");
            config_proto.WriteLine("}");
            config_proto.Close();
        }
        public static void start_cmd_parser(string parser_directory, string parser_subjects, string config_file_name)
        {
            string path = parser_directory + "/" + parser_subjects + "/cmd_start.bat";
            System.IO.StreamWriter cmd_start = new System.IO.StreamWriter(path, false);
            cmd_start.WriteLine("cd /d " + "\"" + parser_directory.Replace("/", "\\") + "\\" + parser_subjects.Replace("/", "\\") + "\\" + "\"");
            cmd_start.WriteLine("tomitaparser.exe " + config_file_name);
            cmd_start.Close();

            System.Threading.Thread.Sleep(100);

            if (File.Exists(path))
            {
                Process.Start(path);
            }

            System.Threading.Thread.Sleep(2900);
        }
    }
}
