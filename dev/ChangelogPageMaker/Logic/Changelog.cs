﻿using Markdig;
using System.Text;

namespace ChangelogPageMaker.Logic;

internal class ChangelogChange
{
    public string Markdown;

    public ChangelogChange(string markdown)
    {
        Markdown = markdown;
    }

    public string GetHtml()
    {
        string md = Markdown;

        string[] words = md
            .Replace("(", " ")
            .Replace(")", " ")
            .Replace("_", " ")
            .Replace("!", " ")
            .Replace(".", " ")
            .Replace(":", " ")
            .Replace(",", " ")
            .Split(" ");

        List<int> issues = new();
        List<string> ids = new();

        foreach (string word in words)
        {
            if (word.StartsWith("#"))
            {
                if (int.TryParse(word.Substring(1), out int issue))
                {
                    issues.Add(issue);
                }
            }
            else if (word.StartsWith("@"))
            {
                ids.Add(word.Substring(1));
            }
        }

        foreach (int issue in issues.OrderByDescending(x => x))
        {
            md = md.Replace($"#{issue}", $"[#_ISSUE{issue}](https://github.com/ScottPlot/ScottPlot/issues/{issue})");
        }

        foreach (string id in ids)
        {
            md = md.Replace($"@{id}", $"[@_USER{id}](https://github.com/{id})");
        }

        md = md.Replace("#_ISSUE", "#");
        md = md.Replace("@_USER", "@");

        string html = Markdig.Markdown.ToHtml(md).Replace("<p>", "").Replace("</p>", "");
        return html;
    }
}

internal class ChangelogRelease
{
    public string Date { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<ChangelogChange> Changes { get; } = new();

    public string GetHtml()
    {
        StringBuilder sb = new();
        sb.AppendLine($"<h1 class='mb-0'>{Title}</h1>");

        sb.AppendLine($"<div><i>NuGet packages published {Date}</i></div>");

        sb.AppendLine($"<div class='mt-1 mb-5'>");
        sb.AppendLine($"<ul>");
        foreach (ChangelogChange change in Changes)
        {
            sb.AppendLine($"<li>{change.GetHtml()}</li>");
        }
        sb.AppendLine($"</ul>");
        sb.AppendLine($"</div>");

        return sb.ToString();
    }
}

internal class Changelog
{
    public readonly List<ChangelogRelease> Releases = new();

    public Changelog(string markdown)
    {
        string[] lines = markdown.Split("\n");

        ChangelogRelease thisRelease = new();

        foreach (string line in lines)
        {
            // ignore top-level headings
            if (line.StartsWith("# "))
                continue;

            if (line.StartsWith("## "))
            {
                if (thisRelease.Changes.Any())
                {
                    Releases.Add(thisRelease);
                    thisRelease = new ChangelogRelease();
                }

                thisRelease.Title = line.Substring(3).Trim();
                continue;
            }

            if (line.StartsWith("_Published"))
            {
                thisRelease.Date = line.Trim().Split(" ").Last().Replace("_", "");
                continue;
            }

            if (line.StartsWith("* "))
            {
                ChangelogChange change = new(line.Substring(2).Trim());
                thisRelease.Changes.Add(change);
            }
        }

        if (thisRelease.Changes.Any())
            Releases.Add(thisRelease);
    }

    public string GetHtml()
    {
        StringBuilder sb = new();

        foreach (ChangelogRelease release in Releases)
        {
            sb.AppendLine(release.GetHtml());
        }

        return sb.ToString();
    }
}
