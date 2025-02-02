﻿namespace ChangelogPageMaker.Logic;

internal class ChangelogTests
{
    Changelog Changelog = new(SampleChangelog.Text, RepoPaths.ContributorImageFolder);

    [Test]
    public void Test_Changelog_HasSections()
    {
        Changelog.Releases.Should().NotBeEmpty();

        foreach (ChangelogRelease release in Changelog.Releases)
        {
            Console.WriteLine($"{release.Title} ({release.Date}) has {release.Changes.Count} changes");
            release.Changes.Should().NotBeEmpty();
        }
    }

    [Test]
    public void Test_SampleData_ContainsAllContributors()
    {
        foreach (string author in SampleChangelog.Contributors)
        {
            string githubID = "@" + author;
            if (!SampleChangelog.Text.Contains(githubID))
            {
                Assert.Fail(githubID);
            }
        }
    }

    [Test]
    public void Test_GithubIDs_AreAllFound()
    {
        foreach (string id in Changelog.Contributors)
        {
            if (!SampleChangelog.Contributors.Contains(id))
            {
                Assert.Fail(id);
            }
        }
    }

    [Test]
    public void Test_Contributors_NoDuplicates()
    {
        HashSet<string> unique = new();

        foreach (string id in Changelog.Contributors)
        {
            if (unique.Contains(id))
                Assert.Fail(id);

            unique.Add(id);
        }

        Assert.That(Changelog.Contributors.Length, Is.EqualTo(unique.Count));
    }
}
