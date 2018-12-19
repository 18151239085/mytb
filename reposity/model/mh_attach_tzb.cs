using DapperEx;
using System;
using System.Collections.Generic;

namespace Models
{
	/// <summary>
	///附件表
	/// </summary>
	[Serializable]
	public partial class mh_attach_tzb
	{
		[IsId]
		public tzb tzb { get; set; }
		public IList<attach> attach { get; set; }
	}
}
