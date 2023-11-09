using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionPathB.Models
{
	internal class Dates
	{
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Day { get; set; }

		public override string ToString()
		{
			return StartDate + " " + EndDate + " " + Day;
		}
	}
}
