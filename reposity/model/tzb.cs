using DapperEx;
using System;

namespace Models
{
	/// <summary>
	///附件表
	/// </summary>
	[Serializable]
	public partial class tzb
	{
		/// <summary>
		/// 主键
		/// </summary>
		[IsId]
		public virtual string id { get; set; }
		public virtual string bt { get; set; }
		public virtual string content { get; set; }
		public virtual DateTime create_time { get; set; }
		public virtual string cjr { get; set; }
	
	}
}
