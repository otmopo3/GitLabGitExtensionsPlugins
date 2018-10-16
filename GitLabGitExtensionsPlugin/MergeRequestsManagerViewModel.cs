using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

			FillMergeRequests();

			UpdatePipeLines();
		}

		public ObservableCollection<MergeRequestViewModel> OpenedMergeRequests { get; } = new ObservableCollection<MergeRequestViewModel>();

		private void FillMergeRequests()
		{
			var openedMergeRequests = _gitLabModel.GetOpenedMergeRequests();

			var currentBranch = _gitModel.GetCurrentBramch();

			foreach (var mergeRequest in openedMergeRequests)
			{
				var mergeRequestVm = new MergeRequestViewModel(mergeRequest, _gitModel, _gitLabModel)
				{
					IsBranchCheckedOut = mergeRequest.SourceBranch == currentBranch
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
