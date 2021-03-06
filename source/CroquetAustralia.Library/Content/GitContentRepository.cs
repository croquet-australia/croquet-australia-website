﻿using System;
using System.IO;
using Anotar.LibLog;
using Casper.Data.Git.Git;
using Casper.Domain.Features.Authors;
using CroquetAustralia.Library.Settings;
using LibGit2Sharp;
using NullGuard;
using OpenMagic.Extensions;

namespace CroquetAustralia.Library.Content
{
    // todo: refactor for better integration with Casper's Git repository.
    public class GitContentRepository : IGitContentRepository
    {
        private readonly Lazy<IGitRepository> _gitRepository;
        private readonly GitContentRepositorySettings _settings;

        public GitContentRepository(GitContentRepositorySettings settings)
        {
            _settings = settings;
            _gitRepository = new Lazy<IGitRepository>(() => new GitRepository(new GitRepositorySettings(new DirectoryInfo(_settings.Directory), _settings.UserName, _settings.Password)));
        }

        private IGitRepository GitRepository => _gitRepository.Value;

        public string Directory => _settings.Directory;

        public void CommitAndPush(string relativePath, Author author)
        {
            LogTo.Trace("CommitAndPush(relativePath: {0})", relativePath);

            Commit(relativePath, author);
            Push();
        }

        public void Start()
        {
            LogTo.Trace("Start()");

            if (!System.IO.Directory.Exists(Directory))
            {
                CloneRepository();
            }
        }

        private void Commit(string relativePath, Author author)
        {
            GitRepository.CommitAsync(GitBranches.Master, relativePath, $"Published page '{relativePath.TrimEnd(".md")}'.", author).Wait();
        }

        private void Push()
        {
            GitRepository.PushAsync(GitBranches.Master).Wait();
        }

        private void CloneRepository()
        {
            LogTo.Info("Cloning content repository...");

            EnsureParentDirectoryExists();

            string clonedToDirectory;

            var options = new CloneOptions
            {
                BranchName = "master",
                CredentialsProvider = GitCredentials,
                Checkout = true
            };

            try
            {
                LogTo.Debug($"Cloning '{options.BranchName}' branch of '{_settings.Url}' to '{_settings.Directory}'...");
                clonedToDirectory = Repository.Clone(_settings.Url, _settings.Directory, options);
            }
            catch (Exception cloneException)
            {
                try
                {
                    LogTo.ErrorException($"Cloning '{options.BranchName}' branch of '{_settings.Url}' to '{_settings.Directory}'...", cloneException);

                    if (System.IO.Directory.Exists(_settings.Directory))
                    {
                        LogTo.Info($"Deleting '{_settings.Directory}' directory after clone failed...");
                        System.IO.Directory.Delete(_settings.Directory, true);
                        LogTo.Info($"Successfully deleted '{_settings.Directory}' directory after clone failed.");
                    }
                }
                catch (Exception deleteException)
                {
                    LogTo.FatalException($"Error while deleting '{_settings.Directory}' after clone failed.", deleteException);
                }
                throw;
            }

            LogTo.Info("Successfully cloned content repository to {0}.", clonedToDirectory);
        }

        public Credentials GitCredentials(string url, [AllowNull] string usernameFromUrl, SupportedCredentialTypes types)
        {
            LogTo.Trace("GitCredentials(url: {0}, usernameFromUrl: {1}, types: {2})", url, usernameFromUrl, types);

            if (types != SupportedCredentialTypes.UsernamePassword)
            {
                throw new ArgumentOutOfRangeException(nameof(types), types, "Value must be SupportedCredentialTypes.UsernamePassword.");
            }

            return new UsernamePasswordCredentials
            {
                Username = _settings.UserName,
                Password = _settings.Password
            };
        }

        /// <summary>
        ///     Ensures the Git repository's parent directory exists.
        /// </summary>
        /// <remarks>
        ///     Typically the Git directory is ~/App_Data/Content/Git. Git requires ~/App_Data/Content directory exists.
        /// </remarks>
        private void EnsureParentDirectoryExists()
        {
            LogTo.Trace("EnsureParentDirectoryExists()");

            var directory = new DirectoryInfo(_settings.Directory);

            // ReSharper disable once PossibleNullReferenceException
            directory.Parent.Create();
        }
    }
}