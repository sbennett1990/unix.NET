/*
 * Copyright (c) 2015 Scott Bennett
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * 
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.IO;

namespace tail
{
	/// <summary>
	/// tail - A reimplimentation of the Unix utility in C#
	/// 
	/// This program takes a file as its argument and prints the last n lines 
	/// to stdout.
	/// 
	/// Note: This program is not POSIX compliant.
	/// </summary>
	public class tail
	{
		const string usage = "usage: tail [-n number] -p filepath";

		/// <summary>
		/// Read the file given by filePath. Return an array where each entry is 
		/// one line in the file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string[] readFile(string filePath) {
			string[] fileLines = new string[0];

			if (!File.Exists(filePath)) {
				Console.WriteLine(string.Format("error: cannot find {0}", filePath));
				return fileLines;
			}

			try {
				fileLines = File.ReadAllLines(filePath);
			}
			catch (Exception e) {
				Console.WriteLine("error: " + e.Message);
			}

			return fileLines;
		}

		/// <summary>
		/// Print the last n lines of a file, which are stored in the given 
		/// array. 
		/// </summary>
		/// <param name="fileLines"></param>
		/// <param name="numLines"></param>
		public static void print(string[] fileLines, uint numLines) {
			if (fileLines == null || fileLines.Length < 1) {
				return;
			}

			try {
				uint startPosition = 0;

				if (fileLines.Length > numLines) {
					startPosition = ((uint) fileLines.Length - numLines);
				}

				for (uint i = startPosition; i < fileLines.Length; i++) {
					Console.WriteLine(fileLines[i]);
				}
			}
			catch (Exception e) {
				Console.WriteLine("error: " + e.Message);
			}
		}

		static void Main(string[] args) {
			// Put a newline before doing anything. Looks better.
			Console.WriteLine();

			bool p_flag = false;

			/* Path to file */
			string filePath = string.Empty;

			/* Default number of lines to print */
			uint numLines = 10;

			try {
				for (int i = 0; i < args.Length; i++) {
					switch (args[i]) {
						case "-n":
							numLines = uint.Parse(args[i + 1]);
							break;
						case "-p":
							filePath = args[i + 1];
							p_flag = true;
							break;
						default:
							break;
					}
				}

				if (!p_flag) {
					Console.WriteLine("error: no file given");
					Console.WriteLine(usage);
					goto Exit;
				}

				print(readFile(filePath), numLines);
			}
			catch (IndexOutOfRangeException e) {
				Console.WriteLine("error: invalid number of arguments");
				Console.WriteLine(usage);
			}
			catch (OverflowException e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(usage);
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(usage);
			}

		Exit:
			// Put a newline after everything is done. Looks better.
			Console.WriteLine();
		}
	}
}
