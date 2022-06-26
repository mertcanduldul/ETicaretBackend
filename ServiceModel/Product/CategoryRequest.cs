using System.ComponentModel.DataAnnotations;

namespace ServiceModel.Product;

public class CategoryRequest
{
    [Required(ErrorMessage = "Kategori adı zorunludur !")]
    [MinLength(1, ErrorMessage = "Kategori adı en az 1 karakter olmalıdır !")]
    public string KATEGORI_ADI { get; set; }

    
    
}