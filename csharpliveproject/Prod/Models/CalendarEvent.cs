using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheatreCMS3.Areas.Prod;
using System.ComponentModel.DataAnnotations;

namespace TheatreCMS3.Areas.Prod.Models
{
	public class CalendarEvent
	{
		[Key]
		public int EventId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		[DataType(DataType.Date)] // These annotations set the time and date formats
		[DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? StartDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? EndDate { get; set; }
		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		public DateTime? StartTime { get; set; }
		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		public DateTime? EndTime { get; set; }
		public bool IsProduction { get; set; }
		public int? ProductionId { get; set; }
	}
}