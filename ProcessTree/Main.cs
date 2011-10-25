using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Proc = System.Tuple<int, int, string>;

namespace ProcessTree {
	
	class ProcessTree {

		public static void Main (string[] args) { new ProcessTree().Run(args); }
		
		readonly IDictionary<int, Proc> pMap = new Dictionary<int, Proc>();
		readonly IDictionary<int, ICollection<int>> tMap = new Dictionary<int, ICollection<int>>();
		readonly TextWriter bOut = new StreamWriter(new BufferedStream(Console.OpenStandardOutput()));
		
		public void Run(string[] args) {
			var header = Console.ReadLine();
			var ws = new[] { '\t', ' ' };
			var cols = header.Split(ws, StringSplitOptions.RemoveEmptyEntries);
			var iPid = Array.IndexOf(cols, "PID");
			var iPpid = Array.IndexOf(cols, "PPID");
			var iCmd = Math.Max(header.IndexOf("CMD"), header.IndexOf("COMMAND"));

			string line = null;
			while ((line = Console.ReadLine()) != null) {
				var words = line.Split(ws, StringSplitOptions.RemoveEmptyEntries);
				var pid = int.Parse(words[iPid]);
				pMap[pid] = new Proc(pid, int.Parse(words[iPpid]), line.Substring(iCmd));
			}
			foreach (var p in pMap.Values) {
				if (! tMap.ContainsKey(p.Item2)) { tMap[p.Item2] = new List<int>(8); }
				tMap[p.Item2].Add(p.Item1);
			}

			foreach (var k in tMap[0]) PrintTree(0, k);
			bOut.Flush();
		}
		
		public void PrintTree(int l, int i) { 
			for (int j = 0; j < l; j ++) bOut.Write(' ');
			bOut.Write(i);
			bOut.Write(':');
			bOut.Write(' ');
			bOut.Write(pMap[i].Item3);
			bOut.Write('\n');
			if (tMap.ContainsKey(i))
				foreach (var k in tMap[i]) PrintTree(l + 1, k);
		}
	}
}