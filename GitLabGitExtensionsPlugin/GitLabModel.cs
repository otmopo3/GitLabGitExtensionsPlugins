using NGitLab;
using NGitLab.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GitLabGitExtensionsPlugin
{
	class GitLabModel
	{
		private readonly GitLabClient _gitLabCLient;
		private readonly Project _project;

		private GitLabModel(GitLabClient gitLabCLient, Project project)
		{
			_gitLabCLient = gitLabCLient;
			_project = project;
		}

		public static GitLabModel Create(string gitLabAddress, string gitLabKey, string projectName)
		{
			GitLabClient gitLabCLient = GitLabClient.Connect(gitLabAddress, gitLabKey);

			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

			var accessibleProjects = gitLabCLient.Projects.Accessible().ToList();

			var project = accessibleProjects.First(p => p.SshUrl.ToLowerInvariant().Contains(projectName.ToLowerInvariant()) ||
			p.HttpUrl.ToLowerInvariant().Contains(projectName.ToLowerInvariant()));

			return new GitLabModel(gitLabCLient, project);
		}

		public List<MergeRequest> GetOpenedMergeRequests()
		{
			var mergeRequestClient = _gitLabCLient.GetMergeRequest(_project.Id);

			var openedMergeRequests = mergeRequestClient.AllInState(MergeRequestState.opened).ToList();

			//var reopenedMergeRequests = mergeRequestClient.AllInState(MergeRequestState.reopened).ToList();

			//openedMergeRequests.AddAll(reopenedMergeRequests);

			return openedMergeRequests;
		}

		public string GetMergeRequestUrl(MergeRequest mergeRequest)
		{
			var projectUrl = _project.WebUrl;

			var mrUrl = $"{projectUrl}/merge_requests/{mergeRequest.Iid}";

			return mrUrl;
		}

		public IEnumerable<PipelineData> GetPipelines()
		{
			var repository = _gitLabCLient.GetRepository(_project.Id);

			var pipelines = repository.Pipelines.All();

			return pipelines;
		}
	}
}
