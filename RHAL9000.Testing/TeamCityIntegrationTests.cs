using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.EventMonitoring;
using NUnit.Framework;
using RHAL9000.Monitors.Builds;

namespace RHAL9000.Testing
{
    [TestFixture]
    public class TeamCityIntegrationTests
    {
        private static TeamCityClient GetTeamcityClient()
        {
            return new TeamCityClient("http://localhost:8080", "apiuser", "pa55w0rd");
        }

        [Test]
        public void BuildMonitor_Can_Setup_Projects()
        {
            TeamCityClient teamcityClient = GetTeamcityClient();
            var monitor = new TeamCityBuildMonitor(teamcityClient);

            monitor.Initialise();

            IEnumerable<BuildProjectModel> projects = monitor.GetAllBuildProjects();

            projects.Should().NotBeEmpty().And.HaveCount(c => c > 0);
        }

        [Test]
        public void BuildMonitor_Establishes_BuildHistory()
        {
            var teamcityClient = GetTeamcityClient();
            var monitor = new TeamCityBuildMonitor(teamcityClient);

            monitor.MonitorEvents();

            monitor.Initialise();

            monitor.ShouldRaise("BuildUpdated").WithSender(monitor);
        }

        [Ignore]
        [Test]
        public void Can_Retrieve_BuildProgress()
        {
            TeamCityClient teamcityClient = GetTeamcityClient();

            IBuildProgress build = teamcityClient.GetInProgressBuild("bt2");
            build.Should().NotBeNull();
        }

        [Test]
        public void Can_Retrieve_BuildType()
        {
            TeamCityClient teamcityClient = GetTeamcityClient();

            IBuildType buildType = teamcityClient.GetBuildType("bt2");

            buildType.Should().NotBeNull();
            buildType.Paused.Should().BeFalse();
            buildType.WebUri.Should().Be(new Uri("http://localhost:8080/viewType.html?buildTypeId=bt2"));
            buildType.RunParameters.Should().NotBeEmpty();
            Assert.AreEqual(20, buildType.RunParameters.Keys.Count);
        }

        [Test]
        public void Can_Retrieve_CompletedBuild()
        {
            TeamCityClient teamcityClient = GetTeamcityClient();

            IFullBuildResult build = teamcityClient.GetCompletedBuild("4");

            build.Should().NotBeNull();
            build.Id.Should().Be("4");
            build.Number.Should().Be("4");
            build.Status.Should().Be(BuildStatus.Failure);
        }

        [Test]
        public void Can_Retrieve_List_Of_Completed_Builds()
        {
            TeamCityClient teamcityClient = GetTeamcityClient();

            IEnumerable<IBuildResult> builds = teamcityClient.GetCompletedBuilds();

            builds.Should().NotBeEmpty().And.HaveCount(c=>c > 0);
        }

        [Test]
        public void Can_Retrieve_List_Of_Project_Summaries()
        {
            TeamCityClient teamcityClient = GetTeamcityClient();

            IEnumerable<ISummary> projects = teamcityClient.GetAllProjectSummaries();

            projects.Should().NotBeEmpty().And.HaveCount(2);
        }
    }
}