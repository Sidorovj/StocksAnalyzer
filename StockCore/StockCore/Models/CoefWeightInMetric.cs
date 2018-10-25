using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksAnalyzer.Data
{
	public class CoefWeightInMetric
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public double? Weight { get; set; }

		[ForeignKey("MetricId")]
		public Metric Metric { get; set; }

		[ForeignKey("CoefficientId")]
		public Coefficient Coefficient { get; set; }
		[ForeignKey("CustomMetricId")]
		public CustomMetric CustomMetric { get; set; }
	}
}