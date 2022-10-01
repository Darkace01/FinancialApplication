namespace FinancialApplication.DTO;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsSubcategory { get; set; } = false;
    public string UserId { get; set; }
}

public class CategoryCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsSubcategory { get; set; } = false;
}

public class CategoryUpdateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsSubcategory { get; set; } = false;
}