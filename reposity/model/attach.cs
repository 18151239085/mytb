using DapperEx;
using System;

namespace Models
{
	/// <summary>
	///附件表
	/// </summary>
	[Serializable]
	public partial class attach
	{
		[IsId]
		public virtual string id { get; set; }
		public virtual string tp_name { get; set; }
		public virtual string tp_lj { get; set; }
		public virtual int ywlx { get; set; }
		public virtual string yw_id { get; set; }
		public virtual DateTime create_time { get; set; }
	
	}
}
