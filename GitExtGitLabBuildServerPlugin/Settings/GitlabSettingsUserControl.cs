using GitUIPluginInterfaces;
using GitUIPluginInterfaces.BuildServerIntegration;
using ResourceManager;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace GitExtGitLabBuildServerPlugin.Settings
{
	[Export(typeof(IBuildServerSettingsUserControl))]
	[BuildServerSettingsUserControlMetadata(GitLabBuildServerAdapter.PluginName)]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class GitlabSettingsUserControl : GitExtensionsControl, IBuildServerSettingsUserControl
	{
		private string _defaultProjectName;

		public GitlabSettingsUserControl()
		{
			InitializeComponent();

			InitializeComplete();
		}
		
		public void Initialize(string defaultProjectName, IEnumerable<string> remotes)
		{
			_defaultProjectName = defaultProjectName;
		}

		public void LoadSettings(ISettingsSource buildServerConfig)
		{
			if (buildServerConfig != null)
			{
				GitlabAddressTb.Text = buildServerConfig.GetString(GitlabSettingsConstants.GitlabAddress, string.Empty);
				DefaultProjectIdTb.Text = buildServerConfig.GetString(GitlabSettingsConstants.DefaultProjectId, _defaultProjectName);
				GitlabKeyTb.Text = buildServerConfig.GetString(GitlabSettingsConstants.GitlabKey, string.Empty);
			}
		}

		public void SaveSettings(ISettingsSource buildServerConfig)
		{
			buildServerConfig.SetString(GitlabSettingsConstants.GitlabAddress, GitlabAddressTb.Text);
			buildServerConfig.SetString(GitlabSettingsConstants.DefaultProjectId, DefaultProjectIdTb.Text);
			buildServerConfig.SetString(GitlabSettingsConstants.GitlabKey, GitlabKeyTb.Text);
		}
	}
}
