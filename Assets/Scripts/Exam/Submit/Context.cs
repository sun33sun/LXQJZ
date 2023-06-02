using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXQJZ.Exam
{
	public class Context
	{
		public string username;
		public string title;
		//1：普通学生实验保存；2：评审实验保存
		public int status;
		public int score;
		public DateTime startTime;
		public DateTime endTIme;
		public TimeSpan timeUsed;
		public int appid;
		public int originId;
		public int group_id;
		public string group_name;
		public string role_in_group;
		public string group_members;
		public List<Step> steps;
	}
}
