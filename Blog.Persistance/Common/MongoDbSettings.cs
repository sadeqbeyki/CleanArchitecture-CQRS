using Blog.Application.Interfaces;

namespace Blog.Persistance.Common;

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }
    //public string CollectionName { get; set; }
}
