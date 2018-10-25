using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksAnalyzer.Data
{
	public class CustomMetric
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Fill metric name")]
		public string Name { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		public ICollection<CoefWeightInMetric> CoefWeights { get; set; }
	}
}