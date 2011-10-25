using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RHAL9000.Monitors.Builds;
using Rhino.Mocks;

namespace RHAL9000.Testing
{
    [TestFixture]
    public class BuildMonitorUnitTests
    {
        private IBuildClient SetupMockedClient(MockRepository mocks)
        {
            var client = mocks.DynamicMock<IBuildClient>();

            using (mocks.Record())
            {
                SetupResult.For(client.GetAllProjectSummaries()).Return(
                    new[]
                        {
                            new Summary {Id = "1", Name = "A"},
                            new Summary {Id = "2", Name = "B"}
                        });

                SetupResult.For(client.GetProject("1"))
                    .Return(new BuildProject
                                {
                                    Id = "1",
                                    Name = "A",
                                    BuildTypes = new[]
                                                     {
                                                         new Summary {Id = "3", Name = "C"},
                                                         new Summary {Id = "4", Name = "D"}
                                                     }
                                });

                SetupResult.For(client.GetProject("2"))
                    .Return(new BuildProject
                                {
                                    Id = "2",
                                    Name = "B",
                                    BuildTypes = new[]
                                                     {
                                                         new Summary {Id = "5", Name = "E"},
                                                         new Summary {Id = "6", Name = "F"}
                                                     }
                                });

                SetupResult.For(client.GetBuildType("3")).Return(new BuildType
                                                                     {BuildProjectId = "1", Id = "3", Name = "C"});
                SetupResult.For(client.GetBuildType("4")).Return(new BuildType
                                                                     {BuildProjectId = "1", Id = "4", Name = "D"});
                SetupResult.For(client.GetBuildType("5")).Return(new BuildType
                                                                     {BuildProjectId = "2", Id = "5", Name = "E"});
                SetupResult.For(client.GetBuildType("6")).Return(new BuildType
                                                                     {BuildProjectId = "2", Id = "6", Name = "F"});
            }
            return client;
        }

        [Test]
        public void Initialise_Does_Not_Cache_Project_With_Unspecified_Build_Types()
        {
            var mocks = new MockRepository();

            IBuildClient client = SetupMockedClient(mocks);

            using (mocks.Playback())
            {
                var monitor = new TeamCityBuildMonitor(client, "5", "6");

                monitor.Initialise();

                IEnumerable<BuildProjectModel> projects = monitor.GetAllBuildProjects();

                projects.Should().NotBeEmpty().And.HaveCount(1);

                BuildProjectModel one = projects.First();

                one.Id.Should().Be("2");
                one.BuildTypes.Should().NotBeEmpty().And.HaveCount(2);
            }
        }

        [Test]
        public void Initialise_Loads_Only_One_Build_Type_From_Each_Project()
        {
            var mocks = new MockRepository();

            IBuildClient client = SetupMockedClient(mocks);

            using (mocks.Playback())
            {
                var monitor = new TeamCityBuildMonitor(client, "3", "6");

                monitor.Initialise();

                IEnumerable<BuildProjectModel> projects = monitor.GetAllBuildProjects();

                projects.Should().NotBeEmpty().And.HaveCount(2);

                BuildProjectModel one = projects.First();

                one.Id.Should().Be("1");
                one.BuildTypes.Should().NotBeEmpty().And.HaveCount(1);

                BuildProjectModel two = projects.Last();

                two.Id.Should().Be("2");
                two.BuildTypes.Should().NotBeEmpty().And.HaveCount(1);
            }
        }

        [Test]
        public void Initialise_Loads_Project_Cache_With_All_Projects_Types_For_Empty_Id_List()
        {
            var mocks = new MockRepository();

            IBuildClient client = SetupMockedClient(mocks);

            using (mocks.Playback())
            {
                var monitor = new TeamCityBuildMonitor(client);

                monitor.Initialise();

                IEnumerable<BuildProjectModel> projects = monitor.GetAllBuildProjects();

                projects.Should().NotBeEmpty().And.HaveCount(2);

                BuildProjectModel one = projects.First();

                one.Id.Should().Be("1");
                one.Name.Should().Be("A");
                one.BuildTypes.Should().NotBeEmpty().And.HaveCount(2);

                BuildProjectModel two = projects.Last();

                two.Id.Should().Be("2");
                two.Name.Should().Be("B");
                two.BuildTypes.Should().NotBeEmpty().And.HaveCount(2);
            }
        }
    }
}