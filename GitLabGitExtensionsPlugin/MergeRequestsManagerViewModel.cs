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

			var mergeRequests = _gitLabModel.GetOpenedMergeRequests().Select(mr => new MergeRequestViewModel(mr, gitModel, gitLabModel));

			OpenedMergeRequests.AddAll(mergeRequests);

			UpdatePipeLines();
		}

		public ObservableCollection<MergeRequestViewModel> OpenedMergeRequests { get; } = new ObservableCollection<MergeRequestViewModel>();

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
