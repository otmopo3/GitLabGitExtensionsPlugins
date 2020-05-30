using GitCommands;
using GitLabGitExtensionsPlugin.Properties;
using GitUI;
using GitUIPluginInterfaces;
using ResourceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace GitLabGitExtensionsPlugin
{
	[Export(typeof(IGitPlugin))]
	public class GitLabPlugin : GitPluginBase, IGitPluginForRepository
	{
		private StringSetting _gitLabAddress = new StringSetting("GitLab Address", "dev-lab");

		private StringSetting _gitLabPrivateKey = new StringSetting("GitLab Private Key", "");

		private StringSetting _gitLabFavoriteGroup = new StringSetting("GitLab Favorite Group", "dev");

		public GitLabPlugin()
		{
			SetNameAndDescription("!!!!!GitLab");
			Translate();
			Icon = Resources.gitlab;
		}

		public override IEnumerable<ISetting> GetSettings()
		{
			yield return _gitLabAddress;

			yield return _gitLabPrivateKey;

			yield return _gitLabFavoriteGroup;
		}

		public override bool Execute(GitUIEventArgs args)
		{
			ShowPluginWindow(args);			

			return true;
		}

		private async void ShowPluginWindow(GitUIEventArgs args)
		{
			try
			{
				var module = args.GitModule;

				GitModule gitModule = (GitModule)module;

				var remoteUrl = (await gitModule.GetRemotesAsync()).First().FetchUrl;

				GitModel gitModel = new GitModel(gitModule, args.OwnerForm, (GitUICommands)args.GitUICommands);

				var gitLabAddress = _gitLabAddress.ValueOrDefault(Settings);

				var gitLabKey = _gitLabPrivateKey.ValueOrDefault(Settings);

				var gitFavoriteGroup = _gitLabFavoriteGroup.ValueOrDefault(Settings);

				var gitLabModel = await GitLabModel.CreateAsync(gitLabAddress, gitLabKey, remoteUrl, gitFavoriteGroup);

				var pluginWindow = new PluginWindow
				{
					DataContext = new MergeRequestsManagerViewModel(gitLabModel, gitModel)
				};

				pluginWindow.Show();
			}
			catch (Exception)
			{
			}
		}

		private static string GetProjectName(IGitModule module)
		{
			var workingDir = module.WorkingDir;

			var projectFolder = Path.GetDirectoryName(workingDir);

			string projectName = projectFolder.Split(Path.DirectorySeparatorChar).Last().ToLowerInvariant();

			return projectName;
		}
	}
}
