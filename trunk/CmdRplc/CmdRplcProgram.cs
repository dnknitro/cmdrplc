using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CmdRplc
{
	internal class CmdRplcProgram
	{
		static void Main(string[] args)
		{
			var mainArgs = new List<string>();
			var isTest = false;
			var prefix = string.Empty;
			var regexOption = RegexOptions.Compiled;
			foreach(var arg in args)
			{
				if(arg.ToUpper() == "/M") regexOption = regexOption | RegexOptions.Multiline;
				else if(arg.ToUpper() == "/I") regexOption = regexOption | RegexOptions.IgnoreCase;
				else if(arg.ToUpper() == "/E") regexOption = regexOption | RegexOptions.ECMAScript;
				else if(arg.ToUpper() == "/S") regexOption = regexOption | RegexOptions.Singleline;
				else if(arg.ToUpper() == "/TEST") isTest = true;
				else if(arg.ToUpper().StartsWith("/PREFIX=")) prefix = arg.Replace("/prefix=", string.Empty).Replace("/PREFIX=", string.Empty);
				else mainArgs.Add(arg);
			}

			if(mainArgs.Count < 3)
			{
				Console.WriteLine("Author: Volodymyr Shcherbyna");
				Console.WriteLine("");
				Console.WriteLine("Usage CmdRplc.exe filename/mask search replace /M /I /S");
				Console.WriteLine("\t/m RegexOptions.Multiline");
				Console.WriteLine("\t/i RegexOptions.IgnoreCase");
				Console.WriteLine("\t/s RegexOptions.Singleline");
				Console.WriteLine("\t/e RegexOptions.ECMAScript");
				Console.WriteLine("\t/test test mode, no actual replacement");
				Console.WriteLine("\t/prefix=<some string prefix for the output file>");
				return;
			}


			Console.WriteLine("Loading files from " + mainArgs[0]);
			var searchParts = GetPathAndFilename(mainArgs[0]);
			var files = Directory.GetFiles(searchParts[0], searchParts[1]);
			var regex = new Regex(mainArgs[1], regexOption);
			var tempPath = Path.Combine( System.IO.Path.GetTempPath(), "CmdRplc");
			if(string.IsNullOrEmpty(prefix) && !Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
			foreach(var file in files)
			{
				Console.WriteLine("Processing " + file);
				if(!File.Exists(file)) continue;
				var inputText = File.ReadAllText(file);
				if(isTest)
				{
					var matches = regex.Matches(inputText);
					Console.WriteLine(file + " matches=" + matches.Count);
				}
				else
				{
					var outputFilename = file;
					if(string.IsNullOrEmpty(prefix))
					{
						//backup original file, since we don't have prefix
						var bakPath = Path.Combine(tempPath, Path.GetFileName(outputFilename)) + ".CmdRplc." + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
						File.Copy(outputFilename, bakPath, true);
					}
					else
					{
						var outputParts = GetPathAndFilename(outputFilename);
						outputFilename = outputParts[0] + prefix + outputParts[1];
					}
					File.WriteAllText(outputFilename, regex.Replace(inputText, mainArgs[2]));
				}
			}
		}

		static string[] GetPathAndFilename(string inputPath)
		{
			var result = new string[2];
			inputPath = inputPath.Replace('/', '\\');
			var index = inputPath.LastIndexOf('\\');
			result[0] = index == -1 ? string.Empty : inputPath.Substring(0, index) + "\\";
			result[1] = index == -1 ? inputPath : inputPath.Substring(index + 1);
			return result;
		}
	}
}