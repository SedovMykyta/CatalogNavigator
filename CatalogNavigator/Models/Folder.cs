namespace CatalogNavigator.Models;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    public int? ParentId { get; set; }
    public virtual Folder? Parent { get; set; }
    
    public List<Folder> Children { get; set; } = new();
}