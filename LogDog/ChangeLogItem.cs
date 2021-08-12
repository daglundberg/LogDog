using System.Collections.Generic;
using System.Linq;
using DotLiquid;
using LibGit2Sharp;

namespace LogDog
{
    public class ChangeLogItem : Drop
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }
        public string Hash { get; set; }

        public SemanticVersion Version { get; set; }

        public static ChangeLogItem[] GetChangeLogItems(string repository, out SemanticVersion latestVersion)
        {
			var changeLogItems = new List<ChangeLogItem>();

            Repository repository1 = new Repository(repository);
           
            var version = new SemanticVersion("0.1.0");
            foreach (Commit c in repository1.Commits.Reverse())
            {
                version.Patch += 1;

                foreach (Note note in c.Notes)
                {
                    if (note.Message.ToLower().Contains("minor"))
					{
                        version.Minor += 1;
                        version.Patch = 0;
                        break;
                    }
                    else if (note.Message.ToLower().Contains("major"))
                    {
                        version.Major += 1;
                        version.Minor = 0;
                        version.Patch = 0;
                        break;
                    }

                    if (note.Message.ToLower().Contains("changelog"))
                    {
                        ChangeLogItem changeLogItem = new ChangeLogItem()
                        {
                            Date = c.Committer.When.DateTime.ToString("D"),
                            Hash = c.Sha,
                            Version = version,
                            Body = c.Message.Remove(0, c.MessageShort.Length).Trim(),
                            Subject = c.MessageShort
                        };
                        changeLogItems.Add(changeLogItem);
                    }
                }
            }

            latestVersion = version;
            changeLogItems.Reverse();
            return changeLogItems.ToArray();
        }
	}
}