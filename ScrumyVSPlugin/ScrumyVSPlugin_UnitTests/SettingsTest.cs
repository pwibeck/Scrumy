//using System;
//using System.Drawing;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using PeterWibeck.ScrumyVSPlugin.TFS;

//namespace ScrumyVSPlugin_UnitTests
//{
//    [TestClass]
//    public class SettingsTest
//    {
//        [TestMethod]
//        public void Settings_Constructor()
//        {
//            var settings = new Settings();
//            var expectedSettings = new Settings
//                {
//                    ScrumTeam = "",
//                    Sprint = "",
//                    TfsProject = "",
//                    BugBackGroundColor = Color.White,
//                    DeliverableBackGroundColor = Color.White,
//                    TaskBackGroundColor = Color.White
//                };

//            Assert.AreEqual(expectedSettings, settings);
//        }

//        [TestMethod]
//        public void Settings_ResetSettings()
//        {
//            var settings = new Settings
//                {
//                    ScrumTeam = "1",
//                    Sprint = "2",
//                    TfsProject = "3",
//                    BugBackGroundColor = Color.Black,
//                    DeliverableBackGroundColor = Color.Blue,
//                    TaskBackGroundColor = Color.Red
//                };

//            var expectedSettings = new Settings
//                {
//                    ScrumTeam = "",
//                    Sprint = "",
//                    TfsProject = "",
//                    BugBackGroundColor = Color.White,
//                    DeliverableBackGroundColor = Color.White,
//                    TaskBackGroundColor = Color.White
//                };

//            Assert.AreNotEqual(expectedSettings, settings);
//            settings.ResetSettings();
//            Assert.AreEqual(expectedSettings, settings);
//        }

//        [TestMethod]
//        public void Settings_LoadSettings()
//        {
//            var settings = new Settings
//                {
//                    ScrumTeam = "1",
//                    Sprint = "2",
//                    TfsProject = "3",
//                    BugBackGroundColor = Color.Black,
//                    DeliverableBackGroundColor = Color.Blue,
//                    TaskBackGroundColor = Color.Red
//                };

//            var memoryStream = new MemoryStream();
//            var formatter = new BinaryFormatter();
//            formatter.Serialize(memoryStream, settings);
//            memoryStream.Flush();
//            memoryStream.Position = 0;

//            var store = new Mock<IIsolatedStorageFile>();
//            var stream = new Mock<IIsolatedStorageFileStream>();

//            stream.Setup(s => s.BaseStream).Returns(memoryStream);
//            store.Setup(s => s.CreateStream(It.IsAny<string>(), FileMode.Open)).Returns(stream.Object);
//            store.Setup(s => s.FileExists(It.IsAny<string>())).Returns(true);

//            var settings2 = new Settings();
//            settings2.LoadSettings(store.Object);

//            Assert.AreEqual(settings, settings2);
//        }

//        [TestMethod]
//        public void Settings_LoadSettings_FileDoesNotExist_ExpectedLoadDefautlSettings()
//        {
//            var store = new Mock<IIsolatedStorageFile>();
//            store.Setup(s => s.CreateStream(It.IsAny<string>(), FileMode.Open)).Throws(new ArgumentNullException());
//            store.Setup(s => s.FileExists(It.IsAny<string>())).Returns(false);

//            var actual = new Settings();
//            actual.LoadSettings(store.Object);

//            Assert.AreEqual(new Settings(), actual);
//        }

//        [TestMethod]
//        public void Settings_LoadSettings_NoPossibleToLoadFile_ExpectedLoadDefautlSettings()
//        {
//            var store = new Mock<IIsolatedStorageFile>();
//            var stream = new Mock<IIsolatedStorageFileStream>();

//            stream.Setup(s => s.BaseStream).Throws(new ArgumentException());
//            store.Setup(s => s.CreateStream(It.IsAny<string>(), FileMode.Open)).Returns(stream.Object);
//            store.Setup(s => s.FileExists(It.IsAny<string>())).Returns(true);
//            store.Setup(s => s.DeleteFile(It.IsAny<string>()));

//            var actual = new Settings();
//            actual.LoadSettings(store.Object);

//            Assert.AreEqual(new Settings(), actual);
//        }

//        [TestMethod]
//        public void Settings_SaveSettings()
//        {
//            var expedtSettings = new Settings
//                {
//                    ScrumTeam = "1",
//                    Sprint = "2",
//                    TfsProject = "3",
//                    BugBackGroundColor = Color.Black,
//                    DeliverableBackGroundColor = Color.Blue,
//                    TaskBackGroundColor = Color.Red
//                };

//            var memoryStream = new MemoryStream();
//            var store = new Mock<IIsolatedStorageFile>();
//            var stream = new Mock<IIsolatedStorageFileStream>();

//            stream.Setup(s => s.BaseStream).Returns(memoryStream);
//            store.Setup(s => s.CreateStream(It.IsAny<string>(), FileMode.Create)).Returns(stream.Object);
//            expedtSettings.SaveSettings(store.Object);

//            var formatter = new BinaryFormatter();
//            memoryStream.Flush();
//            memoryStream.Position = 0;
//            Assert.AreEqual(expedtSettings, (Settings) formatter.Deserialize(memoryStream));
//        }
//    }
//}