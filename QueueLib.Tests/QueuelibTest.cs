using System;
using NUnit.Framework;
using QueueLib.Tests.Helper;

namespace QueueLib.Tests
{
    [TestFixture]
    public class QueuelibTest
    {
        [Test]
        public void ConstructorWithoutparametersTest_CreateInstanceAddElement_CheckDequeueElement()
        {
            Queue<int> actualQueue = new Queue<int>();
            actualQueue.Enqueue(1);
            Assert.AreEqual(1, actualQueue.Dequeue());
        }

        [TestCase(new int[] { 5, 4, 3, 2, 1 })]
        [TestCase(new int[] { -18, 15 })]
        [TestCase(new int[] { -500 })]
        [TestCase(new int[] { 13, 15, 25, 26, -36, -38, -39, -48, 56, 1050 })]
        public void CtorWhichTakesICollectionTest_InputArrayDifferentLength(int[] sourceArray)
        {
            Queue<int> actualQueue = new Queue<int>(sourceArray);
            for (int i = 0; i < sourceArray.Length; i++)
            {
                Assert.AreEqual(sourceArray[i], actualQueue.Dequeue());
            }
        }

        [TestCase(new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { -5, 15 })]
        [TestCase(new int[] { 0 })]
        [TestCase(new int[] { 17, 11 })]
        public void EnqueueTest_DifferentValues_AssertInputedValues(int[] sourceArray)
        {
            Queue<int> actualQueue = new Queue<int>();
            for (int i = 0; i < sourceArray.Length; i++)
            {
                actualQueue.Enqueue(sourceArray[i]);
            }

            CollectionAssert.AreEqual(sourceArray, actualQueue);
        }

        [TestCase(new int[] { 1, 2, 3, 4 })]

        public void CopyToTest_DifferentValues_AssertInputedValues(int[] sourceArray)
        {
            Queue<int> actualQueue = new Queue<int>(sourceArray);
            actualQueue.Enqueue(5);
            actualQueue.Dequeue();
            actualQueue.Dequeue();
            int[] actualArray = new int[4];
            actualQueue.CopyTo(actualArray, 1);
            int[] expectedlArray = new int[] { 0, 3, 4, 5 };
            CollectionAssert.AreEqual(expectedlArray, actualArray);
        }

        [TestCase(new int[] { -1 }, ExpectedResult = -1)]
        [TestCase(new int[] { 12, 15 }, ExpectedResult = 12)]
        [TestCase(new int[] { 154, 18, 295 }, ExpectedResult = 154)]
        [TestCase(new int[] { 13, 17, 18, 19, 20, 21, 23, 11 }, ExpectedResult = 13)]
        public int PeekTest_DifferentValues_AssertInputedValues(int[] sourceArray)
            => new Queue<int>(sourceArray).Peek();

        [TestCase(new int[] { -1, 5, 8 })]
        public void EnumeratorTest_ModifyCollectionWhenIterated_InValidOperationException(int[] sourceArray)
        {
            Queue<int> actualQueue = new Queue<int>(sourceArray);
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in actualQueue)
                {
                    actualQueue.Enqueue(0);
                }
            });
        }

        [TestCase(-100)]
        [TestCase(-5)]
        public void ConstructorTest_InputIncorrectData_ArgumentException(int number)
            => Assert.Throws<ArgumentException>(() => new Queue<int>(number));

        [Test]
        public void ConstructorTest_InputIncorrectData_ArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new Queue<string>().Enqueue(null));

        [Test]
        public void PersonTest_TwoPersonWithDifferentReferences_CompareReferenceEquality()
        {
            Person person = new Person("Ivanov", "Ivan");
            Person secondPerson = new Person("Ivanov", "Ivan");
            Person thirdPerson = new Person("Ivanov", "Ivan");

            Queue<Person> personQueue = new Queue<Person>();
            personQueue.Enqueue(new Person("Ivanov", "Ivan"));
            personQueue.Enqueue(new Person("Ivanov", "Ivan"));
            personQueue.Enqueue(thirdPerson);
            Assert.IsFalse(personQueue.Contains(person));
            Assert.IsFalse(personQueue.Contains(secondPerson));
            Assert.IsTrue(personQueue.Contains(thirdPerson));
        }

        [Test]
        public void PersonIEquatableTest_TwoPersonWithDifferentReferences_CompareReferenceEquality()
        {
            PersonIEquatable person = new PersonIEquatable("Ivanov", "Ivan");
            PersonIEquatable secondPerson = new PersonIEquatable("Ivanov", "Ivan");
            PersonIEquatable thirdPerson = new PersonIEquatable("Ivanov", "Ivan");

            Queue<PersonIEquatable> personQueue = new Queue<PersonIEquatable>();
            personQueue.Enqueue(new PersonIEquatable("Ivanov", "Ivan"));
            personQueue.Enqueue(new PersonIEquatable("Ivanov", "Ivan"));
            personQueue.Enqueue(thirdPerson);
            Assert.IsTrue(personQueue.Contains(person));
            Assert.IsTrue(personQueue.Contains(secondPerson));
            Assert.IsTrue(personQueue.Contains(thirdPerson));

            Assert.IsTrue(person.Equals(secondPerson));
            Assert.IsTrue(person.Equals(personQueue.Peek()));
        }

        [Test]
        public void PersonOverridedEqualsTest_TwoPersonWithDifferentReferences_CompareReferenceEquality()
        {
            PersonIEquatable person = new PersonIEquatable("Ivanov", "Ivan");
            PersonIEquatable secondPerson = new PersonIEquatable("Ivanov", "Ivan");
            PersonIEquatable thirdPerson = new PersonIEquatable("Ivanov", "Ivan");

            Queue<PersonIEquatable> personQueue = new Queue<PersonIEquatable>();
            personQueue.Enqueue(new PersonIEquatable("Ivanov", "Ivan"));
            personQueue.Enqueue(new PersonIEquatable("Ivanov", "Ivan"));
            personQueue.Enqueue(thirdPerson);
            Assert.IsTrue(personQueue.Contains(person));
            Assert.IsTrue(personQueue.Contains(secondPerson));
            Assert.IsTrue(personQueue.Contains(thirdPerson));

            Assert.IsTrue(person.Equals(secondPerson));
            Assert.IsTrue(person.Equals(personQueue.Peek()));
        }
    }
}