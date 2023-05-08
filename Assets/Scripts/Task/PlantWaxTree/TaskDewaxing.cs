using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LXQJZ.Task
{
	public class TaskDewaxing : TaskBase
	{
		bool isSuccess1 = false;
		bool isSuccess2 = false;
		bool isSuccess3 = false;

		void OnEnable()
		{
			ParticleManager.Stop(GetObj("WeldingWaxMachine_Pen_Fire"));
			ParticleManager.Stop(GetObj("FireGun_Fire"));
		}

		public override void RegisterSteps()
		{
			Step step1 = new Step();
			step1.objList.Add(GetObj("GypsumEffect"));
			step1.OnClickObj += ClickObj1;
			step1.CheckState += CheckState1;
			step1.tips = "将凝固好的石膏放入烤炉中，进行脱蜡";
			step1.Prepare += Prepare1;

			Step step2 = new Step();
			step2.objList.Add(GetObj("Oven1"));
			step2.OnClickObj += ClickObj2;
			step2.CheckState += CheckState2;

			Step step3 = new Step();
			step3.objList.Add(GetObj("Tongs"));
			step3.OnClickObj += ClickObj3;
			step3.CheckState += CheckState3;
		}





		#region Step1
		void Prepare1()
		{
			GetObj("WaxCup1").GetComponent<BoxCollider>().enabled = false;
		}

		private void ClickObj1()
		{
			GetObj("WaxCup1").GetComponent<BoxCollider>().enabled = false;
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working1());
		}

		IEnumerator working1()
		{
			WaitForSeconds wait4 = new WaitForSeconds(4);
			WaitForSeconds wait01 = new WaitForSeconds(0.1f);
			WaitForSeconds wait1 = new WaitForSeconds(1);

			GetObj("GypsumEffect").transform.SetParent(GetObj("Tool").transform);
			//石膏模放入烤炉
			AnimStart("Oven", "OvenOpen");
			AnimStart("GypsumEffect", "GypsumEffect_Enter_Oven");
			RoamCamera.Instance.MoveTo(new Vector3(-2.25f, -0.31f, 1.026f));
			yield return wait4;
			//关闭烤箱门
			AnimStart("Oven", "OvenClose");
			yield return wait1;
			//显示进度条
			GetUI<Image>("DeaerationMixer_Slider").gameObject.SetActive(true);
			Text txtSlider = GetUI<Text>("txtSlider");
			txtSlider.text = "温度为500°";
			Image imgFill = GetUI<Image>("DeaerationMixer_Fill");
			imgFill.fillAmount = 0;
			float process = 0;
			while (process < 12)
			{
				process += 0.1f;
				imgFill.fillAmount = process / 12;
				yield return wait01;
			}
			imgFill.fillAmount = 0;
			txtSlider.text = "";
			GetUI<Image>("DeaerationMixer_Slider").gameObject.SetActive(false);
			//相机返回原点
			RoamCamera.Instance.MoveToOrigin();
			yield return wait4;
			isSuccess1 = true;
		}

		StepState CheckState1()
		{
			return CheckState(isSuccess1);
		}
		#endregion


		#region Step2
		private void ClickObj2()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working2());
		}

		IEnumerator working2()
		{
			WaitForSeconds wait1 = new WaitForSeconds(1);

			//打开烤箱门
			AnimStart("Oven", "OvenOpen");
			yield return wait1;

			isSuccess2 = true;
		}

		StepState CheckState2()
		{
			return CheckState(isSuccess2);
		}
		#endregion


		#region Step3
		void ClickObj3()
		{
			RoamCamera.Instance.IsEnable = false;
			StartCoroutine(working3());
		}

		IEnumerator working3()
		{
			GameObject GypsumEffect = GetObj("GypsumEffect");

			//取出石膏模
			AnimStart("Tongs", "Tongs_Enter_Oven");
			yield return new WaitForSeconds(2.2f);
			GypsumEffect.transform.SetParent(GetObj("Tongs").transform);
			AnimStart("GypsumEffect", "GypsumEffect_Forward_Tongs");
			yield return new WaitForSeconds(1.8f);
			GypsumEffect.transform.SetParent(GetObj("Tool").transform);
			AnimStart("GypsumEffect", "GypsumEffect_Enter_GypsumTile");
			yield return new WaitForSeconds(1.4f);
			//关闭烤箱门
			AnimStart("Oven", "OvenClose");
			yield return new WaitForSeconds(1);
			isSuccess3 = true;
		}

		StepState CheckState3()
		{
			return CheckState(isSuccess3);
		}
		#endregion
	}
}

