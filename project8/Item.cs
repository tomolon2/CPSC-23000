using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Models
{
	public class Item
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string ItemNum { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public int OnHand { get; set; }

		[MaxLength(20)]
		public string Category { get; set; } = string.Empty;

		public int Storehouse { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
	}
}
