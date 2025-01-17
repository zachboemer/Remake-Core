﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubterfugeCore.Core.Entities.Positions;
using SubterfugeCore.Core.Timing;
using System.Collections.Generic;
using SubterfugeCore.Core;
using SubterfugeCore.Core.Components;
using SubterfugeCore.Core.Entities;
using SubterfugeCore.Core.GameEvents;
using SubterfugeCore.Core.GameEvents.NaturalGameEvents.combat;
using SubterfugeCore.Core.Players;
using SubterfugeCore.Core.Topologies;

namespace SubterfugeCoreTest
{
    [TestClass]
    public class TimeMachineTest
    {
        TestUtils testUtils = new TestUtils();
        Player player = new Player("1");
        private Game _game;
        
        [TestInitialize]
        public void Setup()
        {
            _game = new Game(testUtils.GetDefaultGameConfiguration(new List<Player> { player }));

        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(_game.TimeMachine);
            Assert.IsNotNull(_game.TimeMachine.GetState());
        }

        [TestMethod]
        public void AddEvent()
        {
            Player player1 = new Player("1");
            Rft map = new Rft(3000, 3000);
            Outpost outpost = new Generator("0", new RftVector(map, 0, 0), player1);
            outpost.GetComponent<DrillerCarrier>().AddDrillers(10);
            Sub sub = new Sub("1", outpost, outpost, new GameTick(), 10, player1);
            CombatEvent arriveEvent = new CombatEvent(sub, outpost, new GameTick());

            _game.TimeMachine.AddEvent(arriveEvent);
            Assert.AreEqual(2, _game.TimeMachine.GetQueuedEvents().Count);
            Assert.AreEqual(arriveEvent, _game.TimeMachine.GetQueuedEvents()[0]);
        }

        [TestMethod]
        public void CanAdvanceTime()
        {
            GameTick initialTick = _game.TimeMachine.GetCurrentTick();
            _game.TimeMachine.Advance(5);
            
            Assert.IsTrue(initialTick < _game.TimeMachine.GetCurrentTick());
            Assert.AreEqual(initialTick.Advance(5).GetTick(), _game.TimeMachine.GetCurrentTick().GetTick());
        }
        
        [TestMethod]
        public void CanRewindTime()
        {
            GameTick initialTick = _game.TimeMachine.GetCurrentTick();
            _game.TimeMachine.Advance(5);
            
            Assert.IsTrue(initialTick < _game.TimeMachine.GetCurrentTick());
            Assert.AreEqual(initialTick.Advance(5).GetTick(), _game.TimeMachine.GetCurrentTick().GetTick());

            GameTick advancedTick = _game.TimeMachine.GetCurrentTick();
            
            _game.TimeMachine.Rewind(1);
            
            
            Assert.IsTrue(advancedTick > _game.TimeMachine.GetCurrentTick());
            Assert.AreEqual(advancedTick.Rewind(1).GetTick(), _game.TimeMachine.GetCurrentTick().GetTick());
        }
        
        [TestMethod]
        public void EventsSwitchQueuesWhenPassedForward()
        {
            Player player1 = new Player("1");
            Rft map = new Rft(3000, 3000);
            Outpost outpost = new Generator("0", new RftVector(map, 0, 0), player1);
            outpost.GetComponent<DrillerCarrier>().AddDrillers(10);
            Sub sub = new Sub("1", outpost, outpost, new GameTick(), 10, player1);
            CombatEvent arriveEvent = new CombatEvent(sub, outpost, new GameTick(5));

            _game.TimeMachine.AddEvent(arriveEvent);
            Assert.AreEqual(2, _game.TimeMachine.GetQueuedEvents().Count);
            Assert.AreEqual(arriveEvent, _game.TimeMachine.GetQueuedEvents()[0]);
            
            // Go past the tick
            _game.TimeMachine.Advance(6);
            Assert.AreEqual(1, _game.TimeMachine.GetQueuedEvents().Count);
        }

        [TestMethod]
        public void EventsSwitchQueuesWhenRewind()
        {
            Player player1 = new Player("1");
            Rft map = new Rft(3000, 3000);
            Outpost outpost = new Generator("0", new RftVector(map, 0, 0), player1);
            outpost.GetComponent<DrillerCarrier>().AddDrillers(10);
            Sub sub = new Sub("1", outpost, outpost, new GameTick(), 10, player1);
            CombatEvent arriveEvent = new CombatEvent(sub, outpost, new GameTick(5));

            _game.TimeMachine.AddEvent(arriveEvent);
            Assert.AreEqual(2, _game.TimeMachine.GetQueuedEvents().Count);
            Assert.AreEqual(arriveEvent, _game.TimeMachine.GetQueuedEvents()[0]);
            
            // Go past the tick
            _game.TimeMachine.Advance(6);
            Assert.AreEqual(1, _game.TimeMachine.GetQueuedEvents().Count);
            
            // Rewind back
            _game.TimeMachine.Rewind(6);
            Assert.AreEqual(2, _game.TimeMachine.GetQueuedEvents().Count);
            Assert.AreEqual(arriveEvent, _game.TimeMachine.GetQueuedEvents()[0]);
        }
        
        [TestMethod]
        public void CanRemoveEvents()
        {
            Player player1 = new Player("1");
            Rft map = new Rft(3000, 3000);
            Outpost outpost = new Generator("0", new RftVector(map, 0, 0), player1);
            outpost.GetComponent<DrillerCarrier>().AddDrillers(10);
            Sub sub = new Sub("1", outpost, outpost, new GameTick(), 10, player1);
            CombatEvent arriveEvent = new CombatEvent(sub, outpost, new GameTick(5));

            _game.TimeMachine.AddEvent(arriveEvent);
            Assert.AreEqual(2, _game.TimeMachine.GetQueuedEvents().Count);
            Assert.AreEqual(arriveEvent, _game.TimeMachine.GetQueuedEvents()[0]);
            
            _game.TimeMachine.RemoveEvent(arriveEvent);
            Assert.AreEqual(1, _game.TimeMachine.GetQueuedEvents().Count);
        }
        
        [TestMethod]
        public void CanGoToAGameTick()
        {

            GameTick initialTick = _game.TimeMachine.GetCurrentTick();
            _game.TimeMachine.GoTo(new GameTick(5));
            
            Assert.IsTrue(initialTick < _game.TimeMachine.GetCurrentTick());
            Assert.AreEqual(initialTick.Advance(5).GetTick(), _game.TimeMachine.GetCurrentTick().GetTick());
        }
        
        [TestMethod]
        public void CanGoToAnEvent()
        {
            Player player1 = new Player("1");
            Rft map = new Rft(3000, 3000);
            Outpost outpost = new Generator("0", new RftVector(map, 0, 0), player1);
            outpost.GetComponent<DrillerCarrier>().AddDrillers(10);
            Sub sub = new Sub("1", outpost, outpost, new GameTick(), 10, player1);
            CombatEvent arriveEvent = new CombatEvent(sub, outpost, new GameTick(5));

            _game.TimeMachine.AddEvent(arriveEvent);
            Assert.AreEqual(2, _game.TimeMachine.GetQueuedEvents().Count);
            Assert.AreEqual(arriveEvent, _game.TimeMachine.GetQueuedEvents()[0]);
            
            _game.TimeMachine.GoTo(arriveEvent);
            Assert.AreEqual(arriveEvent.GetOccursAt().GetTick(), _game.TimeMachine.GetCurrentTick().GetTick());
            Assert.AreEqual(1, _game.TimeMachine.GetQueuedEvents().Count);
        }

    }
}
