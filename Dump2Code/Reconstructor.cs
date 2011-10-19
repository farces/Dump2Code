using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Dump2Code
{
    class Reconstructor
    {
        private static List<String> _output = new List<String>();
        private static List<String> _lines = new List<string>(); 
        public static List<string> ParseInput(string input)
        {
            _lines.Clear();
            _output.Clear();

            //generate list of lines instead of working on it as an array
            string[] temp = input.Split('\n');
            for (int x=1;x<temp.Length;x++)
            {
                _lines.Add(temp[x].Trim());
            }

            //and now for the shitty part
            Parse(_lines);
            return _output;

        }

        private static void Parse(List<string> lines)
        {
            int num_unknown = 0;
            int current_line = 0;
            int indent_level = 0;
            string tabs = new String(' ', indent_level*3);

            foreach (string line in lines)
            {
                current_line++;
                //handle the regular cases
                if (line == "{")
                {
                    _output.Add(String.Format("{0}{{", tabs));
                    indent_level++;
                    tabs = new String(' ', indent_level * 3);
                    continue;
                }
                if (line == "}")
                {
                    indent_level--;
                    tabs = new String(' ', indent_level*3);
                    if (indent_level == 0) _output.Add(String.Format("{0}}};", tabs));
                    else _output.Add(String.Format("{0}}},", tabs)); 
                    continue;
                }
                //

                string[] currentLine = line.Split(':');

                if (currentLine.Length == 2 && currentLine[1] == "")
                {
                    if (current_line == 1)
                    {
                        _output.Add(String.Format("{0}new {1}()", tabs, currentLine[0]));
                        continue;
                    }

                    string field_name = String.Format("Unknown_{0}",num_unknown);
                    num_unknown++;

                    if (currentLine[0] == "WorldPlace") field_name = "Place";
                    _output.Add(String.Format("{0}{1} = new {2}() //FIXME: may be dummy field name", tabs, field_name, currentLine[0]));
                    continue;
                }
                if (currentLine.Length == 2)
                {
                    string[] value_split = currentLine[1].Trim().Split(' ');
                    string[] field_split = currentLine[0].Trim().Split('.');
                    currentLine[0] = field_split[0];

                    //some basic replaces for easier pasting i guess
                    if (currentLine[0].ToLower() == "actorid") value_split[0] = "this.DynamicID";
                    if (currentLine[0].ToLower() == "actorsno") value_split[0] = "this.ActorSNO";
                    if (currentLine[0].ToLower() == "worldid") value_split[0] = "this.World.DynamicID";
                    if (currentLine[0].ToLower() == "x" || currentLine[0].ToLower() == "y" || currentLine[0].ToLower() == "z")
                        value_split[0] = String.Format("{0}f", value_split[0]);

                    _output.Add(String.Format("{0}{1} = {2},", tabs, currentLine[0], value_split[0]));
                    continue;
                }

            }


        }

    }
}
