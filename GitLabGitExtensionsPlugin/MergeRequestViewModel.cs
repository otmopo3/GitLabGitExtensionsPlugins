using NGitLab.Models;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace GitLabGitExtensionsPlugin
{
	class MergeRequestViewModel
	{
		private readonly MergeRequest _mergeRequest;
		private readonly GitModel _gitModel;
		private readonly GitLabModel _gitLabModel;

		public MergeRequestViewModel(MergeRequest mergeRequest, GitModel gitModel, GitLabModel gitLabModel)
		{
			_mergeRequest = mergeRequest;
			_gitModel = gitModel;
			_gitLabModel = gitLabModel;
			SwitchToBranchCommand = new RelayCommand<object>(OnSwitchToBranchCommand);

			OpenInBrowserCommand = new RelayCommand<object>(OnOpenInBrowserCommand);
		}
			

		public string SourceBranch => _mergeRequest.SourceBranch;

		public string TargetBranch => _mergeRequest.TargetBranch;

		public string Assignee => _mergeRequest.Assignee.Username;

		public string UpdatedAt => _mergeRequest.UpdatedAt.ToShortDateString();

		public string Title => _mergeRequest.Title;

		public int DownVotes => _mergeRequest.Downvotes;

		public int UpVotes => _mergeRequest.Upvotes;

		public ICommand SwitchToBranchCommand { get; }

		public ICommand OpenInBrowserCommand { get; }

		private void OnSwitchToBranchCommand(object state)
		{
			_gitModel.CheckoutBranch(SourceBranch);
		}

		private void OnOpenInBrowserCommand(object obj)
		{
			var url = _gitLabModel.GetMergeRequestUrl(_mergeRequest);

			Process.Start(url);
		}


	}
}
