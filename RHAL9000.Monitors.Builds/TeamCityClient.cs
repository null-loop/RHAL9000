using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using RHAL9000.Core;

namespace RHAL9000.Monitors.Builds
{
    public class TeamCityClient : IBuildClient
    {
        private const string XmlDateTimeFormatString = "yyyyMMddTHHmmsszzz";
        private static readonly CultureInfo Culture = new CultureInfo("en-GB", true);

        public TeamCityClient(Uri baseUrl, string username, string password)
            : this(new HttpXmlAccessor(baseUrl, "/httpAuth/app/rest/", username, password))
        {
        }

        public TeamCityClient(IXmlAccessor accessor)
        {
            if (accessor == null) throw new ArgumentNullException("accessor");
            Accessor = accessor;
        }

        private IXmlAccessor Accessor { get; set; }

        #region IBuildClient Members

        public IEnumerable<ISummary> GetAllProjectSummaries()
        {
            XElement xml = Accessor.GetXml("projects");

            return xml.Elements("project").Select(
                p => new Summary
                         {
                             Id = p.Attribute("id").Value,
                             Name = p.Attribute("name").Value
                         });
        }

        public IBuildProject GetProject(string projectId)
        {
            XElement xml = Accessor.GetXml("projects/id:" + projectId);

            return new BuildProject
                       {
                           Id = xml.Attribute("id").Value,
                           Name = xml.Attribute("name").Value,
                           WebUrl = new Uri(xml.Attribute("webUrl").Value),
                           Archived = bool.Parse(xml.Attribute("archived").Value),
                           Description = xml.Attribute("description").Value,
                           BuildTypes =
                               xml.Element("buildTypes").Elements("buildType").Select(
                                   b => new Summary {Id = b.Attribute("id").Value, Name = b.Attribute("name").Value})
                       };
        }

        public IBuildType GetBuildType(string buildTypeId)
        {
            XElement xml = Accessor.GetXml("buildTypes/id:" + buildTypeId);

            return new BuildType
                       {
                           Id = xml.Attribute("id").Value,
                           Name = xml.Attribute("name").Value,
                           BuildProjectId = xml.Element("project").Attribute("id").Value,
                           Description = xml.Attribute("description") != null ? xml.Attribute("description").Value : "",
                           WebUrl = new Uri(xml.Attribute("webUrl").Value),
                           Paused = bool.Parse(xml.Attribute("paused").Value),
                           RunParameters =
                               xml.Element("runParameters").Elements("property").ToDictionary(
                                   p => p.Attribute("name").Value, p => p.Attribute("value").Value)
                       };
        }

        public IEnumerable<IBuildResult> GetCompletedBuilds()
        {
            XElement xml = Accessor.GetXml("builds?count=1000");

            return
                xml.Elements("build").Select(
                    x =>
                    new BuildResult
                        {
                            BuildTypeId = x.Attribute("buildTypeId").Value,
                            Id = x.Attribute("id").Value,
                            Number = x.Attribute("number").Value,
                            Status = ConvertBuildStatusString(x.Attribute("status").Value),
                            WebUri = new Uri(x.Attribute("webUrl").Value)
                        });
        }

        public IBuildProgress GetInProgressBuild(string buildTypeId)
        {
            //http://build.caternet.co.uk:8100/httpAuth/app/rest/buildTypes/id:bt8/builds/running:true
            XElement xml = Accessor.GetXml("buildTypes/id:" + buildTypeId + "/builds/running:true");

            if (xml == null) return null;

            return new BuildProgress
                       {
                           Id = xml.Attribute("id").Value,
                           Number = xml.Attribute("number").Value,
                           Status = ConvertBuildStatusString(xml.Attribute("status").Value),
                           WebUri = new Uri(xml.Attribute("webUrl").Value),
                           Personal = bool.Parse(xml.Attribute("personal").Value),
                           History = bool.Parse(xml.Attribute("history").Value),
                           Pinned = bool.Parse(xml.Attribute("pinned").Value),
                           StatusText = xml.Element("statusText").Value,
                           Started = ConvertXmlDateTime(xml.Element("startDate").Value),
                           BuildTypeId = xml.Element("buildType").Attribute("id").Value,
                           AgentId = xml.Element("agent").Attribute("id").Value,
                           PercentageComplete =
                               xml.Element("running-info").Attribute("percentageComplete") != null
                                   ? decimal.Parse(xml.Element("running-info").Attribute("percentageComplete").Value)
                                   : 0,
                           ElapsedRunTime =
                               TimeSpan.FromSeconds(
                                   Double.Parse(xml.Element("running-info").Attribute("elapsedSeconds").Value)),
                           EstimatedRunTime =
                               xml.Element("running-info").Attribute("estimatedTotalSeconds") != null
                                   ? TimeSpan.FromSeconds(
                                       Double.Parse(xml.Element("running-info").Attribute("estimatedTotalSeconds").Value))
                                   : TimeSpan.FromSeconds(0),
                           CurrentStage = xml.Element("running-info").Attribute("currentStageText").Value,
                           Outdated = bool.Parse(xml.Element("running-info").Attribute("outdated").Value),
                           ProbablyHanging = bool.Parse(xml.Element("running-info").Attribute("probablyHanging").Value)
                       };
        }

        public IFullBuildResult GetCompletedBuild(string buildId)
        {
            XElement xml = Accessor.GetXml("builds/running:true,id:" + buildId);

            return new FullBuildResult
                       {
                           Id = xml.Attribute("id").Value,
                           Number = xml.Attribute("number").Value,
                           Status = ConvertBuildStatusString(xml.Attribute("status").Value),
                           WebUri = new Uri(xml.Attribute("webUrl").Value),
                           Personal = bool.Parse(xml.Attribute("personal").Value),
                           History = bool.Parse(xml.Attribute("history").Value),
                           Pinned = bool.Parse(xml.Attribute("pinned").Value),
                           StatusText = xml.Element("statusText").Value,
                           Started = ConvertXmlDateTime(xml.Element("startDate").Value),
                           Finished = ConvertXmlDateTime(xml.Element("finishDate").Value),
                           BuildTypeId = xml.Element("buildType").Attribute("id").Value,
                           AgentId = xml.Element("agent").Attribute("id").Value
                       };
        }

        #endregion

        internal static BuildStatus ConvertBuildStatusString(string value)
        {
            switch (value)
            {
                case "SUCCESS":
                    return BuildStatus.Success;
                case "ERROR":
                    return BuildStatus.Error;
                case "FAILURE":
                    return BuildStatus.Failure;
                default:
                    return BuildStatus.Unknown;
            }
        }

        private static DateTime ConvertXmlDateTime(string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, XmlDateTimeFormatString, Culture);
        }
    }
}