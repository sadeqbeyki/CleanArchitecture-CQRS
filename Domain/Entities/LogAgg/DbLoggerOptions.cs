using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.LogAgg
{
	public class DbLoggerOptions
	{
		public string ConnectionString { get; init; }

		public string[] LogFields { get; init; }

		public string LogTable { get; init; }

		public DbLoggerOptions()
		{
		}
	}
}
