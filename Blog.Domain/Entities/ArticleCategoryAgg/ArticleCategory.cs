using Blog.Domain.Attributes;
using Blog.Domain.Entities.ArticleAgg;
using Blog.Domain.Entities.Base;

namespace Blog.Domain.Entities.ArticleCategoryAgg;
[BsonCollection("articlecategories")]

public class ArticleCategory : Document
{
    public string Name { get; private set; }
    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }
    public string PictureTitle { get; private set; }
    public string Description { get; private set; }
    public int ShowOrder { get; private set; }
    public string Slug { get; private set; }
    public string Keywords { get; private set; }
    public string MetaDescription { get; private set; }
    public string CanonicalAddress { get; private set; }
    public List<Article> Articles { get; private set; }

    public ArticleCategory(string name, string picture, string pictureAlt, string pictureTitle,
        string description, int showOrder, string slug, string keywords, string metaDescription,
        string canonicalAddress)
    {
        Name = name;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Description = description;
        ShowOrder = showOrder;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
        CanonicalAddress = canonicalAddress;

    }

    public void Edit(string name, string picture, string pictureAlt, string pictureTitle,
        string description, int showOrder, string slug, string keywords, string metaDescription,
        string canonicalAddress)
    {
        Name = name;
        if (!string.IsNullOrWhiteSpace(picture))
        {
            Picture = picture;
        }
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Description = description;
        ShowOrder = showOrder;
        Slug = slug;
        Keywords = keywords;
        MetaDescription = metaDescription;
        CanonicalAddress = canonicalAddress;
    }
}
