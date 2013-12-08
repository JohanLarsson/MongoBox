using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;

namespace MongoBox
{
    class MongoUsageTests
    {
        private const string DatabaseName = "test";
        private const string CollectionName = "test";

        public MongoServer Server
        {
            get
            {
                var client = new MongoClient();
                MongoServer server = client.GetServer();
                server.Connect();
                return server;
            }
        }

        public MongoDatabase MongoDatabase
        {
            get
            {
                return Server.GetDatabase(DatabaseName);
            }
        }

        public MongoCollection<BsonDocument> DocCollection
        {
            get
            {
                return MongoDatabase.GetCollection(CollectionName);
            }
        }

        [Test]
        public void CreateCollectionTest()
        {
            MongoCollection<BsonDocument> collection = MongoDatabase.GetCollection(CollectionName);
        }

        [Test]
        public void DropCollectionTest()
        {
            Assert.IsTrue(MongoDatabase.CollectionExists(CollectionName));
            DocCollection.Drop();
            Assert.IsFalse(MongoDatabase.CollectionExists(CollectionName));
        }

        [Test]
        public void InsertTest()
        {
            long before = DocCollection.Count();
            var data = new TestClass { Value = 5 };
            DocCollection.Insert(data.ToBsonDocument());
            Assert.AreEqual(before+1,DocCollection.Count());
        }

        [Test]
        public void FindTest()
        {
            MongoCursor<BsonDocument> mongoCursor = DocCollection.Find(null);
            foreach (var doc in mongoCursor)
            {
                Console.Write(doc.ToJson());
            }
        }

        [Test]
        public void FindTypedTest()
        {
            MongoCursor<TestClass> find = DocCollection.FindAs<TestClass>(null);
            foreach (var testClass in find)
            {
                Console.WriteLine(testClass.Value);
            }
        }

        [Test]
        public void UpsertTest()
        {
            TestClass find = DocCollection.FindOneAs<TestClass>(null);
            find.Value = -5;
            DocCollection.Save(find.ToBsonDocument());
            Console.WriteLine(DocCollection.FindOneAs<TestClass>(null).Value);
        }

        [Test]
        public void QueryTest()
        {
            DocCollection.Drop();
            var collection = MongoDatabase.GetCollection("test");
            for (int i = 0; i < 10; i++)
            {
                collection.Insert(new TestClass { Value = i });
            }
            IQueryable<TestClass> testClasses = collection.AsQueryable<TestClass>()
                                                           .Where(x => x.Value > 5);
            Assert.AreEqual(4, testClasses.Count());
        }
    }
}
