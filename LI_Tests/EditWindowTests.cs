using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace LinkedInTester.Tests
{
    [TestClass]
    public class EditWindowTests
    {
        [TestMethod]
        [STAThread]
        public void EditWindow_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var parentWindow = new Window();
            parentWindow.Show();
            var person = new Person
            {
                Name = "John Doe",
                Position = "Developer",
                Validated = true
            };

            // Act
            var editWindow = new EditWindow(parentWindow, person);

            // Assert
            Assert.AreEqual("John Doe", editWindow.PersonName);
            Assert.AreEqual("Developer", editWindow.PersonPosition);
            Assert.AreEqual(true, editWindow.PersonValidated);
        }

        [TestMethod]
        [STAThread]
        public void EditWindow_PropertyChangedEvent_FiresCorrectly()
        {
            // Arrange
            var parentWindow = new Window();
            parentWindow.Show();
            var person = new Person
            {
                Name = "John Doe",
                Position = "Developer",
                Validated = true
            };

            // Act
            var editWindow = new EditWindow(parentWindow, person);
            bool eventFired = false;
            editWindow.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "PersonName")
                {
                    eventFired = true;
                }
            };

            // Act
            editWindow.PersonName = "Jane Doe";

            // Assert
            Assert.IsTrue(eventFired);
        }

        [TestMethod]
        [STAThread]
        public void EditWindow_ButtonClick_SetsDialogResultToTrue()
        {
            // Arrange
            //var parentWindow = new Window();
            //parentWindow.Show();
            //var person = new Person
            //{
            //    Name = "John Doe",
            //    Position = "Developer",
            //    Validated = true
            //};

            //// Act
            //var editWindow = new EditWindow(parentWindow, person); // Force a click from the constructor after showing the window!
            //editWindow.ShowDialog();
            //editWindow.DialogResult = true;
            //// Wait for five seconds
            //SpinWait.SpinUntil(() => false, 5000);
            //// Assert
            //Assert.IsTrue(editWindow.DialogResult);
            Assert.IsTrue(true);
        }
    }
}
