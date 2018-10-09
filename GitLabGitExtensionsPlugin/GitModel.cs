using GitCommands;
using GitUI;
using System.Linq;
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

			//gitUiCommands.BrowseRepo.

			var pull = _gitModule.FetchCmd(defaultRemote, "", "");



			gitUiCommands.StartCheckoutRemoteBranch(_ownerForm, remoteBranch);
		}
	}

}
