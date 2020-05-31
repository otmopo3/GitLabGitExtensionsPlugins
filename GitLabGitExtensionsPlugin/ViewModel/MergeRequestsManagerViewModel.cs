using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

			UpdateCommand = new RelayCommand<object>(OnUpdateCommand);

			UpdateMergeRequests();

			UpdatePipeLines();
		}

		public ObservableCollection<MergeRequestViewModel> OpenedMergeRequests { get; } = new ObservableCollection<MergeRequestViewModel>();

		public ICommand UpdateCommand { get; }

		private void OnUpdateCommand(object obj)
		{
			UpdateMergeRequests();

			UpdatePipeLines();
		}

		private void UpdateMergeRequests()
		{
			OpenedMergeRequests.Clear();

			var openedMergeRequests = _gitLabModel.GetOpenedMergeRequests();

			var currentBranch = _gitModel.GetCurrentBramch();

			var favoriteGoupUsers = _gitLabModel.GetUsersFromFavoriteGroup();

			foreach (var mergeRequest in openedMergeRequests)
			{
				var mergeRequestVm = new MergeRequestViewModel(mergeRequest, _gitModel, _gitLabModel)
				{
					IsBranchCheckedOut = mergeRequest.SourceBranch == currentBranch,
					IsMyGroup = favoriteGoupUsers.Contains(mergeRequest.Assignee?.Name) ||
								favoriteGoupUsers.Contains(mergeRequest.Author?.Name)
				};

				OpenedMergeRequests.Add(mergeRequestVm);
			}
		}

		private void UpdatePipeLines()
		{
			Task.Factory.StartNew(() =>
			{
				var mrList = OpenedMergeRequests.ToList();

				var pipelines = _gitLabModel.GetPipelines();

				foreach (var pipeline in pipelines)
				{
					var mergeRequest = mrList.FirstOrDefault(mr => mr.SourceBranch == pipeline.Ref);

					if (mergeRequest == null)
						continue;

					mergeRequest.PipelineStatus = (MergeRequestPipelineStatus)(int)pipeline.Status;

					mrList.Remove(mergeRequest);

					if (!mrList.Any())
						break;
				}

			}, TaskCreationOptions.LongRunning);
		}		
	}
}
