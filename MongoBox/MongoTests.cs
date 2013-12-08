using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Wrappers;
using NUnit.Framework;

namespace MongoBox
{
    public class MongoTests
    {
        [Test]
        public void ConnectTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
        }

        [Test]
        public void CreateDbTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
        }

        [Test]
        public void CreateCollectionTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
        }

        [Test]
        public void DropCollectionTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
            collection.Drop();
        }

        [Test]
        public void InsertTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
            var data = new TestClass { Value = 5 };
            collection.Insert(data.ToBsonDocument());
            Console.WriteLine(collection.Count());
        }

        [Test]
        public void FindTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
            MongoCursor<BsonDocument> mongoCursor = collection.Find(null);
            foreach (var doc in mongoCursor)
            {
                Console.Write(doc.ToJson());
            }

        }

        [Test]
        public void FindTypedTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
            MongoCursor<TestClass> find = collection.FindAs<TestClass>(null);
            foreach (var testClass in find)
            {
                Console.WriteLine(testClass.Value);
            }
        }

        [Test]
        public void UpsertTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
            TestClass find = collection.FindOneAs<TestClass>(null);
            find.Value = -5;
            collection.Save(find.ToBsonDocument());
            Console.WriteLine(collection.FindOneAs<TestClass>(null).Value);
        }

        [Test]
        public void QueryTest()
        {
            var client = new MongoClient();
            MongoServer server = client.GetServer();
            server.Connect();
            MongoDatabase database = server.GetDatabase("test");
            MongoCollection<BsonDocument> collection = database.GetCollection("test");
            collection.Drop();
            collection = database.GetCollection("test");
            for (int i = 0; i < 10; i++)
            {
                collection.Insert(new TestClass {Value = i});
            }
            IQueryable<TestClass> testClasses = collection.AsQueryable<TestClass>()
                                                           .Where(x => x.Value > 5);
            Assert.AreEqual(4,testClasses.Count());
        }
    }

    public class TestClass
    {
        [BsonId]
        public ObjectId _id;
        public int Value { get; set; }
    }
}
