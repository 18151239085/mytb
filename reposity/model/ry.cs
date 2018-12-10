using DapperEx;
using System;

namespace Models
{
	/// <summary>
	///附件表
	/// </summary>
	[Serializable]
	public partial class ry
	{
		/// <summary>
		/// 主键
		/// </summary>
		[IsId]
		public virtual string id { get; set; }
		public virtual string nc { get; set; }
		public virtual string yhm { get; set; }
		public virtual int rylx { get; set; }
		public virtual string mm { get; set; }
		public virtual string sjh { get; set; }
		public virtual DateTime create_time { get; set; }
	
	}
}
