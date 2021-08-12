using DotLiquid;
using System;
using System.IO;

namespace LogDog
{
	partial class Program
	{
        static void Main(string[] args)
        {
            var project = new Project();

            project.Directory = Directory.GetCurrentDirectory();

            if (args.Length > 0)
                if (!String.IsNullOrWhiteSpace(args[0]))
                    if (Directory.Exists(args[0]))
                        project.Directory = args[0];
                    else
                    {
                        Console.WriteLine("Directory does not exist.");
                        return;
                    }

            if (!Directory.Exists(Path.Combine(project.Directory, ".git/")))
			{
				Console.WriteLine("Directory is not a git repository.");
                return;
			}

            project.Name = Path.GetFileName(project.Directory);
            project.ChangeLogItems = ChangeLogItem.GetChangeLogItems(project.Directory, out var semanticVersion);
            project.LatestVersion = semanticVersion;
            project.Copyright = $"Copyright © { DateTime.Now.Year }";
            WriteChangeLog(project);
            Console.WriteLine("Created change_log.html");

            return;
        }

        public static void WriteChangeLog(Project project)
		{
            using var streamWriter = new StreamWriter(Path.Combine(project.Directory, "change_log.html"));

            Template template = Template.Parse(Resources.template);

            streamWriter.Write(template.Render(Hash.FromAnonymousObject(project)));
            streamWriter.Flush();
        }		
    }
}