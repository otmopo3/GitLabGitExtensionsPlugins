﻿using System;
using Newtonsoft.Json;
using NGitLab;
using NGitLab.Models;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RestSharp.Serializers.NewtonsoftJson;

namespace GitLabGitExtensionsPlugin
{
	public class GitLabModel
	{
		private readonly string _gitLabAddress;
		private readonly string _gitLabKey;
		private readonly GitLabClient _gitLabCLient;
		private readonly Project _project;
		private readonly string _favoriteGroupFullPath;

		static GitLabModel()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
		}

		private GitLabModel(string gitLabAddress, string gitLabKey, GitLabClient gitLabCLient, Project project, string favoriteGroupFullPath)
		{
			_gitLabAddress = gitLabAddress;
			_gitLabKey = gitLabKey;
			_gitLabCLient = gitLabCLient;
			_project = project;
			_favoriteGroupFullPath = favoriteGroupFullPath;
		}

		public static async Task<GitLabModel> CreateAsync(string gitLabAddress, string gitLabKey, string projectName, string favoriteGroupFullPath)
		{
			return await Task.Run(() =>
			{
				return Create(gitLabAddress, gitLabKey, projectName, favoriteGroupFullPath);
			});
		}

		public static GitLabModel Create(string gitLabAddress, string gitLabKey, string projectName, string favoriteGroupFullPath)
		{
			GitLabClient gitLabCLient = GitLabClient.Connect(gitLabAddress, gitLabKey);

			var accessibleProjects = gitLabCLient.Projects.Accessible().ToList();

			var project = accessibleProjects.First(p => p.SshUrl.ToLowerInvariant().Contains(projectName.ToLowerInvariant()) ||
			p.HttpUrl.ToLowerInvariant().Contains(projectName.ToLowerInvariant()));

			return new GitLabModel(gitLabAddress, gitLabKey, gitLabCLient, project, favoriteGroupFullPath);
		}

		public List<MergeRequest> GetOpenedMergeRequests()
		{
			var mergeRequestClient = _gitLabCLient.GetMergeRequest(_project.Id);

			var openedMergeRequests = mergeRequestClient.AllInState(MergeRequestState.opened).ToList();

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

		public void AcceptMergeRequest(MergeRequest mergeRequest)
		{
			var mergeRequestClient = _gitLabCLient.GetMergeRequest(mergeRequest.ProjectId);

			MergeCommitMessage message = new MergeCommitMessage
			{
				Message = mergeRequest.Title
			};

			mergeRequestClient.Accept(mergeRequest.Iid, message);
		}

		public IEnumerable<string> GetUsersFromFavoriteGroup()
		{
			var favoriteGroup = _gitLabCLient.Groups.Accessible().FirstOrDefault(g => g.FullPath == _favoriteGroupFullPath);

			var membersList = new List<string>();

			if (favoriteGroup == null)
				return membersList;

			var client = new RestClient(_gitLabAddress);
			client.AddDefaultParameter("private_token", _gitLabKey);
			client.UseNewtonsoftJson();

			var getGroupMembersRequest = new RestRequest($"api/v4/groups/{favoriteGroup.Id}/members");


			var members = client.Get<List<GroupMember>>(getGroupMembersRequest);

			foreach (var item in members.Data)
			{
				membersList.Add(item.Name);
			}

			return membersList;
		}

		public string GetPipelinetUrl(PipelineData pipeline)
		{
			var projectUrl = _project.WebUrl;

			var pipelineUrl = $"{projectUrl}/pipelines/{pipeline.Id}";

			return pipelineUrl;
		}

		[JsonObject]
		[Serializable]
		class GroupMember
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("username")]
			public string Username { get; set; }

			[JsonProperty("state")]
			public string State { get; set; }

			[JsonProperty("avatar_url")]
			public string AvatarUrl { get; set; }

			[JsonProperty("web_url")]
			public string WebUrl { get; set; }

			[JsonProperty("access_level")]
			public int AccessLevel { get; set; }

			[JsonProperty("expires_at")]
			public object ExpiresAt { get; set; }
		}
	}
}
