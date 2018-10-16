using NGitLab.Models;
using System;
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
		private bool _isBranchCheckedOut;

		public MergeRequestViewModel(MergeRequest mergeRequest, GitModel gitModel, GitLabModel gitLabModel)
		{
			_mergeRequest = mergeRequest;
			_gitModel = gitModel;
			_gitLabModel = gitLabModel;

			SwitchToBranchCommand = new RelayCommand<object>(OnSwitchToBranchCommand);

			OpenInBrowserCommand = new RelayCommand<object>(OnOpenInBrowserCommand);

			AcceptMergeRequestCommand = new RelayCommand<object>(OnAcceptMergeRequestCommand, _ => UpVotes >= 1 && DownVotes < 1);
		}

		public string SourceBranch => _mergeRequest.SourceBranch;

		public string TargetBranch => _mergeRequest.TargetBranch;

		public string User => _mergeRequest.Assignee?.Username ?? _mergeRequest.Author.Username;

		public string UpdatedAt => _mergeRequest.UpdatedAt.ToShortDateString();

		public string Title => _mergeRequest.Title;

		public int DownVotes => _mergeRequest.Downvotes;

		public int UpVotes => _mergeRequest.Upvotes;

		public string Description => _mergeRequest.Description;

		public string Labels => String.Join(";", _mergeRequest.Labels);

		public bool IsBranchCheckedOut
		{
			get { return _isBranchCheckedOut; }

			internal set
			{
				if (value == _isBranchCheckedOut)
					return;

				_isBranchCheckedOut = value;
				OnPropertyChanged();
			}
		}

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

		public ICommand AcceptMergeRequestCommand { get; }

		private void OnSwitchToBranchCommand(object state)
		{
			_gitModel.CheckoutBranch(SourceBranch);
		}

		private void OnOpenInBrowserCommand(object obj)
		{
			var url = _gitLabModel.GetMergeRequestUrl(_mergeRequest);

			Process.Start(url);
		}

		private void OnAcceptMergeRequestCommand(object obj)
		{
			try
			{
				_gitLabModel.AcceptMergeRequest(_mergeRequest);
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show(ex.Message);
			}			
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
