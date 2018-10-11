using GitCommands;
using GitUIPluginInterfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GitLabGitExtensionsPlugin
{
	class MergeRequestsManagerViewModel
	{
		private readonly GitLabModel _gitLabModel;
		private readonly GitModel _gitModel;

		public MergeRequestsManagerViewModel(GitLabModel gitLabModel, GitModel gitModel)
		{
			_gitLabModel = gitLabModel;
			_gitModel = gitModel;

			var mergeRequests = _gitLabModel.GetOpenedMergeRequests().Select(mr => new MergeRequestViewModel(mr, gitModel, gitLabModel));

			OpenedMergeRequests.AddAll(mergeRequests);			
		}

		public ObservableCollection<MergeRequestViewModel> OpenedMergeRequests { get; } = new ObservableCollection<MergeRequestViewModel>();		
	}
}
