using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogNavigator.Models;

public class Folder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    // [ForeignKey("Parent")]
    public int? ParentId { get; set; }
    public virtual Folder Parent { get; set; }
}