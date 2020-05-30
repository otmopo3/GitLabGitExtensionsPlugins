using GitUIPluginInterfaces.BuildServerIntegration;
using System;
using System.ComponentModel.Composition;


namespace GitExtGitLabBuildServerPlugin
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class)]
	public class GitLabIntegrationMetadata : BuildServerAdapterMetadataAttribute
	{
		public GitLabIntegrationMetadata(string buildServerType)
			: base(buildServerType)
		{
		}

		public override string CanBeLoaded
		{
			get
			{
				return null;
			}
		}
	}
}
