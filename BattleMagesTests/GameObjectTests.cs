using System;
using BattleMages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace BattleMagesTests
{
    internal class DummyComponent : Component
    {
        public bool PreInitialized { get; private set; }
        public bool Initialized { get; private set; }
        public int UpdateCalls { get; private set; }

        public DummyComponent()
        {
            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void PreInitialize(PreInitializeMsg msg)
        {
            PreInitialized = true;
        }

        private void Initialize(InitializeMsg msg)
        {
            Initialized = true;
        }

        private void Update(UpdateMsg msg)
        {
            UpdateCalls++;
        }
    }

    [TestClass]
    public class GameObjectTests
    {
        private readonly Vector2 testPos = Vector2.Zero;

        [TestMethod]
        public void CallConstructor()
        {
            //The constructor itself only takes a Vector2 and has no other execution paths. Nothing can really go wrong here
            GameObject obj = new GameObject(testPos);

            //However, it should create a Transform component and assign it to the property of same name.
            Assert.IsNotNull(obj.Transform);

            //The newly created Transform's position should be the one given in the constructor.
            Vector2 expectedPos = testPos;
            Vector2 actualPos = obj.Transform.Position;
            Assert.AreEqual(expectedPos, actualPos);

            //After processing components, the Transform should also be available on the component list.
            Assert.IsNull(obj.GetComponent<Transform>());
            obj.ProcessComponents();
            Assert.IsNotNull(obj.GetComponent<Transform>());
        }

        [TestMethod]
        public void AddSingleComponent()
        {
            GameObject obj = new GameObject(testPos);
            DummyComponent comp = new DummyComponent();
            obj.AddComponent(comp);

            //When a component is added, its GameObject property should be set to the object it is added to
            Assert.AreEqual(comp.GameObject, obj);

            //The component should also have been pre-initialized by this
            Assert.IsTrue(comp.PreInitialized);
            //But not initialzed
            Assert.IsFalse(comp.Initialized);

            //After processing components, the component should also be available on the component list.
            Assert.IsNull(obj.GetComponent<DummyComponent>());
            obj.ProcessComponents();
            Assert.IsNotNull(obj.GetComponent<DummyComponent>());

            //It should also be initialized now
            Assert.IsTrue(comp.Initialized);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDupeComponent()
        {
            //This test checks to make sure that adding the same component to an object twice is not possible
            GameObject obj = new GameObject(testPos);

            //First component should be added fine
            try
            {
                obj.AddComponent(new DummyComponent());
            }
            catch (InvalidOperationException)
            {
                Assert.Fail();
            }

            //An InvalidOperationException should be thrown here
            obj.AddComponent(new DummyComponent());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDupeComponentInDifferentFrame()
        {
            //Same as previous test but with ProcessComponents() to simulate adding components in different frames
            GameObject obj = new GameObject(testPos);

            //First component should be added fine (Checked by previous test)
            obj.AddComponent(new DummyComponent());

            //Process components - new component is now on components list instead of componentsToAdd list
            obj.ProcessComponents();

            //An InvalidOperationException should be thrown here
            obj.AddComponent(new DummyComponent());
        }

        [TestMethod]
        public void RemoveComponent()
        {
            GameObject obj = new GameObject(testPos);

            //Component to test removal of
            DummyComponent comp = new DummyComponent();

            //Add the component first
            obj.AddComponent(comp);

            //Process components to get it on the object list
            obj.ProcessComponents();

            //Now try to remove it
            obj.RemoveComponent<DummyComponent>();

            //The component should stay on the list until ProcessComponents is called
            Assert.IsNotNull(obj.GetComponent<DummyComponent>());
            obj.ProcessComponents();
            Assert.IsNull(obj.GetComponent<DummyComponent>());
        }

        [TestMethod]
        public void AddRemoveComponentInSameFrame()
        {
            //When a component is added and then removed during the same frame, it should be removed
            GameObject obj = new GameObject(testPos);

            obj.AddComponent(new DummyComponent());
            obj.RemoveComponent<DummyComponent>();

            Assert.IsNull(obj.GetComponent<DummyComponent>());
        }

        [TestMethod]
        public void RemoveMissingComponent()
        {
            GameObject obj = new GameObject(testPos);

            //Removing a nonexistant component should not cause any problems.
            obj.RemoveComponent<DummyComponent>();

            obj.ProcessComponents();
        }

        [TestMethod]
        public void SendUpdateMessage()
        {
            //All messages are sent the same way so this should validate that all message types work correctly.
            GameObject obj = new GameObject(testPos);

            DummyComponent comp = new DummyComponent();
            obj.AddComponent(comp);

            //The dummy component is now added, but not processed, ie put on the component list
            //Send a message and make sure it didn't arrive
            obj.SendMessage(new UpdateMsg());

            int expected = 0;
            int actual = comp.UpdateCalls;
            Assert.AreEqual(expected, actual);

            //Now, process components and try again
            obj.ProcessComponents();

            obj.SendMessage(new UpdateMsg());

            //Check if the message was properly forwarded to the dummy component
            int expected2 = 1;
            int actual2 = comp.UpdateCalls;
            Assert.AreEqual(expected2, actual2);
        }
    }
}