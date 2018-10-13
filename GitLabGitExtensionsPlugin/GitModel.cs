using GitCommands;
using GitUI;
using System.Windows.Forms;

namespace GitLabGitExtensionsPlugin
{
	class GitModel
	{
		private readonly GitModule _gitModule;
		private readonly IWin32Window _ownerForm;
		private GitUICommands _gitUiCommands;

		public GitModel(GitModule gitModule, IWin32Window ownerForm, GitUICommands gitUiCommands)
		{
			_gitModule = gitModule;
			_ownerForm = ownerForm;
			_gitUiCommands = gitUiCommands;
		}

		public void CheckoutBranch(string branch)
		{
			var defaultRemote = _gitModule.GetCurrentRemote();

			var remoteBranch = $"{defaultRemote}/{branch}";

			var fetchCmdArguments = _gitModule.FetchCmd(defaultRemote, "", "");

			var fetchCmdResult = _gitModule.RunGitCmd(fetchCmdArguments);

			_gitUiCommands.StartCheckoutRemoteBranch(_ownerForm, remoteBranch);			
		}
	}
}
