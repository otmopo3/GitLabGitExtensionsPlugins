using NGitLab.Models;
using System.Windows.Input;

namespace GitLabGitExtensionsPlugin
{
	class MergeRequestViewModel
	{
		private readonly MergeRequest _mergeRequest;
		private readonly GitModel _gitModel;

		public MergeRequestViewModel(MergeRequest mergeRequest, GitModel gitModel)
		{
			_mergeRequest = mergeRequest;
			_gitModel = gitModel;
			SwitchToBranchCommand = new RelayCommand<object>(OnSwitchToBranchCommand);
		}

		public string SourceBranch => _mergeRequest.SourceBranch;

		public string TargetBranch => _mergeRequest.TargetBranch;

		public string Assignee => _mergeRequest.Assignee.Username;

		public string UpdatedAt => _mergeRequest.UpdatedAt.ToShortDateString();

		public string Title => _mergeRequest.Title;

		public ICommand SwitchToBranchCommand { get; }

		private void OnSwitchToBranchCommand(object state)
		{
			_gitModel.CheckoutBranch(SourceBranch);
		}


	}
}
