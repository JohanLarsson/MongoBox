using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoBox
{
    public class TestClass
    {
        [BsonId]
        public ObjectId _id;
        public int Value { get; set; }
    }
}