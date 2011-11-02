using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;
using RHAL9000.Core;
using RHAL9000.Core.Configuration;
using RHAL9000.Display.Builds;
using RHAL9000.Monitors.Builds;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace RHAL9000.Testing.TDD
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class ConfigurationLoadingTests
    {
        /* 
         * remember - one test - test one thing
		 *            arrange / act / assert
		 *			  test code is first class code
         */

        [Test]
        public void String_To_Type_Lookups_Used_By_TypeBuilder()
        {
            var configurationXml = XDocument.Load(@"TDD\TeamCityConfig.xml");
            var teamCityTypeLookup = new TeamCityDataTypeLookup();
            var coreTypeLookup = new CoreTypeLookup();

            var typeBuilder = new DefaultTypeBuilder(teamCityTypeLookup, coreTypeLookup);

            var teamCityXml = configurationXml.Root.Element("DataSources").Element("BuildMonitor");
            var built = typeBuilder.Build(teamCityXml) as TeamCityBuildMonitor;

            built.Should().NotBeNull();
        }

        [Test]
        public void Discovery_Finds_Core_TypeLookup()
        {
            // arrange
            // act
            var typeLookups = TypeLookupDiscovery.FromAssembly(Assembly.GetAssembly(typeof (CoreTypeLookup)));
            // assert
            typeLookups.Should().HaveCount(1);
        }

        [Test]
        public void Discovery_Finds_Display_TeamCityViewTypeLookup()
        {
            // arrange
            // act
            var typeLookups = TypeLookupDiscovery.FromAssembly(Assembly.GetAssembly(typeof (TeamCityViewTypeLookup)));
            // assert
            typeLookups.Should().ContainItemsAssignableTo<TeamCityViewTypeLookup>();
        }

        [Test]
        public void Discovery_Finds_TeamCityDataTypeLookup()
        {
            // arrange
            // act
            var typeLookups = TypeLookupDiscovery.FromAssembly(Assembly.GetAssembly(typeof (TeamCityDataTypeLookup)));
            // assert
            typeLookups.Should().ContainItemsAssignableTo<TeamCityDataTypeLookup>();
        }
    }

    // ReSharper restore InconsistentNaming 
}