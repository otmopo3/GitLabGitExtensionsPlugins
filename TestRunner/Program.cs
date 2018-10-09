using GitCommands;
using GitUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunner
{
	class Program
	{
		static void Main(string[] args)
		{
			var gitModule = new GitModule(@"Q:\mc\Cameras");

			var gitUiCommands = new GitUICommands(gitModule);

			var defaultRemote = gitModule.GetCurrentRemote();

			var cmd = gitModule.FetchCmd(defaultRemote, "", "");




		}
	}
}
