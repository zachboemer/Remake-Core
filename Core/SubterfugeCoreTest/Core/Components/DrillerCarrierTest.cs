﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SubterfugeCore.Core.Components;
using SubterfugeCore.Core.Entities;
using SubterfugeCore.Core.Players;

namespace SubterfugeCoreTest.Core.Components
{
    [TestClass]
    public class DrillerCarrierTest
    {
        private Mock<IEntity> _mockEntity;

        private void MockDrillerCarrierEntity(
            int initialDrillers,
            Player owner
            )
        {
            _mockEntity = new Mock<IEntity>();
            _mockEntity.Setup(it => it.GetComponent<DrillerCarrier>())
                .Returns(new DrillerCarrier(_mockEntity.Object, initialDrillers, owner));
        }

        [TestMethod]
        public void CanInitializeDrillerCarrier()
        {
            MockDrillerCarrierEntity(0, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
        }
        
        [TestMethod]
        public void DrillerCarrierSetsInitialDrillers()
        {
            var initialDrillers = 10;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(initialDrillers, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
            
            initialDrillers = 20;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(initialDrillers, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
        }
        
        [TestMethod]
        public void DrillerCarrierSetsInitialOwner()
        {
            var initialOwner = new Player("1");
            MockDrillerCarrierEntity(0, initialOwner);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(initialOwner, _mockEntity.Object.GetComponent<DrillerCarrier>().GetOwner());
            
            initialOwner = new Player("2");
            MockDrillerCarrierEntity(0, initialOwner);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(initialOwner, _mockEntity.Object.GetComponent<DrillerCarrier>().GetOwner());
        }
        
        [TestMethod]
        public void CanAddDrillersToDrillerCarrier()
        {
            var initialDrillers = 0;
            var drillersToAdd = 100;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            _mockEntity.Object.GetComponent<DrillerCarrier>().AddDrillers(drillersToAdd);
            Assert.AreEqual(initialDrillers + drillersToAdd,
                _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
        }
        
        [TestMethod]
        public void CanRemoveDrillersFromDrillerCarrier()
        {
            var initialDrillers = 100;
            var drillersToRemove = 40;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            _mockEntity.Object.GetComponent<DrillerCarrier>().RemoveDrillers(drillersToRemove);
            Assert.AreEqual(initialDrillers - drillersToRemove, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
            Assert.AreEqual(false, _mockEntity.Object.GetComponent<DrillerCarrier>().IsCaptured());
        }
        
        [TestMethod]
        public void RemovingMoreThanTheTotalDrillerCountCapturesTheCarrier()
        {
            var initialDrillers = 50;
            var drillersToRemove = 100;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            _mockEntity.Object.GetComponent<DrillerCarrier>().RemoveDrillers(drillersToRemove);
            Assert.AreEqual(0, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
            Assert.AreEqual(true, _mockEntity.Object.GetComponent<DrillerCarrier>().IsCaptured());
        }
        
        [TestMethod]
        public void CanSetDrillerCountOfDrillerCarrier()
        {
            var initialDrillers = 100;
            var newDrillers = 40;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            _mockEntity.Object.GetComponent<DrillerCarrier>().SetDrillerCount(newDrillers);
            Assert.AreEqual(newDrillers, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
        }
        
        [TestMethod]
        public void DrillerCarrierHasDrillers()
        {
            var initialDrillers = 100;
            MockDrillerCarrierEntity(initialDrillers, new Player("1"));
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.IsTrue(_mockEntity.Object.GetComponent<DrillerCarrier>().HasDrillers(75));
            Assert.IsTrue(_mockEntity.Object.GetComponent<DrillerCarrier>().HasDrillers(100));
            Assert.IsFalse(_mockEntity.Object.GetComponent<DrillerCarrier>().HasDrillers(101));
            Assert.IsFalse(_mockEntity.Object.GetComponent<DrillerCarrier>().HasDrillers(600));
        }
        
        [TestMethod]
        public void CanSetNewOwnerAndDrillerCountOfCarrier()
        {
            var initialDrillers = 100;
            var initialPlayer = new Player("1");
            MockDrillerCarrierEntity(initialDrillers, initialPlayer);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(initialPlayer, _mockEntity.Object.GetComponent<DrillerCarrier>().GetOwner());
            Assert.AreEqual(initialDrillers, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());

            var newDrillerCount = 20;
            var newOwner = new Player("2");
            _mockEntity.Object.GetComponent<DrillerCarrier>().SetNewOwner(newOwner, newDrillerCount);
            Assert.AreEqual(newOwner, _mockEntity.Object.GetComponent<DrillerCarrier>().GetOwner());
            Assert.AreEqual(newDrillerCount, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
        }
        
        [TestMethod]
        public void CanSetOwner()
        {
            var initialDrillers = 100;
            var initialPlayer = new Player("1");
            MockDrillerCarrierEntity(initialDrillers, initialPlayer);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(initialPlayer, _mockEntity.Object.GetComponent<DrillerCarrier>().GetOwner());

            var newOwner = new Player("2");
            _mockEntity.Object.GetComponent<DrillerCarrier>().SetOwner(newOwner);
            Assert.AreEqual(newOwner, _mockEntity.Object.GetComponent<DrillerCarrier>().GetOwner());
        }
        
        [TestMethod]
        public void CanSetCaptured()
        {
            var initialDrillers = 100;
            var initialPlayer = new Player("1");
            MockDrillerCarrierEntity(initialDrillers, initialPlayer);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(false, _mockEntity.Object.GetComponent<DrillerCarrier>().IsCaptured());

            _mockEntity.Object.GetComponent<DrillerCarrier>().SetCaptured(true);
            Assert.AreEqual(true, _mockEntity.Object.GetComponent<DrillerCarrier>().IsCaptured());
            
            _mockEntity.Object.GetComponent<DrillerCarrier>().SetCaptured(false);
            Assert.AreEqual(false, _mockEntity.Object.GetComponent<DrillerCarrier>().IsCaptured());
        }
        
        [TestMethod]
        public void CanDestroy()
        {
            var initialDrillers = 100;
            var initialPlayer = new Player("1");
            MockDrillerCarrierEntity(initialDrillers, initialPlayer);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(false, _mockEntity.Object.GetComponent<DrillerCarrier>().IsDestroyed());
            Assert.AreEqual(initialDrillers, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());

            _mockEntity.Object.GetComponent<DrillerCarrier>().Destroy();
            Assert.AreEqual(true, _mockEntity.Object.GetComponent<DrillerCarrier>().IsDestroyed());
            Assert.AreEqual(0, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
        }
        
        [TestMethod]
        public void CanAddDrillersAfterBeingDestroyed()
        {
            var initialDrillers = 100;
            var initialPlayer = new Player("1");
            MockDrillerCarrierEntity(initialDrillers, initialPlayer);
            Assert.IsNotNull(_mockEntity.Object.GetComponent<DrillerCarrier>());
            Assert.AreEqual(false, _mockEntity.Object.GetComponent<DrillerCarrier>().IsDestroyed());
            Assert.AreEqual(initialDrillers, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());

            _mockEntity.Object.GetComponent<DrillerCarrier>().Destroy();
            Assert.AreEqual(true, _mockEntity.Object.GetComponent<DrillerCarrier>().IsDestroyed());
            Assert.AreEqual(0, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
            
            _mockEntity.Object.GetComponent<DrillerCarrier>().AddDrillers(50);
            Assert.AreEqual(true, _mockEntity.Object.GetComponent<DrillerCarrier>().IsDestroyed());
            Assert.AreEqual(50, _mockEntity.Object.GetComponent<DrillerCarrier>().GetDrillerCount());
        }

    }
}