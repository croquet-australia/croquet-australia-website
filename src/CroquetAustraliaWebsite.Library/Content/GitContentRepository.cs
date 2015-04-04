using System;
using System.IO;
using Anotar.LibLog;
using CroquetAustraliaWebsite.Library.Settings;
using LibGit2Sharp;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class GitContentRepository : IGitContentRepository
    {
        private readonly GitContentRepositorySettings _settings;

        public GitContentRepository(GitContentRepositorySettings settings)
        {
            _settings = settings;
        }

        public string Directory
        {
            get { return _settings.Directory; }
        }

        public void CommitAndPush(string path)
        {
            LogTo.Trace("CommitAndPush(path: {0})", path);
            Commit(path);
            Push();
        }

        private void Commit(string path)
        {
            LogTo.Warn("todo: Commit(path: {0})", path);
        }

        private void Push()
        {
            LogTo.Warn("todo: push");
        }

        public void Start()
        {
            LogTo.Trace("Start()");

            if (!System.IO.Directory.Exists(Directory))
            {
                CloneRepository();
            }
        }

        private void CloneRepository()
        {
            LogTo.Info("Cloning content repository...");

            EnsureParentDirectoryExists();

            string clonedToDirectory;

            try
            {
                var options = new CloneOptions()
                {
                    BranchName = "master",
                    CredentialsProvider = GitCredentials,
                    Checkout = true
                };

                clonedToDirectory = Repository.Clone(_settings.Url, _settings.Directory, options);
            }
            catch (Exception exception)
            {
                LogTo.FatalException(string.Format("Error while cloning content directory to '{0}'.", _settings.Directory), exception);
                System.IO.Directory.Delete(_settings.Directory, true);
                LogTo.Info("Successfully deleted '{0}' directory after clone failed.", _settings.Directory);
                throw;
            }

            LogTo.Info("Successfully cloned content repository to {0}.", clonedToDirectory);
        }

        public Credentials GitCredentials(string url, [AllowNull] string usernameFromUrl, SupportedCredentialTypes types)
        {
            LogTo.Trace("GitCredentials(url: {0}, usernameFromUrl: {1}, types: {2})", url, usernameFromUrl, types);

            if (types != SupportedCredentialTypes.UsernamePassword)
            {
                throw new ArgumentOutOfRangeException("types", types, "Value must be SupportedCredentialTypes.UsernamePassword.");
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
            var directory = new DirectoryInfo(_settings.Directory);

            // ReSharper disable once PossibleNullReferenceException
            directory.Parent.Create();
        }
    }
}