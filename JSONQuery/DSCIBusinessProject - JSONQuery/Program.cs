using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using AzureDataManagement.DataProvider;
using System.Configuration;
using System.Reflection;

namespace DSCIBusinessProject___JSONQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            var runDebug = bool.Parse(ConfigurationManager.AppSettings["DebugRun"]);
            var runProgram = true;

            while(runProgram)
            {
                Console.WriteLine("Type exit at any propmt to exit the program");
                Console.WriteLine("Please enter the full path of the file you want to query:");
                var filePath = string.Empty;
                var errors = new List<KeyValuePair<int, string>>();


                if (runDebug)
                    filePath = @"C:\Users\mbeesley\Documents\Class Data\user_events.csv";
                else
                    filePath = Console.ReadLine();

                runProgram = CheckForExit(filePath);
                if (File.Exists(filePath) && runProgram)
                {
                    Console.WriteLine("Please enter the character that deliminates the file:");
                    var delim = string.Empty;

                    if (runDebug)
                        delim = ",";
                    else
                        delim = Console.ReadLine();
                    char delimChar;

                    runProgram = CheckForExit(delim);
                    if (char.TryParse(delim, out delimChar) && runProgram)
                    {
                        var provider = new CSVProvider(filePath,delimChar);
                        var data = provider.GetData();

                        Console.WriteLine("Please enter the column position that contains the json data. column options: {0}", string.Join(" | ", data.Columns.Values.Select(c => c.ToString())));

                        string jsonColumn = string.Empty;

                        if (runDebug)
                            jsonColumn = "4";
                        else
                            jsonColumn = Console.ReadLine();
                        int columnPosition;

                        runProgram = CheckForExit(jsonColumn);
                        if (int.TryParse(jsonColumn,out columnPosition) && runProgram)
                        {
                            if (columnPosition <= data.Columns.Count)
                            {

                                #region Old Parse Code
                                //var column = data.Columns[columnPosition - 1];
                                //Console.WriteLine("Column Chosen: {0}", column);

                                //var jsonData = new List<DSCIJsonData>();
                                //foreach(var row in data.Rows.Values)
                                //{
                                //    var item = row.RowData.Values.First(rv => rv.ColumnName == column.Name);    
                                //    var itemValue = item.ColumnValue.Trim('\"').Replace("\"\"","\"").Replace("\\\"", "\"");
                                //    try
                                //    {
                                //        var jsonObj = JsonConvert.DeserializeObject<DSCIJsonData>(itemValue);
                                //        jsonObj.EventId = row.RowNumber.ToString();
                                //        jsonObj.Event = row.RowData["event"].ColumnValue;
                                //        jsonObj.Occurrence = row.RowData["occurrence"].ColumnValue;
                                //        jsonData.Add(jsonObj);
                                //    }
                                //    catch(Exception e)
                                //    {
                                //        errors.Add(new KeyValuePair<int, string>(row.RowNumber, e.Message));
                                //    }
                                //}


                                //Console.WriteLine("Please enter the item that you would like to analyze. Options: {0}", string.Join(" | ", RunMetrics.Keys));
                                //var itemToAnalyze = string.Empty;
                                //if (runDebug)
                                //    itemToAnalyze = "search_term";
                                //else
                                //    itemToAnalyze = Console.ReadLine();

                                //runProgram = CheckForExit(itemToAnalyze);
                                //if (RunMetrics.ContainsKey(itemToAnalyze) && runProgram)
                                //{
                                //    Console.WriteLine("Analysis Results: ");
                                //    RunMetrics[itemToAnalyze](jsonData);

                                //    Console.ReadLine();

                                //    Console.WriteLine("Create CSV? Yes or No");
                                //    var createCSV = Console.ReadLine();

                                //    if(createCSV.Equals("yes", StringComparison.OrdinalIgnoreCase))
                                //    {
                                //        var sb = new StringBuilder();
                                //        sb.AppendLine("JSONColumn,ItemName,Count");
                                //        foreach(var analysis in RunAnalysis)
                                //        {
                                //            foreach(var item in analysis.Value(jsonData))
                                //            {
                                //                sb.AppendLine(string.Format("\"{0}\",\"{1}\",{2}", analysis.Key, item.Key, item.Value));
                                //            }
                                //        }
                                //        File.WriteAllText("C:\\JSONcsvData.csv", sb.ToString());
                                //    }


                                //    Console.WriteLine("Create CSV of Raw Data? Yes or No");
                                //    var createRawCSV = Console.ReadLine();

                                //    if (createRawCSV.Equals("yes", StringComparison.OrdinalIgnoreCase))
                                //    {
                                //        var sb = new StringBuilder();
                                //        var headerRow = new StringBuilder();
                                //        var propList = typeof(DSCIJsonData).GetProperties().OrderBy(p=>p.Name).ToList();
                                //        sb.AppendLine(string.Join(",",propList.OrderBy(p=>p.Name).Select(p => p.Name).ToList()));
                                //        foreach (var item in jsonData)
                                //        {
                                //            var builder = new StringBuilder();
                                //            var first = true;
                                //            foreach(var prop in propList.OrderBy(p=>p.Name))
                                //            {
                                //                var itemValue = prop.GetValue(item);
                                //                if(first)
                                //                {
                                //                    builder.Append(string.Format("\"{0}\"", itemValue));
                                //                    first = false;
                                //                }
                                //                else
                                //                {
                                //                    builder.Append(string.Format(",\"{0}\"", itemValue));
                                //                }
                                //            }
                                //            sb.AppendLine(builder.ToString());
                                //        }
                                //        File.WriteAllText("C:\\Users\\mbeesley\\documents\\RawJSONcsvData.csv", sb.ToString());
                                //    }

                                //    Console.WriteLine("Analysis complete type exit to exit or hit enter to run another analysis");
                                //    var finalInput = Console.ReadLine();
                                //    runProgram = CheckForExit(finalInput);
                                //}


                                #endregion // Old Parse Code

                                var column = data.Columns[columnPosition - 1];
                                Console.WriteLine("Column Chosen: {0}", column);

                                var jsonData = new List<DSCIJsonData>();
                                foreach (var row in data.Rows.Values)
                                {
                                    var item = row.RowData.Values.First(rv => rv.ColumnName == column.Name);
                                    var itemValue = item.ColumnValue.Trim('\"').Replace("\"\"", "\"").Replace("\\\"", "\"");
                                    try
                                    {
                                        var jsonObj = new DSCIJsonData();
                                        jsonObj.Json = JsonConvert.DeserializeObject<Dictionary<string, string>>(itemValue);
                                        jsonObj.EventId = row.RowNumber.ToString();
                                        jsonObj.Event = row.RowData["event"].ColumnValue;
                                        jsonObj.Occurrence = row.RowData["occurrence"].ColumnValue;
                                        jsonData.Add(jsonObj);
                                    }
                                    catch (Exception e)
                                    {
                                        errors.Add(new KeyValuePair<int, string>(row.RowNumber, e.Message));
                                    }
                                }

                                Console.WriteLine("Create CSV of Raw Data? Yes or No");
                                var createRawCSV = Console.ReadLine();

                                if (createRawCSV.Equals("yes", StringComparison.OrdinalIgnoreCase))
                                {
                                    var sb = new StringBuilder();
                                    sb.AppendLine("EventId,Event,Occurrence,Key,Value");
                                    foreach (var item in jsonData)
                                    {
                                        foreach (var obj in item.Json)
                                        {
                                            sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\""
                                                , item.EventId
                                                , item.Event
                                                , item.Occurrence
                                                , obj.Key
                                                , obj.Value));
                                        }
                                    }
                                    File.WriteAllText("C:\\Users\\mbeesley\\documents\\RawJSONcsvData.csv", sb.ToString());
                                }

                                Console.WriteLine("Analysis complete type exit to exit or hit enter to run another analysis");
                                var finalInput = Console.ReadLine();
                                runProgram = CheckForExit(finalInput);
                            }
                        }
                    }
                }
            }
        }

        static bool CheckForExit(string userInput)
        {
            if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                return false;
            return true;
        }

        static Dictionary<string, Action<List<DSCIJsonData>>> RunMetrics = new Dictionary<string, Action<List<DSCIJsonData>>>()
        {
            {
                "from", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.from)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "authenticated_with", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.authenticated_with)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "search_term", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.search_term)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "current_result_filter", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.current_result_filter)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "organization_id", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.organization_id)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "organization_name", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.organization_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "organization", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.organization)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "origin", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.origin)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "created_by_name", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.created_by_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "post_name", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.post_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            },
            {
                "owned_by_name", new Action<List<DSCIJsonData>>(data =>
                {
                    data.GroupBy(j => j.owned_by_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .ToList()
                                        .ForEach(i => Console.WriteLine("Item: {0} | Count: {1}", i.item, i.count));
                })
            }
        };

        /// <summary>
        /// Dictionary of items that enable analysis of json data
        /// </summary>
        static Dictionary<string, Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>> RunAnalysis = new Dictionary<string, Func<List<DSCIJsonData>, List<KeyValuePair<String,int>>>>()
        {
            {
                "from", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.from)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "authenticated_with", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.authenticated_with)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "search_term", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.search_term)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "current_result_filter", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.current_result_filter)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "organization_id", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.organization_id)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "organization_name", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.organization_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "organization", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.organization)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "origin", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.origin)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "created_by_name", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.created_by_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "post_name", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.post_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            },
            {
                "owned_by_name", new Func<List<DSCIJsonData>, List<KeyValuePair<String, int>>>(data =>
                {
                    return data.GroupBy(j => j.owned_by_name)
                                        .Select(g => new { item = g.Key, count = g.Count() })
                                        .OrderByDescending(x => x.count)
                                        .Take(10)
                                        .Select(x => new KeyValuePair<string,int>(x.item,x.count))
                                        .ToList();
                })
            }
        };
    }
}
