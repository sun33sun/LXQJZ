using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Exam
{
	public enum Option
	{
		A, B, C, D
	}
	public interface ITitle
	{
		int Score { get; }
		bool IsRight { get; }
		void InitTitleData(TitleData source);
	}
}
