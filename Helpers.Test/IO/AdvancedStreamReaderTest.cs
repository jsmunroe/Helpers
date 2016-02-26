using System;
using System.IO;
using System.Text;
using Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.IO
{
    [TestClass]
    public class AdvancedStreamReaderTest
    {
        private static readonly string NL = Environment.NewLine;

        [TestMethod]
        public void Read()
        {
            // Setup
            var bytes = Encoding.UTF8.GetBytes($"This is a test,{NL}a test,{NL}a test!{NL}Doo dee doo.");
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream);
            reader.SeekCharacter(15);

            // Execute
            var result = (char)reader.Read();

            // Assert
            Assert.AreEqual(Environment.NewLine[0], result);
            Assert.AreEqual(16, reader.CharacterPosition);
        }


        [TestMethod]
        public void ReadToBuffer()
        {
            // Setup
            var bytes = Encoding.UTF8.GetBytes($"This is a test,{NL}a test,{NL}a test!{NL}Doo dee doo.");
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream);
            reader.SeekCharacter(15);

            // Execute
            var buffer = new char[15];
            var result = reader.Read(buffer, 0, 15);

            // Assert
            Assert.AreEqual($"{NL}a test,{NL}a te", new string(buffer));
            Assert.AreEqual(15, result);
            Assert.AreEqual(30, reader.CharacterPosition);
        }


        [TestMethod]
        public void ReadLine()
        {
            // Setup
            var bytes = Encoding.UTF8.GetBytes($"This is a test,{NL}a test,{NL}a test!{NL}Doo dee doo.");
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream);

            // Execute
            var result = reader.ReadLine();

            // Assert
            Assert.AreEqual("This is a test,", result);
            Assert.AreEqual(17, reader.CharacterPosition);
        }


        [TestMethod]
        public void ReadLineWithUtf8()
        {
            // Setup
            var text = $"ƒun ‼Æ¢ with åò☺ encoding!{NL}ƒun ‼Æ¢ with åò☺ encoding!{NL}ƒun ‼Æ¢ with åò☺ encoding!{NL}Ha!";
            var bytes = Encoding.UTF8.GetBytes(text);
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream);

            // Execute
            var result = reader.ReadLine();

            // Assert
            Assert.AreEqual("ƒun ‼Æ¢ with åò☺ encoding!", result);
            Assert.AreEqual(28, reader.CharacterPosition);

        }

        [TestMethod]
        public void ReadLineWithNewLineOnly()
        {
            // Setup
            var text = $"ƒun ‼Æ¢ with åò☺ encoding!\nƒun ‼Æ¢ with åò☺ encoding!\nƒun ‼Æ¢ with åò☺ encoding!\nHa!";
            var bytes = Encoding.UTF8.GetBytes(text);
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream, NewLineType.Nl);
            reader.ReadLine();

            // Execute
            var result = reader.ReadLine();

            // Assert
            Assert.AreEqual("ƒun ‼Æ¢ with åò☺ encoding!", result);
            Assert.AreEqual(54, reader.CharacterPosition);
        }

        [TestMethod]
        public void SeekCharacter()
        {
            // Setup
            var bytes = Encoding.UTF8.GetBytes($"This is a test,{NL}a test,{NL}a test!{NL}Doo dee doo.");
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream);

            // Execute
            reader.SeekCharacter(17);

            // Assert
            Assert.AreEqual(17, reader.CharacterPosition);
            Assert.AreEqual($"a test,{NL}a test!{NL}Doo dee doo.", reader.ReadToEnd());
        }


        [TestMethod]
        public void SeekCharacterWithUtf8()
        {
            // Setup
            var text = $"ƒun ‼Æ¢ with åò☺ encoding!{NL}ƒun ‼Æ¢ with åò☺ encoding!{NL}ƒun ‼Æ¢ with åò☺ encoding!{NL}Ha!";
            var bytes = Encoding.UTF8.GetBytes(text);
            var stream = new MemoryStream(bytes);
            var reader = new AdvancedStreamReader(stream);

            // Pre-condition assert
            Assert.IsTrue(bytes.Length > text.Length); // More bytes than characters in sample text.

            // Execute
            reader.SeekCharacter(84);

            // Assert
            Assert.AreEqual(84, reader.CharacterPosition);
            Assert.AreEqual($"Ha!", reader.ReadToEnd());
        }
    }
}
