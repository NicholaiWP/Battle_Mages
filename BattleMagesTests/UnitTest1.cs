using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleMages;

namespace BattleMagesTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// A new Test Method needs to be created for each test case
        /// to better indicate which error fails of passes
        /// </summary>
        [TestMethod]
        //anticipation: passed test as there's already added waves.
        public void TestWaveNumberIsNotNull()
        {
            WaveController waveController = new WaveController(new System.Collections.Generic.List<Wave>());

            Assert.IsNotNull(waveController.WaveNumber);
        }

        [TestMethod]
        //anticipated failed test, cant be null if waves exists.
        public void TestWaveNumberIsNull()
        {
            WaveController waveController = new WaveController(new System.Collections.Generic.List<Wave>());

            Assert.IsNull(waveController.WaveNumber);
        }

    }

    [TestClass]
    public class UnitTest2
    {
        //Testing movement properties
        [TestMethod]
        public void TestWhetherMoveSpeedIsNotNull()
        {
            Character character = new Character();
            Assert.IsNotNull(character.MoveSpeed);
        }

        [TestMethod]
        public void TestWhetherMoveAccelerationIsNotNull()
        {
            Character character = new Character();
            Assert.IsNotNull(character.MoveAccel);
        }

        [TestMethod]
        public void TestWhetherFacingDirectionIsNotNull()
        {
            Character character = new Character();
            Assert.IsNotNull(character.FDirection);
        }
        //all proterties returns a passed test, which indicates that
        //if a Null test was run they'd all return "Failed".

        [TestMethod]
        public void Tes2()
        {
            Character character = new Character();
        }
    }
}
