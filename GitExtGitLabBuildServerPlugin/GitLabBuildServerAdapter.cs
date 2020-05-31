using GitExtGitLabBuildServerPlugin.Settings;
using GitLabGitExtensionsPlugin;
using GitUI;
using GitUIPluginInterfaces;
using GitUIPluginInterfaces.BuildServerIntegration;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace GitExtGitLabBuildServerPlugin
{
	[Export(typeof(IBuildServerAdapter))]
	[GitLabIntegrationMetadata(PluginName)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	internal class GitLabBuildServerAdapter : IBuildServerAdapter
	{
		public const string PluginName = "GitLab";
		private IBuildServerWatcher _buildServerWatcher;
		private string _gitLabAddress;
		private string _gitLabKey;
		private string _projectName;

		string IBuildServerAdapter.UniqueKey => _gitLabAddress;

		void IBuildServerAdapter.Initialize(IBuildServerWatcher buildServerWatcher, ISettingsSource config, Func<ObjectId, bool> isCommitInRevisionGrid)
		{
			if (_buildServerWatcher != null)
			{
				throw new InvalidOperationException("Already initialized");
			}

			_buildServerWatcher = buildServerWatcher;

			_gitLabAddress = config.GetString(GitlabSettingsConstants.GitlabAddress, null);
			_gitLabKey = config.GetString(GitlabSettingsConstants.GitlabKey, null);
			_projectName = config.GetString(GitlabSettingsConstants.DefaultProjectId, null);
		}

		IObservable<BuildInfo> IBuildServerAdapter.GetFinishedBuildsSince(IScheduler scheduler, DateTime? sinceDate)
		{
			// GetBuilds() will return the same builds as for GetRunningBuilds().
			// Multiple calls will fetch same info multiple times and make debugging very confusing
			// Similar as for AppVeyor
			return Observable.Empty<BuildInfo>();
		}

		IObservable<BuildInfo> IBuildServerAdapter.GetRunningBuilds(IScheduler scheduler)
		{
			return GetBuilds(scheduler, sinceDate: null, running: true);
		}

		public void Dispose()
		{

		}

		private IObservable<BuildInfo> GetBuilds(IScheduler scheduler, DateTime? sinceDate = null, bool? running = null)
		{
			return Observable.Create<BuildInfo>((observer, cancellationToken) =>
				ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
				{
					await TaskScheduler.Default;
					return scheduler.Schedule(() => ObserveBuilds(sinceDate, running, observer, cancellationToken));
				}).Task);
		}

		private void ObserveBuilds(DateTime? sinceDate, bool? running, IObserver<BuildInfo> observer, CancellationToken cancellationToken)
		{
			try
			{
				var gitLab = GitLabModel.Create(_gitLabAddress, _gitLabKey, _projectName, string.Empty);

				var pipelines = gitLab.GetPipelines();

				foreach (var pipeline in pipelines)
				{
					if (cancellationToken.IsCancellationRequested)
						break;

					string commitSha = pipeline.Sha1.ToString().ToLowerInvariant();
					ObjectId objectId;

					if (!ObjectId.TryParse(commitSha, out objectId))
						continue;

					BuildInfo.BuildStatus status = ConvertBuildStatus(pipeline);

					BuildInfo buildInfo = new BuildInfo
					{
						Status = status,
						Description = pipeline.Ref,
						Duration = 100, //todo: get duration?
						Id = pipeline.Id.ToString(),
						StartDate = DateTime.UtcNow.AddMinutes(-1),  //todo: get startdate?
						Url = gitLab.GetPipelinetUrl(pipeline),
						ShowInBuildReportTab = true,
						CommitHashList = new List<ObjectId> { objectId }
					};

					observer.OnNext(buildInfo);
				}
			}
			catch (Exception ex)
			{
				observer.OnError(ex);
			}
			finally
			{
				observer.OnCompleted();
			}
		}

		private static BuildInfo.BuildStatus ConvertBuildStatus(NGitLab.Models.PipelineData pipeline)
		{
			BuildInfo.BuildStatus status;

			switch (pipeline.Status)
			{
				case NGitLab.Models.PipelineStatus.success:
					status = BuildInfo.BuildStatus.Success;
					break;
				case NGitLab.Models.PipelineStatus.aborted:
				case NGitLab.Models.PipelineStatus.canceled:
					status = BuildInfo.BuildStatus.Stopped;
					break;
				case NGitLab.Models.PipelineStatus.running:
				case NGitLab.Models.PipelineStatus.pending:
				case NGitLab.Models.PipelineStatus.created:
					status = BuildInfo.BuildStatus.InProgress;
					break;
				case NGitLab.Models.PipelineStatus.failed:
					status = BuildInfo.BuildStatus.Failure;
					break;
				case NGitLab.Models.PipelineStatus.undefined:
				default:
					status = BuildInfo.BuildStatus.Unknown;
					break;
			}

			return status;
		}
	}
}
