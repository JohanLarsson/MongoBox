using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Wrappers;
using NUnit.Framework;

namespace MongoBox
{
    public class MongoConnectionTests
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

    }
}
