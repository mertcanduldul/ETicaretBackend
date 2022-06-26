using System.ComponentModel.DataAnnotations;

namespace ServiceModel.Product;

public class UpdateCategoryRequest
{
    [Required(ErrorMessage = "ID_KATEGORI zorunludur !")]
    public int ID_KATEGORI { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur !")]
    [MinLength(1, ErrorMessage = "Kategori adı en az 1 karakter olmalıdır !")]
    public string KATEGORI_ADI { get; set; }
}