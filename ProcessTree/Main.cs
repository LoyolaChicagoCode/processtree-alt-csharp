using System;
using System.IO;
using System.Collections.Generic;

namespace ProcessTree {
	
	class ProcessTree {

		public static void Main (string[] args) { new ProcessTree().Run(args); }
		
		readonly IDictionary<int, String> cMap = new Dictionary<int, String>();
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
				var words = line.Substring(0, iCmd).Split(ws, StringSplitOptions.RemoveEmptyEntries);
				var pid = int.Parse(words[iPid]);
				var ppid = int.Parse(words[iPpid]);
				cMap[pid] = line.Substring(iCmd);
				if (! tMap.ContainsKey(ppid)) { tMap[ppid] = new List<int>(8); }
				tMap[ppid].Add(pid);
			}

			foreach (var k in tMap[0]) PrintTree(0, k);
			bOut.Flush();
		}
		
		public void PrintTree(int l, int i) { 
			for (int j = 0; j < l; j ++) bOut.Write(' ');
			bOut.Write(i);
			bOut.Write(':');
			bOut.Write(' ');
			bOut.Write(cMap[i]);
			bOut.Write('\n');
			if (tMap.ContainsKey(i))
				foreach (var k in tMap[i]) PrintTree(l + 1, k);
		}
	}
}