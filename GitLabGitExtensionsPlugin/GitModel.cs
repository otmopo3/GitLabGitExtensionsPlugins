using GitCommands;
using GitUI;
using System.Windows.Forms;

namespace GitLabGitExtensionsPlugin
{
	class GitModel
	{
		private readonly GitModule _gitModule;
		private readonly IWin32Window _ownerForm;

		public GitModel(GitModule gitModule, IWin32Window ownerForm)
		{
			_gitModule = gitModule;
			_ownerForm = ownerForm;
		}

		public void CheckoutBranch(string branch)
		{
			var gitUiCommands = new GitUICommands(_gitModule);

			var defaultRemote = _gitModule.GetCurrentRemote();

			var remoteBranch = $"{defaultRemote}/{branch}";

			var fetchCmdArguments = _gitModule.FetchCmd(defaultRemote, "", "");

			var fetchCmdResult = _gitModule.RunGitCmd(fetchCmdArguments);

			gitUiCommands.StartCheckoutRemoteBranch(_ownerForm, remoteBranch);
		}
	}
}
