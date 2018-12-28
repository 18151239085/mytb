using DapperEx;
using System;

namespace Models
{
	[Serializable]
	public partial class yjfk
	{
		[IsId]
		public virtual string id { get; set; }
		public virtual string tzb_id { get; set; }
		public virtual string hf_nr { get; set; }
		public virtual DateTime hf_time { get; set; }
		public virtual string ry_id { get; set; }
		public virtual int  status { get; set; }
		public virtual string plnr { get; set; }

	}
}
