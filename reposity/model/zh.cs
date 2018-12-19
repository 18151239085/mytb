using DapperEx;
using System;
using System.Collections.Generic;

namespace Models
{
	/// <summary>
	///附件表
	/// </summary>
	[Serializable]
	public partial class zh
	{
		public IList<tzb> tzb { get; set; }
		public IList<attach> attach { get; set; }
	}
}
