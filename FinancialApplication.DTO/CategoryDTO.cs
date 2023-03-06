namespace FinancialApplication.DTO;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public class CategoryCreateDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public class CategoryUpdateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}