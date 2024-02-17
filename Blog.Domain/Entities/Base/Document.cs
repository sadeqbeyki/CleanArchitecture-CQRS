using MongoDB.Bson;

namespace Blog.Domain.Entities.Base;

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }

    public DateTime CreatedAt => Id.CreationTime;
}