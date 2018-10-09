using GitCommands;
using GitLabGitExtensionsPlugin.Properties;
using GitUI;
using GitUIPluginInterfaces;
using ResourceManager;
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
		}

		public override bool Execute(GitUIEventArgs args)
		{
			var module = args.GitModule;

			var describe = module.GetCurrentRemote();

			string projectName = GetProjectName(module);

			GitModule gitModule = (GitModule)module;

			var remoteUrl = gitModule.GetRemotes().First().FetchUrl;

			GitModel gitModel = new GitModel(gitModule, args.OwnerForm);						

			var gitLabAddress = _gitLabAddress.ValueOrDefault(Settings);

			var gitLabKey = _gitLabPrivateKey.ValueOrDefault(Settings);

			var gitLabModel = GitLabModel.Create(gitLabAddress, gitLabKey, remoteUrl);

			var pluginWindow = new PluginWindow();

			pluginWindow.DataContext = new MergeRequestsManagerViewModel(gitLabModel, gitModel);

			pluginWindow.Show();

			return true;
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
