using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LXQJZ.Exam
{
	public enum Option
	{
		A, B, C, D, None
	}
	public interface ITitle
	{
		int Score { get; }
		bool IsRight { get; }
		GameObject gameObject { get; }

		void InitTitleData(TitleData source);

		void ShowTip();

		void SetInteractive(bool isInteractive);
	}
}
