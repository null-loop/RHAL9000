using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;
using RHAL9000.Core;
using RHAL9000.Monitors.Builds;
using Rhino.Mocks;

namespace RHAL9000.Testing.Unit.Monitor.Builds
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class TeamCityClientTests
    {
        /* 
         * remember - one test - test one thing
		 *            arrange / act / assert
		 *			  test code is first class code
         */

        [Test]
        public void GetAllProjectSummaries_Get_Xml_For_Single()
        {
            // arrange
            var mocks = new MockRepository();
            var xmlAccessor = mocks.StrictMock<IXmlAccessor>();
            var teamCityClient = new TeamCityClient(xmlAccessor);

            using (mocks.Record())
            {
                // set expectations
                Expect.Call(xmlAccessor.GetXml("projects")).Return(new XElement("projects",
                                                                                new XElement("project", new XAttribute("id", 1),
                                                                                             new XAttribute("name", "TeamCity Test Project")))).
                    Repeat.Once();
            }
            using (mocks.Playback())
            {
                // act
                var summaries = teamCityClient.GetAllProjectSummaries();
                // assert
                summaries.Should().HaveCount(1);

                var summary = summaries.First();
                summary.Id.Should().Be("1");
                summary.Name.Should().Be("TeamCity Test Project");
            }
        }

        [Test]
        public void GetProject_Gets_And_Parses_Xml()
        {
            // arrange

            var mocks = new MockRepository();
            var xmlAccessor = mocks.StrictMock<IXmlAccessor>();
            var teamCityClient = new TeamCityClient(xmlAccessor);

            using (mocks.Record())
            {
                Expect.Call(xmlAccessor.GetXml("projects/id:1")).Return(new XElement("project", 
                                                                            new XAttribute("id", "1"), 
                                                                            new XAttribute("name", "My first project"),
                                                                            new XAttribute("webUrl", "http://localhost:8001/buildProject1"),
                                                                            new XAttribute("archived", false),
                                                                            new XAttribute("description", "builds my project"),
                                                                            new XElement("buildTypes",
                                                                                new XElement("buildType", new XAttribute("id", "2"),
                                                                                    new XAttribute("name", "Master CI"))))).Repeat.Once();

            }

            using (mocks.Playback())
            {
                
                // act
                var result = teamCityClient.GetProject("1");
                // assert
                result.Should().NotBeNull();
                result.Id.Should().Be("1");
                result.Name.Should().Be("My first project");
                result.WebUrl.Should().Be(new Uri("http://localhost:8001/buildProject1"));
                result.Archived.Should().Be(false);
                result.Description.Should().Be("builds my project");
                result.BuildTypes.Should().NotBeNull();
                result.BuildTypes.Should().HaveCount(1);
                var buildType = result.BuildTypes.First();
                buildType.Id.Should().Be("2");
                buildType.Name.Should().Be("Master CI");
            }
        }

        [Test]
        public void GetBuildType_Gets_And_Parses_XML()
        {
            // arrange

            var mocks = new MockRepository();
            var xmlAccessor = mocks.StrictMock<IXmlAccessor>();
            var teamCityClient = new TeamCityClient(xmlAccessor);

            using (mocks.Record())
            {
                // set expectations
                Expect.Call(xmlAccessor.GetXml("buildTypes/id:2")).Return(new XElement("buildType", new XAttribute("id", "2"),
                                                                                new XAttribute("name", "Master CI"),
                                                                                new XAttribute("webUrl", "http://localhost"),
                                                                                new XAttribute("paused", false),
                                                                                new XAttribute("description", "My build type"),
                                                                                new XElement("project", new XAttribute("id", "1")),
                                                                                new XElement("runParameters",
                                                                                    new XElement("property",
                                                                                        new XAttribute("name", "dot-net-framework"),
                                                                                        new XAttribute("value", "4.0"))))).Repeat.Once();
            }
            using (mocks.Playback())
            {
                // act
                var buildType = teamCityClient.GetBuildType("2");
                // assert
                buildType.Should().NotBeNull();
                buildType.Id.Should().Be("2");
                buildType.Name.Should().Be("Master CI");
                buildType.WebUrl.Should().Be(new Uri("http://localhost"));
                buildType.Paused.Should().Be(false);
                buildType.Description.Should().Be("My build type");
                buildType.BuildProjectId.Should().Be("1");
                buildType.RunParameters.Should().HaveCount(1);
                buildType.RunParameters.Should().ContainKey("dot-net-framework");
                buildType.RunParameters["dot-net-framework"].Should().Be("4.0");
            }
        }

        [Test,
         TestCaseSource("ConvertBuildStatusString_TestCases")]
        public void ConvertBuildStatusString(string buildStatus, BuildStatus expected)
        {
            // arrange
            // act
            var result = TeamCityClient.ConvertBuildStatusString(buildStatus);
            // assert
            result.Should().Be(expected);
        }


        // tests cases for 
        private static IEnumerable<object[]> ConvertBuildStatusString_TestCases()
        {
            yield return new object[] { "SUCCESS", BuildStatus.Success };
            yield return new object[] { "ERROR", BuildStatus.Error };
            yield return new object[] { "FAILURE", BuildStatus.Failure };
            yield return new object[] { "ANYTHING ELSE", BuildStatus.Unknown };
            yield return new object[] { null, BuildStatus.Unknown };
            yield return new object[] { string.Empty, BuildStatus.Unknown };
        }
    }

    // ReSharper restore InconsistentNaming 
}