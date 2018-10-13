﻿using NGitLab.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GitLabGitExtensionsPlugin
{
	class MergeRequestViewModel : INotifyPropertyChanged
	{
		private readonly MergeRequest _mergeRequest;
		private readonly GitModel _gitModel;
		private readonly GitLabModel _gitLabModel;

		private MergeRequestPipelineStatus _pipelineStatus;

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

		public string User => _mergeRequest.Assignee?.Username ?? _mergeRequest.Author.Username;

		public string UpdatedAt => _mergeRequest.UpdatedAt.ToShortDateString();

		public string Title => _mergeRequest.Title;

		public int DownVotes => _mergeRequest.Downvotes;

		public int UpVotes => _mergeRequest.Upvotes;

		public MergeRequestPipelineStatus PipelineStatus
		{
			get { return _pipelineStatus; }

			internal set
			{
				if (value == _pipelineStatus)
					return;

				_pipelineStatus = value;
				OnPropertyChanged();
			}
		}

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

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public enum MergeRequestPipelineStatus
	{
		Undefined,
		Pending,
		Success,
		Created,
		Failed,
		Aborted,
		Running,
		Canceled,
	}
}